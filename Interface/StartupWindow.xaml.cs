using Microsoft.Win32;
using NeOMecS.Utilities;
using System.Windows;

namespace NeOMecS.Interface;

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
            var window = new SimulationWindow(SaveLoadSystem.Decode(SaveLoadSystem.Load(openFileDialog.FileName)), openFileDialog.FileName);
            Application.Current.MainWindow = window;
            window.Show();
            Close();
        }
    }

    private void NewSimulationButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new SimulationWindow(new SimState(), "");
        Application.Current.MainWindow = window;
        window.Show();
        Close();
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {

    }
}
