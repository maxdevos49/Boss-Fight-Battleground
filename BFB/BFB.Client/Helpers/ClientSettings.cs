using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace BFB.Client.Helpers
{
    [UsedImplicitly]
    public class ClientSettings
    {
        public bool DevelopmentMode { get; set; }
        
        [UsedImplicitly]
        public ServerSettings ServerSettings { get; set; }
       
        [UsedImplicitly]
        public SoundSettings SoundSettings { get; set; }
        
        [UsedImplicitly]
        public ControlSettings ControlSettings { get; set; }
        
        
        #region GetSettings

        public static ClientSettings GetSettings()
        {
            string json;

            //Get file for Parsing
            using (StreamReader r = new StreamReader("settings.json"))
            {
                json = r.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<ClientSettings>(json);
        }
        
        #endregion
        
        #region SaveSettings
        
        public static void SaveSettings(ClientSettings model)
        {
            string serializedWorldData = JsonConvert.SerializeObject(model, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            try
            {
                string path = Directory.GetCurrentDirectory() + "/settings.json";

                using (StreamWriter s = File.CreateText(path))
                {
                    s.Write(serializedWorldData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        
        #endregion
    }

    #region ServerSettings
    
    public class ServerSettings
    {
        [UsedImplicitly]
        public string DirectConnect { get; set; }
        
        [UsedImplicitly]
        public List<ServerMenuItem> Servers { get; set; }

        public ServerSettings()
        {
            Servers = new List<ServerMenuItem>();
        }
        

    }
    
    [UsedImplicitly]
    public class ServerMenuItem
    {
        [UsedImplicitly]
        public string Name { get; set; }

        [UsedImplicitly]
        public string Ip { get; set; }
    }
    
    #endregion

    #region SoundSettings
    
    [UsedImplicitly]
    public class SoundSettings
    {
        [UsedImplicitly]
        public bool MasterMute { get; set; }
        
        [UsedImplicitly]
        public float MasterVolume { get; set; }
        
        [UsedImplicitly]
        public bool MusicMute { get; set; }
        
        [UsedImplicitly]
        public float MusicVolume { get; set; }
        
        [UsedImplicitly]
        public bool SoundEffectMute { get; set; }
        
        [UsedImplicitly]
        public float SoundEffectVolume { get; set; }
    }
    
    #endregion
    
    #region ControlSettings

    [UsedImplicitly]
    public class ControlSettings
    {
        public float VerticalScrollSensitivity { get; set; }
        
        public float HorizontalScrollSensitivity { get; set; }
        
        public Dictionary<string, Keys> KeyboardControls { get; set; }
    }
    
    #endregion
    
    #region ConnectionSettings
    
    [UsedImplicitly]
    public class ConnectionSettings
    {
        public static string Ip { get; set; }
        
        public static int Port { get; set; }
    }
    
    #endregion
   
}