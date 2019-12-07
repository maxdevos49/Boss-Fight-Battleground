using System;
using System.Collections.Generic;
using System.Text;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class PostGameComponent : GameComponent
    {
        public PostGameComponent() : base()
        {
            Console.WriteLine("GAME ENDED");
        }

        public void Update()
        {

        }
    }
}
