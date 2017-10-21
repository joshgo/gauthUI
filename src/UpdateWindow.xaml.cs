using MahApps.Metro.Controls;
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

namespace gauthUI
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : MetroWindow
    {
        private readonly UpdateModel _updateModel = null;
        public UpdateWindow(UpdateModel model)
        {
            InitializeComponent();
            _updateModel = model;
            _updateModel.OnUpdateEvent += OnUpdateEventHandler;
        }

        public UpdateWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(var u in _updateModel.Updates())
            {
                txtUpdates.AppendText(u);
                txtUpdates.AppendText(Environment.NewLine);
             }
        }
        private void UpdateBtnClick(object sender, RoutedEventArgs e)
        {
            _updateModel.RunUpdates();
        }

        private void ButtonOkClick(object sender, RoutedEventArgs e)
        {
            var parameter = OkOrCancelButton.CommandParameter.ToString();

            if (string.Equals(parameter, "Ok", StringComparison.OrdinalIgnoreCase))
                DialogResult = true;
            else
                DialogResult = false;
        }


        private void OnUpdateEventHandler(string message)
        {
            txtUpdates.Dispatcher.Invoke(new Action<string>(x =>
            {
                txtUpdates.AppendText(x + Environment.NewLine);
            }), message);
        }

    }
}
