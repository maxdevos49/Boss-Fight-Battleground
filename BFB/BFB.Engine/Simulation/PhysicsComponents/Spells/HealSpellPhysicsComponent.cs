using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Math;

namespace BFB.Engine.Simulation.PhysicsComponents.Spells
{
    public class HealSpellPhysicsComponent : IPhysicsComponent
    {
        private readonly SimulationEntity _owner;
        private SimulationEntity _effect;
        private int _heal;
        private int _cost;
        private int _cooldown;
        private bool _onCooldown;

        public HealSpellPhysicsComponent(SimulationEntity owner)
        {
            _heal = 20;
            _cost = 50;
            _cooldown = 0;
            _onCooldown = false;
            _effect = null;
        }

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
        }

        public void OnUse(SimulationEntity simulationEntity, Simulation simulation)
        {
            _onCooldown = true;
            _cooldown = 10;

            if (((CombatComponent) _owner.Combat).Mana < _cost) return;

            ((CombatComponent) _owner.Combat).Health += _heal;
            ((CombatComponent) _owner.Combat).Mana -= _cost;

            _effect = new SimulationEntity(
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    AnimatedTextureKey = "Heart",
                    Position = new BfbVector(simulationEntity.Position.X, simulationEntity.Position.Y),
                    Dimensions = new BfbVector(50, 50),
                    Rotation = 0,
                    Origin = new BfbVector(25, 25),
                }, new ComponentOptions());

            simulation.AddEntity(_effect);
        }
    }
}
