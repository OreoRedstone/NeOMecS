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
    /// Interaction logic for ProgressTimeWindow.xaml
    /// </summary>
    public partial class ProgressTimeWindow : Window
    {
        SimState simulation;

        public ProgressTimeWindow(SimState simulation)
        {
            InitializeComponent();
            this.simulation = simulation;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            double frequency = 0;
            double timePeriod = 0;
            try
            {
                frequency = Convert.ToDouble(FrequencyEntry.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Error while parsing the frequency.");
                return;
            }
            try
            {
                timePeriod = Convert.ToDouble(TimePeriodEntry.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Error while parsing the time period.");
                return;
            }

            for (int i = 0; i < frequency * timePeriod; i++)
            {
                simulation = SimulationPhysics.SimulateStep(simulation, 1 / frequency);
            }

            var simWindow = (SimulationWindow)Application.Current.MainWindow;
            simWindow.UpdateBodySidebar();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
