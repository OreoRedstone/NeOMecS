using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NeOMecS.Utilities
{
    public class Vector2
    {
        public double x;
        public double y;

        /// <summary>
        /// The default initialiser for Vector2.
        /// </summary>
        public Vector2()
        {
            this.x = 0;
            this.y = 0;
        }

        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// The vector equivalent of zero.
        /// </summary>
        public static Vector2 Zero
        {
            get => new Vector2(0, 0);
        }

        /// <summary>
        /// The vector representing (-1, 0).
        /// </summary>
        public static Vector2 Left
        {
            get => new Vector2(-1, 0);
        }

        /// <summary>
        /// The vector representing (1, 0).
        /// </summary>
        public static Vector2 Right
        {
            get => new Vector2(1, 0);
        }

        /// <summary>
        /// The vector representing (0, 1).
        /// </summary>
        public static Vector2 Up
        {
            get => new Vector2(0, 1);
        }

        /// <summary>
        /// The vector representing (0, -1).
        /// </summary>
        public static Vector2 Down
        {
            get => new Vector2(0, -1);
        }

        ///<returns>
        ///The input vector multiplied by the scalar value.
        ///</returns>
        public static Vector2 operator *(Vector2 vector, double scalar)
        {
            return new Vector2(vector.x * scalar, vector.y * scalar);
        }

        ///<returns>
        ///The input vector divided by the scalar value.
        ///</returns>
        public static Vector2 operator /(Vector2 vector, double scalar)
        {
            return new Vector2(vector.x / scalar, vector.y / scalar);
        }

        ///<returns>
        ///The magnitude of the input vector.
        ///</returns>
        public static double GetMagnitude(Vector2 vector)
        {
            double sqrMagnitude = Math.Pow(vector.x, 2) + Math.Pow(vector.y, 2);
            double magnitude = Math.Sqrt(sqrMagnitude);
            return magnitude;
        }

        ///<returns>
        ///The vector in the same direction as the input, but with a magnitude of 1.
        ///</returns>
        public static Vector2 GetNormalised(Vector2 vector)
        {
            double magnitude = GetMagnitude(vector);
            Vector2 normalised = new Vector2(vector.x / magnitude, vector.y / magnitude);
            return normalised;
        }

        ///<returns>
        ///The vector sum of a and b.
        ///</returns>
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        ///<returns>
        ///The vector a - b.
        ///</returns>
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        ///<returns>
        ///The distance between a and b.
        ///</returns>
        public static double GetDistance(Vector2 a, Vector2 b)
        {
            double distance = GetMagnitude(a - b);
            return distance;
        }
    }
}