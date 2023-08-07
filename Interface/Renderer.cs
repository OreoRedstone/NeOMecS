using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeOMecS.Physics;
using NeOMecS.Utilities;
using SharpGL;

namespace NeOMecS.Interface
{
    class Renderer
    {
        private Vector2 resolution = new Vector2();

        public void RenderFrame(object sender, SharpGL.WPF.OpenGLRoutedEventArgs args)
        {
            Circle circle = new Circle(100, 100, 100, 100);
            circle.verticies = circle.CircleGenerator(100, 100, 100, 100);

            OpenGL gl = args.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.ClearColor(1, 1, 1, 1);
            gl.LoadIdentity();
        }

        public void DrawCircle(OpenGL gl, Body body, Circle circle)
        {
            gl.Color(body.GetColour().GetAsFloatArray());
            gl.Begin(OpenGL.GL_TRIANGLE_FAN);
            for (int i = 0; i < circle.verticies.Length / 2; i++)
            {
                gl.Vertex(circle.verticies[i], circle.verticies[i + 1]);
            }
            gl.End();
        }
    }
}
