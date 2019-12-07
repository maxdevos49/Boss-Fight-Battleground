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
            
            BfbVector directionVector = BfbVector.Sub(entity.ControlState.Mouse, entity.Position);
            float direction = (float)System.Math.Atan2(directionVector.Y, directionVector.X) + (float)(System.Math.PI / 2);
                
            //place a copy of the spell config inside the projectile
            InventoryManager inventory = new InventoryManager();
            Item newItem = new Item(item.ItemConfigKey);
            inventory.Insert(newItem);
            
            SimulationEntity spellEntity = SimulationEntity.SimulationEntityFactory(itemConfig.EntitySpawnKey);
            spellEntity.SteeringVector = directionVector;
            spellEntity.Rotation = direction;
            spellEntity.ParentEntityId = entity.EntityId;
            spellEntity.Position = entity.Position.Clone();
            spellEntity.Position.X += entity.Dimensions.X / 2;
            spellEntity.Position.Y -= entity.Origin.Y / 4;
            spellEntity.Inventory = inventory;
            
            simulation.AddEntity(spellEntity);
        }
    }
}