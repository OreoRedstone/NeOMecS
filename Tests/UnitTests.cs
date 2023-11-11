using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using NeOMecS.Utilities;
using NeOMecS.Physics;

namespace NeOMecS.Tests;

public class UnitTests
{
    [Fact]
    public void EncodeTest1()
    {
        //Arrange
        var bodies = new List<Body>();
        var universe = new Universe(bodies, 1);
        var state = new SimState(1, universe, new Vector2(0, 0));

        var expectedResult = "<simSpeed>1<simSpeed/>\n<gravitationalConstant>1<gravitationalConstant/>\n<cameraPosition>0, 0<cameraPosition/>\n<bodies>\n<bodies/>";

        //Act
        var result = SaveLoadSystem.Encode(state);

        //Assert1
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void EncodeTest2()
    {
        //Arrange
        var bodies = new List<Body>
        {
            new Body("Earth", 10, new Colour(1, 1, 1), Vector2.Zero, Vector2.Zero, Vector2.Zero, 1, null)
        };
        var universe = new Universe(bodies, 1);
        universe.bodies[0].parent = universe;
        var state = new SimState(1, universe, new Vector2(0, 0));

        var expectedResult = "<simSpeed>1<simSpeed/>\n<gravitationalConstant>1<gravitationalConstant/>\n<cameraPosition>0, 0<cameraPosition/>\n<bodies>\n\t<body>\n\t\t<name>Earth<name/>\n\t\t<radius>10<radius/>\n\t\t<colour>1, 1, 1<colour/>\n\t\t<position>0, 0<position/>\n\t\t<velocity>0, 0<velocity/>\n\t\t<mass>1<mass/>\n\t<body/>\n<bodies/>";

        //Act
        var result = SaveLoadSystem.Encode(state);

        //Assert1
        Assert.Equal(expectedResult, result);
    }
}
