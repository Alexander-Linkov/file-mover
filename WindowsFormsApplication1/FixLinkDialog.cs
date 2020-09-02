using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    
    public partial class FixLinkDialog : Form
    {
        public string new_path { get; set; }

        public FixLinkDialog(string broken_link, string file_content, int selection_start, int selection_length)
        {
            InitializeComponent(/*old_path, file_content, selection_start, selection_length*/);

            //string broken_link = old_path.Remove(0, Form1.project.Length + 1);
            textBox1.Text = broken_link;
            textBox2.Text = file_content;
            textBox2.SelectionStart = selection_start;
            textBox2.SelectionLength = selection_length;
            textBox2.SelectionBackColor = System.Drawing.Color.Yellow;

            List<string> memory_list = Form1.corrected_links_list;
            List<string> keys = new List<string>(Form1.correction_links_memory.Keys);
            keys.Sort();
            int sel_index = -1;
            
            // проходимся по памяти исправлений и заносим их в лист
            foreach (string br_link in keys)
            {
                //listBox1.Items.Add(link);
                string item = String.Format("{0} >>> {1}", br_link, Form1.correction_links_memory[br_link]);
                int cur_index = listBox1.Items.Add(item);

                // попутно находим индекс ссылки, если такое исправление уже было
                if (broken_link == br_link)
                {
                    sel_index = cur_index;
                }
            }

            // попробовать найти и выбрать существующий индекс?
            if (sel_index > -1)
            {
                listBox1.SelectedIndex = sel_index;
                listBox1.Focus();
            } else
            {
                textBox1.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowHelp = true;
            //openFileDialog1.InitialDirectory = Form1.project;
            string file_name = textBox1.Text.Remove(0, textBox1.Text.LastIndexOf('\\')+1);
            openFileDialog1.FileName = file_name;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            { 
                textBox1.Text = openFileDialog1.FileName.Remove(0, Form1.project.Length+1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.new_path = Form1.project + "\\" + textBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            string item = (string)listBox1.SelectedItem;
            item = item.Remove(0, item.LastIndexOf('>') + 2);
            textBox1.Text = item;
        }

        private void listBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox1.Text = (string)listBox1.SelectedItem;
            }
            button1.Focus();
        }

    }
}
