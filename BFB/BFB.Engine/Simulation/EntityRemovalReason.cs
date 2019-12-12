namespace BFB.Engine.Simulation
{
    public enum EntityRemovalReason : byte
    {
        Other,
        Disconnect,
        TimeExpiration,
        TileCollision,
        WorldBoundaryCollision,
        EntityCollision,
        BossSpawn
    }
}