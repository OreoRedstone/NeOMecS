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
            Multiselect = false,
            DefaultExt = "simulation",
            Filter = "Simulation Files (*.simulation)|*.simulation|All files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == false) return;
        
        try
        {
            var simulation = SaveLoadSystem.DecodeSimulation(SaveLoadSystem.Load(openFileDialog.FileName));
            var window = new SimulationWindow(simulation, openFileDialog.FileName);
            Application.Current.MainWindow = window;
            window.Show();
            Close();
        }
        catch (System.Exception)
        {
            MessageBox.Show("Error while loading file. The file may be unreadable.");
        }
    }

    private void NewSimulationButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new SimulationWindow(new SimState(), "");
        Application.Current.MainWindow = window;
        window.Show();
        Close();
    }
}
