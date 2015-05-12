using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace gauthUI
{
	public class AuthKeyException : Exception
	{
		public AuthKeyException(string message) : base(message) { }
	}

	public class AuthKeyConfig
	{
		private ObservableCollection<AuthKey> _keys = new ObservableCollection<AuthKey>();
		private HashSet<string> _keysSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private string _filename = null;

		public AuthKeyConfig()
		{
		}

		public ObservableCollection<AuthKey> Keys { get { return _keys; } }

		public void Add(string name, string key)
		{
			if (_keysSet.Contains(name))
				throw new AuthKeyException(string.Format("Supplied [{0}] already in use", name));

			_keysSet.Add(name);
			_keys.Add(new AuthKey(name, key));
		}

		public void Delete(string name)
		{
			if (!_keysSet.Contains(name))
				return;

			_keysSet.Remove(name);

			for (int i = 0; i < _keys.Count; i++)
			{
				if (string.Equals(_keys[i].Name, name, StringComparison.OrdinalIgnoreCase))
				{
					_keys.RemoveAt(i);
					break;
				}
			}
		}

		public void Save()
		{
            using (var writer = new StreamWriter(_filename))
            {
                writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
            }
		}

		public void Load(string filename)
		{
            if (string.IsNullOrWhiteSpace(filename))
                filename = "gauthUI.config";

            _filename = filename;

			_keys.CollectionChanged -= AuthKeys_CollectionChanged;

            if (!System.IO.File.Exists(filename))
            {
                Save();
            }

			using (var reader = new StreamReader(filename))
			{
				AuthKeyConfig tmp = JsonConvert.DeserializeObject<AuthKeyConfig>(reader.ReadToEnd());
				this.Keys.Clear();
				_keysSet.Clear();

				foreach (var t in tmp.Keys)
				{
					_keysSet.Add(t.Name);
					this.Keys.Add(t);
				}
			}

			_keys.CollectionChanged += AuthKeys_CollectionChanged;
		}

		private void AuthKeys_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Save();
		}
	}
}
