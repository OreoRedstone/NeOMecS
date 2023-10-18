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
using System.Windows.Threading;

namespace NeOMecS.Interface;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class SimulationWindow : Window
{
    Renderer renderer;
    private Body? selectedObject;
    public Body? followedObject;
    private List<Button> followButtons;
    private List<Key> pressedKeys = new List<Key>();
    private Stopwatch timeSinceLastFrame;
    public SimulationPlayState playState = SimulationPlayState.Stopped;
    private SimState stoppedState { get; set; }

    private Body previouslySelectedBody;

    public SimulationWindow(SimState state)
    {
        renderer =  new Renderer();
        followButtons = new();
        timeSinceLastFrame = Stopwatch.StartNew();
        InitializeComponent();

        Simulation.simulation.universe.AddBody(new Body("Earth", 10, new Colour(0, 1, 0), new Vector2(1000, 0), new Vector2(0, 300), Vector2.Zero, 100000000));
        Simulation.simulation.universe.AddBody(new Body("The Sun", 100, new Colour(1, 0, 0), new Vector2(0, 0), Vector2.Zero, Vector2.Zero, -10000000000));

        var bodies = Simulation.simulation.universe.bodies;
        UpdateBodySidebar(bodies);

        if (bodies.Count > 0)
        {
            selectedObject = bodies[0];
            followedObject = bodies[0];
            followButtons[0].Visibility = Visibility.Hidden;
        }
        
        var dispatcherTimer = new DispatcherTimer();
        dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
        dispatcherTimer.Interval = TimeSpan.FromMilliseconds(10);
        dispatcherTimer.Start();

        UpdateInfoSidebarValues(selectedObject);
    }

    private void dispatcherTimer_Tick(object? sender, EventArgs e)
    {
        if(playState != SimulationPlayState.Playing) return;
        Simulation.simulation = SimulationPhysics.SimulateStep(Simulation.simulation);
    }

    private void OpenGLControl_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
    {
        Simulation.simulation = SimulationPhysics.CalculateAccelerations(Simulation.simulation, this);
        Vector2 cameraMoveAmount = Vector2.Zero;
        if(RenderWindow.IsKeyboardFocused || RenderWindow.IsMouseOver)
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

            renderer.cameraTargetPosition += Vector2.GetNormalised(cameraMoveAmount) * timeSinceLastFrame.ElapsedTicks * renderer.targetScale * 0.0001;
        }
        else
        {
            renderer.cameraTargetPosition = followedObject.position;
            if (Vector2.GetDistance(renderer.cameraPosition, followedObject.position) < followedObject.velocity.Magnitude)
            {
                renderer.cameraPosition = followedObject.position;
            }
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
    public void UpdateBodySidebar(List<Body> bodies)
    {
        followButtons.Clear();
        BodySidebarGrid.Children.Clear();
        BodySidebarGrid.RowDefinitions.Clear();
        int i = 0;
        bodies = Simulation.simulation.universe.GetBodiesOrdered();
        foreach (Body body in bodies)
        {
            if (body == null) continue;

            //Creates a new row, configuring it to the correct grid and height.
            var row = new RowDefinition
            {
                Height = new GridLength(20)
            };
            BodySidebarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20)});

            var bodyEntry = new Button
            {
                Content = body.name,
                Name = body.name.Replace(" ", "_"),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Tag = body.guid,
                Style = (Style)FindResource("LeftPanelBodyButton"),
                Margin = new Thickness(body.GetParentNestingCount(Simulation.simulation.universe) * 10, 1, 1, 1),
            };
            bodyEntry.Click += new RoutedEventHandler(LeftPanelBodyButtonCall);
            BodySidebarGrid.Children.Add(bodyEntry);
            bodyEntry.SetValue(Grid.RowProperty, i);

