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
		public static string des;

		public FormOptions()
		{
			InitializeComponent();
			CreateDir("Backup");
			des = $@"{AppContext.BaseDirectory}\Backup";
			LoadFromIni();
			tbDes.Text = des;
		}

		void SaveToIni()
		{
			FormBackup.ini.Write("des",des);
		}

		void LoadFromIni()
		{
			des = FormBackup.ini.Read("des",des);
		}

		void CreateDir(string dir)
		{
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);
		}

		private void FormOptions_FormClosing(object sender, FormClosingEventArgs e)
		{
			des = tbDes.Text;
			SaveToIni();
			FormBackup.ini.Save();
		}

		private void bDes_Click(object sender, EventArgs e)
		{
			folderBrowserDialog.SelectedPath = AppContext.BaseDirectory;
			DialogResult result = folderBrowserDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				tbDes.Text = folderBrowserDialog.SelectedPath;
			}
		}
	}
}
