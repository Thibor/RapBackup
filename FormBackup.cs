﻿using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading.Tasks;
using RapIni;
using System.Xml.Linq;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RapBackup
{
	public partial class FormBackup : Form
	{
		int timerCounter = 0;
		string lastFolder = string.Empty;
		string lastBackup = string.Empty;
		List<string> fileList = new List<string>();
		public Stopwatch timer = new Stopwatch();
		readonly CSynMsg synMsg = new CSynMsg();
		readonly CRecList recList = new CRecList();
		public static CRapIni ini = new CRapIni();
		readonly FormOptions formOptions = new FormOptions();

		public FormBackup()
		{
			lastFolder = AppContext.BaseDirectory;
			InitializeComponent();
			LoadFromIni();
			recList.LoadFromIni();
			UpdateList(lastBackup);
		}

		void LoadFromIni()
		{
			lastFolder = ini.Read("backup>folder", lastFolder);
			lastBackup = ini.Read("backup>backup", lastBackup);
		}

		void SaveToIni()
		{
			ini.Write("backup>folder", lastFolder);
			ini.Write("backup>backup", lastBackup);
		}

		void CheckAll(bool check)
		{
			foreach (ListViewItem lvItem in lvExt.Items)
				lvItem.Checked = check;
		}

		void ClickBackup()
		{
			CRec r = SettingsToRec();
			ClickBackup(r, fileList);
		}

		void BackupTask(CRec r, string dirDes, List<string> fl)
		{
			Stopwatch timer = new Stopwatch();
			timer.Restart();
			List<string> folderList = new List<string>(r.dirList);
			List<string> fileList = new List<string>(fl);
			CSynMsg sm = new CSynMsg();
			CMsg msg = sm.GetMsg();
			msg.progress = 0;
			string date = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
			string nameZip = $@"{dirDes}\{r.name} {date}.zip";
			string root = r.Root;
			string folder = r.folder;
			using (FileStream zipFile = File.Open(nameZip, FileMode.Create))
			using (var archive = new ZipArchive(zipFile, ZipArchiveMode.Create))
			{
				foreach (string d in folderList)
					if (Directory.Exists($@"{r.folder}\{d}"))
						archive.CreateEntry($@"{root}{d}\");
				for (int n = 0; n < fileList.Count; n++)
				{
					msg.progress = (double)n / fileList.Count;
					string fn = fileList[n];
					string ex = Path.GetExtension(fn);
					if (r.PathOk(fn))
					{
						string sp = CRec.CreateShortFile(folder, fn);
						archive.CreateEntryFromFile(fn, $@"{root}{sp}");
						msg.msg = Path.GetFileName(fn);
					}
					sm.SetMsg(msg);
				}
			}
			TimeSpan ts = timer.Elapsed;
			msg.msg = $"{r.name} backuped ({ts.TotalSeconds:N2})";
			msg.progress = 2;
			sm.SetMsg(msg);
		}

		async void ClickBackup(CRec r, List<string> fl)
		{
			await Task.Run(() => BackupTask(r, FormOptions.Des, fl));
		}

		void ClickDelete()
		{
			CRec r = GetRec();
			if (r == null)
				return;
			DialogResult dr = MessageBox.Show($"Are you sure to delete {r.name}?", "Confirm Delete", MessageBoxButtons.YesNo);
			if (dr != DialogResult.Yes)
				return;
			tssInfo.Text = "Delete";
			recList.Remove(r);
			recList.SaveToIni();
			UpdateList();
			timer.Stop();
			TimeSpan ts = timer.Elapsed;
			RecToSettings();
			tssInfo.Text = $"{r.name} deleted ({ts.TotalSeconds:N2})";
		}

		void ClickNew()
		{
			folderBrowserDialog.SelectedPath = lastFolder;
			DialogResult result = folderBrowserDialog.ShowDialog();
			if (result == DialogResult.OK)
				CreateRec(folderBrowserDialog.SelectedPath);
		}

		void ClickSave()
		{
			timer.Restart();
			tssInfo.Text = "Save";
			CRec rec = GetRec();
			CRec r = SettingsToRec();
			r.SaveToIni();
			ini.Save();
			TimeSpan ts = timer.Elapsed;
			tssInfo.Text = $"{r.name} saved ({ts.TotalSeconds:N2}s)";
			recList.LoadFromIni();
			if (rec.name != tbName.Text)
				UpdateList(r.name);
			timer.Stop();
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

		int BackupsCount(string name, out DateTime newest, out long size)
		{
			newest = DateTime.MinValue;
			size = 0;
			string mask = $@"{name} ????-??-?? ??????.zip";
			string[] files = Directory.GetFiles(FormOptions.Des, mask);
			foreach (string f in files)
			{
				FileInfo fi = new FileInfo(f);
				if (newest < fi.CreationTime)
				{
					newest = fi.CreationTime;
					size = fi.Length;
				}
			}
			return files.Length;
		}

		int BackupsCount(string name, out string oldest)
		{
			oldest = string.Empty;
			DateTime dtBst = DateTime.MaxValue;
			string mask = $@"{name} ????-??-?? ??????.zip";
			string[] files = Directory.GetFiles(FormOptions.Des, mask);
			foreach (string f in files)
			{
				FileInfo fi = new FileInfo(f);
				if (dtBst > fi.CreationTime)
				{
					dtBst = fi.CreationTime;
					oldest = fi.FullName;
				}
			}
			return files.Length;
		}

		void BackupsLimit(string name)
		{
			if (FormOptions.backups == 0)
				return;
			while (BackupsCount(name, out string oldest) > FormOptions.backups)
				File.Delete(oldest);
		}

		void UpdateInfo(string name)
		{
			int cb = BackupsCount(name, out DateTime dt, out long size);
			tssDate.Text = dt == DateTime.MinValue ? string.Empty : dt.ToString("yyyy-MM-dd HH:mm:ss");
			tssBackups.Text = $"Backups {cb}";
			tssSize.Text = $"{size:N0}";
			tssInfo.Text = String.Empty;
		}

		void UpdateList(string s = "")
		{
			lvBackups.Items.Clear();
			foreach (CRec r in recList)
			{
				ListViewItem lvItem = new ListViewItem(new[] { r.name }) { Selected = r.name == s };
				lvBackups.Items.Add(lvItem);
			}
		}

		bool FillList(string folder, string path, List<string> files, List<string> dir, List<string> ext)
		{
			if (!Directory.Exists(folder))
				return false;
			CSynMsg sm = new CSynMsg();
			CMsg m = sm.GetMsg();
			if (m.stop)
				return false;
			string[] ad = Directory.GetDirectories(path + '\\');
			foreach (string d in ad)
			{
				if (!FillList(folder, d, files, dir, ext))
					return false;
				dir.Add(CRec.CreateShortFile(folder, d));
			}
			string[] af = Directory.GetFiles(path);
			foreach (string f in af)
			{
				files.Add(f);
				string e = Path.GetExtension(f).ToLower();
				if (ext.IndexOf(e) < 0)
					ext.Add(e);
				m.msg = Path.GetFileName(f);
				sm.SetMsg(m);
			}
			return true;
		}

		async void FillList(string folder)
		{
			await Task.Run(() =>
			{
				timer.Restart();
				List<string> files = new List<string>();
				List<string> dir = new List<string>();
				List<string> ext = new List<string>();
				FillList(folder, folder, files, dir, ext);
				TimeSpan ts = timer.Elapsed;
				CSynMsg sm = new CSynMsg();
				CMsg m = sm.GetMsg();
				m.listDone = true;
				m.files = files;
				m.dir = dir;
				m.ext = ext;
				m.msg = $"{m.name} selected ({ts.TotalSeconds:N2}s)";
				sm.SetMsg(m);
			});
		}

		void FillExt(CRec r, List<string> extList)
		{
			extList.Sort();
			lvExt.Items.Clear();
			foreach (string e in extList)
			{
				ListViewItem lvItem = new ListViewItem(new[] { e });
				lvItem.Checked = (r.extList.IndexOf(lvItem.Text) >= 0) || (r.extList.Count == 0);
				lvExt.Items.Add(lvItem);
			}
		}

		void FillDir(CRec r, TreeNode tn, string path)
		{
			bool check = (r.dirList.IndexOf(path) >= 0) || (r.extList.Count == 0);
			string[] ap = path.Split('\\');
			for (int n = 0; n < ap.Length; n++)
			{
				string d = ap[n];
				int i = tn.Nodes.IndexOfKey(d);
				if (i >= 0)
					tn = tn.Nodes[i];
				else
					tn = tn.Nodes.Add(d, d);
				if (check)
				{
					tn.Checked = true;
					tn.Expand();
				}
			}
		}

		void FillDir(CRec r, List<string> dirList)
		{
			dirList.Sort();
			treeView.Nodes.Clear();
			string root = Path.GetFileName(r.folder);
			if (string.IsNullOrEmpty(root))
				root = "Backup";
			TreeNode tn = treeView.Nodes.Add(root, root);
			tn.Checked = true;
			foreach (string p in dirList)
				FillDir(r, tn, p);
			tn.Expand();
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
			lFolder.Text = String.Empty;
			tbName.Text = String.Empty;
			treeView.Nodes.Clear();
			lvExt.Items.Clear();
			if (r == null)
				return;
			lFolder.Text = r.folder;
			tbName.Text = r.name;
			lastBackup = r.name;
			UpdateInfo(r.name);
		}

		void RecToSettings()
		{
			CRec r = GetRec();
			RecToSettings(r);
		}

		CRec SettingsToRec()
		{
			CRec r = new CRec
			{
				folder = lFolder.Text,
				name = tbName.Text
			};
			r.dirList.Clear();
			if (treeView.Nodes.Count > 0)
				r.dirList = GetDirList(treeView.Nodes[0], String.Empty);
			r.extList = GetCheckedExt();
			return r;
		}

		void CreateRec(string folder)
		{
			lastFolder = folder;
			string name = Path.GetFileName(folder);
			lastBackup = name;
			CRec r = new CRec(name) { folder = folder.Trim('\\') };
			recList.Add(r);
			recList.SaveToIni();
			UpdateList(name);
		}

		private void bNew_Click(object sender, EventArgs e)
		{
			ClickNew();
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

		private void FormBackup_FormClosing(object sender, FormClosingEventArgs e)
		{
			SaveToIni();
			ini.Save();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			CMsg m = synMsg.GetMsg();
			bBackup.Enabled = (fileList.Count > 0) && m.listFinished;
			if (m.progress > 0)
			{
				if (m.progress == 2)
				{
					tssInfo.Text = m.msg;
					m.msg = String.Empty;
					m.progress = 0;
					BackupsLimit(m.name);
					synMsg.SetMsg(m);
					lvBackups.Focus();
				}
				progressBar.Value = Convert.ToInt32(progressBar.Maximum * m.progress);
			}
			if (m.msg != String.Empty)
			{
				tssInfo.Text = m.msg;
				m.msg = String.Empty;
				synMsg.SetMsg(m);
			}
			CRec r = GetRec();
			if (r == null)
				return;
			if (r.name != String.Empty)
			{
				if (m.listDone && !m.listFinished && (m.name == r.name) && (m.folder == r.folder))
				{
					fileList = new List<string>(m.files);
					FillDir(r, m.dir);
					FillExt(r, m.ext);
					UpdateInfo(r.name);
					m.listFinished = true;
					m.files.Clear();
					m.dir.Clear();
					m.ext.Clear();
					synMsg.SetMsg(m);
				}
				else if ((!m.listStarted || m.listDone) && (m.name != r.name))
				{
					fileList.Clear();
					m.name = r.name;
					m.folder = r.folder;
					m.stop = false;
					m.listStarted = true;
					m.listDone = false;
					m.listFinished = false;
					synMsg.SetMsg(m);
					FillList(r.folder);
				}
				else if (!m.stop && (m.folder != r.folder))
				{
					m.stop = true;
					synMsg.SetMsg(m);
				}

			}
			if (++timerCounter > 10)
			{
				timerCounter = 0;
				statusStrip.Update();
			}

		}//timer1_Tick

		private void lvBackups_SelectedIndexChanged(object sender, EventArgs e)
		{
			RecToSettings();
		}

	}
}
