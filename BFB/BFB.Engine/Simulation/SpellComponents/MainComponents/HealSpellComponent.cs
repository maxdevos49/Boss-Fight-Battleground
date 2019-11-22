using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using BFB.Engine.Simulation.PhysicsComponents;
using BFB.Engine.Simulation.SimulationComponents;

namespace BFB.Engine.Simulation.SpellComponents.MainComponents
{
    public class HealSpellComponent : ISpellComponent
    {
        private SimulationEntity _effect;
        private readonly ushort _heal;
        private readonly ushort _cost;
        private int _cooldown;
        private bool _onCooldown;

        public HealSpellComponent()
        {
            _heal = 20;
            _cost = 50;
            _cooldown = 0;
            _onCooldown = false;
            _effect = null;
        }

        public void OnUse(SimulationEntity simulationEntity, Simulation simulation, BfbVector mouse)
        {
            if (simulationEntity.Meta != null && simulationEntity.Meta.Mana < _cost)
                return;
                
            if (_onCooldown)
                return;

            _onCooldown = true;
            _cooldown = 10;

            if (simulationEntity.Meta != null)
            {
                simulationEntity.Meta.Health += _heal;
                simulationEntity.Meta.Mana -= _cost;
            }

            _effect = new SimulationEntity(
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    TextureKey = "Heart",
                    Position = new BfbVector(simulationEntity.Position.X, simulationEntity.Position.Y),
                    Dimensions = new BfbVector(50, 50),
                    Rotation = 0,
                    Origin = new BfbVector(25, 25),
                }, new List<EntityComponent>());

            simulation.AddEntity(_effect);
        }
        
        #region Update
        
        public void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            if (_cooldown > 0)
            {
                _cooldown -= 1;
                if (_cooldown <= 5 && _effect != null)
                {
                    simulation.RemoveEntity(_effect.EntityId);
                    _effect = null;
                }
            }

            if (_cooldown == 0 && _onCooldown)
                _onCooldown = false;

            if (_effect != null)
            {
                _effect.Position.Y -= 1;
            }
        }
        
        #endregion
    }
}
