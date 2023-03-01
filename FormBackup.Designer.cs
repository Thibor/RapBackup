namespace RapBackup
{
	partial class FormBackup
	{
		/// <summary>
		/// Wymagana zmienna projektanta.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Wyczyść wszystkie używane zasoby.
		/// </summary>
		/// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Kod generowany przez Projektanta formularzy systemu Windows

		/// <summary>
		/// Metoda wymagana do obsługi projektanta — nie należy modyfikować
		/// jej zawartości w edytorze kodu.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBackup));
			this.panel1 = new System.Windows.Forms.Panel();
			this.lvBackups = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panDirExt = new System.Windows.Forms.Panel();
			this.treeView = new System.Windows.Forms.TreeView();
			this.lvExt = new System.Windows.Forms.ListView();
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.checkAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.uncheckAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panel3 = new System.Windows.Forms.Panel();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.bBackup = new System.Windows.Forms.Button();
			this.panel5 = new System.Windows.Forms.Panel();
			this.tbName = new System.Windows.Forms.TextBox();
			this.bSave = new System.Windows.Forms.Button();
			this.panel4 = new System.Windows.Forms.Panel();
			this.lFolder = new System.Windows.Forms.Label();
			this.bNew = new System.Windows.Forms.Button();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.sslInfo = new System.Windows.Forms.ToolStripStatusLabel();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.panel1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panDirExt.SuspendLayout();
			this.contextMenuStrip2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel4.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lvBackups);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(0, 24);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(211, 401);
			this.panel1.TabIndex = 0;
			// 
			// lvBackups
			// 
			this.lvBackups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.lvBackups.ContextMenuStrip = this.contextMenuStrip1;
			this.lvBackups.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvBackups.FullRowSelect = true;
			this.lvBackups.HideSelection = false;
			this.lvBackups.Location = new System.Drawing.Point(0, 0);
			this.lvBackups.MultiSelect = false;
			this.lvBackups.Name = "lvBackups";
			this.lvBackups.Size = new System.Drawing.Size(211, 401);
			this.lvBackups.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvBackups.TabIndex = 0;
			this.lvBackups.UseCompatibleStateImageBehavior = false;
			this.lvBackups.View = System.Windows.Forms.View.Details;
			this.lvBackups.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 200;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(108, 26);
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.panDirExt);
			this.panel2.Controls.Add(this.panel3);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(211, 24);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(589, 401);
			this.panel2.TabIndex = 1;
			// 
			// panDirExt
			// 
			this.panDirExt.Controls.Add(this.treeView);
			this.panDirExt.Controls.Add(this.lvExt);
			this.panDirExt.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panDirExt.Location = new System.Drawing.Point(0, 0);
			this.panDirExt.Name = "panDirExt";
			this.panDirExt.Size = new System.Drawing.Size(589, 307);
			this.panDirExt.TabIndex = 1;
			// 
			// treeView
			// 
			this.treeView.CheckBoxes = true;
			this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView.Location = new System.Drawing.Point(0, 0);
			this.treeView.Name = "treeView";
			this.treeView.Size = new System.Drawing.Size(471, 307);
			this.treeView.TabIndex = 1;
			this.treeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterCheck);
			// 
			// lvExt
			// 
			this.lvExt.CheckBoxes = true;
			this.lvExt.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
			this.lvExt.ContextMenuStrip = this.contextMenuStrip2;
			this.lvExt.Dock = System.Windows.Forms.DockStyle.Right;
			this.lvExt.FullRowSelect = true;
			this.lvExt.HideSelection = false;
			this.lvExt.Location = new System.Drawing.Point(471, 0);
			this.lvExt.MultiSelect = false;
			this.lvExt.Name = "lvExt";
			this.lvExt.Size = new System.Drawing.Size(118, 307);
			this.lvExt.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvExt.TabIndex = 2;
			this.lvExt.UseCompatibleStateImageBehavior = false;
			this.lvExt.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Extensions";
			this.columnHeader2.Width = 100;
			// 
			// contextMenuStrip2
			// 
			this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkAllToolStripMenuItem,
            this.uncheckAllToolStripMenuItem});
			this.contextMenuStrip2.Name = "contextMenuStrip2";
			this.contextMenuStrip2.Size = new System.Drawing.Size(136, 48);
			// 
			// checkAllToolStripMenuItem
			// 
			this.checkAllToolStripMenuItem.Name = "checkAllToolStripMenuItem";
			this.checkAllToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.checkAllToolStripMenuItem.Text = "Check all";
			this.checkAllToolStripMenuItem.Click += new System.EventHandler(this.checkAllToolStripMenuItem_Click);
			// 
			// uncheckAllToolStripMenuItem
			// 
			this.uncheckAllToolStripMenuItem.Name = "uncheckAllToolStripMenuItem";
			this.uncheckAllToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.uncheckAllToolStripMenuItem.Text = "Uncheck all";
			this.uncheckAllToolStripMenuItem.Click += new System.EventHandler(this.uncheckAllToolStripMenuItem_Click);
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.progressBar);
			this.panel3.Controls.Add(this.bBackup);
			this.panel3.Controls.Add(this.panel5);
			this.panel3.Controls.Add(this.panel4);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 307);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(589, 94);
			this.panel3.TabIndex = 2;
			// 
			// progressBar
			// 
			this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.progressBar.Location = new System.Drawing.Point(66, 62);
			this.progressBar.Maximum = 1000;
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(523, 32);
			this.progressBar.TabIndex = 4;
			// 
			// bBackup
			// 
			this.bBackup.Dock = System.Windows.Forms.DockStyle.Left;
			this.bBackup.Location = new System.Drawing.Point(0, 62);
			this.bBackup.Name = "bBackup";
			this.bBackup.Size = new System.Drawing.Size(66, 32);
			this.bBackup.TabIndex = 3;
			this.bBackup.Text = "Backup";
			this.bBackup.UseVisualStyleBackColor = true;
			this.bBackup.Click += new System.EventHandler(this.bBackup_Click);
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.tbName);
			this.panel5.Controls.Add(this.bSave);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel5.Location = new System.Drawing.Point(0, 31);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(589, 31);
			this.panel5.TabIndex = 1;
			// 
			// tbName
			// 
			this.tbName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.tbName.Location = new System.Drawing.Point(66, 0);
			this.tbName.Name = "tbName";
			this.tbName.Size = new System.Drawing.Size(523, 26);
			this.tbName.TabIndex = 1;
			// 
			// bSave
			// 
			this.bSave.Dock = System.Windows.Forms.DockStyle.Left;
			this.bSave.Location = new System.Drawing.Point(0, 0);
			this.bSave.Name = "bSave";
			this.bSave.Size = new System.Drawing.Size(66, 31);
			this.bSave.TabIndex = 0;
			this.bSave.Text = "Save";
			this.bSave.UseVisualStyleBackColor = true;
			this.bSave.Click += new System.EventHandler(this.bSave_Click);
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.lFolder);
			this.panel4.Controls.Add(this.bNew);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Location = new System.Drawing.Point(0, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(589, 31);
			this.panel4.TabIndex = 0;
			// 
			// lFolder
			// 
			this.lFolder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lFolder.Location = new System.Drawing.Point(66, 0);
			this.lFolder.Name = "lFolder";
			this.lFolder.Size = new System.Drawing.Size(523, 31);
			this.lFolder.TabIndex = 1;
			this.lFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// bNew
			// 
			this.bNew.Dock = System.Windows.Forms.DockStyle.Left;
			this.bNew.Location = new System.Drawing.Point(0, 0);
			this.bNew.Name = "bNew";
			this.bNew.Size = new System.Drawing.Size(66, 31);
			this.bNew.TabIndex = 0;
			this.bNew.Text = "New";
			this.bNew.UseVisualStyleBackColor = true;
			this.bNew.Click += new System.EventHandler(this.bNew_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(800, 24);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.optionsToolStripMenuItem.Text = "Options";
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
			// 
			// statusStrip
			// 
			this.statusStrip.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.sslInfo});
			this.statusStrip.Location = new System.Drawing.Point(0, 425);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(800, 22);
			this.statusStrip.TabIndex = 3;
			this.statusStrip.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
			// 
			// toolStripStatusLabel2
			// 
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
			// 
			// sslInfo
			// 
			this.sslInfo.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.sslInfo.ForeColor = System.Drawing.Color.DarkRed;
			this.sslInfo.Name = "sslInfo";
			this.sslInfo.Size = new System.Drawing.Size(0, 17);
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// FormBackup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 447);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.statusStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormBackup";
			this.Text = "RapBackup";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBackup_FormClosing);
			this.panel1.ResumeLayout(false);
			this.contextMenuStrip1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panDirExt.ResumeLayout(false);
			this.contextMenuStrip2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.panel5.PerformLayout();
			this.panel4.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ListView lvBackups;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panDirExt;
		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.ListView lvExt;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Button bNew;
		private System.Windows.Forms.Label lFolder;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Button bSave;
		private System.Windows.Forms.Button bBackup;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
		private System.Windows.Forms.ToolStripStatusLabel sslInfo;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
		private System.Windows.Forms.ToolStripMenuItem checkAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem uncheckAllToolStripMenuItem;
		private System.Windows.Forms.Timer timer1;
	}
}

