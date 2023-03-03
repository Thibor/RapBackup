using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RapBackup
{
	public partial class FormOptions : Form
	{
		public static string folder = AppContext.BaseDirectory;
		public static int backups = 0;

		public static string Des
		{
			get
			{
				if (Directory.Exists(folder))
					return folder;
				else
					return AppContext.BaseDirectory;
			}
		}

		public FormOptions()
		{
			InitializeComponent();
			LoadFromIni();
			tbDes.Text = Des;
			nudBackups.Value = backups;
		}

		void SaveToIni()
		{
			FormBackup.ini.Write("options>folder",folder);
			FormBackup.ini.Write("options>backups", backups);
		}

		void LoadFromIni()
		{
			folder = FormBackup.ini.Read("options>folder",folder);
			backups = FormBackup.ini.ReadInt("options>backups", backups);
		}

		private void FormOptions_FormClosing(object sender, FormClosingEventArgs e)
		{
			folder = tbDes.Text;
			SaveToIni();
			FormBackup.ini.Save();
		}

		private void bDes_Click(object sender, EventArgs e)
		{
			folderBrowserDialog.SelectedPath = Des;
			DialogResult result = folderBrowserDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				folder = folderBrowserDialog.SelectedPath;
				tbDes.Text = folder;
			}
		}

		private void nudBackups_ValueChanged(object sender, EventArgs e)
		{
			backups = (int)nudBackups.Value;
		}

	}
}
