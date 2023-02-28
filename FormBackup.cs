using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Windows.Forms;
using RapIni;

namespace RapBackup
{
	public partial class FormBackup : Form
	{
		string recName = string.Empty;
		readonly List<string> fileList = new List<string>();
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

		void ClickBackup()
		{
			CRec rec = GetRec();
			progressBar.Maximum = fileList.Count;
			string date = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
			string nameZip = $@"{FormOptions.Des}\{tbName.Text} {date}.zip";
			using (FileStream zipFile = File.Open(nameZip, FileMode.Create))
			using (var archive = new ZipArchive(zipFile, ZipArchiveMode.Create))
			{
				for (int n = 0; n < fileList.Count; n++)
				{
					progressBar.Value = n;
					string fn = fileList[n];
					string ex = Path.GetExtension(fn);
					if (rec.PathOk(fn))
					{
						string sp = rec.CreateShortFile(fn);
						archive.CreateEntryFromFile(fn, sp);
						sslInfo.Text = Path.GetFileName(fn);
						statusStrip.Update();
					}
				}
			}
			progressBar.Value = 0;
			UpdateInfo(tbName.Text);
			sslInfo.Text = "Backup completed";
		}

		void ClickDelete()
		{
			CRec r = GetRec();
			if (r == null)
				return;
			DialogResult dr = MessageBox.Show($"Are you sure to delete {r.name}?", "Confirm Delete", MessageBoxButtons.YesNo);
			if (dr != DialogResult.Yes)
				return;
			sslInfo.Text = $"Start delete";
			statusStrip.Update();
			recList.Remove(r);
			recList.SaveToIni();
			UpdateList();
			sslInfo.Text = $"{r.name} deleted successfully";
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
			sslInfo.Text = $"Start save";
			statusStrip.Update();
			recList.Remove(r);
			r = SettingsToRec();
			recList.Add(r);
			recList.SaveToIni();
			recName = r.name;
			lvBackups.SelectedItems[0].Text = r.name;
			sslInfo.Text = $"{r.name} saved successfully";
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
			CRec r = recList.GetRec(name);
			if (r == null)
				r = new CRec();
			RecToSettings(r);
			UpdateInfo(name);
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
				string sd = Path.GetExtension(f);
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
			if (ply == 0)
				progressBar.Maximum = ls.Count;
			for (int n = 0; n < ls.Count; n++)
			{
				string d = ls[n];
				string fn = Path.GetFileName(d);
				string p = $@"{path}\{fn}";
				sslInfo.Text = fn;
				statusStrip.Update();
				TreeNode tn = node.Nodes.Add(fn);
				tn.Checked = (r.dirList.Count == 0) || (r.dirList.IndexOf(NodesPath(tn)) >= 0);
				FillTree(ply + 1, r, tn, p);
				if (ply == 0)
					progressBar.Value = n;
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
			ClickBackup();
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
	}
}
