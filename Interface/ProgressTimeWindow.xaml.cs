using NeOMecS.Physics;
using NeOMecS.Utilities;
using System;
using System.Windows;
using System.ComponentModel;
using System.Diagnostics;

namespace NeOMecS.Interface
{
    /// <summary>
    /// Interaction logic for ProgressTimeWindow.xaml
    /// </summary>
    public partial class ProgressTimeWindow
    {
        SimState simulation;

        double frequency;
        double timePeriod;

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

            worker = new()
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            worker.Dispose();

            if (e.Cancelled)
            {
                return;
            }

            var simWindow = (SimulationWindow)Application.Current.MainWindow;
            simWindow.simulation = new SimState(simulation);
            simWindow.UpdateBodySidebar();
            CurrentItemCount.Text = "Finished.";
            ProgressBar.Value = 100;
        }

        private void worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
            switch(e.UserState)
            {
                case TimeSpan timeSpanState:
                    CurrentItemCount.Text = "ETA: " + timeSpanState.Days + "d " + timeSpanState.Hours + "h " + timeSpanState.Minutes + "m " + timeSpanState.Seconds + "s";
                    break;
                case string stringState:
                    CurrentItemCount.Text = stringState;
                    break;
            }
        }

        private void worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            var sw = Stopwatch.StartNew();
            var localSim = new SimState(simulation);
            var localFrequency = frequency;
            var length = frequency * timePeriod;
            for (int i = 0; i < length; i++)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                localSim = SimulationPhysics.SimulateStepForFixedProgress(localSim, 1 / localFrequency);

                if (sw.ElapsedTicks % 1000 != 0)
                    break;

                try
                {
                    TimeSpan eta = (sw.Elapsed / i) * (length - i);
                    double progress = i / length * 100;
                    worker.ReportProgress(Convert.ToInt32(progress), eta);
                }
                catch (Exception)
                {
                    double progress = i / length * 100;
                    worker.ReportProgress(Convert.ToInt32(progress), "ETA too large to be calculated.");
                }
            }
            if(!worker.CancellationPending) simulation = new SimState(localSim);
            sw.Stop();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
            CurrentItemCount.Text = "Cancelled.";
        }
    }
}
