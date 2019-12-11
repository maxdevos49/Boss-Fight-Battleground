using System.Linq;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class GameOverComponent : GameModeComponent
    {
        private bool _gameEnded;
        private int _humansRemaining;
        
        #region Init 
        
        public override void Init(Simulation simulation, SimulationGameMode gameMode)
        {
            base.Init(simulation, gameMode);
            
            _gameEnded = false;
            _humansRemaining = simulation.GetPlayerEntities().Count(x => x.EntityConfiguration.EntityKey == "Human");
        }
        
        #endregion

        #region Reset
        
        public override void Reset(Simulation simulation)
        {
            base.Reset(simulation);

            _gameEnded = false;
            _humansRemaining = 0;
        }
        
        #endregion

        public override void Update(Simulation simulation)
        {
            if (!_gameEnded) return;
            
            foreach (SimulationEntity entity in simulation.GetAllEntities())
            {
                if (entity.EntityType != EntityType.Player)
                    simulation.RemoveEntity(entity.EntityId);
            }
                
            GameMode.SwitchGameState(GameState.PostGame);

        }

        public override void OnEntityRemove(Simulation simulation, SimulationEntity entity, EntityRemovalReason? reason)
        {
            if (entity.EntityConfiguration.EntityKey != "Human")
                return;
            
            _humansRemaining -= 1;

            if (_humansRemaining <= 0)
                _gameEnded = true;
        }
    }
}
