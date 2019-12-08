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
        }

        public override void Init(Simulation simulation)
        {
            _humansRemaining = (simulation.GetPlayerEntities().Where(x => x.EntityConfiguration.EntityKey == "Human")).Count();
        }

        public override void Update(Simulation simulation)
        {
            if (_humansRemaining <= 0)
                _gameEnded = true;

            if (_gameEnded)
            {
                foreach (SimulationEntity entity in simulation.GetAllEntities())
                {
                    if (entity.EntityType != EntityType.Player)
                        simulation.RemoveEntity(entity.EntityId);
                }
                simulation.gameComponents.Clear();

                simulation.gameState = GameState.PostGame;
                GameComponent component = new PostGameComponent();
                simulation.gameComponents.Add(component);
            }
        }

        public override void OnEntityRemove(Simulation simulation, SimulationEntity entity, EntityRemovalReason? reason)
        {
            if (entity.EntityConfiguration.EntityKey == "Human")
                _humansRemaining -= 1;
        }
    }
}
