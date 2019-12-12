using System.Collections.Generic;
using BFB.Engine.Simulation;
using BFB.Engine.Simulation.GameModeComponents;

namespace BFB.Server.GameModes
{
    public class Infection : SimulationGameMode
    {
        #region Component Configuration
      
        private static readonly Dictionary<GameState, List<GameModeComponent>> ComponentConfig = new Dictionary<GameState, List<GameModeComponent>>
        {
            {GameState.Uninitialized, new List<GameModeComponent>
            {
            }},
            {GameState.PreGame, new List<GameModeComponent>
            {
                new PreGameModeComponent()
            }},
            {GameState.InProgress, new List<GameModeComponent>
            {
                new AIMobSpawnModeComponent(),
                new BossSpawnModeComponent(),
                new PlayerRespawnModeComponent(),
                new GameOverComponent()

            }},
            {GameState.PostGame, new List<GameModeComponent>
            {
                new PostGameModeComponent()
            }},
            {GameState.Error, new List<GameModeComponent>()}
        };
        
        #endregion

        public Infection() : base(ComponentConfig,2)
        {
            
        }
    }
}