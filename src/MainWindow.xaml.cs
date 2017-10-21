using MahApps.Metro.Controls;
using System;
using System.Collections.Specialized;
using System.Windows;

namespace gauthUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
	{
		private AuthKeyConfig _config = null;
		private string _filename = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".gauth", "keys.json");
        private UpdateModel _updateModel = new UpdateModel();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
            if (_updateModel.NeedsUpdate())
            {
                var updateWindow = new UpdateWindow(_updateModel);
                updateWindow.ShowDialog();

                if (updateWindow.DialogResult.HasValue && updateWindow.DialogResult.Value)
                {
                    Application.Current.Shutdown();
                    return;
                }
            }

			_config = new AuthKeyConfig();
			_config.Keys.CollectionChanged += new NotifyCollectionChangedEventHandler(AuthKeys_CollectionChanged);
			_config.Load(_filename);
		}

		private void AuthKeys_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				stackPanel1.Children.Add(new CodeControl(e.NewItems[0] as AuthKey, _config));
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				stackPanel1.Children.RemoveAt(e.OldStartingIndex);
			}
		}

		private void MenuItem_Add_Click(object sender, RoutedEventArgs e)
		{
			var form = new AddCodeWindow(_config);
			form.ShowDialog();
		}
	}
}
