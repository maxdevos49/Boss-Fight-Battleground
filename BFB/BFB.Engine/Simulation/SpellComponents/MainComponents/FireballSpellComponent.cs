using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using BFB.Engine.Simulation.PhysicsComponents;
using BFB.Engine.Simulation.SpellComponents.Physics;

namespace BFB.Engine.Simulation.SpellComponents.MainComponents
{
    public class FireballSpellComponent : ISpellComponent
    {
        private readonly int _cost;
        private int _cooldown;
        private bool _onCooldown;

        public FireballSpellComponent()
        {
            _cost = 30;
            _cooldown = 0;
            _onCooldown = false;
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

        public void OnUse(SimulationEntity simulationEntity, Simulation simulation, BfbVector mouse)
        {
            if (((CombatComponent)simulationEntity.Combat).Mana < _cost) return;
            if (_onCooldown) return;

            _onCooldown = true;
            _cooldown = 10;

            ((CombatComponent)simulationEntity.Combat).Mana -= _cost;

            BfbVector directionVector = new BfbVector(mouse.X - simulationEntity.Position.X, mouse.Y - simulationEntity.Position.Y);
            float direction = (float)System.Math.Atan2(directionVector.Y, directionVector.X);
            simulation.AddEntity(new SimulationEntity(
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    AnimatedTextureKey = "Fireball",
                    Position = new BfbVector(simulationEntity.Position.X, simulationEntity.Position.Y),
                    Dimensions = new BfbVector(50, 50),
                    Rotation = direction + (float)(System.Math.PI / 2),
                    Origin = new BfbVector(25, 25),
                }, new ComponentOptions()
                {
                    Physics = new FireballSpellPhysicsComponent(directionVector, simulationEntity)
                }));
        }
    }
}
