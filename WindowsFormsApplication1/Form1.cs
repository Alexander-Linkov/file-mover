using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;


namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {

        public static string project = Directory.GetCurrentDirectory();
        static string log_file = project + "\\file_mover.log";
        static bool old_load_from_file = false;
        static bool new_load_from_file = false;
        

        //***OPTIONS
        static bool just_fix = false;
        static bool fix_after_rename = false;
        static bool fix_only_template = false;
        static bool fix_ignore_upper_case = false;
        static bool just_rename = false;
        static bool make_relative_links = false;
        static bool use_cl = false; // Use Command Line Tools (Git,...)
        static bool auto_fix = false;
        static string scs_type = "";
        static string scs_path = "";
        static bool delete_unused = false;
        static bool rewrite_if_exists = false;

        static List<string> ignore_list = new List<string>() { ".orig", ".html", ".htm" };

        int link_to_file = 0;

        static string corrected_file_name; // used in correcting link
        static bool is_dialog; //used to pause background worker while is correcting dialog is showed.
        static bool is_canceled; // чтобы зафиксировать отмену правки ссылки
        static int dialog_result; //если нужен результат диалога
        
        // for correction memmory        
        public static List<string> corrected_links_list = new List<string>();
        public static List<string> broken_links_list = new List<string>();
        public static Dictionary<string, string> correction_links_memory = new Dictionary<string, string>();

        //private string new_name;

        public Form1()
        {
            InitializeComponent();
        }

        private void Rename(string full_old_name, string full_new_name)
        {

            string backup_file = full_old_name + ".orig";
            File.Copy(full_old_name, backup_file, true);

            string message = String.Format("File rename: \"{0}\" >>> \"{1}\"", full_old_name, full_new_name);
            log_process(message);

            string new_dir = full_new_name.Remove(full_new_name.LastIndexOf("\\"));
            if (!Directory.Exists(new_dir)) Directory.CreateDirectory(new_dir);

            if (use_cl)
            {
                // use command line
                string command = "";
                switch (scs_type)
                {
                    case "Git": command = String.Format("mv \"{0}\" \"{1}\"", full_old_name, full_new_name);
                        break;
                }

                Run_CSC_CommandLine(command);
                if (File.Exists(backup_file)) File.Delete(backup_file);
            }
            else
            {
                File.Move(full_old_name, full_new_name);
            }

            
        }

        private void Delete(string full_name)
        {
            string message = String.Format("File delete: \"{0}\"", full_name);
            log_process(message);

            if (use_cl)
            {
                string command = "";
                switch (scs_type)
                {
                    case "Git":
                        command = String.Format("rm {0}", full_name);
                        break;
                }

                Run_CSC_CommandLine(command);
            } else
            {
                File.Move(full_name, full_name + ".orig");
            }

        }

        private void Run_CSC_CommandLine(string command)
        {
            Process process = new Process();
            process.StartInfo.FileName = scs_path;
            process.StartInfo.Arguments = command;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            string message = String.Format("Operation upon the file {0}: command:\r\n{1} {2}\r\nOutput:\r\n{3}", scs_type, scs_path, command, output);
            log_process(message);
        }

        private void SetOptions()
        {
            just_fix = checkBox_FixLink.Checked && checkBox_FixLink.Enabled;
            fix_after_rename = checkBox_FixLink_AfterRename.Enabled && checkBox_FixLink_AfterRename.Checked;
            fix_only_template = checkBox_IgnoreNotTemplateLinks.Checked;
            fix_ignore_upper_case = checkBox_IgnoreUpperCase.Checked;
            make_relative_links = checkBox_MakeRefLink.Checked && checkBox_MakeRefLink.Enabled;
            use_cl = checkBox_RenameUsingCL.Checked && checkBox_RenameUsingCL.Enabled;
            auto_fix = checkBox_autoFix.Checked && checkBox_autoFix.Enabled;
            delete_unused = checkBox_DeleteUnused.Checked && checkBox_DeleteUnused.Enabled;
            rewrite_if_exists = checkBox_RewriteIfExists.Checked && checkBox_RewriteIfExists.Enabled;
            if (use_cl)
            {
                scs_type = comboBoxSCS.Text;
                scs_path = "\"" + textBox_SCSPath.Text + "\"";
            }
        }

        private void Start_Rename()
        {
            SetOptions();

            string old_path = get_root_path(oldName_box);
            string new_path = get_root_path(newName_box);
            int checked_items = newFilesList.CheckedItems.Count;

            // проверка того, что количество начальных и конечных файлов совпадает.
            // количество может различаться если загружать правый список из файла.
            int oldCount = oldFilesList.Items.Count;
            int newCount = newFilesList.Items.Count;
            if (oldCount != newCount)
            {
                if (MessageBox.Show("File lists contain different number of elements.\n Continue?", "File Mover", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    label3.Text = "File lists contain different number of elements";
                    return;
                }
            }

            // Проверка указаных путей
            if (use_cl == true)
            {
                if (File.Exists(textBox_SCSPath.Text) == false)
                {
                    MessageBox.Show("The path to sourcecontrol is not correct");
                    return;
                }
            }

            string message = "";
            if (just_fix)
            {
                label3.Text = "Checking...";
                message = String.Format("Check links in {0} files in \n{1}", checked_items, old_path);

            }
            else
            {
                label3.Text = "Moving...";
                message = String.Format("Move {0} files \nfrom\n {1}\n to\n {2}", checked_items, old_path, new_path);
            }

            // подтверждение переноса\проверки
            if (MessageBox.Show(message, "File mover", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                log_process(message);
                btn_Cancel.Enabled = true;
                if (!backgroundWorker1.IsBusy)
                {
                    // Запуск
                    backgroundWorker1.RunWorkerAsync();
                } else
                {
                    MessageBox.Show("Work is already in progress. Please wait", "Work is already in progress", MessageBoxButtons.OK);
                }
            }

        }

        private void log_process(string message)
        {
            string time_stamp = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            File.AppendAllText(log_file, time_stamp +": " + message + "\n");
        }

        // кусок из stackoverflow для получения коллекции айтемов из другого потока

        private delegate ListView.ListViewItemCollection GetItems(ListView lstview);

        private ListView.ListViewItemCollection getListViewItems(ListView lstview)
        {
            ListView.ListViewItemCollection temp = new ListView.ListViewItemCollection(new ListView());
            if (!lstview.InvokeRequired)
            {
                foreach (ListViewItem item in lstview.Items)
                    temp.Add((ListViewItem)item.Clone());
                return temp;
            }
            else
                return (ListView.ListViewItemCollection)this.Invoke(new GetItems(getListViewItems), new object[] { lstview });
        }

        // конец куска
        
        // Запуск 
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            string message;
            FileInfo moving_topic; // переносимый топик

            // плдучаем коллекцию айтемов
            ListView.ListViewItemCollection oldFiles = getListViewItems(oldFilesList);
            ListView.ListViewItemCollection newFiles = getListViewItems(newFilesList);

            // унифицируем пути source и target (в результатах последний символ всегда будет слеш)
            string old_path = "";
            if ((!old_load_from_file) && (!fix_after_rename)) old_path = get_root_path(oldName_box);
            string new_path = "";
            if ((!new_load_from_file) && (!fix_after_rename)) new_path = get_root_path(newName_box);


            foreach (ListViewItem item in oldFiles)
            {

                int cur_index = item.Index; // индекс исходного файла

                ListViewItem new_item = newFiles[cur_index]; // по индексу находим соответствующий новый путь

                if (new_item.Checked == false)
                {
                    log_process(new_item.Text + " is not checked. Skipped");
                    continue; // проверяем, что он отмечен
                }

                string old_name = old_path + item.Text; //старый путь от проекта
                string new_name = new_path + new_item.Text; // будущий путь от проекта

                if (fix_after_rename)
                {
                    backgroundWorker1.ReportProgress(1, new_name);
                } else
                {
                    backgroundWorker1.ReportProgress(1, old_name);
                }

                if (backgroundWorker1.CancellationPending)
                {
                    return;
                }

                string old_full_name = project + "\\" + old_name; // полный путь до оригинала
                string new_full_name = project + "\\" + new_name; // полный путь до нового файла

                if (!fix_after_rename) // если старые файлы не были переименованы
                {
                    bool exit = true;
                    bool next_file = false;
                    do
                    {
                        // проверяем, что исходный файл существует
                        if (!File.Exists(old_full_name))
                        {
                            DialogResult answer = MessageBox.Show("The source file does not exist", "File Mover", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                            if (answer == DialogResult.Abort)
                            {
                                return;
                            }
                            else if (answer == DialogResult.Retry)
                            {
                                exit = false;
                                continue;
                            } else if (answer == DialogResult.Ignore)
                            {
                                next_file = true;
                                exit = true;
                            }
                        }
                    } while (!exit);
                    if (next_file) continue;

                    if (!just_fix) // если планируется переименовывать
                    {
                        // проверяем, есть ли новый файл
                        if (File.Exists(new_full_name) && !rewrite_if_exists)
                        {
                            is_dialog = true;
                            dialog_result = -1;
                            // show dialog in UI thread
                            this.Invoke((MethodInvoker)delegate ()
                            {
                                string file_name = new_item.Text;
                                DialogResult awnser = MessageBox.Show("File " + file_name + " is already exists.\n Rewrite it? \nClick Cancel to stop moving.", "Target exist message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                                if (awnser == DialogResult.No)
                                {
                                    // отменить только этот файл
                                    dialog_result = 0;
                                }
                                else if (awnser == DialogResult.Cancel)
                                {
                                    // отменить всю конвертацию
                                    dialog_result = 1;
                                }
                                is_dialog = false;
                            });

                            // pause worker thread until dialog is shown
                            while (is_dialog)
                            {
                                System.Threading.Thread.Sleep(100);
                            }

                            switch (dialog_result)
                            {
                                case 0: continue;
                                case 1: return;
                            }
                        }
                    }
                } else // если файлы были уже переименованы
                {
                    if (!File.Exists(new_full_name)) //проверить, что новый файл существует.
                    {
                        is_dialog = true;
                        // show dialog in UI thread
                        this.Invoke((MethodInvoker)delegate ()
                        {
                            // если файла нет, то предлагаем указать новый
                            string msg = String.Format("File {0} is not found.\n Probably it was renamed. Do you want to select new file?", new_full_name);
                            DialogResult res = MessageBox.Show(msg, "File Mover Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                            if (res == DialogResult.Yes)
                            {
                                openFileDialog1.FileName = new_full_name.Remove(0, new_full_name.LastIndexOf('\\') + 1);
                                openFileDialog1.ShowHelp = true;
                                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                                {
                                    corrected_file_name = openFileDialog1.FileName;
                                }
                                else
                                {
                                    corrected_file_name = null;
                                }
                            }
                            else
                            {
                                corrected_file_name = null;
                            }
                            is_dialog = false;
                        });

                        // pause worker thread until dialog is shown
                        while (is_dialog)
                        {
                            System.Threading.Thread.Sleep(100);
                        }

                        if (corrected_file_name != null)
                        {
                            new_full_name = corrected_file_name;
                            new_name = corrected_file_name.Remove(0, project.Length + 1);
                        }
                        else
                        {
                            return;
                        }

                    }
                }

                // узнаем расширение файла
                string file_type = old_name.Substring(old_name.LastIndexOf(".")+1);

                // массивы пути
                string[] old_dirs;
                old_dirs = old_name.Split('\\');
                string[] new_dirs;
                new_dirs = new_name.Split('\\');

                message = String.Format("Starting moving file {0}", item.Text);
                log_process(message);

                if (!just_fix)
                {
                    // если не отмечена опция "только проверка", то перенос файлов
                    Rename(old_full_name, new_full_name);
                }

                moving_topic = new FileInfo(new_full_name);

                // проверка ссылок ИЗ файла только если не омечено just rename и тип файла xml
                if (!just_rename && file_type == "xml")
                {
                    FixLinksNew(moving_topic, old_dirs, new_dirs, new_full_name);
                }

                if (!just_rename && (!just_fix || fix_after_rename))
                { // проверка ссылок НА файлы нужна только если имеет место переименовывание
                    message = "Fixing links TO the file:";
                    log_process(message);

                    // обнуляем счетчик ссылок на файл
                    link_to_file = 0;

                    DirectoryInfo project_dir_info = new DirectoryInfo(project);
                    // правим ссылки на файл
                    ScanDir(project_dir_info, old_dirs, new_dirs);

                    if (delete_unused && link_to_file==0)
                    {
                        Delete(new_full_name);
                    }

                }

                /*
                if (!just_fix) // раньше здесь исходный файл становился резервным. Теперь функционал уехал в Rename функцию.
                {
                    // переименовываем исходный файл
                    
                    if (File.Exists(backup_file)) File.Delete(backup_file);

                    message = String.Format("File {0} >>> {1}", old_full_name, new_full_name);
                    log_process(message);

                    File.Move(old_full_name, backup_file);
                }*/
            }

        }

        private void populateList(string attr, string fileContent, List<string> target_list, List<int> sel_start, List<int> sel_len)
        {
            List<string> _res = new List<string>();
            string pattern = "(?<=" + attr + "=\")[^:\"]*?(?=\")";
            Regex regex = new Regex(pattern);
            MatchCollection mathces = regex.Matches(fileContent);
            foreach (Match match in mathces)
            {
                if (!target_list.Contains(match.Value))
                {
                    target_list.Add(match.Value);
                    sel_start.Add(match.Index);
                    sel_len.Add(match.Length);
                }
            }
            //return _res;
        }

        private string Correct_link(string broken_path, string fileContent, int select_start, int select_lenght)
        {
            string res = null;
            //ShowCorrectionDialog(broken_path, fileContent, log_match_item.Index + lenght_diff - removed_symbols, log_match_item.Length);
            is_dialog = true;
            int removed_symbols = 0;
            if (fileContent.Length >= select_start + select_lenght)
            {
                removed_symbols = fileContent.Substring(0, select_start + select_lenght).Count(s => s == '\r');
            }
            is_canceled = ShowCorrectionDialog(broken_path, fileContent, select_start - removed_symbols, select_lenght);
            while (is_dialog)
            {
                System.Threading.Thread.Sleep(100);
            }
            if (corrected_file_name != null && corrected_file_name != "" && corrected_file_name != broken_path)
            {
                AddFixToMemory(broken_path.Remove(0, project.Length + 1), corrected_file_name.Remove(0, project.Length + 1));
                res = corrected_file_name;
            }
            return res;
        }

        private void FixLinksNew(FileInfo file, string[] old_dirs, string[] new_dirs, string new_full_name)
        {
            /*
            получаем коллекцию matches
            для каждой matches
	            добавляем value в коллекцию

            Повторяем с str

            для каждого айтема в коллекции
	            находим нормальный путь
	            проверяем его валидность
	            делаем из пути новый относительный путь
	            string.replace
            */
            List<string> links = new List<string>();
            List<int> select_starts = new List<int>();
            List<int> select_lens = new List<int>();
            string fileContent;
            int lenght_diff = 0;

            string dtdPath = textBox_dtdPath.Text;
            string xslPath = textBox_xslPath.Text;

            if (fix_after_rename == true)
            {

                fileContent = File.ReadAllText(new_full_name);
            }
            else
            {
                fileContent = File.ReadAllText(file.FullName);
            }
            string newFileContent = fileContent;
            string message;


            // добавляем ссылку на dtd
            string dtd_pattern = "(?<=SYSTEM \")[^\"]*?.dtd(?=\")";
            Regex dtd_regex = new Regex(dtd_pattern);
            MatchCollection _dtds = dtd_regex.Matches(newFileContent);
            if (_dtds.Count == 1)
            {
                Match dtd_link = _dtds[0];
                links.Add(dtd_link.Value);
                select_starts.Add(dtd_link.Index);
                select_lens.Add(dtd_link.Length);
            }

            // находим ссылки href (на xls, топики)
            populateList("href", newFileContent, links, select_starts, select_lens);

            // находим ссылки src (на картинки)
            populateList("src", newFileContent, links, select_starts, select_lens);

            //находим нормальный путь
            /* три варианта
            - конреф должен остаться конрефом (начинаются с /)
            - топики назад по дереву (начинаются с ..)
            - siblings или глубже (начинаются с символа ИЛИ ./)
            */
            foreach (string link in links)
            {
                if (link == "") continue;
                string link_from_root = "";
                string link_from_root_after_rename = "";
                string _link = link;
                bool is_conref = false;
                if (_link[0] == '/')
                {
                    // конреф ссылка (абсолютная) менять не надо, но надо проверить валидность
                    link_from_root = _link.Remove(0, 1);
                    link_from_root_after_rename = link_from_root;
                    is_conref = true;
                }
                else
                {
                    // если не абсолютная ссылка, то получаем абсолютную ссылку от проекта
                    int path_back = 0;
                    if (_link.Substring(0, 2) == "./")
                    {
                        _link = _link.Remove(0, 2);
                    }
                    if (_link.Substring(0,2) == "..")
                    {
                        Regex path_back_finder = new Regex("\\.\\./");
                        path_back = path_back_finder.Matches(_link).Count;
                        _link = _link.Remove(0, 3 * path_back);
                    }
                    for (int i = 0; i < old_dirs.Length - path_back - 1; i++)
                    {
                        link_from_root += old_dirs[i] + "/";
                    }
                    if (fix_after_rename)
                    {
                         // если файл уже перемещен, то надо запомнить ссылку по новому пути
                         for (int i = 0; i < new_dirs.Length - path_back - 1; i++)
                         {
                             link_from_root_after_rename += new_dirs[i] + "/";
                         }
                     }
                    link_from_root += _link;
                    link_from_root_after_rename += _link;
                }

                string full_link = project + "\\" + link_from_root.Replace('/', '\\');
               

                // проверяем, что ссылка ведет на файл
                is_canceled = false;
                bool to_continue = false;
                bool rename_link_checked = false;
                if (full_link == null)
                {
                    Console.WriteLine(link + "produces NULL Link");
                }
                while (!File.Exists(full_link) && !is_canceled)
                {
                    if (fix_after_rename && !rename_link_checked)
                    {
                        // если было переименовывание проверим ссылку по новому пути
                        // полная (возможно) исправленная ссылка по новому пути
                        string full_link_after_rename = project + "\\" + link_from_root_after_rename.Replace('/', '\\');
                        if (File.Exists(full_link_after_rename))
                        {
                            full_link = full_link_after_rename;
                            link_from_root = link_from_root_after_rename;
                        }
                        rename_link_checked = true;
                        continue;
                    }
                    if (full_link.Remove(0, full_link.LastIndexOf('.') + 1) == "xsl")
                    {
                        //link_from_root = "_templates/topic2html.xsl";
                        link_from_root = xslPath.Replace('\\', '/');
                        full_link = project + "\\" + link_from_root.Replace('/', '\\');
                        continue;
                    }
                    if (full_link.Remove(0, full_link.LastIndexOf('.') + 1) == "dtd")
                    {
                        //link_from_root = "_templates/aqScheme.dtd";
                        link_from_root = dtdPath.Replace('\\', '/');
                        full_link = project + "\\" + link_from_root.Replace('/', '\\');
                        continue;
                    }
                    if (fix_only_template)
                    {
                        is_canceled = true;
                        to_continue = true;
                    }
                    else {
                        char[] upper_abc = new char[] { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' };
                        if (link_from_root.IndexOfAny(upper_abc) != -1 && fix_ignore_upper_case)
                        {
                            is_canceled = true;
                            to_continue = true;
                        }
                        else if (link_from_root.IndexOf('#') != -1)
                        {
                            is_canceled = true;
                            to_continue = true;
                        }
                        else
                        {
                            int link_ind = links.IndexOf(link);
                            full_link = Correct_link(full_link, newFileContent, select_starts[link_ind] + lenght_diff, select_lens[link_ind]);
                            if (full_link != null)
                            {
                                link_from_root = full_link.Remove(0, project.Length + 1).Replace('\\', '/');
                            }
                        }
                    }
                }
                if (to_continue) continue;
                if (is_canceled) continue;

                //  составляем новый путь
                string[] file_dirs = link_from_root.Split('/');
                string new_link = "";

                int common_dirs = GetCommonInPath(new_dirs, file_dirs);

                if (!is_conref || make_relative_links)
                {
                    // составляем относительную ссылку
                    for (int i = 0; i < new_dirs.Length - common_dirs - 1; i++)
                    {
                        new_link += "../";
                    }
                    for (int j = common_dirs; j < file_dirs.Length - 1; j++)
                    {
                        new_link += file_dirs[j] + "/";
                    }
                    new_link += file_dirs[file_dirs.Length - 1];
                } else
                {
                    new_link = "/" + link_from_root;
                }
                if (new_link != link)
                {
                    message = String.Format("\"{0}\" >>> \"{1}\"", link, new_link);
                    log_process(message);

                    int old_len = newFileContent.Length;
                    string to_find = "\"" + link + "\"";
                    string to_replace = "\"" + new_link + "\"";
                    newFileContent = newFileContent.Replace(to_find, to_replace);
                    lenght_diff = old_len - newFileContent.Length;
                }
            }

            if (!fix_after_rename)
            {
                // создаем новый файл с исправленными ссылками
                string new_dir = new_full_name.Remove(new_full_name.LastIndexOf("\\"));
                if (!Directory.Exists(new_dir)) Directory.CreateDirectory(new_dir);
                File.WriteAllText(new_full_name, newFileContent);

                message = String.Format("Create file \"{0}\"", new_full_name);
                log_process(message);
            }
            else
            {
                // перезаписываем файл
                File.WriteAllText(new_full_name, newFileContent);
                message = String.Format("Link fixed in \"{0}\"", new_full_name);
                log_process(message);
            }

        }

        /*
        private void FixLinksOld(FileInfo file, string[] old_dirs, string[] new_dirs, string new_full_name)
        {
            string fileContent;
            int lenght_diff = 0;

            if (fix_after_rename)
            {
                
                fileContent = File.ReadAllText(new_full_name);
            } else
            {
                fileContent = File.ReadAllText(file.FullName);
            }
            string newFileContent = fileContent;
            string message;

            // убираем возможную точку в начале ссылки (href="./)
            newFileContent = newFileContent.Replace("\"./", "\"");


            message = "Fixing links FROM the file:";
            log_process(message);

            string path_to_root = "\""; //глубина вложения нового файла
            for (int deep = 0; deep < new_dirs.Length - 1; deep++)
            {
                path_to_root += "../";
            }

            string pattern;
            for (int i = 0; i < old_dirs.Length - 1; i++)
            {
                int group_index; //группа нахождения регулярной строки, которая будет вставлена при замене
                switch (i) {
                    case 0:
                        // паттерн для нахождения ссылок на топики одного уровня или глубже
                        // (?<=[cf]\=)"([^/][^.<>]*?\.xml)
                        pattern = "(?<=[cf]\\=)\"([^/][^.<>]*?\\.xml)"; // siblings and deeper
                        group_index = 1;
                        break;
                    default:
                        // паттерн для нахождения остальных ссылок (увеличивается количество ../ (уходов выше)
                        pattern = "\"(\\.\\./){" + i + "}" + "([^._]*?\\.xml)\"";
                        group_index = 2;
                        break;
                }
                Regex notMaxRegX = new Regex(pattern);
                if (notMaxRegX.IsMatch(newFileContent))
                {
                    // генерируем новый путь
                    string new_path_from_root = "";
                    for (int old_paths = 0; old_paths < old_dirs.Length - 1 - i; old_paths++)
                    {
                        new_path_from_root += old_dirs[old_paths] + "/";
                    }
                    
                    // logging process
                    MatchCollection log_matches = notMaxRegX.Matches(newFileContent);
                    lenght_diff = 0;
                    foreach (Match log_match_item in log_matches)
                    {
                        string path_for_link = new_path_from_root;
                        if (fix_after_rename)
                        {
                            string full_link_to_check = project + "\\";
                            for (int path = 0; path < new_dirs.Length - i - 1; path++)
                            {
                                full_link_to_check += new_dirs[path] + "\\";
                            }
                            full_link_to_check += log_match_item.Value.Remove(0, 1 + 3*i);
                            if (i>0)
                            {
                                full_link_to_check = full_link_to_check.Remove(full_link_to_check.Length - 1, 1);
                            }
                            if (File.Exists(full_link_to_check))
                            {
                                continue;
                            }
                        }

                        string file_name;
                        string full_file_name;

                        if (i == 0)
                        {
                            file_name = log_match_item.Groups[group_index].ToString();
                            full_file_name = project + "\\" + path_for_link.Replace('/', '\\') + file_name.Replace('/', '\\');
                        } else
                        {
                            string group_text = log_match_item.Groups[group_index].ToString();
                            int file_name_start = group_text.LastIndexOf('/') + 1;
                            file_name = group_text.Substring(file_name_start);
                            path_for_link += group_text.Substring(0, file_name_start);
                            full_file_name = project + "\\" + path_for_link.Replace('/', '\\') + file_name;
                        }
                        

                        if (!File.Exists(full_file_name))
                        {
                            char[] upper_abc = new char[] { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' };
                            if (log_match_item.Value.IndexOfAny(upper_abc) != -1)
                            {
                                continue;
                            }

                            int removed_symbols = newFileContent.Substring(0, log_match_item.Index + lenght_diff).Count(s => s == '\r');
                            string broken_link = full_file_name.Remove(0, project.Length + 1);
                            string broken_file_path = project + "\\" + broken_link;
                            ShowCorrectionDialog(broken_file_path, newFileContent, log_match_item.Index + lenght_diff - removed_symbols, log_match_item.Length);
                            while (is_dialog)
                            {
                                System.Threading.Thread.Sleep(100);
                            }
                            if (corrected_file_name != null && corrected_file_name != "" && corrected_file_name != broken_link)
                            {

                                AddFixToMemory(broken_link, corrected_file_name.Remove(0, project.Length + 1));
                                path_for_link = corrected_file_name.Remove(0, project.Length + 1).Replace('\\', '/');
                                file_name = path_for_link.Substring(path_for_link.LastIndexOf('/') + 1);
                                path_for_link = path_for_link.Remove(path_for_link.LastIndexOf('/') + 1);
                            }
                            else
                            {
                                message = String.Format("Incorrect link: {0} - Not replaced", full_file_name);
                                log_process(message);
                                continue;
                            }
                        }

                        string new_path = path_to_root + path_for_link;
                        string new_link = new_path + file_name;

                        if (i > 0)
                        {
                            new_link += "\"";
                        }

                        message = String.Format("\"{0}\" >>> \"{1}\"", log_match_item.Value, new_link);
                        log_process(message);
                        // заменяем старый путь на новый
                        //newFileContent = notMaxRegX.Replace(newFileContent, new_path + log_match_item.Groups[group_index]);
                        int old_length = newFileContent.Length;
                        newFileContent = newFileContent.Replace(log_match_item.Value, new_link);
                        lenght_diff += newFileContent.Length - old_length;
                    }
                    
                    
                }
            }

            // все топики на которые ссылались через "корень" проекта (на уровень выше _topic)
            // это ссылки на перенесенные топики, на _images, _templates и ссылки из новых топиков на неперенесенные
            int max_deep = old_dirs.Length - 1;
            pattern = "\"(\\.\\./){" + max_deep + "}([^.]*?\\.[a-z]{3,4})\"";
            Regex MaxRegX = new Regex(pattern);
            MatchCollection matches = MaxRegX.Matches(newFileContent);
            lenght_diff = 0;
            foreach(Match match in matches)
            {
                string new_link = "\"";
                string match_string = match.Groups[0].Value;
                string path_from_root = match.Groups[2].Value;

                string full_file_name = project +"\\" + path_from_root.Replace('/', '\\');

                

                if (!File.Exists(full_file_name))
                {
                    char[] upper_abc = new char[] { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' };
                    if (match_string.IndexOfAny(upper_abc) != -1)
                    {
                        continue;
                    }

                    if (fix_after_rename)
                    {
                        string old_full_link = project + "\\" + path_from_root.Replace('/', '\\');
                        if (File.Exists(old_full_link))
                        {
                            message = String.Format("Link {} was correct.", match_string);
                            log_process(message);
                            continue;
                        }
                    }
                    if (full_file_name.Remove(0, full_file_name.LastIndexOf('\\') + 1) == "topic2html.xsl")
                    {
                        path_from_root = "_templates/topic2html.xsl";
                    }
                    else {
                        int removed_symbols = newFileContent.Substring(0, match.Index + lenght_diff).Count(s => s == '\r');
                        string broken_link = full_file_name.Remove(0, project.Length + 1);
                        string broken_file_path = project + "\\" + broken_link;
                        ShowCorrectionDialog(broken_file_path, newFileContent, match.Index + lenght_diff - removed_symbols, match.Length);
                        while (is_dialog)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                        if (corrected_file_name != null && corrected_file_name != "" && corrected_file_name != broken_link)
                        {
                            
                            AddFixToMemory(broken_link, corrected_file_name.Remove(0, project.Length + 1));
                            path_from_root = corrected_file_name.Remove(0, project.Length + 1).Replace('\\', '/');

                        }
                        else
                        {
                            message = String.Format("Incorrect link: {0} - Not replaced", full_file_name);
                            log_process(message);
                            continue;
                        }
                    }
                }

                // находим количество общих родительских топиков для построения относительной ссылки
                string[] sibling_dirs = path_from_root.Split('/');
                int common_path = GetCommonInPath(sibling_dirs, new_dirs);
                int way_back = new_dirs.Length - 1 - common_path; // количество возвращений
                // уходим назад до первого общего родителя
                for (var i = 0; i < way_back; i++)
                {
                    
                    new_link += "../";
                }
                // строим остальной путь
                for (var j = common_path; j < sibling_dirs.Length - 1; j++)
                {
                    new_link += sibling_dirs[j] + "/";
                }
                // добавляем имя самого файла
                new_link += sibling_dirs[sibling_dirs.Length - 1] + "\"";

                message = String.Format("\"{0}\" >>> \"{1}\"", match_string, new_link);
                log_process(message);

                int old_length = newFileContent.Length;
                newFileContent = newFileContent.Replace(match_string, new_link);
                lenght_diff += newFileContent.Length - old_length;
            }

            if (!fix_after_rename)
            {
                // создаем новый файл с исправленными ссылками
                string new_dir = new_full_name.Remove(new_full_name.LastIndexOf("\\"));
                if (!Directory.Exists(new_dir)) Directory.CreateDirectory(new_dir);
                File.WriteAllText(new_full_name, newFileContent);

                message = String.Format("Create file \"{0}\"", new_full_name);
                log_process(message);
            } else
            {
                // перезаписываем файл
                File.WriteAllText(new_full_name, newFileContent);
                message = String.Format("Link fixed in \"{0}\"", new_full_name);
                log_process(message);
            }
        }
        */

        // итерируемся по папкам и файлам
        private void ScanDir(DirectoryInfo parent_dir_info, string[] old_dirs, string[] new_dirs)
        {
            foreach (DirectoryInfo dir in parent_dir_info.GetDirectories())
            {
                if (dir.Name != ".git" && dir.Name != "Generated Files")
                {
                    ScanDir(dir, old_dirs, new_dirs);
                }
            }
            
            
            foreach (FileInfo file in parent_dir_info.GetFiles("*.xml"))
            {
                // открываем только xml
                string cur_path = file.FullName;

                string temp_path = cur_path.Remove(0, project.Length+1);
                // делаем массив из пути до проверяемого топика
                string[] cur_dirs;
                cur_dirs = temp_path.Split('\\');

                //if (cur_path.Contains("_topics"))
                //{
                // находим общие папки
                int common = GetCommonInPath(cur_dirs, old_dirs);
                // при полном совпадении пути, это переносимый файл, пропускаем его.
                if (common == cur_dirs.Length) continue;

                // правим ссылки на файл
                FindRef(file, common, cur_dirs, old_dirs, new_dirs);
                //}
            }
        }

        private void FindRef(FileInfo file, int common, string[] cur_dirs, string[] old_dirs, string[] new_dirs)
        {

            string message; // for logging

            // количество "уходов назад" для нахождения относительных ссылок
            int back_refs = cur_dirs.Length - common - 1;

            // построение ссылки на файл из текущего топика
            string pattern = "\"(\\.\\./){" + back_refs.ToString() + "}";
            for (var i = common; i<old_dirs.Length-1; i++)
            {
                pattern += old_dirs[i] + "/";
            }
            pattern += old_dirs[old_dirs.Length-1];

            // построение конреф (неотносительной) ссылки на переносимый топик
            string conref_pattern = "\"";
            for (var c = 0; c < old_dirs.Length; c++)
            {
                conref_pattern += "/" + old_dirs[c];
            }

            string fileContent = File.ReadAllText(file.FullName);

            string newFileContent = fileContent;

            Regex rgx = new Regex(pattern);
            if (rgx.IsMatch(newFileContent))
            {
                // при нахождении ссылки повышаем счетчик ссылок
                link_to_file++;


                string new_link = "\"";
                int new_com = GetCommonInPath(new_dirs, cur_dirs);
                // при нахождении относительной ссылки, строим новую относительную ссылку
                for (var i = 0; i < cur_dirs.Length - new_com - 1; i++)
                {
                    new_link += "../";
                }
                for (int j = new_com; j < new_dirs.Length - 1; j++)
                {
                    new_link += new_dirs[j] + "/";
                }
                new_link += new_dirs[new_dirs.Length - 1];
                
                // logging process
                MatchCollection log_matches = rgx.Matches(newFileContent);

                message = String.Format("-----In file {0}:", file.FullName);
                log_process(message);

                foreach (Match log_match_item in log_matches)
                {
                    message = String.Format("\"{0}\" >>> \"{1}\"", log_match_item.Value, new_link);
                    log_process(message);
                }

                newFileContent = rgx.Replace(newFileContent, new_link);
                File.WriteAllText(file.FullName, newFileContent);
            }

            if (newFileContent.Contains(conref_pattern))
            {
                // при нахождении ссылки повышаем счетчик ссылок
                link_to_file++;

                // при нахождении конреф ссылки (полной) заменяем ее на новый путь
                System.Console.WriteLine("Conref link detected");
                string newConrefLink = "\"";
                for (var nc=0; nc<new_dirs.Length; nc++)
                {
                    newConrefLink += "/" + new_dirs[nc];
                }
                newFileContent = newFileContent.Replace(conref_pattern, newConrefLink);

                message = String.Format("-----In file {0} were full refs: {1} >>> {2}", file.FullName, conref_pattern, newConrefLink);
                log_process(message);

                File.WriteAllText(file.FullName, newFileContent);
            }

        }

        // функция возвращает количество общих папок в пути. Используется для построения относительных ссылок
        private int GetCommonInPath(string[] pathA, string[] pathB)
        {
            int common = 0;
            for (int i = 0; i < Math.Min(pathA.Length, pathB.Length); i++)
            {
                if (pathA[i] == pathB[i])
                {
                    common++;
                }
                else
                {
                    return common;
                }
            }
            return common;
        }

        // обновляет список файлов в текущей папке
        private void UpdateOldList()
        {
            /*string old_dir = project + "\\" + oldName_box.Text;
            if (oldName_box.Text.Last() != '\\')
            {
                old_dir += "\\";
            }*/
            string old_dir = project + "\\" + get_root_path(oldName_box);
            DirectoryInfo old_dir_info = new DirectoryInfo(old_dir);
            oldFilesList.Items.Clear();
            addItemsToList(old_dir_info, old_dir);
            if (!new_load_from_file) UpdateNewList();
        }

        // унифицирует путь до текущей папки (делает обязательный слеш на конце)
        private string get_root_path(TextBox root_path)
        {
            if (root_path.Text == "") return "";
            string res = root_path.Text;
            if (root_path.Text.Last() != '\\')
            {
                res += "\\";
            }
            return res;
        }

        private void addItemsToList(DirectoryInfo dirInfo, string rootPath)
        {
            if (checkBox_subFolder.Checked)
            {
                foreach (DirectoryInfo childDirInfo in dirInfo.GetDirectories())
                {
                    addItemsToList(childDirInfo, rootPath);
                }
            }

            FileInfo[] files;

            try
            {
                files = dirInfo.GetFiles();
            } catch
            {
                return;
            }

            foreach (FileInfo fileInfo in files)
            { 
                string file_path = fileInfo.FullName.Remove(0, rootPath.Length);
                // игнорируем файлы
                
                if (ignore_list.Contains(fileInfo.Extension) ) { continue; }
                oldFilesList.Items.Add(file_path);
            }
        }

        private void UpdateNewList()
        {
            ListView.ListViewItemCollection items = oldFilesList.Items;
            newFilesList.Items.Clear();
            foreach (ListViewItem item in items)
            {
                string new_name = item.Text;
                ListViewItem cur_item = newFilesList.Items.Add(new_name);
                cur_item.Checked = true;
                //cur_item.BeginEdit();
            }
        }
        
        
        private void button2_Click(object sender, EventArgs e)
        {
            string old_path = "";
            if (!old_load_from_file) old_path = get_root_path(oldName_box);
            string cur_dir = project + "\\" + old_path;
            while (!Directory.Exists(cur_dir))
            {
                int lastSlash = cur_dir.LastIndexOf("\\");
                if (lastSlash != -1)
                {
                    cur_dir = cur_dir.Remove(cur_dir.LastIndexOf("\\"));
                }
                else
                {
                    // если такой директории нет (и по какой то причине отличается от project), то выходим из цикла явно существующей папкой project
                    cur_dir = project;
                }
            }

            folderBrowserDialog1.SelectedPath = cur_dir;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                old_load_from_file = false;
                oldName_box.Enabled = true;
                checkBox_subFolder.Enabled = true;
                string sel_path = folderBrowserDialog1.SelectedPath;
                if (sel_path != project)
                {
                    oldName_box.Text = sel_path.Remove(0, project.Length + 1);
                }
                UpdateOldList();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string new_path = "";
            if (!new_load_from_file) new_path = get_root_path(newName_box);
            string cur_dir = project + "\\" + new_path;
            // находим максимально близкую существующую директорию от указанной в newName_box
            while (!Directory.Exists(cur_dir))
            {
                int lastSlash = cur_dir.LastIndexOf("\\");
                if (lastSlash != -1)
                {
                    cur_dir = cur_dir.Remove(cur_dir.LastIndexOf("\\"));
                } else
                {
                    // если такой директории нет (и по какой то причине отличается от project), то выходим из цикла явно существующей папкой project
                    cur_dir = project;
                }
            }

            folderBrowserDialog1.SelectedPath = cur_dir;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (new_load_from_file)
                {
                    new_load_from_file = false;
                    newName_box.Enabled = true;
                    UpdateNewList();
                }
                if (folderBrowserDialog1.SelectedPath != project)
                {
                    newName_box.Text = folderBrowserDialog1.SelectedPath.Remove(0, project.Length + 1);
                }
                //if (oldFilesList.Items.Count > 0) UpdateNewList();
            }
        }

        private void oldName_box_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            if (textbox.Text == "") return;
            replace_slash(textbox);
            string temp_path = project + "\\" + oldName_box.Text;
            if (newName_box.Text != "")
            { 
                if ((oldName_box.Text.Last() == '\\') && (newName_box.Text.Last() != '\\'))
                {
                    newName_box.Text += '\\';
                } else
                { 
                    if ((oldName_box.Text.Last() != '\\') && (newName_box.Text.Last() == '\\'))
                    {
                        newName_box.Text = newName_box.Text.Remove(newName_box.Text.Length-1);
                    }
                }
            }
            if (Directory.Exists(temp_path)) UpdateOldList();
            
        }

        private void newName_box_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            if (textbox.Text == "") return;
            replace_slash(textbox);
            string temp_path = project + "\\" + newName_box.Text;
            //UpdateNewList();
        }

        private void replace_slash(TextBox sender)
        {
            sender.Text = sender.Text.Replace('/', '\\');
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label3.Text = "Complete";
            log_process("Moving complete.====================================\n\n");
            System.Media.SystemSounds.Exclamation.Play();
            if (!old_load_from_file) UpdateOldList();
            btn_Cancel.Enabled = false;
        }

        private void Sync_lists(ListView sender)
        {
            if ((newFilesList.Items.Count > 0) && (oldFilesList.Items.Count > 0))
            {
                int cur_top_index = sender.TopItem.Index;
                ListViewItem future_top_item;
                if (sender == oldFilesList)
                {
                    if (newFilesList.Items.Count >= cur_top_index)
                    {
                        future_top_item = newFilesList.Items[cur_top_index];
                        newFilesList.TopItem = future_top_item;
                    }
                }
                else
                {
                    if (oldFilesList.Items.Count >= cur_top_index)
                    {
                        future_top_item = oldFilesList.Items[cur_top_index];
                        oldFilesList.TopItem = future_top_item;
                    }
                }
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            Form active_form = Form.ActiveForm;
            if (active_form == null) return;
            Control control = active_form.ActiveControl;
            if (control == newFilesList)
            {
                Sync_lists(newFilesList);
            }
            else {
                Sync_lists(oldFilesList);
            }
        }

        private void newFilesList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2 && newFilesList.LabelEdit)
            {
                newFilesList.FocusedItem.BeginEdit();
            }
        }

        private void oldFilesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // при выборе файла в списке источников, его содержимое показывается в текстовом поле
            if (oldFilesList.FocusedItem == null) return;
            string selected_file = project + "\\" + get_root_path(oldName_box) + "\\" + oldFilesList.FocusedItem.Text;
            show_text_file(selected_file);
        }

        // показывает содержимое переданного файла
        private void show_text_file(string file)
        {
            if (File.Exists(file))
            {
                string file_text = File.ReadAllText(file);
                textBox1.Text = file_text;
            } else
            {
                textBox1.Text = "File is not found";
            }
            
        }

        private void btn_selAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in newFilesList.Items)
            {
                item.Checked = true;
            }
        }

        private void btn_selNone_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in newFilesList.Items)
            {
                item.Checked = false;
            }
        }

        private void btn_selinv_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in newFilesList.Items)
            {
                item.Checked = !item.Checked;
            }
        }

        private void checkBox_subFolder_CheckedChanged(object sender, EventArgs e)
        {
            UpdateOldList();
        }

        private void newFilesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // при выборе файла в списке источников, его содержимое показывается в текстовом поле
            if (newFilesList.FocusedItem == null) return;
            int selected_index = newFilesList.FocusedItem.Index;
            if (oldFilesList.Items.Count > selected_index)
            {
                string selected_file = project + "\\" + get_root_path(oldName_box) + "\\" + oldFilesList.Items[selected_index].Text;
                show_text_file(selected_file);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string user_state = e.UserState as string;
            string action;
            if (fix_after_rename)
            {
                action = "Checking ";
            } else
            {
                action = "Moving ";
            }
            label3.Text = action + user_state;
        }

        private void btn_oldLoadFromFile_Click(object sender, EventArgs e)
        {
            string file_to_load = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                file_to_load = openFileDialog1.FileName;
                old_load_from_file = true;
                oldName_box.Enabled = false;
                checkBox_subFolder.Enabled = false;
                oldName_box.Text = file_to_load;
                load_from_file(file_to_load, oldFilesList);
            }
        }

        private void load_from_file(string file, ListView targetList)
        {
            targetList.Items.Clear();
            IEnumerable<string> files_list = File.ReadLines(file);
            foreach (string line in files_list)
            {
                targetList.Items.Add(line);
            }
            if (!new_load_from_file) UpdateNewList();
        }

        private void btn_LoadNewFromFile_Click(object sender, EventArgs e)
        {
            string file_to_load = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                file_to_load = openFileDialog1.FileName;
                new_load_from_file = true;
                newName_box.Enabled = false;
                newName_box.Text = file_to_load;
                load_from_file(file_to_load, newFilesList);
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        public bool ShowCorrectionDialog(string old_file_path, string newFileContent, int startIndex, int Length)
        {
            is_dialog = true;
            corrected_file_name = null;
            bool is_canceled = false; // нужен чтобы определить была ли отменена замена в диалоге
            string broken_link = old_file_path.Remove(0, project.Length + 1);

            if (auto_fix)
            {
                if (correction_links_memory.ContainsKey(broken_link))
                {
                    corrected_file_name = project + "\\" + correction_links_memory[broken_link];
                    is_dialog = false;
                    is_canceled = false;
                    return is_canceled;
                }
            }

            
            this.Invoke((MethodInvoker)delegate ()
            {
                FixLinkDialog dialog = new FixLinkDialog(broken_link, newFileContent, startIndex, Length);
                System.Media.SystemSounds.Beep.Play();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    corrected_file_name = dialog.new_path;
                    is_dialog = false;
                    is_canceled = false;
                }
                else
                {
                    is_dialog = false;
                    is_canceled = true;
                }

            });
            return is_canceled;
        }

        private void AddFixToMemory(string broken_link, string fixed_link)
        {
            correction_links_memory[broken_link] = fixed_link;
            /*
            if (!corrected_links_list.Contains(fixed_link))
            {
                
            }
            */
        }

        private void updateFixLinkCheckBoxes(CheckBox justFixCheckBox)
        {
            bool state = justFixCheckBox.Checked;

            checkBox_FixLink_AfterRename.Enabled = state;
            if (state)
            {  //выбирается "только исправлять ссылки"
                btn_Rename.Text = "Fix Links";
                MakeNewListAsOld();
            } else
            {
                btn_Rename.Text = "Rename";
                UnlockNewList();
            }

        }

        private void MakeNewListAsOld()
        {
            newName_box.Text = oldName_box.Text;
            newName_box.Enabled = false;
            //newFilesList.Enabled = false;
            newFilesList.LabelEdit = false;
            btn_LoadNewFromFile.Enabled = false;
            UpdateNewList();
        }

        private void UnlockNewList()
        {
            newFilesList.Enabled = true;
            newName_box.Enabled = true;
            btn_LoadNewFromFile.Enabled = true;
        }

        private void checkBox_FixLink_CheckedChanged(object sender, EventArgs e)
        {
            updateFixLinkCheckBoxes((CheckBox)sender);
        }

        private void btn_Rename_Click(object sender, EventArgs e)
        {
            just_rename = checkBox_justRename.Checked;
            if (just_rename)
            {
                ExportListsAndRename();
                return;
            }
            if (checkBox_FixLink.Checked)
            {
                fix_after_rename = false;
                Start_Rename();
            }
            else
            {
                fix_after_rename = checkBox_FixLink_AfterRename.Checked;
                log_process("starting fixing links without moving");
                if (!backgroundWorker1.IsBusy)
                {
                    Start_Rename();
                    //backgroundWorker1.RunWorkerAsync();
                    btn_Cancel.Enabled = true;
                }
            }
        }

        private void checkBox_FixLink_AfterRename_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _sender = (CheckBox)sender;
            if (!_sender.Checked)
            {
                MakeNewListAsOld();
            }
            else
            {
                UnlockNewList();
            }
        }

        private void button_setDTD_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = project;
            openFileDialog1.FilterIndex = 4;
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox_dtdPath.Text = openFileDialog1.FileName.Remove(0, project.Length+1);
            }
        }

        private void button_setXSL_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = project;
            openFileDialog1.FilterIndex = 5;
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox_xslPath.Text = openFileDialog1.FileName.Remove(0, project.Length + 1);
            }
        }

        private void checkBox_justRename_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _sender = (CheckBox)sender;
            bool state = _sender.Checked;
            checkBox_FixLink.Enabled = !state;
            checkBox_FixLink_AfterRename.Enabled = !state;
            checkBox_IgnoreNotTemplateLinks.Enabled = !state;
            checkBox_IgnoreUpperCase.Enabled = !state;
            checkBox_MakeRefLink.Enabled = !state;
            checkBox_autoFix.Enabled = !state;
        }

        private void ExportListsAndRename()
        {
            string new_file_name;
            if (new_load_from_file)
            {
                new_file_name = newName_box.Text.Replace('.', '-') + "-NEW";
            } else
            {
                new_file_name = project + "\\" + oldName_box.Text.Replace('\\', '-') + "-NEW";
            }
            string new_file_base_name = new_file_name;
            string old_file_name;
            if (old_load_from_file)
            {
                old_file_name = oldName_box.Text.Replace('.', '-') + "-OLD";
            } else
            {
                old_file_name = project + "\\" + oldName_box.Text.Replace('\\', '-') + "-OLD";
            }
            string old_file_base_name = old_file_name;
            int postfix = 0;
            while (File.Exists(new_file_name+".orig") || File.Exists(old_file_name + ".orig"))
            {
                new_file_name = new_file_base_name + "-" + ++postfix;
                old_file_name = old_file_base_name + "-" + postfix;
            }
            new_file_name += ".orig";
            old_file_name += ".orig";

            string old_file_content = "";
            string new_file_content = "";

            string old_path = get_root_path(oldName_box);
            string new_path = get_root_path(newName_box);
            foreach (ListViewItem item in newFilesList.Items)
            {
                if (!item.Checked) continue;
                int index = item.Index;
                new_file_content += new_path + item.Text + "\r\n";
                ListViewItem old_item = oldFilesList.Items[index];
                old_file_content += old_path + old_item.Text + "\r\n";
            }
            File.WriteAllText(old_file_name, old_file_content);
            File.WriteAllText(new_file_name, new_file_content);
            /*
            using (StreamWriter old_names_file = new StreamWriter(old_file_name, false))
            {
                string old_path = get_root_path(oldName_box);
                foreach (ListViewItem item in oldFilesList.Items)
                {
                    
                    old_names_file.WriteLine(old_path + item.Text);
                }
                
            }
            using (StreamWriter new_names_file = new StreamWriter(new_file_name, false))
            {
                string new_path = get_root_path(newName_box);
                foreach (ListViewItem item in newFilesList.Items)
                {
                    new_names_file.WriteLine(new_path + item.Text);
                }
            }
            */
            Start_Rename();
            MessageBox.Show("Files are renaming.\r\nList of old files was saved to " + old_file_name + ".\r\nList of new files was saved to " + new_file_name + ".\r\n1) Commit renames\r\n2) Check both the 'Just fix links' and 'After rename' checkboxes to fix links.\r\n3) Import file linsts from the created files.");
        }

        private void newFilesList_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            /*
            ListViewItem new_item = e.Item;
            bool state = new_item.Checked;
            
            int index = new_item.Index;
            checkedItems[index] = state;
            ListViewItem old_item = oldFilesList.Items[index];

            if (state)
            {
                if (new_names_pool[index] != "")
                {
                    new_item.Text = new_names_pool[index];
                }
            } else
            {
                new_names_pool[index] = new_item.Text;
                new_item.Text = old_item.Text;
                new_item.ForeColor = Color.Gray;
                
            }
            */
        }

        private void checkBox_RenameUsingCL_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _sender = (CheckBox)sender;
            bool state = _sender.Checked;
            comboBoxSCS.Enabled = state;
            textBox_SCSPath.Enabled = state;
        }

        private void button_setSCSPath_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.ShowHelp = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox_SCSPath.Text = openFileDialog1.FileName;
            }
        }
    }
}
