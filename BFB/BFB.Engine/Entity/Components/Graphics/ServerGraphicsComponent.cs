namespace BFB.Engine.Entity.Components.Graphics
{
    public class ServerGraphicsComponent : IGraphicsComponent
    {
        public void Draw(ServerEntity serverEntity)
        {
            //Do nothing or print just Animation state name because we are on the server.
        }
    }
}