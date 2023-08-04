using NeOMecS.Physics;
using SharpGL;
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
        private Body? selectedObject;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.WPF.OpenGLRoutedEventArgs args)
        {
            Simulation.SimulateStep();
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
                    text.Text = body.GetName();
                    text.VerticalAlignment = VerticalAlignment.Center;
                    text.Name = body.GetName();
                    BodySidebarGrid.Children.Add(text);
                    text.SetValue(Grid.RowProperty, i);

                    //Increases the row count.
                    i++;
                } 
            }
        }
    }
}