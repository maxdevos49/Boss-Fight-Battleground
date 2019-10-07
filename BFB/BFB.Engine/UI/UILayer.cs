using BFB.Engine.UI.Components;

namespace BFB.Engine.UI
{
    // ReSharper disable once InconsistentNaming
    public abstract class UILayer
    {
        public string Key { get; set; }   
        
        public UIRootComponent RootUI { get; set; }
        
        public UILayer(string key)
        {
            Key = key;
            RootUI = null;
        }

        public void SetRoot(UIRootComponent rootNode)
        {
            RootUI = rootNode;
        }
        
        public abstract void Body();
    }
}