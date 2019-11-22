namespace BFB.Engine.Simulation
{
    public enum EntityRemovalReason : byte
    {
        Disconnect,
        TimeExpiration,
        TileCollision,
        WorldBoundaryCollision,
        EntityCollision
    }
}