using System.Collections.Generic;
using System.Linq;
using BFB.Client.Helpers;
using BFB.Engine.Entity;
using BFB.Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Client.UI
{
    public class ControlUI : UILayer
    {
        
        private ControlSettings Model { get; set; }
        
        public ControlUI() : base(nameof(ControlUI)) { }

        protected override void Init()
        {
            Model = ClientSettings.GetSettings().ControlSettings;
        }

        public override void Body()
        {
            RootUI.Vstack(v1 =>
                {
                    v1.Text("Control Settings")
                        .FontSize(1.5f)
                        .Color(Color.White)
                        .Grow(2);

                    
                    v1.Vstack(v2 =>
                    {
                        v2.Text("Vertical Scroll Sensitivity:")
                            .Color(Color.White)
                            .JustifyText(JustifyText.Start);

                        v2.SliderFor(Model, x => x.VerticalScrollSensitivity, 0, 100);
                    }).Grow(3);

                    v1.Spacer();
                    
                    v1.Vstack(v2 =>
                    {
                        v2.Text("Horizontal Scroll Sensitivity:")
                            .Color(Color.White)
                            .JustifyText(JustifyText.Start);

                        v2.SliderFor(Model, x => x.HorizontalScrollSensitivity, 0, 100);
                    }).Grow(3);

                    v1.Spacer();
                    v1.Text("Keyboard Controls:").JustifyText(JustifyText.Start).Color(Color.White);
                        
                    v1.ScrollableContainer(s2 =>
                    {
                        s2.ListFor(Model, x => x.KeyboardControls.ToList(), (stack, item) =>
                            {
                                stack.Hstack(h3 =>
                                {
                                    h3.Text(item.Key);
                                    h3.ControlFor(item, x => x.Value, (e,s) =>
                                    {
                                        
                                    });
                                }).Height(0.3f);
                            });

                    })
                        .Background(Color.Silver)
                        .Border(3, new Color(211, 212, 210))
                        .Grow(6);

                    v1.Spacer();

                    v1.Hstack(h2 =>
                    {
                        h2.Button("Back to Settings",
                                clickAction: (e, a) => { UIManager.StartLayer(nameof(SettingsMenuUI), ParentScene); })
                            .Height(0.9f);

                        h2.Button("Save Settings",
                                clickAction: (e, a) =>
                                {
                                    //Save sound settings
                                    ClientSettings settings = ClientSettings.GetSettings();
                                    settings.ControlSettings = Model;
                                    ClientSettings.SaveSettings(settings);
                                })
                            .Height(0.9f);
                    }).Grow(2);
                })
                    .Width(0.7f)
                    .Height(0.9f)
                    .Center();
        }
    }
}