using NeOMecS.Physics;
using NeOMecS.Utilities;
using System;
using System.Windows;

namespace NeOMecS.Interface;

/// <summary>
/// Interaction logic for AddBody.xaml
/// </summary>
public partial class AddBodyWindow : Window
{
    private SimState simulation;

    public AddBodyWindow(SimState simulation)
    {
        InitializeComponent();
        this.simulation = simulation;
    }

    private void OKButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var name = BodyNameBox.Text;
            var radius = Convert.ToDouble(BodyRadiusBox.Text);
            var colour = Colour.HexToColour(BodyColourBox.Text);
            var position = new Vector2(Convert.ToDouble(BodyPositionXBox.Text), Convert.ToDouble(BodyPositionYBox.Text));
            var velocity = new Vector2(Convert.ToDouble(BodyVelocityXBox.Text), Convert.ToDouble(BodyVelocityYBox.Text)); 
            var acceleration = Vector2.Zero;
            var mass = Convert.ToDouble(BodyMassBox.Text);

            var body = new Body(name, radius, colour, position, velocity, acceleration, mass, simulation.universe);
            simulation.universe.AddBody(body);
            ((SimulationWindow)Application.Current.MainWindow).UpdateBodySidebar(simulation.universe.bodies);
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
