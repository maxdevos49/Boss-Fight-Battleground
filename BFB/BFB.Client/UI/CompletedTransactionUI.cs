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

                    v1.Hstack(h2 =>
                        {
                            h2.Hstack(h3 =>
                                {
                                    h3.Text(
                                        "Your Transaction has been completed. The items have been added to your account. Happy Hunting!");
                                })
                                .Height(1.5f)
                                .Width(1.75f)
                                .Left(480);
                        })
                        .Grow(10);

                    v1.Spacer();
                    v1.Hstack(h2 =>
                    {
                        h2.Hstack(h3 =>
                        {
                            h3.Button("Menu",
                                    clickAction: (e, a) => { UIManager.StartLayer(nameof(MainMenuUI), ParentScene); })
                                .Height(0.8f)
                                .Width(0.6f)
                                .Center();
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
