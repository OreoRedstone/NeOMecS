using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using NeOMecS.Physics;
using System.Windows.Shapes;

namespace NeOMecS.Utilities;

public static class SaveLoadSystem
{
    public static string EncodeSimulation(SimState state)
    {
        var builder = new StringBuilder();
        builder.Append($"<simSpeed>{state.simSpeed}<simSpeed/>\n");
        builder.Append($"<gravitationalConstant>{state.universe.gravitationalConstant}<gravitationalConstant/>\n");
        builder.Append($"<cameraPosition>{state.cameraPosition.x}, {state.cameraPosition.y}<cameraPosition/>\n");
        builder.Append("<bodies>\n");
        foreach (var body in state.universe.bodies)
        {
            builder.Append("\t<body>\n");
            builder.Append($"\t\t<name>{body.name}<name/>\n");
            builder.Append($"\t\t<radius>{body.radius}<radius/>\n");
            builder.Append($"\t\t<colour>{body.colour.red}, {body.colour.green}, {body.colour.blue}<colour/>\n");
            builder.Append($"\t\t<position>{body.position.x}, {body.position.y}<position/>\n");
            builder.Append($"\t\t<velocity>{body.velocity.x}, {body.velocity.y}<velocity/>\n");
            builder.Append($"\t\t<mass>{body.mass}<mass/>\n");
            builder.Append("\t<body/>\n");
        }
        builder.Append("<bodies/>");

        var text = builder.ToString();

        return text;
    }

    public static SimState DecodeSimulation(string text)
    {
        var state = new SimState();

        text = text.Replace("\n", "").Replace("\t", "").Replace("\r", "");

        state.simSpeed = Convert.ToDouble(Regex.Match(text, @"<simSpeed>(.*?)<simSpeed/>").Groups[1].Value);
        state.universe.gravitationalConstant = Convert.ToDouble(Regex.Match(text, @"<gravitationalConstant>(.*?)<gravitationalConstant/>").Groups[1].Value);
        var cameraPos = Regex.Match(text, @"<cameraPosition>(.*?), (.*?)<cameraPosition/>");
        state.cameraPosition.x = Convert.ToDouble(cameraPos.Groups[1].Value);
        state.cameraPosition.y = Convert.ToDouble(cameraPos.Groups[2].Value);

        foreach (Match match in Regex.Matches(text, @"<body><name>(.*?)<name/><radius>(.*?)<radius/><colour>(.*?), (.*?), (.*?)<colour/><position>(.*?), (.*?)<position/><velocity>(.*?), (.*?)<velocity/><mass>(.*?)<mass/><body/>").ToList<Match>())
        {
            var name = match.Groups[1].Value;
            var radius = Convert.ToDouble(match.Groups[2].Value);
            var colour = new Colour(Convert.ToInt32(match.Groups[3].Value), Convert.ToInt32(match.Groups[4].Value), Convert.ToInt32(match.Groups[5].Value));
            var position = new Vector2(Convert.ToDouble(match.Groups[6].Value), Convert.ToDouble(match.Groups[7].Value));
            var velocity = new Vector2(Convert.ToDouble(match.Groups[8].Value), Convert.ToDouble(match.Groups[9].Value));
            var mass = Convert.ToDouble(match.Groups[10].Value);
            state.universe.AddBody(new Body(name, radius, colour, position, velocity, Vector2.Zero, mass, state.universe));
        }

        return state;
    }

    public static string EncodePreset(Body body)
    {
        var builder = new StringBuilder();

        builder.Append("<body>\n");
        builder.Append($"\t<name>{body.name}<name/>\n");
        builder.Append($"\t<radius>{body.radius}<radius/>\n");
        builder.Append($"\t<colour>{body.colour.red}, {body.colour.green}, {body.colour.blue}<colour/>\n");
        builder.Append($"\t<position>{body.position.x}, {body.position.y}<position/>\n");
        builder.Append($"\t<velocity>{body.velocity.x}, {body.position.y}<velocity/>\n");
        builder.Append($"\t<mass>{body.mass}<mass/>\n");
        builder.Append("<body/>");

        return builder.ToString();
    }
   
    public static Body DecodePreset(string text)
    {
        var match = Regex.Match(text.Replace("\n", "").Replace("\t", "").Replace("\r", ""), @"<body><name>(.*?)<name/><radius>(.*?)<radius/><colour>(.*?), (.*?), (.*?)<colour/><position>(.*?), (.*?)<position/><velocity>(.*?), (.*?)<velocity/><mass>(.*?)<mass/><body/>");

        var name = match.Groups[1].Value;
        var radius = Convert.ToDouble(match.Groups[2].Value);
        var colour = new Colour(Convert.ToInt32(match.Groups[3].Value), Convert.ToInt32(match.Groups[4].Value), Convert.ToInt32(match.Groups[5].Value));
        var position = new Vector2(Convert.ToDouble(match.Groups[6].Value), Convert.ToDouble(match.Groups[7].Value));
        var velocity = new Vector2(Convert.ToDouble(match.Groups[8].Value), Convert.ToDouble(match.Groups[9].Value));
        var mass = Convert.ToDouble(match.Groups[10].Value);

        return new Body(name, radius, colour, position, velocity, Vector2.Zero, mass, null);
    }

    public static int Save(string text, string path)
    {
        try
        {
            File.WriteAllText(path, text);

            return 1;
        }
        catch (Exception)
        {
            return -1;
        }
    }

    public static string Load(string path)
    {
        try
        {
            return File.ReadAllText(path);
        }
        catch (Exception)
        {
            return "";
        }
    }
}