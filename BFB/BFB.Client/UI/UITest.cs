using System.Runtime.ConstrainedExecution;
using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using BFB.Engine.UI.Modifiers;
using Microsoft.Xna.Framework;

namespace BFB.Client.UI
{

    public class UITest : UILayer
    {
        public UITest() :base(nameof(UITest)) { }
        
        public override void Body()
        {
            RootUI.Hstack((h1) =>
            {
                h1.Spacer();//Spacers have a default grow proportion of 1
                
                h1.Vstack(v1 =>
                    {
                        v1.Spacer().Background(Color.Green).Width(0.7f).Center();
                        
                        v1.Hstack(h2 =>
                        {
                            h2.Spacer().Background(Color.Blue);
                            h2.Spacer().Background(Color.Blue);
                        });
                        
                        v1.Hstack(h2 =>
                        {
                            h2.Spacer().Background(Color.Red);
                            h2.Spacer().Background(Color.Red);
                        });
                        
                        v1.Hstack(h2 =>
                        {
                            h2.Spacer()
                                .Background(Color.Yellow);
                            h2.Spacer()
                                .Background(Color.Yellow);
                            h2.Spacer()
                                .Background(Color.Yellow);
                        });

                        v1.Hstack(h2 =>
                        {
                            h2.Spacer().Background(Color.Orange).Width(0.7f).Center();
                        });

                    })
                    .Background(Color.Beige)
                    .Grow(4);
                
                h1.Spacer();//Could just apply a width instead of these but oh well they still work
            })
                .Height(0.8f)
                .Center();

//            RootUI.Hstack((h1) =>
//            {
//                h1.Spacer();
//
//                //Main menu panel
//                h1.Vstack((v2) =>
//                    {
//                        //Title
//                        v2.Hstack((h2) =>
//                            {
//                                
//                            })
//                            .Background(Color.Red)
//                            .Grow(2);
//
//                        //Connection
//                        v2.Hstack((h2) =>
//                        {
//                            
//                        }).Background(Color.Yellow);
//
//                        //Non Connection
//                        v2.Hstack((h2) =>
//                        {
//                            
//                        }).Background(Color.Yellow);
//
//                        //Tile map
//                        v2.Hstack((h2) =>
//                        {
//                            
//                        }).Background(Color.Yellow);
//
//                        //bottom spacer
//                        v2.Spacer(1).Background(Color.Green);
//                        
//                    })
//                        .Background(Color.Firebrick)
//                        .Grow(3);
//
//                h1.Spacer();
//
//            })
//                .Width(0.8f)
//                .Height(0.8f)
//                .Center();
        }
    }
}