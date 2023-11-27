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
        var body = new Body("", 1, new Colour(1, 1, 1), Vector2.Zero, Vector2.Zero, Vector2.Zero, 1, null);
        string name = "";
        double radius = 1;
        Colour colour = new Colour(1, 1, 1);
        Vector2 position = Vector2.Zero;
        Vector2 velocity = Vector2.Zero;
        Vector2 acceleration = Vector2.Zero;
        double mass = 1;

        try
        {
            name = BodyNameBox.Text;
        }
        catch (Exception)
        {
            MessageBox.Show("Error while parsing the body's name.");
            return;
        }

        try
        {
            radius = Convert.ToDouble(BodyRadiusBox.Text);
            if (radius > Int32.MaxValue * 0.5) throw new Exception();
            if (radius <= 0) throw new Exception();
        }
        catch (Exception)
        {
            MessageBox.Show("Error while parsing the body's radius.");
            return;
        }

        try
        {
            colour = Colour.HexToColour(BodyColourBox.Text);
        }
        catch (Exception)
        {
            MessageBox.Show("Error while parsing the body's colour.");
            return;
        }

        try
        {
            position = new Vector2(Convert.ToDouble(BodyPositionXBox.Text), Convert.ToDouble(BodyPositionYBox.Text));
            if (position.x > Int32.MaxValue * 0.5) throw new Exception();
            if (position.y > Int32.MaxValue * 0.5) throw new Exception();
        }
        catch (Exception)
        {
            MessageBox.Show("Error while parsing the body's position.");
            return;
        }

        try
        {
            velocity = new Vector2(Convert.ToDouble(BodyVelocityXBox.Text), Convert.ToDouble(BodyVelocityYBox.Text));
        }
        catch (Exception)
        {
            MessageBox.Show("Error while parsing the body's velocity.");
            return;
        }

        try
        {
            mass = Convert.ToDouble(BodyMassBox.Text);
        }
        catch (Exception)
        {
            MessageBox.Show("Error while parsing the body's mass.");
            return;
        }
        
        try
        {
            body = new Body(name, radius, colour, position, velocity, acceleration, mass, simulation.universe);
        }
        catch (FormatException)
        {
            MessageBox.Show("An error occurred while parsing the text boxes. Please try again.");
        }
        catch (Exception)
        {
            MessageBox.Show("An error occured. Please try again.");
        }

        try
        {
            simulation.universe.AddBody(body);
        }
        catch (Exception)
        {
            MessageBox.Show("An error occured while adding the body to the simulation.");
            return;
        }

        try
        {
            ((SimulationWindow)Application.Current.MainWindow).UpdateBodySidebar();
            Close();
        }
        catch (Exception)
        {
            simulation.universe.bodies.Remove(body);
            MessageBox.Show("An error occured while adding the body to the simulation.");
            return;
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
