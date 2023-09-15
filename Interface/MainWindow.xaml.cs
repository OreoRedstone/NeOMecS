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
    Renderer renderer;
    private Body? selectedObject;
    public Body? followedObject;
    private List<Button> followButtons;
    private List<Key> pressedKeys = new List<Key>();
    private Stopwatch timeSinceLastFrame;

    public MainWindow()
    {
        renderer =  new Renderer();
        followButtons = new();
        timeSinceLastFrame = new();
        timeSinceLastFrame.Start();
        InitializeComponent();

        Simulation.Reset();
        var bodies = Simulation.GetBodiesAsArray();
        UpdateBodySidebar(bodies);

        if (bodies.Length > 0)
        {
            selectedObject = bodies[0];
            followedObject = bodies[0];
            followButtons[0].Visibility = Visibility.Hidden;
        }
        
        var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
        dispatcherTimer.Interval = TimeSpan.FromMilliseconds(10);
        dispatcherTimer.Start();
    }

    private void dispatcherTimer_Tick(object? sender, EventArgs e)
    {
        Simulation.SimulateStep(10);
    }

    private void OpenGLControl_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
    {
        Vector2 cameraMoveAmount = Vector2.Zero;
        if(RenderWindow.IsKeyboardFocused)
        {
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
        }

        if(Vector2.GetMagnitude(cameraMoveAmount) != 0 || followedObject == null)
        {
            foreach (var button in followButtons)
            {
                button.Visibility = Visibility.Visible;
            }
            followedObject = null;

            renderer.cameraTargetPosition += Vector2.GetNormalised(cameraMoveAmount) * timeSinceLastFrame.ElapsedMilliseconds * renderer.targetScale * 0.5;
        }
        else
        {
            renderer.cameraPosition = followedObject.position;
            renderer.cameraTargetPosition = followedObject.position;
        }

        if(!(timeSinceLastFrame.ElapsedMilliseconds > 1000))
        {
            //Simulation.SimulateStep(timeSinceLastFrame.ElapsedMilliseconds);
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
        followButtons.Clear();
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
                Name = body.name.Replace(" ", "_"),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Tag = body.guid
            };
            text.MouseLeftButtonUp += new MouseButtonEventHandler(TextBlockClickCall);
            BodySidebarGrid.Children.Add(text);
            text.SetValue(Grid.RowProperty, i);

            var followText = new TextBlock
            {
                Text = "Follow",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            var followButton = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(1,1,2,1),
                Background = Brushes.White,
                Tag = body.guid,
                Content = followText
            };
            BodySidebarGrid.Children.Add(followButton);
            
            followButton.Click += new RoutedEventHandler(FollowButtonClickCall);
            followButton.SetValue(Grid.RowProperty, i);
            followButtons.Add(followButton);
            
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

    private void FollowButtonClickCall(object sender, RoutedEventArgs e)
    {
        foreach (var button in followButtons)
        {
            if(button == sender)
            {
                button.Visibility = Visibility.Hidden;
                Body body = Simulation.GetBodiesAsArray().Single(b => b.guid == button.Tag.ToString());
                followedObject = body;
            }
            else
            {
                button.Visibility = Visibility.Visible;
            }
        }
    }

    private void UpdateInfoSidebar(Body body)
    {
        InfoSidebarTitle.Text = body.name;
        InfoSidebarMass.Text = Math.Round(body.mass, 1).ToString();
        InfoSidebarSpeed.Text = Math.Round(Vector2.GetMagnitude(body.velocity), 1).ToString();
        InfoSidebarPositionX.Text = Math.Round(body.position.x, 1).ToString();
        InfoSidebarPositionY.Text = Math.Round(body.position.y, 1).ToString();
        InfoSidebarVelocityX.Text = Math.Round(body.velocity.x, 1).ToString();
        InfoSidebarVelocityY.Text = Math.Round(body.velocity.y, 1)  .ToString();
        InfoSidebarAccelerationX.Text = Math.Round(body.acceleration.x, 1).ToString();
        InfoSidebarAccelerationY.Text = Math.Round(body.acceleration.y, 1).ToString();
    }

    public Vector2 GetRenderWindowSize()
    {
        var size = new Vector2(RenderWindow.ActualWidth, RenderWindow.ActualHeight);
        return size;
    }

    private void AddNewBody(object sender, RoutedEventArgs e)
    {
        var body = new Body("Placeholder", 1, new Colour(255, 255, 255), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), 1, "");
        Simulation.AddBody(body);
        UpdateBodySidebar(Simulation.GetBodiesAsArray());
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

    private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (!RenderWindow.IsKeyboardFocused) return;
        renderer.targetScale += e.Delta / -200.0;
        if(renderer.targetScale < 1) renderer.targetScale = 1;
    }

    private void RenderWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        RenderWindow.Focus();
    }
}