using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Entity;
using BFB.Engine.Simulation.GameModeComponents;
using JetBrains.Annotations;

namespace BFB.Engine.Simulation
{
    public abstract class SimulationGameMode
    {
        #region Properties

        [UsedImplicitly]
        public int CurrentStageTicks { get; private set; }

        private string _currentGameModeStage;

        private readonly Dictionary<string, List<GameModeComponent>> _gameModeComponents;
        
        #endregion
        
        #region Constructor
        
        protected SimulationGameMode(Dictionary<string, List<GameModeComponent>> gameModeComponent)
        {
            _currentGameModeStage = "default";
            _gameModeComponents = gameModeComponent;
        }
        
        #endregion
        
        #region Init

        public virtual void Init(Simulation simulation)
        {
            foreach ((string _, List<GameModeComponent> value) in _gameModeComponents)
                foreach (GameModeComponent gameModeComponent in value)
                    gameModeComponent.Init(simulation);
        }
        
        #endregion
        
        #region Update

        public virtual void Update(Simulation simulation)
        {
            CurrentStageTicks++;
            
            if (!ValidateCurrentGameStage()) 
                return;
            
            foreach (GameModeComponent gameModeComponent in _gameModeComponents[_currentGameModeStage].ToList())
                gameModeComponent.Update(simulation);
        }
        
        #endregion
        
        #region SwitchGameStage

        public void SwitchGameStage(string gameModeStage)
        {
            if (ValidateCurrentGameStage(gameModeStage))
            {
                CurrentStageTicks = 0;
                _currentGameModeStage = gameModeStage;
            }
        }
        
        #endregion
        
        #region EmitSimulationStart

        public void EmitSimulationStart(Simulation simulation)
        {
            if (!ValidateCurrentGameStage()) 
                return;
            
            foreach (GameModeComponent gameModeComponent in _gameModeComponents[_currentGameModeStage].ToList())
                gameModeComponent.OnSimulationStart(simulation);
        }

        #endregion
        
        #region EmitSimulationStop

        public void EmitSimulationStop(Simulation simulation)
        {
            if (!ValidateCurrentGameStage()) 
                return;
            
            foreach (GameModeComponent gameModeComponent in _gameModeComponents[_currentGameModeStage].ToList())
                gameModeComponent.OnSimulationStop(simulation);
        }
        
        #endregion
        
        #region EmitReset

        public void EmitReset(Simulation simulation)
        {
            if (!ValidateCurrentGameStage()) 
                return;
            
            foreach (GameModeComponent gameModeComponent in _gameModeComponents[_currentGameModeStage].ToList())
                gameModeComponent.Reset(simulation);
        }
        
        #endregion
        
        #region EmitGameMessage
        
        public void EmitGameMessage(string message)
        {
            if (!ValidateCurrentGameStage()) 
                return;
            
            foreach (GameModeComponent gameModeComponent in _gameModeComponents[_currentGameModeStage].ToList())
                gameModeComponent.OnGameMessage(message);
        }
        
        #endregion
        
        #region EmitEntityAdd

        public void EmitEntityAdd(Simulation simulation, SimulationEntity entity)
        {
            if (!ValidateCurrentGameStage()) 
                return;

            foreach (GameModeComponent gameModeComponent in _gameModeComponents[_currentGameModeStage].ToList())
                gameModeComponent.OnEntityAdd(simulation,entity);
        }
        
        #endregion
        
        #region EmitEntityRemove

        public void EmitEntityRemove(Simulation simulation, SimulationEntity entity, EntityRemovalReason? reason)
        {
            if (!ValidateCurrentGameStage()) 
                return;
            
            foreach (GameModeComponent gameModeComponent in _gameModeComponents[_currentGameModeStage].ToList())
                gameModeComponent.OnEntityRemove(simulation,entity,reason);
        }
        
        #endregion
        
        #region ValidateCurrentGameStage(private)

        private bool ValidateCurrentGameStage(string gameModeStage = null)
        {
            if (gameModeStage == null)
                return _gameModeComponents.ContainsKey(_currentGameModeStage);

            return _gameModeComponents.ContainsKey(gameModeStage);
        }
        
        #endregion
        
    }
}