using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace gauthUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		private AuthKeyConfig _config = null;
		private string _filename = "gauthUI.config";

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
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
