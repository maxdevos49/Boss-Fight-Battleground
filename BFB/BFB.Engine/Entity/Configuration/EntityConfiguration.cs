using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Math;
using JetBrains.Annotations;

namespace BFB.Engine.Entity.Configuration
{
    [UsedImplicitly]
    public class EntityConfiguration
    {
        [UsedImplicitly]
        public string TextureKey { get; set; }
        
        [UsedImplicitly]
        public EntityType EntityType { get; set; }

        [UsedImplicitly]
        public string EntityKey { get; set; }

        [UsedImplicitly]
        public ushort Health { get; set; }
        
        [UsedImplicitly]
        public ushort Mana { get; set; }
        
        [UsedImplicitly]
        public int Lifetime { get; set; }

        [UsedImplicitly]
        public BfbVector DimensionRange { get; set; }

        [UsedImplicitly]
        public BfbVector Dimensions { get; set; }
        
        [UsedImplicitly]
        public BfbVector Origin { get; set; }
        
        [UsedImplicitly]
        public List<string> Components { get; set; }
        
        [UsedImplicitly]
        public string CollideFilter { get; set; }
        
        [UsedImplicitly]
        public List<string> CollideWithFilters { get; set; }

        public EntityConfiguration Clone()
        {
            return new EntityConfiguration
            {
                TextureKey = TextureKey,
                EntityType = EntityType,
                Health = Health,
                Mana = Mana,
                Lifetime = Lifetime,
                DimensionRange = DimensionRange?.Clone(),
                Dimensions = Dimensions?.Clone(),
                Origin = Origin?.Clone(),
                Components = Components?.ToList(),
                CollideFilter = CollideFilter,
                CollideWithFilters = CollideWithFilters?.ToList()
            };
        }
    }
}