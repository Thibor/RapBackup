using System.IO;
using System.Collections.Generic;

namespace RapBackup
{

	class CMsg
	{
		public double progress = 0;
		public string msg = string.Empty;

		public CMsg()
		{

		}

		public CMsg(CMsg m)
		{
			Assign(m);
		}

		public void Assign(CMsg m)
		{
			progress = m.progress;
			msg = m.msg;
		}

	}

	class CSynMsg
	{
		private static CMsg msg = new CMsg();
		private readonly object locker = new object();

		public CMsg GetMsg()
		{
			lock (locker)
			{
				return new CMsg(msg);
			}
		}

		public void SetMsg(CMsg m)
		{
			lock (locker)
			{
				msg.Assign(m);
			}
		}

	}

	class CRec
	{
		public string name = string.Empty;
		public string folder = string.Empty;
		public List<string> dirList = new List<string>();
		public List<string> extList = new List<string>();

		public string Root
		{
			get
			{
				string root = Path.GetFileName(folder);
				if (string.IsNullOrEmpty(root))
					return string.Empty;
				else
					return $@"{root}\";
			}
		}

		public CRec()
		{

		}

		public CRec(string name)
		{
			this.name = name;
		}

		public void SaveToIni()
		{
			FormBackup.ini.DeleteKey($"name>{name}");
			FormBackup.ini.Write($"name>{name}>folder", folder);
			FormBackup.ini.Write($"name>{name}>ext", extList);
			foreach (string d in dirList)
				FormBackup.ini.Write($"name>{name}>dir>{d}");
		}

		public void LoadFromIni()
		{
			folder = FormBackup.ini.Read($"name>{name}>folder");
			extList = FormBackup.ini.ReadListStr($"name>{name}>ext");
			dirList = FormBackup.ini.ReadKeyList($"name>{name}>dir");
		}

		string CreatePath(string path)
		{
			return $@"{folder}\{path}";
		}

		public bool ExtOk(string ext)
		{
			foreach (string e in extList)
				if (ext.ToLower() == e)
					return true;
			return false;
		}

		public bool PathOk(string path)
		{
			string ext = Path.GetExtension(path);
			if (!ExtOk(ext))
				return false;
			path = Path.GetDirectoryName(path);
			if (path == folder)
				return true;
			foreach (string p in dirList)
				if (path == CreatePath(p))
					return true;
			return false;
		}

		public string CreateShortFile(string path)
		{
			if (path == folder)
				return string.Empty;
			return path.Substring(folder.Length + 1);
		}

		public string CreateShortDir(string path)
		{
			path = Path.GetDirectoryName(path);
			return CreateShortFile(path);
		}

	}

	class CRecList : List<CRec>
	{

		public CRecList()
		{

		}

		public CRec GetRec(string name)
		{
			foreach (CRec r in this)
				if (r.name == name)
					return r;
			return null;
		}

		public void SaveToIni()
		{
			FormBackup.ini.DeleteKey("name");
			foreach (CRec rec in this)
				rec.SaveToIni();
		}

		public void LoadFromIni()
		{
			Clear();
			List<string> en = FormBackup.ini.ReadKeyList("name");
			foreach (string name in en)
			{
				CRec rec = new CRec(name);
				rec.LoadFromIni();
				Add(rec);
			}
		}

		public bool NameExists(CRec rec, string name)
		{
			if (string.IsNullOrEmpty(name))
				return true;
			foreach (CRec r in this)
				if ((r != rec) && (r.name == name))
					return true;
			return false;
		}

		public string CreateUniqueName(CRec rec)
		{
			string name = rec.name;
			string result = name;
			int i = 1;
			while (NameExists(rec, result))
				result = $"{name} ({++i})";
			return result.Trim();
		}

	}

}
