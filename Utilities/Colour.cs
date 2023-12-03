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
        colour[0] = (float)red / 255;
        colour[1] = (float)green / 255;
        colour[2] = (float)blue / 255;
        return colour;
    }

    public static Colour HexToColour(string hex)
    {
        Colour colour = new Colour(255, 255, 255);

        if (hex.Length < 1) return colour;
        if (hex[0] == '#') hex = hex[1..];

        if (hex.Length != 6) return colour;

        hex = hex.ToLower();

        colour.red = hexConverter[hex[0]] * 16 + hexConverter[hex[1]];
        colour.green = hexConverter[hex[2]] * 16 + hexConverter[hex[3]];
        colour.blue = hexConverter[hex[4]] * 16 + hexConverter[hex[5]];

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