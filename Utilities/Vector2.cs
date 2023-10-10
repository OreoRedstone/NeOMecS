using System;

namespace NeOMecS.Utilities;

public class Vector2 : IEquatable<Vector2>
{
    public double x;
    public double y;
    public Vector2 Normalised => GetNormalised(this);
    public double Magnitude => GetMagnitude(this);

    public double Theta => Angle(this, Vector2.Right);

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
    public static Vector2 Zero => new(0, 0);

    /// <summary>
    /// The vector representing (-1, 0).
    /// </summary>
    public static Vector2 Left => new(-1, 0);

    /// <summary>
    /// The vector representing (1, 0).
    /// </summary>
    public static Vector2 Right => new(1, 0);

    /// <summary>
    /// The vector representing (0, 1).
    /// </summary>
    public static Vector2 Up => new(0, 1);

    /// <summary>
    /// The vector representing (0, -1).
    /// </summary>
    public static Vector2 Down => new(0, -1);

    ///<returns>
    ///The vector sum of a and b.
    ///</returns>
    public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.x + b.x, a.y + b.y);

    ///<returns>
    ///The vector a - b.
    ///</returns>
    public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.x - b.x, a.y - b.y);

    ///<returns>
    ///The input vector multiplied by the scalar value.
    ///</returns>
    public static Vector2 operator *(Vector2 vector, double scalar) => new (vector.x* scalar, vector.y* scalar);
    public static Vector2 operator *(double scalar, Vector2 vector) => new(vector.x * scalar, vector.y * scalar);

    ///<returns>
    ///The input vector divided by the scalar value.
    ///</returns>
    public static Vector2 operator /(Vector2 vector, double scalar) => new(vector.x / scalar, vector.y / scalar);

    // override object.Equals
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        Vector2 vecObj = (Vector2)obj;

        return x == vecObj.x && y == vecObj.y;
    }

    public bool Equals(Vector2? other)
    {
        if(other == null) return false;

        return x == other.x && y == other.y;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
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
        double magnitude = vector.Magnitude;
        if (magnitude == 0) return Zero;

        Vector2 normalised = vector / magnitude;
        return normalised;
    }

    ///<returns>
    ///The distance between a and b.
    ///</returns>
    public static double GetDistance(Vector2 a, Vector2 b)
    {
        double distance = GetMagnitude(a - b);
        return distance;
    }

    public static double DotProduct(Vector2 a, Vector2 b)
    {
        return (a.x * b.x) + (a.y * b.y);
    }

    public static double Angle(Vector2 a, Vector2 b)
    {
        var angle = Math.Acos(DotProduct(a, b) / (a.Magnitude * b.Magnitude));
        return angle;
    }

    public static Vector2 PolarToVector2(double r, double theta)
    {
        return PolarToVector2(r, theta, Vector2.Zero);
    }

    public static Vector2 PolarToVector2(double r, double theta, Vector2 center)
    {
        return new Vector2(r * Math.Cos(theta), r * Math.Sin(theta)) + center;
    }
}