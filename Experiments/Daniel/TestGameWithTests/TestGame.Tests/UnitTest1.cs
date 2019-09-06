using System;
using Xunit;

namespace TestGame.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            String expected = "This should always pass!";
            String actual = "This should always pass!";

            Assert.Equal(expected, actual);
        }
    }
}
