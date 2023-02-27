using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RapIni;

namespace RapBackup
{
	class CRec
	{
		public string name = string.Empty;
		public string folder=string.Empty;
		public List<string> dirList = new List<string>();
		public List<string> extList = new List<string>();

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
			FormBackup.ini.Write($"name>{name}>folder",folder);
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

	}

	class CRecList:List<CRec>
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
			FormBackup.ini.Save();
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

		}

}
