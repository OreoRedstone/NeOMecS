using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
        colour[1] = blue;
        colour[2] = green;
        return colour;
    }
}