using JetBrains.Annotations;

namespace BFB.Engine.Simulation.Configuration
{
    [UsedImplicitly]
    public class EntityComponentConfiguration
    {
        [UsedImplicitly] 
        public string FullyQualifiedName { get; set; }

        [UsedImplicitly]
        public object[] Args { get; set; }

    }
}