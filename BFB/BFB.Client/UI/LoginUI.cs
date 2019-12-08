using System;
using BFB.Client.Scenes;
using BFB.Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Client.UI
{
    public class LoginUI : UILayer
    {
        public string Username { get; set; }
        
        public string Password { get; set; }

        public LoginUI() : base(nameof(LoginUI))
        {
            BlockInput = true;
        }

        protected override void Init()
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

                        //Game title
                        v1.Hstack(h2 =>
                            {
                                h2.Text("Boss Fight Battlegrounds")
                                    .Color(Color.White)
                                    .FontSize(1.5f);
                            })
                            .Grow(2);

                        //Username
                        v1.Hstack(h2 =>
                        {
                            h2.Spacer();
                            
                            h2.Text("Username or email:")
                                .Color(Color.White)
                                .JustifyText(JustifyText.Start)
                                .Grow(9);
                        });

                        //Username text box
                        v1.Hstack(h2 =>
                        {
                            h2.TextBoxFor(this, x => x.Username)
                                .Width(0.8f)
                                .Height(0.8f)
                                .Center()
                                .Background(Color.White)
                                .Color(Color.Black);
                        });

                        //Password
                        v1.Hstack(h2 =>
                        {
                            h2.Spacer();
                            
                            h2.Text("Password:")
                                .Color(Color.White)
                                .JustifyText(JustifyText.Start)
                                .Grow(9);
                        });

                        //Password field
                        v1.Hstack(h2 =>
                        {
                            h2.TextBoxFor(this, x => x.Password)
                                .Width(0.8f)
                                .Height(0.8f)
                                .Center()
                                .Background(Color.White)
                                .Color(Color.Black);
                        });

                        v1.Hstack(h2 =>
                        {
                            h2.Button("Login!",
                                    clickAction: (e, a) => { UIManager.StartLayer(nameof(MainMenuUI), ParentScene); })
                                .Width(0.8f)
                                .Height(0.8f)
                                .Center();
                        });

                        v1.Spacer();
                    })
                    .Grow(3)
                    .Center();

                h1.Spacer();
            });
        }
    }
}