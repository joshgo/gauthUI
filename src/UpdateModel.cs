using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace gauthUI
{
    public class UpdateModel
    {
        public delegate void UpdateEvent(string message);

        public event UpdateEvent OnUpdateEvent;
        private readonly string UpdateTitle01 = "Move gauthUI.config to user directory";
        private readonly List<Func<bool>> _updateFuncs = new List<Func<bool>>();
        private readonly List<string> _updateTitles = new List<string>();

        public UpdateModel()
        {
        }

        public bool NeedsUpdate()
        {
            _updateFuncs.Clear();
            _updateTitles.Clear();

            // Does the gauthUI.config exists next to this assemlby.
            //   If so, an update is needed and please move the
            //   file from current location to c:\users\%name%\.gauth\keys.json
            if (File.Exists("gauthUI.config"))
            {
                _updateTitles.Add(UpdateTitle01);
                _updateFuncs.Add(UpdateFn01);
            }

            return _updateTitles.Count() > 0;
        }

        public bool RunUpdates()
        {
            while(_updateFuncs.Count() > 0)
            {
                var fn = _updateFuncs.First();

                if (!fn())
                    return false;

                _updateTitles.RemoveAt(0);
                _updateFuncs.RemoveAt(0);
            }

            return true;
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<string> Updates()
        {
            return _updateTitles.AsReadOnly();
        }

        private bool UpdateFn01()
        {
            var srcFile = "gauthUI.config";
            var destDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".gauth");
            var destFile = Path.Combine(destDir, "keys.json");

            // Make directory c:/users/name/.gauth/
            // Copy file to  keys.json
            OnUpdate("Moving [{0}] to [{1}]", srcFile, destFile);
            Directory.CreateDirectory(destDir);
            OnUpdate("Created [{0}]", destDir);

            if (File.Exists(destFile))
            {
                var choice = MessageBox.Show(string.Format("[{0}] already exists. Overwrite?", destFile), "Overwrite?", MessageBoxButton.YesNo);
                if (choice == MessageBoxResult.No)
                    return false;
            }

            File.Copy(srcFile, destFile, true);
            File.Delete(srcFile);
            OnUpdate("[{0}] moved to [{1}]", srcFile, destFile);
            return true;
        }

        private void OnUpdate(string message, params object[] vals)
        {
            OnUpdateEvent?.Invoke(string.Format(message, vals));
        }
    }
}
