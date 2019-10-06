using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using Microsoft.Xna.Framework;

namespace BFB.Client.UI
{
   
    public class UITest: UIView
    {
        
        public IComponent Body<TModel>(UIContext<TModel> ui)
        {
            //gets root 
            ui.View(root =>
            {
                //Creates a vertical stack
                root.Vstack(vs =>
                    {
                        vs.Vstack(vs2 =>
                            {
                                
                            })
                            .Top(100)
                            .Left(100)
                            .Height(1f)
                            .Width(2f)
                            .Background(Color.Red)
                            .Name("Stack Layer 2");
                    })
                    .Top(50)
                    .Left(50)
                    .Height(0.7f)
                    .Width(0.5f)
                    .Background(Color.Green)
                    .Name("Stack Layer 1");

            }).Name("Root");

            return ui.GetRoot();
        }
    }
}