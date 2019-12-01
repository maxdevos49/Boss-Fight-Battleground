using System;
using System.Collections.Generic;
using BFB.Engine.Entity;
using BFB.Engine.Inventory;
using BFB.Engine.Inventory.Configuration;
using BFB.Engine.Math;
using BFB.Engine.Simulation.EntityComponents;

namespace BFB.Engine.Simulation.ItemComponents
{
    public class CastSpell : IItemComponent
    {
        public void Use(Simulation simulation, SimulationEntity entity, IItem item)
        {
            ItemConfiguration itemConfig = item.Configuration;
            
            if (entity.ControlState == null || entity.Meta == null || entity.Meta.Mana < itemConfig.ManaCost)
                return;
            
            entity.Meta.Health += itemConfig.HealthGain;
            entity.Meta.Health -= itemConfig.HealthCost;
            
            entity.Meta.Mana += itemConfig.ManaGain;
            entity.Meta.Mana -= itemConfig.ManaCost;
            
            BfbVector directionVector = BfbVector.Sub(entity.ControlState.Mouse,entity.Position);
            float direction = (float)System.Math.Atan2(directionVector.Y, directionVector.X);
                
            #region HealSpell
            
            simulation.AddEntity(new SimulationEntity( //TODO simulation entity factory
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    TextureKey = "Heart",
                    Position = new BfbVector(entity.Position.X, entity.Position.Y),
                    Dimensions = new BfbVector(50, 50),
                    Rotation = 0,
                    Origin = new BfbVector(25, 25),
                    EntityType = EntityType.Projectile
                }//, new List<EntityComponent>
                //{
                //    new LifetimeComponent(150)
                //})
                ));
            
            #endregion
            
            #region Fireball
        
            simulation.AddEntity(new SimulationEntity(//TODO entity factory
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    TextureKey = "Fireball",
                    Position = new BfbVector(entity.Position.X, entity.Position.Y),
                    Dimensions = new BfbVector(50, 50),
                    Rotation = direction + (float)(System.Math.PI / 2),
                    Origin = new BfbVector(25, 25),
                    EntityType = EntityType.Projectile
                }//, new List<EntityComponent>
                //{
                 //   new FireballEffectComponent(),
                 //   new SpellHeadingPhysicsComponent()
                //}
                )
            {
                ParentEntityId = entity.EntityId,
                SteeringVector = directionVector,
                CollideFilter = "projectile",
                CollideWithFilters = new List<string> { "tile", "human", "monster"}
            });
            
            #endregion
            
            #region Bomb
            
            simulation.AddEntity(new SimulationEntity( //TODO simulationEntity factory
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    TextureKey = "Bomb",
                    Position = new BfbVector(entity.Position.X, entity.Position.Y),
                    Dimensions = new BfbVector(50, 50),
                    Rotation = 0,
                    Origin = new BfbVector(25, 25),
                    EntityType = EntityType.Projectile
                }//, new List<EntityComponent>()
//                {
//                    new LifetimeComponent(100),
//                    new BombSpellPhysicsComponent()
//                })
                )
            {
                ParentEntityId = entity.EntityId,
                SteeringVector = directionVector,
                CollideFilter = "projectile",
                CollideWithFilters = new List<string> { "tile", "human", "monster"}
            });
            
            #endregion
            
            #region MagicMissile
            
            simulation.AddEntity(new SimulationEntity(//TODO simulation entity factory
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    TextureKey = "Missile",
                    Position = new BfbVector(entity.Position.X, entity.Position.Y),
                    Dimensions = new BfbVector(50, 50),
                    Rotation = direction + (float)(System.Math.PI / 2),
                    Origin = new BfbVector(25, 25),
                    EntityType = EntityType.Projectile
                }//, new List<EntityComponent>
                //{
                 //   new MagicMissileEffectComponent(),
                 //   new SpellHeadingPhysicsComponent()
                //}
        )
            {
                ParentEntityId = entity.EntityId,
                SteeringVector = directionVector,
                CollideFilter = "projectile",
                CollideWithFilters = new List<string> { "tile", "human", "monster"}
            });
            
            #endregion
        }
    }
}