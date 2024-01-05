using NeOMecS.Interface;
using NeOMecS.Utilities;
using System.IO;
using System.Windows;

namespace NeOMecS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string[] args = e.Args;
            if(args.Length >= 1)
            {
                foreach (var arg in args)
                {
                    if(File.Exists(arg))
                    {
                        var window = new SimulationWindow(SaveLoadSystem.DecodeSimulation(SaveLoadSystem.Load(arg)), arg);
                        window.Show();
                        break;
                    }
                }
            }
            else
            {
                var window = new StartupWindow();
                window.Show();
            }
        }
    }
}
