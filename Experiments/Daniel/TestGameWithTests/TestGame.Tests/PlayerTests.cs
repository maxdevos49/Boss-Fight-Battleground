using System;
using Xunit;
using TestGame.Source.Gameplay.World;
using Microsoft.Xna.Framework;

namespace TestGame.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void updatePosition()
        {
            Vector2 dimensions = new Vector2(1, 1);
            Player myPlayer = new Player("", null, null);
            String expected = "This should always pass!";
            String actual = "This should always pass!";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test2()
        {
            String expected = "This should always pass!";
            String actual = "Ha! Just kidding!";

            Assert.Equal(expected, actual);
        }
    }
}
