using BFB.Client.Helpers;
using BFB.Engine.UI;
using Microsoft.Xna.Framework;

namespace BFB.Client.UI
{
    public class SoundSettingsUI : UILayer
    {
        private SoundSettings Model { get; set; }

        public SoundSettingsUI() : base(nameof(SoundSettingsUI)) { }

        protected override void Init()
        {
            Model = ClientSettings.GetSettings().SoundSettings;
        }

        public override void Body()
        {

            RootUI.Vstack(v1 =>
                {
                    v1.Text("Sound Settings")
                        .FontSize(1.5f)
                        .Color(Color.White)
                        .Grow(2);

                    v1.Vstack(v2 =>
                    {
                        v2.Text("Master Volume:")
                            .Color(Color.White)
                            .JustifyText(JustifyText.Start);
                        
                        v2.Hstack(h3 =>
                        {
                            h3.SliderFor(Model, x => x.MasterVolume, 0, 100).Grow(10);
                            h3.Hstack(h4 =>
                            {
                                h4.CheckBoxFor(Model, x => x.MasterMute)
                                    .Center();
                            });
                        });
                    }).Grow(3);

                    v1.Spacer();

                    v1.Vstack(v2 =>
                    {
                        v2.Text("Music Volume:")
                            .Color(Color.White)
                            .JustifyText(JustifyText.Start);
                        
                        v2.Hstack(h3 =>
                        {
                            h3.SliderFor(Model, x => x.MusicVolume, 0, 100).Grow(10);
                            h3.Hstack(h4 =>
                            {
                                h4.CheckBoxFor(Model, x => x.MusicMute)
                                    .Center();
                            });
                        });
                    }).Grow(3);

                    v1.Spacer();

                    v1.Vstack(v2 =>
                    {
                        v2.Text("Sound Effects Volume:")
                            .Color(Color.White)
                            .JustifyText(JustifyText.Start);
                        
                        v2.Hstack(h3 =>
                        {
                            h3.SliderFor(Model, x => x.SoundEffectVolume, 0, 100).Grow(10);
                            h3.Hstack(h4 =>
                            {
                                h4.CheckBoxFor(Model, x => x.SoundEffectMute)
                                    .Center();
                            });
                        });
                    }).Grow(3);
                    
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
                                    settings.SoundSettings = Model;
                                    ClientSettings.SaveSettings(settings);

                                })
                            .Height(0.9f);
                    }).Grow(2);
                    
                })
                    .Width(0.7f)
                    .Height(0.8f)
                    .Center();
        }
    }
}