using System;
using Xunit;

namespace TestGame.Tests
{
    public class Player
    {
        [Fact]
        public void Test1()
        {
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
