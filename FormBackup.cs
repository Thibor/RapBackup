using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading.Tasks;
using RapIni;

namespace RapBackup
{
	public partial class FormBackup : Form
	{
		string recName = string.Empty;
		readonly List<string> fileList = new List<string>();
		public Stopwatch timer = new Stopwatch();
		CMsg msg = new CMsg();
		CSynMsg synMsg = new CSynMsg();
		readonly CRecList recList = new CRecList();
		public static CRapIni ini = new CRapIni();
		readonly FormOptions formOptions = new FormOptions();

		public FormBackup()
		{
			InitializeComponent();
			recList.LoadFromIni();
			UpdateList();
		}

		void CheckAll(bool check)
		{
			foreach (ListViewItem lvItem in lvExt.Items)
				lvItem.Checked = check;
		}

		void ShowInfo(string msg)
		{
			sslInfo.Text = msg;
			statusStrip.Update();
		}

		async void ClickBackup(CRec r)
		{
			await Task.Run(() =>
			{
				CMsg msg = new CMsg();
				CSynMsg sm = new CSynMsg();
				timer.Restart();
				string date = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
				string nameZip = $@"{FormOptions.Des}\{tbName.Text} {date}.zip";
				string root = r.Root;
				using (FileStream zipFile = File.Open(nameZip, FileMode.Create))
				using (var archive = new ZipArchive(zipFile, ZipArchiveMode.Create))
				{
					for (int n = 0; n < fileList.Count; n++)
					{
						msg.progress = (double)n / fileList.Count;
						string fn = fileList[n];
						string ex = Path.GetExtension(fn);
						if (r.PathOk(fn))
						{
							string sp = r.CreateShortFile(fn);
							archive.CreateEntryFromFile(fn, $@"{root}{sp}");
							msg.msg = Path.GetFileName(fn);
						}
						sm.SetMsg(msg);
					}
				}
				timer.Stop();
				TimeSpan ts = timer.Elapsed;
				msg.msg = $"{r.name} backuped ({ts.TotalSeconds:N2})";
				msg.progress = 2;
				sm.SetMsg(msg);
			});
		}

		void ClickDelete()
		{
			CRec r = GetRec();
			if (r == null)
				return;
			DialogResult dr = MessageBox.Show($"Are you sure to delete {r.name}?", "Confirm Delete", MessageBoxButtons.YesNo);
			if (dr != DialogResult.Yes)
				return;
			ShowInfo("Delete");
			recList.Remove(r);
			recList.SaveToIni();
			UpdateList();
			timer.Stop();
			TimeSpan ts = timer.Elapsed;
			sslInfo.Text = $"{r.name} deleted ({ts.TotalSeconds:N2})";
		}

		void ClickNew()
		{
			folderBrowserDialog.SelectedPath = AppContext.BaseDirectory;
			DialogResult result = folderBrowserDialog.ShowDialog();
			if (result == DialogResult.OK)
				CreateRec(folderBrowserDialog.SelectedPath);
		}

		void ClickSave()
		{
			CRec r = GetRec();
			if (r == null)
				return;
			timer.Restart();
			ShowInfo("Save");
			r = SettingsToRec();
			r.SaveToIni();
			ini.Save();
			recName = r.name;
			lvBackups.SelectedItems[0].Text = r.name;
			timer.Stop();
			TimeSpan ts = timer.Elapsed;
			sslInfo.Text = $"{r.name} saved ({ts.TotalSeconds:N2})";
		}

		CRec GetRec()
		{
			if (lvBackups.SelectedItems.Count == 0)
				return null;
			return recList.GetRec(lvBackups.SelectedItems[0].Text);
		}

		private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
		{
			foreach (TreeNode node in treeNode.Nodes)
			{
				node.Checked = nodeChecked;
				if (node.Nodes.Count > 0)
					CheckAllChildNodes(node, nodeChecked);
			}
		}

		int CountBackups(string name, out DateTime bst)
		{
			bst = DateTime.MinValue;
			string mask = $@"{name} ????-??-?? ??????.zip";
			string[] files = Directory.GetFiles(FormOptions.Des, mask);
			foreach (string f in files)
			{
				FileInfo fi = new FileInfo(f);
				if (bst < fi.CreationTime)
					bst = fi.CreationTime;
			}
			return files.Length;
		}

		void UpdateInfo(string name)
		{
			int cb = CountBackups(name, out DateTime dt);
			toolStripStatusLabel1.Text = dt == DateTime.MinValue ? string.Empty : dt.ToString("yyyy-MM-dd hh:mm:ss");
			toolStripStatusLabel2.Text = $"Backups {cb}";
			sslInfo.Text = String.Empty;
		}

		void RecSelected(string name)
		{
			timer.Restart();
			CRec r = recList.GetRec(name);
			if (r == null)
				r = new CRec();
			RecToSettings(r);
			UpdateInfo(name);
			timer.Stop();
			TimeSpan ts = timer.Elapsed;
			sslInfo.Text = $"{r.name} selected ({ts.TotalSeconds:N2})";
		}

		void UpdateList()
		{
			lvBackups.Items.Clear();
			foreach (CRec r in recList)
			{
				ListViewItem lvItem = new ListViewItem(new[] { r.name }) { Selected = r.name == recName };
				lvBackups.Items.Add(lvItem);
			}
			if ((lvBackups.Items.Count > 0) && (lvBackups.SelectedItems.Count == 0))
				lvBackups.Items[0].Selected = true;
		}

		string NodesPath(TreeNode tn)
		{
			string result = tn.Text;
			while (true)
			{
				tn = tn.Parent;
				if (tn == null)
					break;
				if (tn.Level > 0)
					result = $@"{tn.Text}\{result}";
				else
					break;
			}
			return result;
		}

