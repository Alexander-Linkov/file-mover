namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.oldName_box = new System.Windows.Forms.TextBox();
            this.newName_box = new System.Windows.Forms.TextBox();
            this.btn_Rename = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label3 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.newFilesList = new System.Windows.Forms.ListView();
            this.targetNames = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.oldFilesList = new System.Windows.Forms.ListView();
            this.Files_to_rename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn_selAll = new System.Windows.Forms.Button();
            this.btn_selNone = new System.Windows.Forms.Button();
            this.btn_selinv = new System.Windows.Forms.Button();
            this.checkBox_subFolder = new System.Windows.Forms.CheckBox();
            this.btn_oldLoadFromFile = new System.Windows.Forms.Button();
            this.btn_LoadNewFromFile = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox_autoFix = new System.Windows.Forms.CheckBox();
            this.checkBox_MakeRefLink = new System.Windows.Forms.CheckBox();
            this.checkBox_justRename = new System.Windows.Forms.CheckBox();
            this.checkBox_IgnoreNotTemplateLinks = new System.Windows.Forms.CheckBox();
            this.checkBox_IgnoreUpperCase = new System.Windows.Forms.CheckBox();
            this.checkBox_FixLink_AfterRename = new System.Windows.Forms.CheckBox();
            this.checkBox_FixLink = new System.Windows.Forms.CheckBox();
            this.textBox_dtdPath = new System.Windows.Forms.TextBox();
            this.textBox_xslPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button_setDTD = new System.Windows.Forms.Button();
            this.button_setXSL = new System.Windows.Forms.Button();
            this.textBox_SCSPath = new System.Windows.Forms.TextBox();
            this.checkBox_RenameUsingCL = new System.Windows.Forms.CheckBox();
            this.comboBoxSCS = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button_SetSCSPath = new System.Windows.Forms.Button();
            this.checkBox_DeleteUnused = new System.Windows.Forms.CheckBox();
            this.checkBox_RewriteIfExists = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // oldName_box
            // 
            this.oldName_box.Location = new System.Drawing.Point(12, 35);
            this.oldName_box.Name = "oldName_box";
            this.oldName_box.Size = new System.Drawing.Size(434, 20);
            this.oldName_box.TabIndex = 0;
            this.oldName_box.TextChanged += new System.EventHandler(this.oldName_box_TextChanged);
            this.oldName_box.Leave += new System.EventHandler(this.oldName_box_TextChanged);
            // 
            // newName_box
            // 
            this.newName_box.Location = new System.Drawing.Point(452, 35);
            this.newName_box.Name = "newName_box";
            this.newName_box.Size = new System.Drawing.Size(495, 20);
            this.newName_box.TabIndex = 1;
            this.newName_box.TextChanged += new System.EventHandler(this.newName_box_TextChanged);
            this.newName_box.Leave += new System.EventHandler(this.newName_box_TextChanged);
            // 
            // btn_Rename
            // 
            this.btn_Rename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Rename.Location = new System.Drawing.Point(161, 5);
            this.btn_Rename.Name = "btn_Rename";
            this.btn_Rename.Size = new System.Drawing.Size(72, 150);
            this.btn_Rename.TabIndex = 2;
            this.btn_Rename.Text = "Rename";
            this.btn_Rename.UseVisualStyleBackColor = true;
            this.btn_Rename.Click += new System.EventHandler(this.btn_Rename_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Source path";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(449, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Target path";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "XML files (*.xml)|*.xml|Image files (*.gif, *.png, *.jpg, *.bmp)|*.gif;*.png;*.jp" +
    "g;*.bmp|Text files (*.txt)|*.txt|DTD Files (*.dtd)|*.dtd|XSL Files (*.xsl)|*.xsl" +
    "|All files (*.*)|*.*";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(409, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(34, 20);
            this.button2.TabIndex = 5;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(912, 9);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(35, 20);
            this.button3.TabIndex = 6;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 916);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Ready";
            // 
            // newFilesList
            // 
            this.newFilesList.CheckBoxes = true;
            this.newFilesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.targetNames});
            this.newFilesList.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.newFilesList.LabelEdit = true;
            this.newFilesList.Location = new System.Drawing.Point(452, 62);
            this.newFilesList.MultiSelect = false;
            this.newFilesList.Name = "newFilesList";
            this.newFilesList.Size = new System.Drawing.Size(495, 454);
            this.newFilesList.TabIndex = 9;
            this.newFilesList.UseCompatibleStateImageBehavior = false;
            this.newFilesList.View = System.Windows.Forms.View.Details;
            this.newFilesList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.newFilesList_ItemChecked);
            this.newFilesList.SelectedIndexChanged += new System.EventHandler(this.newFilesList_SelectedIndexChanged);
            this.newFilesList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.newFilesList_KeyUp);
            // 
            // targetNames
            // 
            this.targetNames.Text = "Target Names";
            this.targetNames.Width = 600;
            // 
            // oldFilesList
            // 
            this.oldFilesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Files_to_rename});
            this.oldFilesList.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.oldFilesList.Location = new System.Drawing.Point(12, 62);
            this.oldFilesList.MultiSelect = false;
            this.oldFilesList.Name = "oldFilesList";
            this.oldFilesList.Size = new System.Drawing.Size(434, 454);
            this.oldFilesList.TabIndex = 10;
            this.oldFilesList.UseCompatibleStateImageBehavior = false;
            this.oldFilesList.View = System.Windows.Forms.View.Details;
            this.oldFilesList.SelectedIndexChanged += new System.EventHandler(this.oldFilesList_SelectedIndexChanged);
            // 
            // Files_to_rename
            // 
            this.Files_to_rename.Text = "Source Files";
            this.Files_to_rename.Width = 600;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 706);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(935, 202);
            this.textBox1.TabIndex = 11;
            // 
            // btn_selAll
            // 
            this.btn_selAll.Location = new System.Drawing.Point(452, 522);
            this.btn_selAll.Name = "btn_selAll";
            this.btn_selAll.Size = new System.Drawing.Size(68, 25);
            this.btn_selAll.TabIndex = 12;
            this.btn_selAll.Text = "Select All";
            this.btn_selAll.UseVisualStyleBackColor = true;
            this.btn_selAll.Click += new System.EventHandler(this.btn_selAll_Click);
            // 
            // btn_selNone
            // 
            this.btn_selNone.Location = new System.Drawing.Point(526, 524);
            this.btn_selNone.Name = "btn_selNone";
            this.btn_selNone.Size = new System.Drawing.Size(75, 23);
            this.btn_selNone.TabIndex = 13;
            this.btn_selNone.Text = "Select None";
            this.btn_selNone.UseVisualStyleBackColor = true;
            this.btn_selNone.Click += new System.EventHandler(this.btn_selNone_Click);
            // 
            // btn_selinv
            // 
            this.btn_selinv.Location = new System.Drawing.Point(607, 524);
            this.btn_selinv.Name = "btn_selinv";
            this.btn_selinv.Size = new System.Drawing.Size(93, 23);
            this.btn_selinv.TabIndex = 14;
            this.btn_selinv.Text = "Invert Selection";
            this.btn_selinv.UseVisualStyleBackColor = true;
            this.btn_selinv.Click += new System.EventHandler(this.btn_selinv_Click);
            // 
            // checkBox_subFolder
            // 
            this.checkBox_subFolder.AutoSize = true;
            this.checkBox_subFolder.Checked = true;
            this.checkBox_subFolder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_subFolder.Location = new System.Drawing.Point(280, 9);
            this.checkBox_subFolder.Name = "checkBox_subFolder";
            this.checkBox_subFolder.Size = new System.Drawing.Size(114, 17);
            this.checkBox_subFolder.TabIndex = 15;
            this.checkBox_subFolder.Text = "include sub-folders";
            this.checkBox_subFolder.UseVisualStyleBackColor = true;
            this.checkBox_subFolder.CheckedChanged += new System.EventHandler(this.checkBox_subFolder_CheckedChanged);
            // 
            // btn_oldLoadFromFile
            // 
            this.btn_oldLoadFromFile.Location = new System.Drawing.Point(83, 8);
            this.btn_oldLoadFromFile.Name = "btn_oldLoadFromFile";
            this.btn_oldLoadFromFile.Size = new System.Drawing.Size(107, 23);
            this.btn_oldLoadFromFile.TabIndex = 16;
            this.btn_oldLoadFromFile.Text = "Load from file";
            this.btn_oldLoadFromFile.UseVisualStyleBackColor = true;
            this.btn_oldLoadFromFile.Click += new System.EventHandler(this.btn_oldLoadFromFile_Click);
            // 
            // btn_LoadNewFromFile
            // 
            this.btn_LoadNewFromFile.Location = new System.Drawing.Point(517, 8);
            this.btn_LoadNewFromFile.Name = "btn_LoadNewFromFile";
            this.btn_LoadNewFromFile.Size = new System.Drawing.Size(107, 23);
            this.btn_LoadNewFromFile.TabIndex = 17;
            this.btn_LoadNewFromFile.Text = "Load from file";
            this.btn_LoadNewFromFile.UseVisualStyleBackColor = true;
            this.btn_LoadNewFromFile.Click += new System.EventHandler(this.btn_LoadNewFromFile_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Enabled = false;
            this.btn_Cancel.Location = new System.Drawing.Point(872, 911);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 19;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox_autoFix);
            this.panel1.Controls.Add(this.checkBox_MakeRefLink);
            this.panel1.Controls.Add(this.checkBox_justRename);
            this.panel1.Controls.Add(this.btn_Rename);
            this.panel1.Controls.Add(this.checkBox_IgnoreNotTemplateLinks);
            this.panel1.Controls.Add(this.checkBox_IgnoreUpperCase);
            this.panel1.Controls.Add(this.checkBox_FixLink_AfterRename);
            this.panel1.Controls.Add(this.checkBox_FixLink);
            this.panel1.Location = new System.Drawing.Point(711, 522);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(236, 164);
            this.panel1.TabIndex = 20;
            // 
            // checkBox_autoFix
            // 
            this.checkBox_autoFix.AutoSize = true;
            this.checkBox_autoFix.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_autoFix.Location = new System.Drawing.Point(14, 138);
            this.checkBox_autoFix.Name = "checkBox_autoFix";
            this.checkBox_autoFix.Size = new System.Drawing.Size(141, 17);
            this.checkBox_autoFix.TabIndex = 9;
            this.checkBox_autoFix.Text = "Fix Links When Possible";
            this.checkBox_autoFix.UseVisualStyleBackColor = true;
            // 
            // checkBox_MakeRefLink
            // 
            this.checkBox_MakeRefLink.AutoSize = true;
            this.checkBox_MakeRefLink.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_MakeRefLink.Location = new System.Drawing.Point(18, 115);
            this.checkBox_MakeRefLink.Name = "checkBox_MakeRefLink";
            this.checkBox_MakeRefLink.Size = new System.Drawing.Size(137, 17);
            this.checkBox_MakeRefLink.TabIndex = 8;
            this.checkBox_MakeRefLink.Text = "Make All Links Relative";
            this.checkBox_MakeRefLink.UseVisualStyleBackColor = true;
            // 
            // checkBox_justRename
            // 
            this.checkBox_justRename.AutoSize = true;
            this.checkBox_justRename.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_justRename.Location = new System.Drawing.Point(67, 5);
            this.checkBox_justRename.Name = "checkBox_justRename";
            this.checkBox_justRename.Size = new System.Drawing.Size(88, 17);
            this.checkBox_justRename.TabIndex = 7;
            this.checkBox_justRename.Text = "Just Rename";
            this.checkBox_justRename.UseVisualStyleBackColor = true;
            this.checkBox_justRename.CheckedChanged += new System.EventHandler(this.checkBox_justRename_CheckedChanged);
            // 
            // checkBox_IgnoreNotTemplateLinks
            // 
            this.checkBox_IgnoreNotTemplateLinks.AutoSize = true;
            this.checkBox_IgnoreNotTemplateLinks.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_IgnoreNotTemplateLinks.Location = new System.Drawing.Point(33, 71);
            this.checkBox_IgnoreNotTemplateLinks.Name = "checkBox_IgnoreNotTemplateLinks";
            this.checkBox_IgnoreNotTemplateLinks.Size = new System.Drawing.Size(122, 17);
            this.checkBox_IgnoreNotTemplateLinks.TabIndex = 5;
            this.checkBox_IgnoreNotTemplateLinks.Text = "Only Template Links";
            this.checkBox_IgnoreNotTemplateLinks.UseVisualStyleBackColor = true;
            // 
            // checkBox_IgnoreUpperCase
            // 
            this.checkBox_IgnoreUpperCase.AutoSize = true;
            this.checkBox_IgnoreUpperCase.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_IgnoreUpperCase.Location = new System.Drawing.Point(12, 93);
            this.checkBox_IgnoreUpperCase.Name = "checkBox_IgnoreUpperCase";
            this.checkBox_IgnoreUpperCase.Size = new System.Drawing.Size(143, 17);
            this.checkBox_IgnoreUpperCase.TabIndex = 6;
            this.checkBox_IgnoreUpperCase.Text = "Ignore Upper Case Links";
            this.checkBox_IgnoreUpperCase.UseVisualStyleBackColor = true;
            // 
            // checkBox_FixLink_AfterRename
            // 
            this.checkBox_FixLink_AfterRename.AutoSize = true;
            this.checkBox_FixLink_AfterRename.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_FixLink_AfterRename.Enabled = false;
            this.checkBox_FixLink_AfterRename.Location = new System.Drawing.Point(64, 49);
            this.checkBox_FixLink_AfterRename.Name = "checkBox_FixLink_AfterRename";
            this.checkBox_FixLink_AfterRename.Size = new System.Drawing.Size(91, 17);
            this.checkBox_FixLink_AfterRename.TabIndex = 4;
            this.checkBox_FixLink_AfterRename.Text = "After Rename";
            this.checkBox_FixLink_AfterRename.UseVisualStyleBackColor = true;
            this.checkBox_FixLink_AfterRename.CheckedChanged += new System.EventHandler(this.checkBox_FixLink_AfterRename_CheckedChanged);
            // 
            // checkBox_FixLink
            // 
            this.checkBox_FixLink.AutoSize = true;
            this.checkBox_FixLink.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_FixLink.Location = new System.Drawing.Point(66, 26);
            this.checkBox_FixLink.Name = "checkBox_FixLink";
            this.checkBox_FixLink.Size = new System.Drawing.Size(89, 17);
            this.checkBox_FixLink.TabIndex = 3;
            this.checkBox_FixLink.Text = "Just Fix Links";
            this.checkBox_FixLink.UseVisualStyleBackColor = true;
            this.checkBox_FixLink.CheckedChanged += new System.EventHandler(this.checkBox_FixLink_CheckedChanged);
            // 
            // textBox_dtdPath
            // 
            this.textBox_dtdPath.Location = new System.Drawing.Point(48, 527);
            this.textBox_dtdPath.Name = "textBox_dtdPath";
            this.textBox_dtdPath.Size = new System.Drawing.Size(373, 20);
            this.textBox_dtdPath.TabIndex = 21;
            this.textBox_dtdPath.Text = "_params\\sbScheme.dtd";
            // 
            // textBox_xslPath
            // 
            this.textBox_xslPath.Location = new System.Drawing.Point(48, 552);
            this.textBox_xslPath.Name = "textBox_xslPath";
            this.textBox_xslPath.Size = new System.Drawing.Size(373, 20);
            this.textBox_xslPath.TabIndex = 22;
            this.textBox_xslPath.Text = "_params\\topic2html-product-specific-production.xsl";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 529);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "DTD:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 555);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "XSL:";
            // 
            // button_setDTD
            // 
            this.button_setDTD.Location = new System.Drawing.Point(420, 527);
            this.button_setDTD.Name = "button_setDTD";
            this.button_setDTD.Size = new System.Drawing.Size(26, 20);
            this.button_setDTD.TabIndex = 25;
            this.button_setDTD.Text = "...";
            this.button_setDTD.UseVisualStyleBackColor = true;
            this.button_setDTD.Click += new System.EventHandler(this.button_setDTD_Click);
            // 
            // button_setXSL
            // 
            this.button_setXSL.Location = new System.Drawing.Point(420, 552);
            this.button_setXSL.Name = "button_setXSL";
            this.button_setXSL.Size = new System.Drawing.Size(26, 20);
            this.button_setXSL.TabIndex = 26;
            this.button_setXSL.Text = "...";
            this.button_setXSL.UseVisualStyleBackColor = true;
            this.button_setXSL.Click += new System.EventHandler(this.button_setXSL_Click);
            // 
            // textBox_SCSPath
            // 
            this.textBox_SCSPath.Enabled = false;
            this.textBox_SCSPath.Location = new System.Drawing.Point(155, 601);
            this.textBox_SCSPath.Name = "textBox_SCSPath";
            this.textBox_SCSPath.Size = new System.Drawing.Size(266, 20);
            this.textBox_SCSPath.TabIndex = 27;
            this.textBox_SCSPath.Text = "C:\\Program Files\\Git\\bin\\git.exe";
            // 
            // checkBox_RenameUsingCL
            // 
            this.checkBox_RenameUsingCL.AutoSize = true;
            this.checkBox_RenameUsingCL.Location = new System.Drawing.Point(15, 578);
            this.checkBox_RenameUsingCL.Name = "checkBox_RenameUsingCL";
            this.checkBox_RenameUsingCL.Size = new System.Drawing.Size(113, 17);
            this.checkBox_RenameUsingCL.TabIndex = 28;
            this.checkBox_RenameUsingCL.Text = "Use command line";
            this.checkBox_RenameUsingCL.UseVisualStyleBackColor = true;
            this.checkBox_RenameUsingCL.CheckedChanged += new System.EventHandler(this.checkBox_RenameUsingCL_CheckedChanged);
            // 
            // comboBoxSCS
            // 
            this.comboBoxSCS.Enabled = false;
            this.comboBoxSCS.FormattingEnabled = true;
            this.comboBoxSCS.Items.AddRange(new object[] {
            "Git"});
            this.comboBoxSCS.Location = new System.Drawing.Point(12, 601);
            this.comboBoxSCS.Name = "comboBoxSCS";
            this.comboBoxSCS.Size = new System.Drawing.Size(65, 21);
            this.comboBoxSCS.TabIndex = 29;
            this.comboBoxSCS.Text = "Git";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(84, 604);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "Path to EXE:";
            // 
            // button_SetSCSPath
            // 
            this.button_SetSCSPath.Location = new System.Drawing.Point(420, 601);
            this.button_SetSCSPath.Name = "button_SetSCSPath";
            this.button_SetSCSPath.Size = new System.Drawing.Size(26, 20);
            this.button_SetSCSPath.TabIndex = 31;
            this.button_SetSCSPath.Text = "...";
            this.button_SetSCSPath.UseVisualStyleBackColor = true;
            this.button_SetSCSPath.Click += new System.EventHandler(this.button_setSCSPath_Click);
            // 
            // checkBox_DeleteUnused
            // 
            this.checkBox_DeleteUnused.AutoSize = true;
            this.checkBox_DeleteUnused.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_DeleteUnused.Location = new System.Drawing.Point(581, 660);
            this.checkBox_DeleteUnused.Name = "checkBox_DeleteUnused";
            this.checkBox_DeleteUnused.Size = new System.Drawing.Size(119, 17);
            this.checkBox_DeleteUnused.TabIndex = 32;
            this.checkBox_DeleteUnused.Text = "Delete Unused File ";
            this.checkBox_DeleteUnused.UseVisualStyleBackColor = true;
            // 
            // checkBox_RewriteIfExists
            // 
            this.checkBox_RewriteIfExists.AutoSize = true;
            this.checkBox_RewriteIfExists.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_RewriteIfExists.Location = new System.Drawing.Point(595, 637);
            this.checkBox_RewriteIfExists.Name = "checkBox_RewriteIfExists";
            this.checkBox_RewriteIfExists.Size = new System.Drawing.Size(101, 17);
            this.checkBox_RewriteIfExists.TabIndex = 33;
            this.checkBox_RewriteIfExists.Text = "Rewrite If Exists";
            this.checkBox_RewriteIfExists.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 938);
            this.Controls.Add(this.checkBox_RewriteIfExists);
            this.Controls.Add(this.checkBox_DeleteUnused);
            this.Controls.Add(this.button_SetSCSPath);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBoxSCS);
            this.Controls.Add(this.checkBox_RenameUsingCL);
            this.Controls.Add(this.textBox_SCSPath);
            this.Controls.Add(this.button_setXSL);
            this.Controls.Add(this.button_setDTD);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_xslPath);
            this.Controls.Add(this.textBox_dtdPath);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_LoadNewFromFile);
            this.Controls.Add(this.btn_oldLoadFromFile);
            this.Controls.Add(this.checkBox_subFolder);
            this.Controls.Add(this.btn_selinv);
            this.Controls.Add(this.btn_selNone);
            this.Controls.Add(this.btn_selAll);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.oldFilesList);
            this.Controls.Add(this.newFilesList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.newName_box);
            this.Controls.Add(this.oldName_box);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "FileMover";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox oldName_box;
        private System.Windows.Forms.TextBox newName_box;
        private System.Windows.Forms.Button btn_Rename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ListView newFilesList;
        private System.Windows.Forms.ListView oldFilesList;
        private System.Windows.Forms.ColumnHeader targetNames;
        private System.Windows.Forms.ColumnHeader Files_to_rename;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_selAll;
        private System.Windows.Forms.Button btn_selNone;
        private System.Windows.Forms.Button btn_selinv;
        private System.Windows.Forms.CheckBox checkBox_subFolder;
        private System.Windows.Forms.Button btn_oldLoadFromFile;
        private System.Windows.Forms.Button btn_LoadNewFromFile;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBox_IgnoreNotTemplateLinks;
        private System.Windows.Forms.CheckBox checkBox_FixLink_AfterRename;
        private System.Windows.Forms.CheckBox checkBox_FixLink;
        private System.Windows.Forms.TextBox textBox_dtdPath;
        private System.Windows.Forms.TextBox textBox_xslPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_setDTD;
        private System.Windows.Forms.Button button_setXSL;
        private System.Windows.Forms.CheckBox checkBox_IgnoreUpperCase;
        private System.Windows.Forms.CheckBox checkBox_justRename;
        private System.Windows.Forms.TextBox textBox_SCSPath;
        private System.Windows.Forms.CheckBox checkBox_RenameUsingCL;
        private System.Windows.Forms.ComboBox comboBoxSCS;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_SetSCSPath;
        private System.Windows.Forms.CheckBox checkBox_MakeRefLink;
        private System.Windows.Forms.CheckBox checkBox_autoFix;
        private System.Windows.Forms.CheckBox checkBox_DeleteUnused;
        private System.Windows.Forms.CheckBox checkBox_RewriteIfExists;
    }

}

