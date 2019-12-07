using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class GameEndComponent : GameComponent
    {
        private bool _gameEnded;
        private int _humansRemaining;

        public GameEndComponent() : base()
        {
            _gameEnded = false;
            _humansRemaining = 999;
        }

        public override void Init(Simulation simulation)
        {
            Console.WriteLine("INITIALIZED??");
            _humansRemaining = (simulation.GetPlayerEntities().Where(x => x.EntityConfiguration.EntityKey == "Human")).Count();
        }

        public override void Update(Simulation simulation)
        {
            Console.WriteLine(_humansRemaining);
            if (_humansRemaining <= 0)
                _gameEnded = true;

            if (_gameEnded)
            {
                simulation.gameComponents.Clear();

                simulation.gameState = GameState.PostGame;
                GameComponent component = new PostGameComponent();
                component.Init(simulation);
                simulation.gameComponents.Add(component);
            }
        }

        public override void OnEntityRemove(Simulation simulation, SimulationEntity entity)
        {
            if (entity.EntityConfiguration.EntityKey == "Human")
                _humansRemaining -= 1;
        }
    }
}
