using NeOMecS.Interface;
using NeOMecS.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NeOMecS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
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
