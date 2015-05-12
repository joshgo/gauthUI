using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using GoogleAuthenticator;

namespace gauthUI
{
	public class AuthKey
	{
		private GoogleAuthenticator.PasscodeGenerator _generator = null;

		public AuthKey(string name, string key)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");

			if (string.IsNullOrWhiteSpace(key))
				throw new ArgumentNullException("key");

			Name = name;
			Key = key;

			var bytes = Base32String.Instance.Decode(key);
			_generator = new PasscodeGenerator(new HMACSHA1(bytes));
		}

		public string Name { get; set; }
		public string Key { get; set; }

		public string GetCode()
		{
			if (_generator == null)
			{
				var bytes = Base32String.Instance.Decode(Key);
				_generator = new PasscodeGenerator(new HMACSHA1(bytes));
			}
			return _generator.GenerateTimeoutCode();
		}
	}
}
