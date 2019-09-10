using System;
using Xunit;
using TestGame.Source.Gameplay.World;
using Microsoft.Xna.Framework;

namespace TestGame.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void ConstructorTest()
        {
            Player myPlayer = new Player("2d\\stickman-test-2", new Vector2(0,0), new Vector2(0,0));

            Assert.Equal(20, myPlayer.health);
            Assert.Equal(new Vector2(0), myPlayer.velocity);
            Assert.True(myPlayer.isInAir);
        }
    }
}