		List<string> GetListDir(CRec rec)
		{
			List<string> list = new List<string>();
			foreach (string f in fileList)
			{
				string sd = rec.CreateShortDir(f);
				if (list.IndexOf(sd) < 0)
					list.Add(sd);
			}
			return list;
		}

		List<string> GetListExt()
		{
			List<string> list = new List<string>();
			foreach (string f in fileList)
			{
				string sd = Path.GetExtension(f).ToLower();
				if (list.IndexOf(sd) < 0)
					list.Add(sd);
			}
			return list;
		}

		void FillTree(int ply, CRec r, TreeNode node, string path)
		{
			string[] ad = Directory.GetDirectories(path);
			List<string> ls = new List<string>(ad);
			ls.Sort();
			for (int n = 0; n < ls.Count; n++)
			{
				string d = ls[n];
				string fn = Path.GetFileName(d);
				string p = $@"{path}\{fn}";
				ShowInfo(fn);
				TreeNode tn = node.Nodes.Add(fn);
				tn.Checked = (r.dirList.Count == 0) || (r.dirList.IndexOf(NodesPath(tn)) >= 0);
				FillTree(ply + 1, r, tn, p);
			}
			string[] af = Directory.GetFiles(path);
			foreach (string f in af)
				fileList.Add(f);
		}

		void FillTree(CRec r)
		{
			fileList.Clear();
			treeView.Nodes.Clear();
			string root = Path.GetFileName(r.folder);
			if (string.IsNullOrEmpty(root))
				root = "Backup";
			TreeNode tn = treeView.Nodes.Add(root);
			tn.Checked = true;
			FillTree(0, r, tn, r.folder);
			sslInfo.Text = String.Empty;
			progressBar.Value = 0;
		}

		void FillExt(CRec r, List<string> extList)
		{
			extList.Sort();
			lvExt.Items.Clear();
			foreach (string e in extList)
			{
				ListViewItem lvItem = new ListViewItem(new[] { e });
				lvItem.Checked = (r.extList.Count == 0) || (r.extList.IndexOf(lvItem.Text) >= 0);
				lvExt.Items.Add(lvItem);
			}
		}

		List<string> GetDirList(TreeNode node, string path)
		{
			List<string> list = new List<string>();
			if (node.Checked)
			{
				if (string.IsNullOrEmpty(path))
				{
					foreach (TreeNode n in node.Nodes)
						list.AddRange(GetDirList(n, $@"{n.Text}"));
				}
				else
				{
					list.Add(path);
					foreach (TreeNode n in node.Nodes)
						list.AddRange(GetDirList(n, $@"{path}\{n.Text}"));
				}
			}
			return list;
		}

		List<string> GetCheckedExt()
		{
			List<string> list = new List<string>();
			foreach (ListViewItem lvItem in lvExt.Items)
				if (lvItem.Checked)
					list.Add(lvItem.Text);
			return list;
		}

		void RecToSettings(CRec r)
		{
			lFolder.Text = r.folder;
			tbName.Text = r.name;
			FillTree(r);
			FillExt(r, GetListExt());
		}

		CRec SettingsToRec()
		{
			return new CRec()
			{
				folder = lFolder.Text,
				name = tbName.Text,
				dirList = GetDirList(treeView.Nodes[0], String.Empty),
				extList = GetCheckedExt()
			};
		}

		void CreateRec(string folder)
		{
			timer.Restart();
			string name = Path.GetFileName(folder);
			CRec r = new CRec(name) { folder = folder.Trim('\\') };
			r.name = recList.CreateUniqueName(r);
			lFolder.Text = r.folder;
			tbName.Text = r.name;
			FillTree(r);
			r.dirList = GetListDir(r);
			r.extList = GetListExt();
			FillExt(r, r.extList);
			recList.Add(r);
			recList.SaveToIni();
			recName = r.name;
			lvBackups.SelectedIndexChanged -= listView_SelectedIndexChanged;
			ListViewItem lvItem = new ListViewItem(new[] { r.name }) { Selected = true };
			lvBackups.Items.Add(lvItem);
			lvBackups.SelectedIndexChanged -= listView_SelectedIndexChanged;
			timer.Stop();
			TimeSpan ts = timer.Elapsed;
			sslInfo.Text = $"{r.name} created ({ts.TotalSeconds:N2})";
		}

		private void bNew_Click(object sender, EventArgs e)
		{
			ClickNew();
		}

		private void listView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lvBackups.SelectedItems.Count == 0)
				return;
			RecSelected(lvBackups.SelectedItems[0].Text);
		}

		private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (e.Action != TreeViewAction.Unknown)
				if (e.Node.Nodes.Count > 0)
					this.CheckAllChildNodes(e.Node, e.Node.Checked);
		}

		private void bSave_Click(object sender, EventArgs e)
		{
			ClickSave();
		}

		private void bBackup_Click(object sender, EventArgs e)
		{
			bBackup.Enabled = false;
			ClickBackup(GetRec());
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ClickDelete();
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			formOptions.ShowDialog(this);
		}

		private void checkAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckAll(true);
		}

		private void uncheckAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckAll(false);
		}

		private void FormBackup_FormClosing(object sender, FormClosingEventArgs e)
		{
			ini.Save();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			CMsg m = synMsg.GetMsg();
			if (msg.msg != m.msg)
			{
				msg.msg = m.msg;
				sslInfo.Text = m.msg;
			}
			if (m.progress == 2)
			{
				m.progress = 0;
				bBackup.Enabled = true;
			}
			progressBar.Value = Convert.ToInt32(progressBar.Maximum * m.progress);
		}
	}
}
