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
}
