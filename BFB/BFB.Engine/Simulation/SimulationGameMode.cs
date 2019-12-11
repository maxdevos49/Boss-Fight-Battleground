using System;
using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Entity;
using BFB.Engine.Simulation.GameModeComponents;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Simulation
{
    public abstract class SimulationGameMode
    {
        #region Properties

        [UsedImplicitly]
        public int CurrentStageTicks { get; private set; }

        [UsedImplicitly]
        public GameState CurrentGameState { get; private set; }
        
        private GameState TargetState { get; set; }
        
        public int RequiredPlayers { get; private set; }

        private readonly Dictionary<GameState, List<GameModeComponent>> _gameModeComponents;

        private readonly List<SimulationEntity> _respawnEntities;
        
        #endregion
        
        #region Constructor
        
        protected SimulationGameMode(Dictionary<GameState, List<GameModeComponent>> gameModeComponent, int requiredPlayers = 4)
        {
            CurrentGameState = GameState.Uninitialized;
            TargetState = GameState.PreGame;
            _gameModeComponents = gameModeComponent;
            RequiredPlayers = requiredPlayers;
            _respawnEntities = new List<SimulationEntity>();
        }
        
        #endregion
        
        #region Init

        protected virtual void Init(Simulation simulation)
        {
            foreach (GameModeComponent gameModeComponent in _gameModeComponents[CurrentGameState])
                gameModeComponent.Init(simulation, this);
        }
        
        #endregion
        
        #region Reset

        private void Reset(Simulation simulation)
        {
            foreach (GameModeComponent gameModeComponent in _gameModeComponents[CurrentGameState].ToList())
                gameModeComponent.Reset(simulation);
        }
        
        #endregion
        
        #region RespawnEntity

        public void RespawnEntity(SimulationEntity entity)
        {
            _respawnEntities.Add(entity);
        }
        
        #endregion
        
        #region Update

        public virtual void Update(Simulation simulation)
        {
            CurrentStageTicks++;
            
            foreach (GameModeComponent gameModeComponent in _gameModeComponents[CurrentGameState].ToList())
                gameModeComponent.Update(simulation);
            
            //respawn any entities
            if(_respawnEntities.Any())
                foreach (SimulationEntity entity in _respawnEntities)
                {
                    simulation.AddEntity(entity);
                }

            //if game state changes
            if (TargetState == CurrentGameState) 
                return;
            
            //Reset current components
            Reset(simulation);
            //Change game state
            CurrentGameState = TargetState;
            //Game state ticks
            CurrentStageTicks = 0;
            //Init new components
            Init(simulation);
        }
        
        #endregion
        
        #region SwitchGameStage

        public void SwitchGameState(GameState gameModeStage)
        {
            Console.WriteLine(gameModeStage);
            if (!_gameModeComponents.ContainsKey(gameModeStage))
                return;
            
            
            TargetState = gameModeStage;
        }
        
        #endregion
        
        #region EmitSimulationStart

        public void EmitSimulationStart(Simulation simulation)
        {
            foreach (GameModeComponent gameModeComponent in _gameModeComponents[CurrentGameState].ToList())
                gameModeComponent.OnSimulationStart(simulation);
        }

        #endregion
        
        #region EmitSimulationStop

        public void EmitSimulationStop(Simulation simulation)
        {
            foreach (GameModeComponent gameModeComponent in _gameModeComponents[CurrentGameState].ToList())
                gameModeComponent.OnSimulationStop(simulation);
        }
        
        #endregion

        #region EmitGameMessage
        
        public void EmitGameMessage(string message)
        {
            foreach (GameModeComponent gameModeComponent in _gameModeComponents[CurrentGameState].ToList())
                gameModeComponent.OnGameMessage(message);
        }
        
        #endregion
        
        #region EmitEntityAdd

        public void EmitEntityAdd(Simulation simulation, SimulationEntity entity)
        {
            foreach (GameModeComponent gameModeComponent in _gameModeComponents[CurrentGameState].ToList())
                gameModeComponent.OnEntityAdd(simulation,entity);
        }
        
        #endregion
        
        #region EmitEntityRemove

        public void EmitEntityRemove(Simulation simulation, SimulationEntity entity, EntityRemovalReason? reason)
        {
            foreach (GameModeComponent gameModeComponent in _gameModeComponents[CurrentGameState].ToList())
                gameModeComponent.OnEntityRemove(simulation,entity,reason);
        }
        
        #endregion

    }
}