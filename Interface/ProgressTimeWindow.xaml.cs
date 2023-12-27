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
using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.Runtime.InteropServices;

namespace NeOMecS.Interface
{
    /// <summary>
    /// Interaction logic for ProgressTimeWindow.xaml
    /// </summary>
    public partial class ProgressTimeWindow : Window
    {
        SimState simulation;

        double frequency = 0;
        double timePeriod = 0;

        public ProgressTimeWindow(SimState simulation)
        {
            InitializeComponent();
            this.simulation = simulation;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
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
            BackgroundWorker worker = new();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            var simWindow = (SimulationWindow)Application.Current.MainWindow;
            simWindow.UpdateBodySidebar();
            Close();
        }

        private void worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            double length = frequency * timePeriod;
            worker.ReportProgress(0);
            for (double i = 0; i < length; i++)
            {
                simulation = SimulationPhysics.SimulateStep(simulation, 1 / frequency);
                //Thread.Sleep(1);
                double progress = i / length * 100;
                worker.ReportProgress((int)progress);
            }
            worker.ReportProgress(100);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
