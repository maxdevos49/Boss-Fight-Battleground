using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using BFB.Engine.Simulation.PhysicsComponents;
using BFB.Engine.Simulation.SpellComponents.Physics;

namespace BFB.Engine.Simulation.SpellComponents.MainComponents
{
    public class BombSpellComponent : ISpellComponent
    {
        private readonly int _cost;
        private int _cooldown;
        private bool _onCooldown;

        public BombSpellComponent()
        {
            _cost = 100;
            _cooldown = 0;
            _onCooldown = false;
        }

        public void OnUse(SimulationEntity simulationEntity, Simulation simulation, BfbVector mouse)
        {
            if (((CombatComponent)simulationEntity.Combat).Mana < _cost) return;
            if (_onCooldown) return;

            _onCooldown = true;
            _cooldown = 20;

            ((CombatComponent)simulationEntity.Combat).Mana -= _cost;

            simulation.AddEntity(new SimulationEntity(
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    AnimatedTextureKey = "Bomb",
                    Position = new BfbVector(simulationEntity.Position.X, simulationEntity.Position.Y),
                    Dimensions = new BfbVector(50, 50),
                    Rotation = 0,
                    Origin = new BfbVector(25, 25),
                }, new ComponentOptions()
                {
                    Physics = new BombSpellPhysicsComponent(new BfbVector(mouse.X - simulationEntity.Position.X, -40), simulationEntity)
                }));
        }

        public void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            if (_cooldown > 0)
            {
                _cooldown -= 1;
            }

            if (_cooldown == 0 && _onCooldown)
                _onCooldown = false;
        }
    }
}
