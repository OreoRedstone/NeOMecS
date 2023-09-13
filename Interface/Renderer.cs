using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NeOMecS.Physics;
using NeOMecS.Utilities;
using SharpGL;
using SharpGL.WPF;
using static System.Math;

namespace NeOMecS.Interface;

class Renderer
{
    private Stopwatch frameTimer = new Stopwatch();
    private double scale = 1;
    public double targetScale = 1;
    private Vector2 windowSize = new Vector2();
    private Vector2 cameraPosition = Vector2.Zero;
    public Vector2 cameraTargetPosition = Vector2.Zero;

    public void RenderFrame(object sender, OpenGLRoutedEventArgs args)
    {
        if (!frameTimer.IsRunning) frameTimer.Start();
        OpenGL gl = args.OpenGL;
        gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
        gl.ClearColor(0, 0, 0, 0);

        scale += (targetScale - scale) * 0.0000008 * frameTimer.ElapsedTicks;
        cameraPosition += (cameraTargetPosition - cameraPosition) * 0.0000008 * frameTimer.ElapsedTicks;
        frameTimer.Restart();

        RecalculateMatrix(gl);

        foreach (Body body in Simulation.GetBodiesAsArray())
        {
            DrawCircle(gl, body, 100);
        }
    }

    public void OnResize(object sender, OpenGLRoutedEventArgs args)
    {
        OpenGL gl = args.OpenGL;

        windowSize = ((MainWindow)Application.Current.MainWindow).GetRenderWindowSize();
    }

    public void DrawCircle(OpenGL gl, Body body, int segments)
    {
        double x = body.position.x;
        double y = body.position.y;
        double radius = body.radius;

        gl.Color(body.colour.GetAsFloatArray());
        gl.Begin(OpenGL.GL_TRIANGLE_FAN);
        gl.Vertex(x, y);
        for (int i = 0; i < segments; i++)
        {
            gl.Vertex(x + (radius * Cos(i * PI * 2 / segments)), y + (radius * Sin(i * PI * 2 / segments)));
        }
        gl.Vertex(x + radius, y);
        gl.End();
    }

    private void RecalculateMatrix(OpenGL gl)
    {
        System.Numerics.Matrix4x4 projMatrix = System.Numerics.Matrix4x4.CreateOrthographicOffCenter(
            (float)(cameraPosition.x - (scale * windowSize.x / 2)),
            (float)(cameraPosition.x + (scale * windowSize.x / 2)),
            (float)(cameraPosition.y - (scale * windowSize.y / 2)),
            (float)(cameraPosition.y + (scale * windowSize.y / 2)),
            -1, 1);

        //Converting from the matrix to the double[] because apparently that's necessary.
        double[] projArray = new double[16];
        projArray[0] = projMatrix.M11;
        projArray[1] = projMatrix.M12;
        projArray[2] = projMatrix.M13;
        projArray[3] = projMatrix.M14;
        projArray[4] = projMatrix.M21;
        projArray[5] = projMatrix.M22;
        projArray[6] = projMatrix.M23;
        projArray[7] = projMatrix.M24;
        projArray[8] = projMatrix.M31;
        projArray[9] = projMatrix.M32;
        projArray[10] = projMatrix.M33;
        projArray[11] = projMatrix.M34;
        projArray[12] = projMatrix.M41;
        projArray[13] = projMatrix.M42;
        projArray[14] = projMatrix.M43;
        projArray[15] = projMatrix.M44;

        gl.MatrixMode(OpenGL.GL_PROJECTION);
        gl.LoadMatrix(projArray);
        gl.MatrixMode(OpenGL.GL_MODELVIEW);
    }
}
