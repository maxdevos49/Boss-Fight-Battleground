
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace BFB.Engine.Server
{
    [UsedImplicitly]
    public class Command
    {
        public string CommandKey { get; set; }

        public string EventTrigger { get; set; }

        public string Description { get; set; }

    }
}
