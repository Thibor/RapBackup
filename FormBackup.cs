using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RapIni;

namespace RapBackup
{
	public partial class FormBackup : Form
	{
		string recName = string.Empty;
		List<string> fileList = new List<string>();
		List<string> pathList = new List<string>();
		List<string> extList = new List<string>();
		CRecList recList = new CRecList();
		public static CRapIni ini = new CRapIni();
		FormOptions formOptions = new FormOptions();

		public FormBackup()
		{
			InitializeComponent();
			recList.LoadFromIni();
			UpdateList();
		}

		bool ExtOk(string ext)
		{
			foreach (ListViewItem lvItem in lvExt.Items)
				if (lvItem.Text == ext)
					return lvItem.Checked;
			return false;
		}

		void ClickBackup()
		{
			progressBar.Maximum = fileList.Count;
			string date = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
			string nameZip = $@"{FormOptions.des}\{tbName.Text} {date}.zip";
			using (FileStream zipFile = File.Open(nameZip, FileMode.Create))
			using (var archive = new ZipArchive(zipFile, ZipArchiveMode.Create))
			{
				for (int n = 0; n < fileList.Count; n++)
				{
					progressBar.Value = n;
					string fn = fileList[n];
					string nn = pathList[n];
					string ex = Path.GetExtension(fn);
					if (ExtOk(ex))
						archive.CreateEntryFromFile(fn, nn);
				}
			}
			progressBar.Value = 0;
			UpdateInfo(tbName.Text);
			toolStripStatusLabel3.Text = "Backup completed";
		}

		void ClickDeleteName()
		{
			CRec r = GetRec();
			if (r == null)
				return;
			DialogResult dr = MessageBox.Show($"Are you sure to delete {r.name}?", "Confirm Delete", MessageBoxButtons.YesNo);
			if (dr != DialogResult.Yes)
				return;
			recList.Remove(r);
			recList.SaveToIni();
			ini.Save();
			UpdateList();
		}

		void ClickNew()
		{
			folderBrowserDialog.SelectedPath = AppContext.BaseDirectory;
			DialogResult result = folderBrowserDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				CreateRec(folderBrowserDialog.SelectedPath);
			}
		}

		void ClickSave()
		{
			CRec r = GetRec();
			if (r == null)
				return;
			recList.Remove(r);
			r = SettingsToRec();
			recList.Add(r);
			recList.SaveToIni();
			recName = r.name;
			UpdateList();
		}

		CRec GetRec()
		{
			if (listView.SelectedItems.Count == 0)
				return null;
			return recList.GetRec(listView.SelectedItems[0].Text);
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

		int CountBackups(string name,out DateTime bst)
		{
			bst = DateTime.MinValue;
			string mask = $@"{name} ????-??-?? ??????.zip";
			string[] files = Directory.GetFiles(FormOptions.des,mask);
			foreach(string f in files)
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
			toolStripStatusLabel3.Text = String.Empty;
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
			listView.Items.Clear();
			foreach (CRec r in recList)
			{
				ListViewItem lvItem = new ListViewItem(new[] { r.name });
				lvItem.Selected = r.name == recName;
				listView.Items.Add(lvItem);
			}
			if ((listView.Items.Count > 0) && (listView.SelectedItems.Count == 0))
				listView.Items[0].Selected = true;
		}

		string NodesPath(TreeNode tn)
		{
			string result = tn.Text;
			while (tn.Parent != null)
			{
				tn = tn.Parent;
				result = $@"{tn.Text}\{result}";
			}
			return result;
		}

		void FillTree(CRec r, TreeNode node, string path)
		{
			string np = NodesPath(node);
			string[] arr = Directory.GetDirectories(path);
			List<string> ls = new List<string>(arr);
			ls.Sort();
			foreach (string d in ls)
			{
				string fn = Path.GetFileName(d);
				string p = $@"{path}\{fn}";
				TreeNode tn = node.Nodes.Add(fn);
				tn.Checked = (r.dirList.Count == 0) || (r.dirList.IndexOf(NodesPath(tn)) >= 0);
				FillTree(r, tn, p);
			}
			if (!node.Checked)
				return;
			string[] files = Directory.GetFiles(path);
			foreach (string f in files)
			{
				fileList.Add(f);
				pathList.Add($@"{np}\{Path.GetFileName(f)}");
			}
		}

		void FillTree(CRec r)
		{
			fileList.Clear();
			pathList.Clear();
			extList.Clear();
			treeView.Nodes.Clear();
			lvExt.Items.Clear();
			string fn = Path.GetFileName(r.folder);
			TreeNode tn = treeView.Nodes.Add(fn);
			tn.Checked = true;
			FillTree(r, tn, r.folder);
		}

		void FillExt(CRec r)
		{
			foreach (string f in fileList)
			{
				string ext = Path.GetExtension(f);
				if (extList.IndexOf(ext) < 0)
				{
					extList.Add(ext);
				}
			}
			extList.Sort();
			if (r.extList.Count == 0)
				r.extList = extList;
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
				list.Add(path);
			else
				return list;
			foreach (TreeNode n in node.Nodes)
				list.AddRange(GetDirList(n, $@"{path}\{n.Text}"));
			return list;
		}

		List<string> GetExtList()
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
			FillExt(r);
		}

		CRec SettingsToRec()
		{
			CRec r = new CRec();
			r.folder = lFolder.Text;
			r.name = tbName.Text;
			r.dirList = GetDirList(treeView.Nodes[0], treeView.Nodes[0].Text);
			r.extList = GetExtList();
			return r;
		}

		void CreateRec(string folder)
		{
			string name = Path.GetFileName(folder);
			CRec r = new CRec(name) { folder = folder };
			RecToSettings(r);
			recList.Add(r);
			recList.SaveToIni();
			recName = r.name;
			UpdateList();
		}

		private void bNew_Click(object sender, EventArgs e)
		{
			ClickNew();
		}

		private void listView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listView.SelectedItems.Count == 0)
				return;
			RecSelected(listView.SelectedItems[0].Text);
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
			ClickDeleteName();
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			formOptions.ShowDialog(this);
		}

	}
}
