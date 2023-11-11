using System.Collections.Generic;

namespace NeOMecS.Utilities;

public class Colour
{
    public int red;
    public int blue;
    public int green;

    public Colour(int red, int green, int blue)
    {
        this.red = red;
        this.blue = blue;
        this.green = green;
    }

    public float[] GetAsFloatArray()
    {
        float[] colour = new float[3];
        colour[0] = red;
        colour[1] = green;
        colour[2] = blue;
        return colour;
    }

    public static Colour HexToColour(string hex)
    {
        if (hex[0] == '#') hex = hex[1..];

        Colour colour = new Colour(0, 0, 0);

        if (hex.Length != 6) return colour;

        colour.red = (hexConverter[hex[0]] * 16 + hexConverter[hex[1]]) / 255;
        colour.green = (hexConverter[hex[2]] * 16 + hexConverter[hex[3]]) / 255;
        colour.blue = (hexConverter[hex[4]] * 16 + hexConverter[hex[5]]) / 255;

        return colour;
    }

    private static Dictionary<char, int> hexConverter = new Dictionary<char, int>()
    {
        {'0', 0 }, 
        {'1', 1 }, 
        {'2', 2 }, 
        {'3', 3 }, 
        {'4', 4 },
        {'5', 5 },
        {'6', 6 },
        {'7', 7 },
        {'8', 8 },
        {'9', 9 },
        {'a', 10 },
        {'b', 11 },
        {'c', 12 },
        {'d', 13 },
        {'e', 14 },
        {'f', 15 }
    };
}