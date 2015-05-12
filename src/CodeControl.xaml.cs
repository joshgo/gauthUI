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
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Threading;

namespace gauthUI
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class CodeControl : UserControl
	{
		private AuthKey _authKey = null;
		private AuthKeyConfig _config = null;
		private Dictionary<int, TimeSpan> _timespans = new Dictionary<int, TimeSpan>();

		public CodeControl(AuthKey key, AuthKeyConfig config)
		{
			InitializeComponent();

			_authKey = key;
			_config = config;
		}

		private void CodeControl_Loaded(object sender, RoutedEventArgs e)
		{
			_timecodeLabel.Content = _authKey.GetCode();
			_nameLabel.Content = _authKey.Name;

			int dur = GetDuration();
			_progressTime.Value = GetProgressValue(dur);

			var animate = new DoubleAnimation(0, new Duration(new TimeSpan(0, 0, dur)));
			animate.FillBehavior = FillBehavior.Stop;

			animate.Completed += new EventHandler((x, y) =>
			{
				Dispatcher.Invoke(new Action(() =>
				{
					dur = GetDuration();
					_progressTime.Value = GetProgressValue(dur);

					if (animate.Duration.TimeSpan.Seconds != dur)
					{
						if (!_timespans.ContainsKey(dur))
							_timespans[dur] = new TimeSpan(0, 0, dur);

						animate.Duration = _timespans[dur];
					}

					_timecodeLabel.Content = _authKey.GetCode();

					_progressTime.BeginAnimation(ProgressBar.ValueProperty, animate);
				}));
			});

			_progressTime.BeginAnimation(ProgressBar.ValueProperty, animate);
		}

		private int GetDuration()
		{
			int sec = DateTime.Now.Second;
			int dur = (30 - (sec % 30));
			return dur;
		}

		private double GetProgressValue(int duration)
		{
			return 100 * duration / 30.0;
		}

		private void MenuItem_Copy_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(_timecodeLabel.Content.ToString());
		}

		private void MenuItem_Add_Click(object sender, RoutedEventArgs e)
		{
			var form = new AddCodeWindow(_config);
			form.ShowDialog();
		}

		private void MenuItem_Delete_Click(object sender, RoutedEventArgs e)
		{
			_config.Delete(_authKey.Name);
		}
	}
}
