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
    /// Interaction logic for AddBody.xaml
    /// </summary>
    public partial class AddBodyWindow : Window
    {
        public AddBodyWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var name = BodyNameBox.Text;
                var radius = Convert.ToDouble(BodyRadiusBox.Text);
                var colour = new Colour(1, 1, 1);
                var position = new Vector2(Convert.ToDouble(BodyPositionXBox.Text), Convert.ToDouble(BodyPositionYBox.Text));
                var velocity = new Vector2(Convert.ToDouble(BodyVelocityXBox.Text), Convert.ToDouble(BodyVelocityYBox.Text)); 
                var acceleration = Vector2.Zero;
                var mass = Convert.ToDouble(BodyMassBox.Text);
                var parent = "";

                var body = new Body(name, radius, colour, position, velocity, acceleration, mass, parent);
                Simulation.simulation.AddBody(body);
                ((SimulationWindow)Application.Current.MainWindow).UpdateBodySidebar(Simulation.simulation.bodies);
                Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("An error occurred while parsing the text boxes. Please try again.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
