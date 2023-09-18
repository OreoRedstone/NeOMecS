using Microsoft.Win32;
using NeOMecS.Physics;
using NeOMecS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NeOMecS.Interface
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoadSimulationButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var window = new SimulationWindow(SaveLoadSystem.Load(openFileDialog.FileName));
                Application.Current.MainWindow = window;
                window.Show();
                Close();
            }
        }

        private void NewSimulationButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new SimulationWindow(new SaveState());
            Application.Current.MainWindow = window;
            window.Show();
            Close();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
