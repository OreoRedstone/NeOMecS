using NeOMecS.Physics;
using NeOMecS.Utilities;
using SharpGL;
using SharpGL.WPF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NeOMecS.Interface;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    Renderer renderer = new Renderer();
    private Body? selectedObject;
    private List<Key> pressedKeys = new List<Key>();
    private Stopwatch timeSinceLastFrame;

    public MainWindow()
    {
        timeSinceLastFrame = new();
        timeSinceLastFrame.Start();
        InitializeComponent();

        Simulation.Reset();
        var bodies = Simulation.GetBodiesAsArray();
        UpdateBodySidebar(bodies);

        if (bodies.Length > 0)
        {
            selectedObject = bodies[0];
        }
    }

    private void OpenGLControl_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
    {
        Vector2 cameraMoveAmount = Vector2.Zero;
        foreach (var key in pressedKeys)
        {
            switch (key)
            {
                case Key.W:
                    cameraMoveAmount += Vector2.Up;
                    break;
                case Key.S:
                    cameraMoveAmount += Vector2.Down;
                    break;
                case Key.D:
                    cameraMoveAmount += Vector2.Right;
                    break;
                case Key.A:
                    cameraMoveAmount += Vector2.Left;
                    break;
                default:
                    break;
            }
        }
        renderer.cameraPosition += Vector2.GetNormalised(cameraMoveAmount) * 0.00001 * timeSinceLastFrame.ElapsedTicks;
        if(!(timeSinceLastFrame.ElapsedMilliseconds > 1000))
        {
            Simulation.SimulateStep(timeSinceLastFrame.ElapsedMilliseconds);
        }
        UpdateInfoSidebar(selectedObject);
        renderer.RenderFrame(sender, args);
        timeSinceLastFrame.Restart();
    }

    private void OpenGLControl_Resized(object sender, OpenGLRoutedEventArgs args)
    {
        renderer.OnResize(sender, args);
    }

    /// <summary>
    /// This function updates the left sidebar of the main form to fit the array of bodies passed in.
    /// </summary>
    public void UpdateBodySidebar(Body[] bodies)
    {
        BodySidebarGrid.Children.Clear();
        int i = 0;
        foreach (Body body in bodies)
        {
            if (body == null) continue;

            //Creates a new row, configuring it to the correct grid and height.
            var row = new RowDefinition
            {
                Height = new GridLength(20)
            };
            BodySidebarGrid.RowDefinitions.Add(new RowDefinition());

            //Creates a new textblock, configuring it to the right text, alignment, name, grid and row.
            var text = new TextBlock
            {
                Text = body.name,
                Name = body.name,
                VerticalAlignment = VerticalAlignment.Center,
                Tag = body.guid
            };
            text.MouseLeftButtonUp += new MouseButtonEventHandler(TextBlockClickCall);
            BodySidebarGrid.Children.Add(text);
            text.SetValue(Grid.RowProperty, i);

            //Increases the row count.
            i++;
        }
    }
    
    private void TextBlockClickCall(object sender, MouseButtonEventArgs e)
    {
        TextBlock block = (TextBlock)sender;
        Body body = Simulation.GetBodiesAsArray().Single(b => b.guid == block.Tag.ToString());
        selectedObject = body;
    }


    private void UpdateInfoSidebar(Body body)
    {
        InfoSidebarTitle.Text = body.name;
        InfoSidebarMass.Text = body.mass.ToString();
        InfoSidebarSpeed.Text = body.velocity.Magnitude.ToString();
        InfoSidebarPositionX.Text = body.position.x.ToString();
        InfoSidebarPositionY.Text = body.position.y.ToString();
        InfoSidebarVelocityX.Text = body.velocity.x.ToString();
        InfoSidebarVelocityY.Text = body.velocity.y.ToString();
        InfoSidebarAccelerationX.Text = body.acceleration.x.ToString();
        InfoSidebarAccelerationY.Text = body.acceleration.y.ToString();
    }

    public Vector2 GetRenderWindowSize()
    {
        var size = new Vector2(RenderWindow.ActualWidth, RenderWindow.ActualHeight);
        return size;
    }

    private void AddNewBody(object sender, RoutedEventArgs e)
    {
        new Body("Placeholder", 1, new Colour(255, 255, 255), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), 1, "");
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (pressedKeys.Contains(e.Key)) return;
        pressedKeys.Add(e.Key);
    }

    private void Window_KeyUp(object sender, KeyEventArgs e)
    {
        pressedKeys.Remove(e.Key);
    }

    private void Reset_Button_Click(object sender, RoutedEventArgs e)
    {
        Simulation.Reset();
        var bodies = Simulation.GetBodiesAsArray();
        UpdateBodySidebar(bodies);

        if (bodies.Length > 0)
        {
            selectedObject = bodies[0];
        }
    }
}