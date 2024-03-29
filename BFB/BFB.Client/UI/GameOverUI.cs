﻿using System;
using System.Collections.Generic;
using System.Text;
using BFB.Client.Scenes;
using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using BFB.Engine.UI.Constraints;
using Microsoft.Xna.Framework;

namespace BFB.Client.UI
{
    public class GameOverUI : UILayer
    {
        private string displayText;

        public GameOverUI() : base(nameof(GameOverUI))
        {
            displayText = "Game over! Resetting game...";
        }

        protected override void Init()
        {
            AddGlobalListener("onUpdateDisplay", e =>
            {
                displayText = e.Message;
            });
        }

        public override void Body()
        {
            RootUI.Background(Color.Transparent);
            RootUI.Hstack(h1 =>
                {

                    h1.Spacer();

                    h1.Vstack(v1 =>
                        {

                            v1.Spacer();

                            v1.Hstack(h2 =>
                                {
                                    h2.Hstack(h3 => { h3.Text(displayText); })
                                        .Height(0.7f)
                                        .Width(0.7f)
                                        .Center();
                                })
                                .Grow(3);

                            v1.Spacer(3);

                        })
                        .Grow(3);

                    h1.Spacer();

                })
                .Background(new Color(0, 0, 0, 100));
        }
    }
}