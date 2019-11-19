using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using BFB.Engine.UI.Constraints;
using Microsoft.Xna.Framework;

namespace BFB.Client.UI
{
    class CreditCardUI : UILayer
    {
        public NameModel nameModel { get; set; }
        public CCNumberModel cardModel { get; set; }
        public SecurityCodeModel codeModel { get; set; }

        public CreditCardUI() : base(nameof(CreditCardUI))
        {
            nameModel = new NameModel
            {
                name = ""
            };

            cardModel = new CCNumberModel
            {
                number = ""
            };

            codeModel = new SecurityCodeModel
            {
                code = ""
            };
        }

        protected override void Init()
        {
            nameModel = new NameModel
            {
                name = ""
            };

            cardModel = new CCNumberModel
            {
                number = ""
            };

            codeModel = new SecurityCodeModel
            {
                code = ""
            };
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

                        h2.Hstack(h3 => { h3.Text("Transaction - Please Enter Credit Card Info"); })
                            .Height(1.5f)
                            .Width(1.5f)
                            .Right(0)
                            .Center();
                    })
                        .Grow(4);

                        v1.Hstack(h2 =>
                        {
                            h2.Hstack(h4 => { h4.Text("Full Name"); })
                                .Height(1.0f)
                                .Width(1.0f);
                            h2.TextBoxFor(nameModel, x => x.name)
                                .Width(1.0f)
                                .Height(0.8f)
                                .Center()
                                .Background(Color.White)
                                .Color(Color.Black);
                        }).Grow(1);

                        v1.Hstack(h2 =>
                        {
                            h2.Hstack(h4 => { h4.Text("Credit Card Number"); })
                                .Height(1.0f)
                                .Width(1.0f);
                            h2.TextBoxFor(cardModel, x => x.number)
                                .Width(1.0f)
                                .Height(0.8f)
                                .Center()
                                .Background(Color.White)
                                .Color(Color.Black);
                        }).Grow(1);

                        v1.Hstack(h2 =>
                        {
                            h2.Hstack(h4 => { h4.Text("Security Code"); })
                                .Height(1.0f)
                                .Width(1.0f);
                            h2.TextBoxFor(codeModel, x => x.code)
                                .Width(1.0f)
                                .Height(0.8f)
                                .Center()
                                .Background(Color.White)
                                .Color(Color.Black);
                        }).Grow(1);

                        v1.Hstack(h2 =>
                    {
                        h2.Hstack(h3 =>
                        {
                            h3.Button("Purchase",
<<<<<<< HEAD
                                    clickAction: (e, a) => { UIManager.StartLayer(nameof(CompletedTransactionUI)); })
=======
                                    clickAction: (e, a) => { UIManager.Start(nameof(CompletedTransactionUI),ParentScene); })
>>>>>>> 25eb9767d2ff3738c9e8f07fa547477d11c8cf6b
                                .Height(0.8f)
                                .Width(0.3f)
                                .Right(0)
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

    public class NameModel
    {
        public string name { get; set; }
    }

    public class CCNumberModel
    {
        public string number { get; set; }
    }

    public class SecurityCodeModel
    {
        public string code { get; set; }
    }
}
