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
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace gauthUI
{
	/// <summary>
	/// Interaction logic for AddCode.xaml
	/// </summary>
	public partial class AddCodeWindow : MetroWindow
	{
		private AuthKeyConfig _config = null;

		public AddCodeWindow(AuthKeyConfig config)
		{
			InitializeComponent();

			if (config == null)
				throw new ArgumentNullException("config");

			_config = config;
		}

		private void AddBtn_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(_nameTxt.Text))
			{
				MessageBox.Show("Name is empty!", "Invalid value", MessageBoxButton.OK, MessageBoxImage.Stop);
				return;
			}

			if (string.IsNullOrWhiteSpace(_keyTxt.Text))
			{
				MessageBox.Show("Key is empty!", "Invalid value", MessageBoxButton.OK, MessageBoxImage.Stop);
				return;
			}

			try
			{
				_config.Add(_nameTxt.Text, _keyTxt.Text);
			}
			catch(AuthKeyException ae)
			{
				MessageBox.Show(ae.Message, "Add error", MessageBoxButton.OK, MessageBoxImage.Stop);
				return;
			}

			DialogResult = true;
			Close();
		}
	}
}
