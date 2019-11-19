using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using BFB.Engine.UI.Constraints;

namespace BFB.Client.UI
{
    public class CompletedTransactionUI : UILayer
    {
        public CompletedTransactionUI() : base(nameof(CompletedTransactionUI))
        {
        }

        public override void Body()
        {
            RootUI.Hstack(h1 =>
            {

                h1.Spacer();

                h1.Vstack(v1 =>
                {
                    v1.Spacer();

                    v1.Hstack(h2 =>
                        {
                            h2.Hstack(h3 =>
                                {
                                    h3.Text(
                                        "Your Transaction has been completed. The items have been added to your account. Happy Hunting!");
                                })
                                .Height(1.5f)
                                .Width(1.5f)
                                .Right(0)
                                .Center();
                        })
                        .Grow(4);


                    v1.Hstack(h2 =>
                    {
                        h2.Hstack(h3 =>
                        {
                            h3.Button("Menu",
<<<<<<< HEAD
                                    clickAction: (e, a) => { UIManager.StartLayer(nameof(MainMenuUI)); })
=======
                                    clickAction: (e, a) => { UIManager.Start(nameof(MainMenuUI),ParentScene); })
>>>>>>> 25eb9767d2ff3738c9e8f07fa547477d11c8cf6b
                                .Height(0.8f)
                                .Width(0.4f)
                                .Center()
                                .Image("button");
                        });
                    });

                    v1.Spacer();
                })
                    .Grow(3);

                h1.Spacer();

            })
                .AspectRatio(1.78f)
                .Center();
        }
    }
}
