using NeOMecS.Physics;
using NeOMecS.Utilities;
using SharpGL;
using SharpGL.WPF;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NeOMecS.Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Renderer renderer = new Renderer();
        private Body? selectedObject;

        public MainWindow()
        {
            InitializeComponent();
            Body earth = new Body("Earth", 100, new Colour(0, 1, 0), new Vector2(0, 0), Vector2.Zero, Vector2.Zero, 1, "Sun");
        }

        private void OpenGLControl_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
        {
            renderer.RenderFrame(sender, args);
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
                if (body != null)
                {
                    //Creates a new row, configuring it to the correct grid and height.
                    RowDefinition row = new RowDefinition();
                    row.Height = new GridLength(20);
                    BodySidebarGrid.RowDefinitions.Add(new RowDefinition());

                    //Creates a new textblock, configuring it to the right text, alignment, name, grid and row.
                    TextBlock text = new TextBlock();
                    text.Text = body.name;
                    text.Name = body.name;
                    text.VerticalAlignment = VerticalAlignment.Center;
                    BodySidebarGrid.Children.Add(text);
                    text.SetValue(Grid.RowProperty, i);

                    //Increases the row count.
                    i++;
                } 
            }
        }

        public Vector2 GetRenderWindowSize()
        {
            Vector2 size = new Vector2(RenderWindow.ActualWidth, RenderWindow.ActualHeight);
            return size;
        }
    }
}