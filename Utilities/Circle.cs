using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeOMecS.Utilities
{
    public class Circle
    {
        public int count;
        public float[] verticies = new float[362 * 3];

        public Circle(double x, double y, double radius, int triangleCount)
        {
            verticies = CircleGenerator(x, y, radius, triangleCount);
        }

        public float[] CircleGenerator(double x, double y, double radius, int triangleCount)
        {
            float[] circleArray = new float[triangleCount * 3];

            circleArray[0] = (float)x;
            circleArray[1] = (float)y;
            circleArray[2] = -5;
            for (int i = 1; i < triangleCount; i++)
            {
                double heading = i * Math.PI * 2 / triangleCount;
                circleArray[i * 3] = (float)(x + (Math.Cos(heading) * radius));
                circleArray[i * 3 + 1] = (float)(y + (Math.Sin(heading) * radius));
                circleArray[i * 3 + 2] = -5;
            }
            circleArray[triangleCount * 3 - 3] = circleArray[3];
            circleArray[triangleCount * 3 - 2] = circleArray[4];

            return circleArray;
        }
    }
}