            var followButton = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Style = (Style)FindResource("LeftPanelBodyButton"),
                Content = "Follow",
                Margin = new Thickness(1),
                Tag = body.guid,
            };

            BodySidebarGrid.Children.Add(followButton);

            followButton.Click += new RoutedEventHandler(FollowButtonClickCall);
            followButton.SetValue(Grid.RowProperty, i);
            followButtons.Add(followButton);
            //Increases the row count.
            i++;
        }
    }
    
    private void LeftPanelBodyButtonCall(object sender, RoutedEventArgs e)
    {
        Button block = (Button)sender;
        Body body = Simulation.simulation.universe.bodies.Single(b => b.guid == block.Tag.ToString());
        selectedObject = body;
    }

    private void FollowButtonClickCall(object sender, RoutedEventArgs e)
    {
        foreach (var button in followButtons)
        {
            if(button == sender)
            {
                button.Visibility = Visibility.Hidden;
                Body body = Simulation.simulation.universe.bodies.Single(b => b.guid == button.Tag.ToString());
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
        if(body != previouslySelectedBody)
        {
            UpdateInfoSidebarValues(body);
        }


        if(playState == SimulationPlayState.Paused || playState == SimulationPlayState.Stopped)
        {
            InfoSidebarTitle.SetValue(TextBox.IsReadOnlyProperty, false);
            InfoSidebarMass.SetValue(TextBox.IsReadOnlyProperty, false);
            InfoSidebarPositionX.SetValue(TextBox.IsReadOnlyProperty, false);
            InfoSidebarPositionY.SetValue(TextBox.IsReadOnlyProperty, false);
            InfoSidebarVelocityX.SetValue(TextBox.IsReadOnlyProperty, false);
            InfoSidebarVelocityY.SetValue(TextBox.IsReadOnlyProperty, false);

            InfoSidebarTitle.SetValue(Border.BorderThicknessProperty, new Thickness(1));
            InfoSidebarMass.SetValue(Border.BorderThicknessProperty, new Thickness(1));
            InfoSidebarPositionX.SetValue(Border.BorderThicknessProperty, new Thickness(1));
            InfoSidebarPositionY.SetValue(Border.BorderThicknessProperty, new Thickness(1));
            InfoSidebarVelocityX.SetValue(Border.BorderThicknessProperty, new Thickness(1));
            InfoSidebarVelocityY.SetValue(Border.BorderThicknessProperty, new Thickness(1));

            InfoSidebarSpeed.Text = Math.Round(Vector2.GetMagnitude(body.velocity), 1).ToString();
            InfoSidebarAccelerationX.Text = Math.Round(body.acceleration.x, 1).ToString();
            InfoSidebarAccelerationY.Text = Math.Round(body.acceleration.y, 1).ToString();
            InfoSidebarParent.Text = body.parent.name;

            try
            {
                body.name = InfoSidebarTitle.Text;
                var mass = Convert.ToDouble(InfoSidebarMass.Text);
                if (mass == 0) throw new Exception();
                body.mass = mass;
                body.position.x = Convert.ToDouble(InfoSidebarPositionX.Text);
                body.position.y = Convert.ToDouble(InfoSidebarPositionY.Text);
                body.velocity.x = Convert.ToDouble(InfoSidebarVelocityX.Text);
                body.velocity.y = Convert.ToDouble(InfoSidebarVelocityY.Text);
            }
            catch (Exception)
            {

            }

            bool shouldUpdate = false;
            foreach (var currentBody in Simulation.simulation.universe.bodies)
            {
                bool currentBodyHasMatch = false;
                foreach (var displayedBodyButton in BodySidebarGrid.Children)
                {
                    var displayedBodyName = (string)((Button)displayedBodyButton).Content;
                    if (displayedBodyName == currentBody.name) currentBodyHasMatch = true;
                }
                if (!currentBodyHasMatch) shouldUpdate = true;
            }

            if(shouldUpdate)
            {
                UpdateBodySidebar(Simulation.simulation.universe.bodies);
            }
        }
        else
        {
            UpdateInfoSidebarValues(body);
        }

        previouslySelectedBody = body;
    }

    private void UpdateInfoSidebarValues(Body body)
    {
        InfoSidebarTitle.SetValue(Border.BorderThicknessProperty, new Thickness(0));
        InfoSidebarMass.SetValue(Border.BorderThicknessProperty, new Thickness(0));
        InfoSidebarPositionX.SetValue(Border.BorderThicknessProperty, new Thickness(0));
        InfoSidebarPositionY.SetValue(Border.BorderThicknessProperty, new Thickness(0));
        InfoSidebarVelocityX.SetValue(Border.BorderThicknessProperty, new Thickness(0));
        InfoSidebarVelocityY.SetValue(Border.BorderThicknessProperty, new Thickness(0));

        InfoSidebarTitle.SetValue(TextBox.IsReadOnlyProperty, false);
        InfoSidebarMass.SetValue(TextBox.IsReadOnlyProperty, false);
        InfoSidebarPositionX.SetValue(TextBox.IsReadOnlyProperty, false);
        InfoSidebarPositionY.SetValue(TextBox.IsReadOnlyProperty, false);
        InfoSidebarVelocityX.SetValue(TextBox.IsReadOnlyProperty, false);
        InfoSidebarVelocityY.SetValue(TextBox.IsReadOnlyProperty, false);

        InfoSidebarTitle.Text = body.name;
        InfoSidebarMass.Text = Math.Round(body.mass, 1).ToString();
        InfoSidebarSpeed.Text = Math.Round(Vector2.GetMagnitude(body.velocity), 1).ToString();
        InfoSidebarPositionX.Text = Math.Round(body.position.x, 1).ToString();
        InfoSidebarPositionY.Text = Math.Round(body.position.y, 1).ToString();
        InfoSidebarVelocityX.Text = Math.Round(body.velocity.x, 1).ToString();
        InfoSidebarVelocityY.Text = Math.Round(body.velocity.y, 1).ToString();
        InfoSidebarAccelerationX.Text = Math.Round(body.acceleration.x, 1).ToString();
        InfoSidebarAccelerationY.Text = Math.Round(body.acceleration.y, 1).ToString();
        InfoSidebarParent.Text = body.parent.name;

        InfoSidebarTitle.SetValue(TextBox.IsReadOnlyProperty, true);
        InfoSidebarMass.SetValue(TextBox.IsReadOnlyProperty, true);
        InfoSidebarPositionX.SetValue(TextBox.IsReadOnlyProperty, true);
        InfoSidebarPositionY.SetValue(TextBox.IsReadOnlyProperty, true);
        InfoSidebarVelocityX.SetValue(TextBox.IsReadOnlyProperty, true);
        InfoSidebarVelocityY.SetValue(TextBox.IsReadOnlyProperty, true);
    }

    public Vector2 GetRenderWindowSize()
    {
        var size = new Vector2(RenderWindow.ActualWidth, RenderWindow.ActualHeight);
        return size;
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

    private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (!(RenderWindow.IsKeyboardFocused || RenderWindow.IsMouseOver)) return;
        renderer.targetScale += e.Delta / -200.0;
        if(renderer.targetScale < 1) renderer.targetScale = 1;
    }

    private void RenderWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        RenderWindow.Focus();
    }

    private void AddNewBodyButton_Click(object sender, RoutedEventArgs e)
    {
        if (Application.Current.Windows.Cast<Window>().OfType<AddBodyWindow>().Any()) return;

        AddBodyWindow window = new();
        window.Show();
        window.Activate();
    }

    private void PlayButton_Click(object sender, RoutedEventArgs e)
    {
        if(playState == SimulationPlayState.Stopped)
        {
            SimulationPhysics.stepTimer.Restart();
            Simulation.simulation.cameraPosition = renderer.cameraTargetPosition;
            stoppedState = new SimState(Simulation.simulation);
        }
        playState = SimulationPlayState.Playing;
    }

    private void PauseButton_Click(object sender, RoutedEventArgs e)
    {
        playState = SimulationPlayState.Paused;
    }

    private void StopButton_Click(object sender, RoutedEventArgs e)
    {
        Simulation.simulation = stoppedState;
        renderer.cameraTargetPosition = Simulation.simulation.cameraPosition;
        playState = SimulationPlayState.Stopped;
    }
}

public enum SimulationPlayState { Playing, Paused, Stopped }