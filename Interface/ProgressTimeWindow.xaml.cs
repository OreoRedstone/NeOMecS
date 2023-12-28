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
using System.Diagnostics;

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

        BackgroundWorker worker;

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

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
            CurrentItemCount.Text = "Working...";
        }

        private void worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            worker.Dispose();

            if (e.Cancelled)
            {
                CurrentItemCount.Text = "Cancelled.";
                return;
            }

            var simWindow = (SimulationWindow)Application.Current.MainWindow;
            simWindow.simulation = new SimState(simulation);
            simWindow.UpdateBodySidebar();
            CurrentItemCount.Text = "Finished.";
            ProgressBar.Value = 100;
            //Close();
        }

        private void worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            var localSim = new SimState(simulation);
            var localFrequency = frequency;
            double length = frequency * timePeriod;
            for (int i = 0; i < length; i++)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                localSim = SimulationPhysics.SimulateStep(localSim, 1 / localFrequency);
                double progress = i / length * 100;

                if(sw.ElapsedTicks % 1000 == 0) worker.ReportProgress(Convert.ToInt32(progress));
            }
            if(!worker.CancellationPending) simulation = new SimState(localSim);
            sw.Stop();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
        }
    }
}
