using System;

namespace BFB.Engine.Inventory
{
    [Serializable]
    public enum ItemType : byte
    {
        Unknown,
        Tool,
        Wall,
        Block
    }
}