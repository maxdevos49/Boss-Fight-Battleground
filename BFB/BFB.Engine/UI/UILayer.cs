using BFB.Engine.UI.Components;

namespace BFB.Engine.UI
{
    public abstract class UILayer
    {
        public string Key { get; }
        
        public UIRootComponent RootUI { get; set; }

        protected UILayer(string key)
        {
            Key = key;
            RootUI = null;
            
            //
        }

        public void SetRoot(UIRootComponent rootNode)
        {
            RootUI = rootNode;
        }
        
        public abstract void Body();
    }
}