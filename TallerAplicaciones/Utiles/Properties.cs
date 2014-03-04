using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.utiles
{
    public class Properties
    {
        private Dictionary<String, String> _list;
        private String _filename;

        public Properties(String file)
        {
            Reload(file);
        }

        public String Get(String field, String defValue)
        {
            return (Get(field) == null) ? (defValue) : (Get(field));
        }
        public String Get(String field)
        {
            return (_list.ContainsKey(field)) ? (_list[field]) : (null);
        }

        public void Set(String field, Object value)
        {
            if (!_list.ContainsKey(field))
                _list.Add(field, value.ToString());
            else
                _list[field] = value.ToString();
        }

        public bool ContainsKey(string key)
        {
            return _list.ContainsKey(key);
        }

        public void Save()
        {
            Save(this._filename);
        }

        public void Save(String filename)
        {
            this._filename = filename;

            if (!System.IO.File.Exists(filename))
                System.IO.File.Create(filename);

            var file = new System.IO.StreamWriter(filename);

            foreach (String prop in _list.Keys.ToArray())
                //  if (!String.IsNullOrWhiteSpace(list[prop]))
                file.WriteLine(prop + "=" + _list[prop]);

            file.Close();
        }

        public List<string> GetRegisteredUsers()
        {
            List<string> result = new List<string>();
            result.AddRange(_list.Keys);
            return result;
        }

        public void Reload()
        {
            Reload(this._filename);
        }

        public void Reload(String filename)
        {
            this._filename = filename;
            _list = new Dictionary<String, String>();

            if (System.IO.File.Exists(filename))
                loadFromFile(filename);
            else
                System.IO.File.Create(filename);
        }

        private void loadFromFile(String file)
        {
            foreach (String line in System.IO.File.ReadAllLines(file))
            {
                if ((!String.IsNullOrEmpty(line)) &&
                    (!line.StartsWith(";")) &&
                    (!line.StartsWith("#")) &&
                    (!line.StartsWith("'")) &&
                    (line.Contains('=')))
                {
                    int index = line.IndexOf('=');
                    String key = line.Substring(0, index).Trim();
                    String value = line.Substring(index + 1).Trim();

                    if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                        (value.StartsWith("'") && value.EndsWith("'")))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }

                    try
                    {
                        //ignore dublicates
                        _list.Add(key, value);
                    }
                    catch { }
                }
            }
        }

        public int Count { get { return _list.Count; } }

        public override string ToString()
        {
            var res = new StringBuilder();
            foreach (String prop in _list.Keys.ToArray())
                //  if (!String.IsNullOrWhiteSpace(list[prop]))
                res.Append(prop + "=" + _list[prop] + "\r\n");
            return res.ToString();
        }

    }
}
