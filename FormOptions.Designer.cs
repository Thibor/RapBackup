﻿namespace RapBackup
{
	partial class FormOptions
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tbDes = new System.Windows.Forms.TextBox();
			this.bDes = new System.Windows.Forms.Button();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.panel1);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(496, 56);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Destination";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.tbDes);
			this.panel1.Controls.Add(this.bDes);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(3, 16);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(490, 31);
			this.panel1.TabIndex = 2;
			// 
			// tbDes
			// 
			this.tbDes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbDes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.tbDes.Location = new System.Drawing.Point(66, 0);
			this.tbDes.Name = "tbDes";
			this.tbDes.Size = new System.Drawing.Size(424, 26);
			this.tbDes.TabIndex = 1;
			// 
			// bDes
			// 
			this.bDes.Dock = System.Windows.Forms.DockStyle.Left;
			this.bDes.Location = new System.Drawing.Point(0, 0);
			this.bDes.Name = "bDes";
			this.bDes.Size = new System.Drawing.Size(66, 31);
			this.bDes.TabIndex = 0;
			this.bDes.Text = "Select";
			this.bDes.UseVisualStyleBackColor = true;
			this.bDes.Click += new System.EventHandler(this.bDes_Click);
			// 
			// FormOptions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(496, 63);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "FormOptions";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Options";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormOptions_FormClosing);
			this.groupBox1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox tbDes;
		private System.Windows.Forms.Button bDes;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
	}
}