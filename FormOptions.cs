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
		static string des;

		public static string Des
		{
			get
			{
				if(string.IsNullOrEmpty(des))
					return AppContext.BaseDirectory;
				if (Directory.Exists(des))
					return des;
				else
					return AppContext.BaseDirectory;
			}
		}

		public FormOptions()
		{
			InitializeComponent();
			LoadFromIni();
			tbDes.Text = Des;
		}



		void SaveToIni()
		{
			FormBackup.ini.Write("des",Des);
		}

		void LoadFromIni()
		{
			des = FormBackup.ini.Read("des",Des);
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
