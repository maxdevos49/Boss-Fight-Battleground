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
                h1.Vstack(v1 =>
                    {
                        v1.Hstack(h2 =>
                        {
                            h2.Vstack(v2 =>
                            {
                                v2.Text("Title")
                                    .Width(0.8f)
                                    .Height(
                                        0.8f) //Class and Id selectors on on there way after making this layout. They will include default styles for some elements
                                    .FontSize(3)
                                    .Center();
                            });
                        });

                        v1.Hstack(h2 =>
                        {
                            h2.Vstack(v2 =>
                            {
                                v2.Text("Test Text")
                                    .Width(0.8f)
                                    .Height(0.8f)
                                    .Center();
                            });


                            h2.Vstack(v2 =>
                            {
                                v2.Text("Test Text")
                                    .Width(0.8f)
                                    .Height(0.8f)
                                    .Center();
                            });
                        });





                        v1.Hstack(h2 =>
                        {
                            h2.Vstack(v2 =>
                            {
                                v2.Text("Test Text")
                                    .Width(0.8f)
                                    .Height(0.8f)
                                    .Center();
                            });

                            h2.Vstack(v2 =>
                            {
                                v2.Text("Test Text")
                                    .Width(0.8f)
                                    .Height(0.8f)
                                    .Center();
                            });
                        });







                        v1.Hstack(h2 =>
                        {
                            h2.Vstack(v2 =>
                            {
                                v2.Text("Test Text")
                                    .Width(0.8f)
                                    .Height(0.8f)
                                    .Center();
                            });

                            h2.Vstack(v2 =>
                            {
                                v2.Text("Test Text")
                                    .Width(0.8f)
                                    .Height(0.8f)
                                    .Center();
                            });

                            h2.Vstack(v2 =>
                            {
                                v2.Text("Test Text")
                                    .Width(0.8f)
                                    .Height(0.8f)
                                    .Center();
                            });
                        });






                        v1.Vstack(v2 =>
                        {
                            v2.Text("Back")
                                .Width(0.8f)
                                .Height(0.8f)
                                .Center();
                        });

                    })
                    .Grow(4)
                    .AspectRatio(1.78f)
                    .Top(100)
                    .Right(100)
                    .Center();
            });


//            RootUI.Hstack((h1) =>
//            {
//                h1.Spacer();
//                    {
//                        //Title
//
//                //Main menu panel
//                h1.Vstack((v2) =>
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