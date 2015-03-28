namespace Backup_2
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
        	this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        	this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.newBackupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.openBackupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
        	this.updateBackupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.backupPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.listModifiedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.clearLogTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.tabControl1 = new System.Windows.Forms.TabControl();
        	this.tabPage1 = new System.Windows.Forms.TabPage();
        	this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        	this.panel1 = new System.Windows.Forms.Panel();
        	this.BTNbackupremove = new System.Windows.Forms.Button();
        	this.BTNrestore = new System.Windows.Forms.Button();
        	this.label1 = new System.Windows.Forms.Label();
        	this.COMBbackupjob = new System.Windows.Forms.ComboBox();
        	this.BTNupdate = new System.Windows.Forms.Button();
        	this.BTNnew = new System.Windows.Forms.Button();
        	this.LSBbackup = new System.Windows.Forms.ListBox();
        	this.tabPage2 = new System.Windows.Forms.TabPage();
        	this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
        	this.panel2 = new System.Windows.Forms.Panel();
        	this.CHBtimestamp = new System.Windows.Forms.CheckBox();
        	this.RADsynceq = new System.Windows.Forms.RadioButton();
        	this.RADsyncmirror = new System.Windows.Forms.RadioButton();
        	this.RADsynccopy = new System.Windows.Forms.RadioButton();
        	this.BTNsyncremove = new System.Windows.Forms.Button();
        	this.label2 = new System.Windows.Forms.Label();
        	this.COMBsyncjob = new System.Windows.Forms.ComboBox();
        	this.BTNupdatesync = new System.Windows.Forms.Button();
        	this.BTNsynnew = new System.Windows.Forms.Button();
        	this.LSsync = new System.Windows.Forms.ListView();
        	this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
        	this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
        	this.tabPage3 = new System.Windows.Forms.TabPage();
        	this.TXTlogscreen = new System.Windows.Forms.TextBox();
        	this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
        	this.statusStrip1 = new System.Windows.Forms.StatusStrip();
        	this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
        	this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
        	this.clearLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.menuStrip1.SuspendLayout();
        	this.tabControl1.SuspendLayout();
        	this.tabPage1.SuspendLayout();
        	this.tableLayoutPanel1.SuspendLayout();
        	this.panel1.SuspendLayout();
        	this.tabPage2.SuspendLayout();
        	this.tableLayoutPanel2.SuspendLayout();
        	this.panel2.SuspendLayout();
        	this.tabPage3.SuspendLayout();
        	this.tableLayoutPanel3.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// menuStrip1
        	// 
        	this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.fileToolStripMenuItem,
        	        	        	this.toolsToolStripMenuItem,
        	        	        	this.logToolStripMenuItem});
        	this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        	this.menuStrip1.Name = "menuStrip1";
        	this.menuStrip1.Size = new System.Drawing.Size(914, 24);
        	this.menuStrip1.TabIndex = 0;
        	this.menuStrip1.Text = "menuStrip1";
        	// 
        	// fileToolStripMenuItem
        	// 
        	this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.newBackupToolStripMenuItem,
        	        	        	this.openBackupToolStripMenuItem,
        	        	        	this.toolStripMenuItem1,
        	        	        	this.updateBackupToolStripMenuItem});
        	this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        	this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
        	this.fileToolStripMenuItem.Text = "&File";
        	// 
        	// newBackupToolStripMenuItem
        	// 
        	this.newBackupToolStripMenuItem.Name = "newBackupToolStripMenuItem";
        	this.newBackupToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        	this.newBackupToolStripMenuItem.Text = "&New Backup";
        	this.newBackupToolStripMenuItem.Click += new System.EventHandler(this.BTNnew_Click);
        	// 
        	// openBackupToolStripMenuItem
        	// 
        	this.openBackupToolStripMenuItem.Name = "openBackupToolStripMenuItem";
        	this.openBackupToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        	this.openBackupToolStripMenuItem.Text = "&Open Backup";
        	this.openBackupToolStripMenuItem.Click += new System.EventHandler(this.openBackupToolStripMenuItem_Click);
        	// 
        	// toolStripMenuItem1
        	// 
        	this.toolStripMenuItem1.Name = "toolStripMenuItem1";
        	this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
        	// 
        	// updateBackupToolStripMenuItem
        	// 
        	this.updateBackupToolStripMenuItem.Name = "updateBackupToolStripMenuItem";
        	this.updateBackupToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        	this.updateBackupToolStripMenuItem.Text = "&Update Backup";
        	this.updateBackupToolStripMenuItem.Click += new System.EventHandler(this.BTNupdate_Click);
        	// 
        	// toolsToolStripMenuItem
        	// 
        	this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.backupPropertiesToolStripMenuItem,
        	        	        	this.listModifiedToolStripMenuItem});
        	this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
        	this.toolsToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
        	this.toolsToolStripMenuItem.Text = "&Tools";
        	// 
        	// backupPropertiesToolStripMenuItem
        	// 
        	this.backupPropertiesToolStripMenuItem.Name = "backupPropertiesToolStripMenuItem";
        	this.backupPropertiesToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
        	this.backupPropertiesToolStripMenuItem.Text = "&Backup Properties";
        	this.backupPropertiesToolStripMenuItem.Click += new System.EventHandler(this.backupPropertiesToolStripMenuItem_Click);
        	// 
        	// listModifiedToolStripMenuItem
        	// 
        	this.listModifiedToolStripMenuItem.Name = "listModifiedToolStripMenuItem";
        	this.listModifiedToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
        	this.listModifiedToolStripMenuItem.Text = "List Modified";
        	this.listModifiedToolStripMenuItem.Click += new System.EventHandler(this.listModifiedToolStripMenuItem_Click);
        	// 
        	// logToolStripMenuItem
        	// 
        	this.logToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.clearLogTabToolStripMenuItem,
        	        	        	this.clearLogFileToolStripMenuItem});
        	this.logToolStripMenuItem.Name = "logToolStripMenuItem";
        	this.logToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
        	this.logToolStripMenuItem.Text = "&Log";
        	// 
        	// clearLogTabToolStripMenuItem
        	// 
        	this.clearLogTabToolStripMenuItem.Name = "clearLogTabToolStripMenuItem";
        	this.clearLogTabToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        	this.clearLogTabToolStripMenuItem.Text = "&Clear Log Tab";
        	this.clearLogTabToolStripMenuItem.Click += new System.EventHandler(this.clearLogTabToolStripMenuItem_Click);
        	// 
        	// tabControl1
        	// 
        	this.tabControl1.Controls.Add(this.tabPage1);
        	this.tabControl1.Controls.Add(this.tabPage2);
        	this.tabControl1.Controls.Add(this.tabPage3);
        	this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.tabControl1.Location = new System.Drawing.Point(3, 3);
        	this.tabControl1.Name = "tabControl1";
        	this.tabControl1.SelectedIndex = 0;
        	this.tabControl1.Size = new System.Drawing.Size(908, 409);
        	this.tabControl1.TabIndex = 1;
        	// 
        	// tabPage1
        	// 
        	this.tabPage1.Controls.Add(this.tableLayoutPanel1);
        	this.tabPage1.Location = new System.Drawing.Point(4, 22);
        	this.tabPage1.Name = "tabPage1";
        	this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
        	this.tabPage1.Size = new System.Drawing.Size(900, 383);
        	this.tabPage1.TabIndex = 0;
        	this.tabPage1.Text = "Backup";
        	this.tabPage1.UseVisualStyleBackColor = true;
        	// 
        	// tableLayoutPanel1
        	// 
        	this.tableLayoutPanel1.ColumnCount = 1;
        	this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        	this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
        	this.tableLayoutPanel1.Controls.Add(this.LSBbackup, 0, 1);
        	this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
        	this.tableLayoutPanel1.Name = "tableLayoutPanel1";
        	this.tableLayoutPanel1.RowCount = 2;
        	this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
        	this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        	this.tableLayoutPanel1.Size = new System.Drawing.Size(894, 377);
        	this.tableLayoutPanel1.TabIndex = 0;
        	// 
        	// panel1
        	// 
        	this.panel1.Controls.Add(this.BTNbackupremove);
        	this.panel1.Controls.Add(this.BTNrestore);
        	this.panel1.Controls.Add(this.label1);
        	this.panel1.Controls.Add(this.COMBbackupjob);
        	this.panel1.Controls.Add(this.BTNupdate);
        	this.panel1.Controls.Add(this.BTNnew);
        	this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.panel1.Location = new System.Drawing.Point(3, 3);
        	this.panel1.Name = "panel1";
        	this.panel1.Size = new System.Drawing.Size(888, 54);
        	this.panel1.TabIndex = 0;
        	// 
        	// BTNbackupremove
        	// 
        	this.BTNbackupremove.Image = global::Backup_2.Properties.Resources.remove;
        	this.BTNbackupremove.Location = new System.Drawing.Point(312, 20);
        	this.BTNbackupremove.Name = "BTNbackupremove";
        	this.BTNbackupremove.Size = new System.Drawing.Size(28, 23);
        	this.BTNbackupremove.TabIndex = 5;
        	this.toolTip1.SetToolTip(this.BTNbackupremove, "Remove Backup");
        	this.BTNbackupremove.UseVisualStyleBackColor = true;
        	this.BTNbackupremove.Click += new System.EventHandler(this.BTNbackupremove_Click);
        	// 
        	// BTNrestore
        	// 
        	this.BTNrestore.Location = new System.Drawing.Point(347, 3);
        	this.BTNrestore.Name = "BTNrestore";
        	this.BTNrestore.Size = new System.Drawing.Size(54, 40);
        	this.BTNrestore.TabIndex = 4;
        	this.BTNrestore.Text = "Restore";
        	this.BTNrestore.UseVisualStyleBackColor = true;
        	this.BTNrestore.Click += new System.EventHandler(this.BTNrestore_Click);
        	// 
        	// label1
        	// 
        	this.label1.AutoSize = true;
        	this.label1.Location = new System.Drawing.Point(135, 4);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(66, 13);
        	this.label1.TabIndex = 3;
        	this.label1.Text = "Backup jobs";
        	// 
        	// COMBbackupjob
        	// 
        	this.COMBbackupjob.FormattingEnabled = true;
        	this.COMBbackupjob.Location = new System.Drawing.Point(135, 21);
        	this.COMBbackupjob.Name = "COMBbackupjob";
        	this.COMBbackupjob.Size = new System.Drawing.Size(171, 21);
        	this.COMBbackupjob.TabIndex = 2;
        	this.toolTip1.SetToolTip(this.COMBbackupjob, "Open Backup");
        	this.COMBbackupjob.SelectedIndexChanged += new System.EventHandler(this.COMBbackupjobbs_SelectedIndexChanged);
        	// 
        	// BTNupdate
        	// 
        	this.BTNupdate.BackgroundImage = global::Backup_2.Properties.Resources.Update1;
        	this.BTNupdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
        	this.BTNupdate.Location = new System.Drawing.Point(62, 4);
        	this.BTNupdate.Name = "BTNupdate";
        	this.BTNupdate.Size = new System.Drawing.Size(51, 40);
        	this.BTNupdate.TabIndex = 1;
        	this.toolTip1.SetToolTip(this.BTNupdate, "Update Backup");
        	this.BTNupdate.UseVisualStyleBackColor = true;
        	this.BTNupdate.Click += new System.EventHandler(this.BTNupdate_Click);
        	// 
        	// BTNnew
        	// 
        	this.BTNnew.BackgroundImage = global::Backup_2.Properties.Resources.NewBackup;
        	this.BTNnew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
        	this.BTNnew.Location = new System.Drawing.Point(3, 4);
        	this.BTNnew.Name = "BTNnew";
        	this.BTNnew.Size = new System.Drawing.Size(52, 40);
        	this.BTNnew.TabIndex = 0;
        	this.toolTip1.SetToolTip(this.BTNnew, "New Backup");
        	this.BTNnew.UseVisualStyleBackColor = true;
        	this.BTNnew.Click += new System.EventHandler(this.BTNnew_Click);
        	// 
        	// LSBbackup
        	// 
        	this.LSBbackup.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.LSBbackup.FormattingEnabled = true;
        	this.LSBbackup.Location = new System.Drawing.Point(3, 63);
        	this.LSBbackup.Name = "LSBbackup";
        	this.LSBbackup.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
        	this.LSBbackup.Size = new System.Drawing.Size(888, 311);
        	this.LSBbackup.TabIndex = 1;
        	// 
        	// tabPage2
        	// 
        	this.tabPage2.Controls.Add(this.tableLayoutPanel2);
        	this.tabPage2.Location = new System.Drawing.Point(4, 22);
        	this.tabPage2.Name = "tabPage2";
        	this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
        	this.tabPage2.Size = new System.Drawing.Size(900, 383);
        	this.tabPage2.TabIndex = 1;
        	this.tabPage2.Text = "Sync";
        	this.tabPage2.UseVisualStyleBackColor = true;
        	// 
        	// tableLayoutPanel2
        	// 
        	this.tableLayoutPanel2.ColumnCount = 1;
        	this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        	this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 0);
        	this.tableLayoutPanel2.Controls.Add(this.LSsync, 0, 1);
        	this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
        	this.tableLayoutPanel2.Name = "tableLayoutPanel2";
        	this.tableLayoutPanel2.RowCount = 2;
        	this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
        	this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        	this.tableLayoutPanel2.Size = new System.Drawing.Size(894, 377);
        	this.tableLayoutPanel2.TabIndex = 1;
        	// 
        	// panel2
        	// 
        	this.panel2.Controls.Add(this.CHBtimestamp);
        	this.panel2.Controls.Add(this.RADsynceq);
        	this.panel2.Controls.Add(this.RADsyncmirror);
        	this.panel2.Controls.Add(this.RADsynccopy);
        	this.panel2.Controls.Add(this.BTNsyncremove);
        	this.panel2.Controls.Add(this.label2);
        	this.panel2.Controls.Add(this.COMBsyncjob);
        	this.panel2.Controls.Add(this.BTNupdatesync);
        	this.panel2.Controls.Add(this.BTNsynnew);
        	this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.panel2.Location = new System.Drawing.Point(3, 3);
        	this.panel2.Name = "panel2";
        	this.panel2.Size = new System.Drawing.Size(888, 54);
        	this.panel2.TabIndex = 0;
        	// 
        	// CHBtimestamp
        	// 
        	this.CHBtimestamp.AutoSize = true;
        	this.CHBtimestamp.Location = new System.Drawing.Point(706, 4);
        	this.CHBtimestamp.Name = "CHBtimestamp";
        	this.CHBtimestamp.Size = new System.Drawing.Size(101, 17);
        	this.CHBtimestamp.TabIndex = 10;
        	this.CHBtimestamp.Text = "Keep timestamp";
        	this.CHBtimestamp.UseVisualStyleBackColor = true;
        	// 
        	// RADsynceq
        	// 
        	this.RADsynceq.AutoSize = true;
        	this.RADsynceq.Location = new System.Drawing.Point(424, 4);
        	this.RADsynceq.Name = "RADsynceq";
        	this.RADsynceq.Size = new System.Drawing.Size(65, 17);
        	this.RADsynceq.TabIndex = 9;
        	this.RADsynceq.Text = "Equalise";
        	this.toolTip1.SetToolTip(this.RADsynceq, "If a file is in both directories, copy the newer to the older; if it’s in only on" +
        	        	"e, copy it to the other. (This ensures that both directories have the newest pos" +
        	        	"sible files)");
        	this.RADsynceq.UseVisualStyleBackColor = true;
        	// 
        	// RADsyncmirror
        	// 
        	this.RADsyncmirror.AutoSize = true;
        	this.RADsyncmirror.Location = new System.Drawing.Point(367, 24);
        	this.RADsyncmirror.Name = "RADsyncmirror";
        	this.RADsyncmirror.Size = new System.Drawing.Size(51, 17);
        	this.RADsyncmirror.TabIndex = 8;
        	this.RADsyncmirror.Text = "Mirror";
        	this.toolTip1.SetToolTip(this.RADsyncmirror, "Copy every file in the source directory to the destination directory, and delete " +
        	        	"every file in the destination directory that is not in the source directory.");
        	this.RADsyncmirror.UseVisualStyleBackColor = true;
        	// 
        	// RADsynccopy
        	// 
        	this.RADsynccopy.AutoSize = true;
        	this.RADsynccopy.Checked = true;
        	this.RADsynccopy.Location = new System.Drawing.Point(367, 4);
        	this.RADsynccopy.Name = "RADsynccopy";
        	this.RADsynccopy.Size = new System.Drawing.Size(49, 17);
        	this.RADsynccopy.TabIndex = 7;
        	this.RADsynccopy.TabStop = true;
        	this.RADsynccopy.Text = "Copy";
        	this.toolTip1.SetToolTip(this.RADsynccopy, "Copy every file in the source directory to the destination directory based on the" +
        	        	" file checks. Deletes nothing");
        	this.RADsynccopy.UseVisualStyleBackColor = true;
        	// 
        	// BTNsyncremove
        	// 
        	this.BTNsyncremove.Image = global::Backup_2.Properties.Resources.remove;
        	this.BTNsyncremove.Location = new System.Drawing.Point(312, 21);
        	this.BTNsyncremove.Name = "BTNsyncremove";
        	this.BTNsyncremove.Size = new System.Drawing.Size(28, 23);
        	this.BTNsyncremove.TabIndex = 6;
        	this.BTNsyncremove.UseVisualStyleBackColor = true;
        	this.BTNsyncremove.Click += new System.EventHandler(this.BTNsyncremove_Click);
        	// 
        	// label2
        	// 
        	this.label2.AutoSize = true;
        	this.label2.Location = new System.Drawing.Point(135, 4);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(53, 13);
        	this.label2.TabIndex = 3;
        	this.label2.Text = "Sync jobs";
        	// 
        	// COMBsyncjob
        	// 
        	this.COMBsyncjob.FormattingEnabled = true;
        	this.COMBsyncjob.Location = new System.Drawing.Point(135, 21);
        	this.COMBsyncjob.Name = "COMBsyncjob";
        	this.COMBsyncjob.Size = new System.Drawing.Size(171, 21);
        	this.COMBsyncjob.TabIndex = 2;
        	this.COMBsyncjob.SelectedIndexChanged += new System.EventHandler(this.COMBsyncjob_SelectedIndexChanged);
        	// 
        	// BTNupdatesync
        	// 
        	this.BTNupdatesync.BackgroundImage = global::Backup_2.Properties.Resources.Update1;
        	this.BTNupdatesync.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
        	this.BTNupdatesync.Location = new System.Drawing.Point(62, 4);
        	this.BTNupdatesync.Name = "BTNupdatesync";
        	this.BTNupdatesync.Size = new System.Drawing.Size(52, 40);
        	this.BTNupdatesync.TabIndex = 1;
        	this.BTNupdatesync.UseVisualStyleBackColor = true;
        	this.BTNupdatesync.Click += new System.EventHandler(this.BTNupdatesync_Click);
        	// 
        	// BTNsynnew
        	// 
        	this.BTNsynnew.BackgroundImage = global::Backup_2.Properties.Resources.Newsync;
        	this.BTNsynnew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
        	this.BTNsynnew.Location = new System.Drawing.Point(3, 4);
        	this.BTNsynnew.Name = "BTNsynnew";
        	this.BTNsynnew.Size = new System.Drawing.Size(52, 40);
        	this.BTNsynnew.TabIndex = 0;
        	this.BTNsynnew.UseVisualStyleBackColor = true;
        	this.BTNsynnew.Click += new System.EventHandler(this.BTNsynnew_Click);
        	// 
        	// LSsync
        	// 
        	this.LSsync.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
        	        	        	this.columnHeader1,
        	        	        	this.columnHeader2});
        	this.LSsync.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.LSsync.Location = new System.Drawing.Point(3, 63);
        	this.LSsync.Name = "LSsync";
        	this.LSsync.Size = new System.Drawing.Size(888, 311);
        	this.LSsync.TabIndex = 1;
        	this.LSsync.UseCompatibleStateImageBehavior = false;
        	this.LSsync.View = System.Windows.Forms.View.Details;
        	// 
        	// columnHeader1
        	// 
        	this.columnHeader1.Text = "Source FileName";
        	this.columnHeader1.Width = 352;
        	// 
        	// columnHeader2
        	// 
        	this.columnHeader2.Text = "Destination FileName";
        	this.columnHeader2.Width = 292;
        	// 
        	// tabPage3
        	// 
        	this.tabPage3.Controls.Add(this.TXTlogscreen);
        	this.tabPage3.Location = new System.Drawing.Point(4, 22);
        	this.tabPage3.Name = "tabPage3";
        	this.tabPage3.Size = new System.Drawing.Size(900, 383);
        	this.tabPage3.TabIndex = 2;
        	this.tabPage3.Text = "Log";
        	this.tabPage3.UseVisualStyleBackColor = true;
        	// 
        	// TXTlogscreen
        	// 
        	this.TXTlogscreen.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.TXTlogscreen.Location = new System.Drawing.Point(0, 0);
        	this.TXTlogscreen.Multiline = true;
        	this.TXTlogscreen.Name = "TXTlogscreen";
        	this.TXTlogscreen.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        	this.TXTlogscreen.Size = new System.Drawing.Size(900, 383);
        	this.TXTlogscreen.TabIndex = 0;
        	// 
        	// statusStrip1
        	// 
        	this.statusStrip1.Location = new System.Drawing.Point(0, 439);
        	this.statusStrip1.Name = "statusStrip1";
        	this.statusStrip1.Size = new System.Drawing.Size(914, 22);
        	this.statusStrip1.TabIndex = 2;
        	this.statusStrip1.Text = "statusStrip1";
        	// 
        	// tableLayoutPanel3
        	// 
        	this.tableLayoutPanel3.ColumnCount = 1;
        	this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        	this.tableLayoutPanel3.Controls.Add(this.tabControl1, 0, 0);
        	this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 24);
        	this.tableLayoutPanel3.Name = "tableLayoutPanel3";
        	this.tableLayoutPanel3.RowCount = 1;
        	this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
        	this.tableLayoutPanel3.Size = new System.Drawing.Size(914, 415);
        	this.tableLayoutPanel3.TabIndex = 3;
        	// 
        	// clearLogFileToolStripMenuItem
        	// 
        	this.clearLogFileToolStripMenuItem.Name = "clearLogFileToolStripMenuItem";
        	this.clearLogFileToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        	this.clearLogFileToolStripMenuItem.Text = "Clear Log File";
        	this.clearLogFileToolStripMenuItem.Click += new System.EventHandler(this.ClearLogFileToolStripMenuItemClick);
        	// 
        	// Form1
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(914, 461);
        	this.Controls.Add(this.tableLayoutPanel3);
        	this.Controls.Add(this.statusStrip1);
        	this.Controls.Add(this.menuStrip1);
        	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        	this.MainMenuStrip = this.menuStrip1;
        	this.Name = "Form1";
        	this.Text = "Nicks Backup program";
        	this.menuStrip1.ResumeLayout(false);
        	this.menuStrip1.PerformLayout();
        	this.tabControl1.ResumeLayout(false);
        	this.tabPage1.ResumeLayout(false);
        	this.tableLayoutPanel1.ResumeLayout(false);
        	this.panel1.ResumeLayout(false);
        	this.panel1.PerformLayout();
        	this.tabPage2.ResumeLayout(false);
        	this.tableLayoutPanel2.ResumeLayout(false);
        	this.panel2.ResumeLayout(false);
        	this.panel2.PerformLayout();
        	this.tabPage3.ResumeLayout(false);
        	this.tabPage3.PerformLayout();
        	this.tableLayoutPanel3.ResumeLayout(false);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.ToolStripMenuItem clearLogFileToolStripMenuItem;

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BTNrestore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox COMBbackupjob;
        private System.Windows.Forms.Button BTNupdate;
        private System.Windows.Forms.Button BTNnew;
        private System.Windows.Forms.ListBox LSBbackup;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backupPropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openBackupToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox COMBsyncjob;
        private System.Windows.Forms.Button BTNupdatesync;
        private System.Windows.Forms.Button BTNsynnew;
        private System.Windows.Forms.ListView LSsync;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button BTNbackupremove;
        private System.Windows.Forms.Button BTNsyncremove;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem listModifiedToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem newBackupToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox TXTlogscreen;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearLogTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateBackupToolStripMenuItem;
        private System.Windows.Forms.RadioButton RADsynceq;
        private System.Windows.Forms.RadioButton RADsyncmirror;
        private System.Windows.Forms.RadioButton RADsynccopy;
        private System.Windows.Forms.CheckBox CHBtimestamp;
    }
}

