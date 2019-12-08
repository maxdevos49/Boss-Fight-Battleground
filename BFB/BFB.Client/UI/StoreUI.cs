using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using BFB.Engine.UI.Constraints;
using Microsoft.Xna.Framework;

namespace BFB.Client.UI
{
    public class StoreUI : UILayer
    {
        public StoreUI() : base(nameof(StoreUI))
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
                                            h3.Text("Store")
                                                .Color(Color.White)
                                                .FontSize(2f);
                                        })
                                        .Height(0.7f)
                                        .Width(0.7f)
                                        .Right(0)
                                        .Center();
                                })
                                .Grow(4);

                            v1.Hstack(h2 =>
                            {
                                h2.Hstack(h4 => { h4.Text("1 Red Gem - $0.99"); })
                                    .Height(1.0f)
                                    .Width(1.5f);

                                h2.Button("Purchase",
                                        clickAction: (e, a) =>
                                        {
                                            UIManager.StartLayer(nameof(CreditCardUI), ParentScene);
                                        })
                                    .Height(1.0f)
                                    .Width(0.6f)
                                    .Right(0)
                                    .Top(5);
                            }).Grow(1);

                            v1.Hstack(h2 =>
                            {
                                h2.Hstack(h4 => { h4.Text("5 Red Gems - $3.99"); })
                                    .Height(1.0f)
                                    .Width(1.5f);
                                h2.Button("Purchase",
                                        clickAction: (e, a) =>
                                        {
                                            UIManager.StartLayer(nameof(CreditCardUI), ParentScene);
                                        })
                                    .Height(1.0f)
                                    .Width(0.6f)
                                    .Right(0)
                                    .Top(5);
                            }).Grow(1);

                            v1.Hstack(h2 =>
                            {
                                h2.Hstack(h4 => { h4.Text("20 Red Gems - $16.99"); })
                                    .Height(1.0f)
                                    .Width(1.5f);
                                h2.Button("Purchase",
                                        clickAction: (e, a) =>
                                        {
                                            UIManager.StartLayer(nameof(CreditCardUI), ParentScene);
                                        })
                                    .Height(1.0f)
                                    .Width(0.6f)
                                    .Right(0)
                                    .Top(5);
                            }).Grow(1);

                            v1.Hstack(h2 =>
                            {

                            }).Grow(2);

                            v1.Hstack(h2 =>
                            {

                            }).Grow(2);


                            v1.Hstack(h2 =>
                            {
                                h2.Hstack(h3 =>
                                {
                                    h3.Button("Back",
                                            clickAction: (e, a) =>
                                            {
                                                UIManager.StartLayer(nameof(MainMenuUI), ParentScene);
                                            })
                                        .Height(0.8f)
                                        .Width(0.2f)
                                        .Right(0);
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
