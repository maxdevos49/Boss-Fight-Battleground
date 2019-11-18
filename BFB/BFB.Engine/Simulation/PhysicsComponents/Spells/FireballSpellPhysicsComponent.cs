using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Simulation.PhysicsComponents
{
    class SpellPhysicsComponent : IPhysicsComponent
    {
        private int timeToLive;
        private SimulationEntity _owner;
        private readonly BfbVector _acceleration;

        public SpellPhysicsComponent(BfbVector direction, SimulationEntity owner)
        {
            _owner = owner;
            Vector2 directionNorm = direction.ToVector2();
            directionNorm.Normalize();
            timeToLive = 30;
            float xAcc = directionNorm.X;
            float yAcc = directionNorm.Y;
            _acceleration = new BfbVector(xAcc, yAcc);
            _maxSpeed = new BfbVector(15, 15);
        }

        public void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            timeToLive -= 1;
            if (timeToLive <= 0)
                simulation.RemoveEntity(simulationEntity.EntityId);

            //Gives us the speed to move left and right
            simulationEntity.DesiredVector.X = _acceleration.X * 30;
            simulationEntity.DesiredVector.Y = _acceleration.Y * 30;

            //Creates the new velocity
            simulationEntity.Velocity.X = simulationEntity.DesiredVector.X;
            simulationEntity.Velocity.Y = simulationEntity.DesiredVector.Y;

            //Updates the position
            simulationEntity.Position.Add(simulationEntity.Velocity);

            checkCollisions(simulationEntity, simulation);
        }

        private void checkCollisions(SimulationEntity simulationEntity, Simulation simulation)
        {
            // Collisions with monsters
            List<SimulationEntity> targets = new List<SimulationEntity>();
            for (int i = 0; i < 10; i++)
            {
                int xPos = (int) simulationEntity.Position.X;
                SimulationEntity target = simulation.GetEntityAtPosition(xPos, (int) simulationEntity.Position.Y);
                if (target != null && target != simulationEntity && !targets.Contains(target))
                    targets.Add(target);
            }

            targets.Remove(_owner);
            if (targets.Count >= 1)
            {
                DamageTargets(targets);
                simulation.RemoveEntity(simulationEntity.EntityId);
            }
        }

        private void DamageTargets(List<SimulationEntity> targets)
        {
            foreach (SimulationEntity target in targets)
            {
                // Instead of a hard coded value here, you could call a weapon stored on the simulationEntity, and use its damage value.
                ((CombatComponent)target.Combat).Health -= 15;
            }
        }
    }
}
