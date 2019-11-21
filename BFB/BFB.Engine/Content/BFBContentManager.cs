using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;

namespace BFB.Engine.Content
{
    /// <summary>
    /// A modified version of the Monogame ContentManager. Rather than extending it, one of the properties is a Monogame ContentManager.
    /// </summary>
    public class BFBContentManager
    {

        #region Properties

        private readonly ContentManager _contentManager;
        
        private readonly  Dictionary<string, Texture2D> _textureContent;
        
        private readonly Dictionary<string, AnimatedTexture> _animatedTexturesContent;

        private readonly Dictionary<string, AtlasTexture> _atlasTexturesContent;
            
        private readonly  Dictionary<string, SpriteFont> _fontContent;

        private readonly  Dictionary<string, Song> _audioContent;//Probably in future when ready to use this create a wrapper class
        
        
        #endregion
        
        #region Constructor

        /// <summary>
        /// A constructor that does constructor things: initliazes parameters.
        /// </summary>
        /// <param name="contentManager"></param>
        public BFBContentManager(ContentManager contentManager)
        {
            _contentManager = contentManager;
            
            _textureContent = new Dictionary<string, Texture2D>();
            _animatedTexturesContent = new Dictionary<string, AnimatedTexture>();
            _atlasTexturesContent = new Dictionary<string, AtlasTexture>();
            _fontContent = new Dictionary<string, SpriteFont>();
            _audioContent = new  Dictionary<string, Song>();
            
            //TODO in future add groups so groups can be loaded and unloaded together
        }
        
        #endregion
        
        #region AddTexture

        /// <summary>
        /// Adds a static texture to the dictionary, provided it does not already exist.
        /// </summary>
        /// <param name="textureKey"></param>
        /// <param name="texture"></param>
        public void AddTexture(string textureKey, Texture2D texture)
        {
            if (!_textureContent.ContainsKey(textureKey))
                _textureContent.Add(textureKey, texture);
        }
        
        #endregion
        
        #region GetTexture
        /// <summary>
        /// Returns a loaded texture.
        ///
        /// Throws a KeyNotFoundException when the key is not found.
        /// </summary>
        /// <param name="textureKey"></param>
        /// <returns></returns>
        public Texture2D GetTexture(string textureKey)
        {
            if(string.IsNullOrEmpty(textureKey))
                throw new ArgumentNullException(nameof(textureKey),"The textureKey provided was null. Try supplying a valid string");
            
            return _textureContent.ContainsKey(textureKey) ? _textureContent[textureKey] : throw new KeyNotFoundException($"The textureKey: {textureKey} was not found.");
        } 
        
        #endregion

        #region UnloadTexture
        /// <summary>
        /// Unloads the texture if it will never be used again.
        /// </summary>
        /// <param name="textureKey"></param>
        public void UnloadTexture(string textureKey)
        {
            if (_textureContent.ContainsKey(textureKey)) 
                return;
            
            _textureContent[textureKey].Dispose();
            _textureContent.Remove(textureKey);
        }

        
        #endregion
        
        #region AddAudio
        /// <summary>
        /// Adds the loaded audio to the dictionary, provided it does not already exist.
        /// </summary>
        /// <param name="audioKey"></param>
        /// <param name="audio"></param>
        public void AddAudio(string audioKey, Song audio)
        {
            if (!_audioContent.ContainsKey(audioKey))
                _audioContent.Add(audioKey, audio);
        }
        
        #endregion
        
        #region GetAudio
        /// <summary>
        /// Returns the loaded audio from the dictionary.
        /// </summary>
        /// <param name="audioKey"></param>
        /// <returns></returns>
        public Song GetAudio(string audioKey)
        {
            return _audioContent.ContainsKey(audioKey) ? _audioContent[audioKey] :  throw new KeyNotFoundException($"The audioKey: {audioKey} was not found.");
        }
        
        #endregion
        
        #region UnloadAudio
        /// <summary>
        /// Unloads the loaded audio if we'll never use it again. Also disposes it.
        /// </summary>
        /// <param name="audioKey"></param>
        public void UnloadAudio(string audioKey)
        {
            if (_audioContent.ContainsKey(audioKey)) 
                return;
            
            _audioContent[audioKey].Dispose();
            _audioContent.Remove(audioKey);
        }

        
        #endregion
        
        #region AddFont
        /// <summary>
        /// Adds the loaded font to the dictionary, provided it does not already exist.
        /// </summary>
        /// <param name="fontKey"></param>
        /// <param name="font"></param>
        public void AddFont(string fontKey, SpriteFont font)
        {
            if (!_fontContent.ContainsKey(fontKey))
                _fontContent.Add(fontKey, font);
        }
        
        #endregion
        
        #region getFont
        /// <summary>
        /// Returns the loaded font that you wanna use.
        /// </summary>
        /// <param name="fontKey"></param>
        /// <returns></returns>
        public SpriteFont GetFont(string fontKey)
        {
            return _fontContent.ContainsKey(fontKey) ? _fontContent[fontKey] :  throw new KeyNotFoundException($"The fontKey: {fontKey} was not found.");
        }
        
        #endregion
        
        #region UnloadFont
        /// <summary>
        /// Unloads the font.
        /// </summary>
        /// <param name="fontKey"></param>
        public void UnloadFont(string fontKey)
        {
            if (_fontContent.ContainsKey(fontKey)) 
                return;
            
            _fontContent.Remove(fontKey);
        }

        
        #endregion
        
        #region AddAnimatedTexture
        /// <summary>
        /// Adds an animated texture to the dictionary, provided it does not already exist.
        /// </summary>
        /// <param name="textureKey"></param>
        /// <param name="texture"></param>
        public void AddAnimatedTexture(string textureKey, AnimatedTexture texture)
        {
            if (!_animatedTexturesContent.ContainsKey(textureKey))
                _animatedTexturesContent.Add(textureKey, texture);
        }
        
        #endregion
        
        #region GetAnimatedTexture
        /// <summary>
        /// Gives the animated texture that has already been loaded.
        /// </summary>
        /// <param name="textureKey"></param>
        /// <returns>Returns the correct animated texture</returns>
        public AnimatedTexture GetAnimatedTexture(string textureKey)
        {
            return _animatedTexturesContent.ContainsKey(textureKey) ? _animatedTexturesContent[textureKey] : throw new KeyNotFoundException($"The animated texture Key: {textureKey} was not found.");
        }
        
        #endregion
        
        #region GetAtlasTexture
        
        public AtlasTexture GetAtlasTexture(string atlasKey)
        {
            return _atlasTexturesContent.ContainsKey(atlasKey)
                ? _atlasTexturesContent[atlasKey]
                : _atlasTexturesContent["Tiles:Missing"];
        }
        
        #endregion

        #region UnloadAnimatedTexture
        /// <summary>
        /// Unloads the animated texture.
        /// </summary>
        /// <param name="textureKey"></param>
        public void UnloadAnimatedTexture(string textureKey)
        {
            if (_animatedTexturesContent.ContainsKey(textureKey)) 
                return;
            
            _animatedTexturesContent[textureKey].Texture.Dispose();
            _animatedTexturesContent.Remove(textureKey);
        }

        
        #endregion
        
        #region ParseContent
        /// <summary>
        /// Parses content.json to figure out what content needs to be loaded.
        /// </summary>
        public void ParseContent()
        {
            string json;
            
            //Get file for Parsing
            using (StreamReader r = new StreamReader("content.json"))
            {
                json = r.ReadToEnd();
            }
            
            ContentJSONSchema content = JsonConvert.DeserializeObject<ContentJSONSchema>(json);
            
            //Parse texture
            ParseTextures(content.Textures);
            
            //Parse fonts
            ParseFonts(content.Fonts);
            
            //Parse animated textures
            ParseAnimatedTextures(content.AnimatedTextures);
            
            //Parse atlas textures
            ParseAtlasTextures(content.AtlasTextures);

            //Parse audio
//            ParseAudio(/*TODO*/);
        }
        
        #endregion
        
        #region ParseTextures
        /// <summary>
        /// Parses the textures and loads them.
        /// </summary>
        /// <param name="textureConfig"></param>
        private void ParseTextures(Dictionary<string,string> textureConfig)
        {
            foreach (var texture in textureConfig)
            {
                Console.WriteLine("Loading Texture: " + texture.Key);
                
                if(_textureContent.ContainsKey(texture.Key))
                    throw new DuplicateNameException($"The texture key: {texture.Key} was found more then once while parsing textures");
                
                _textureContent.Add(texture.Key, _contentManager.Load<Texture2D>(texture.Value));
            }
        }
        
        #endregion
        
        #region ParseAnimatedTextures
        /// <summary>
        /// Parses and loads the animated textures.
        /// </summary>
        /// <param name="textureConfig"></param>
        private void ParseAnimatedTextures(Dictionary<string,AnimatedTexture> textureConfig)
        {
            foreach ((string key, AnimatedTexture value) in textureConfig)
            {
                Console.WriteLine("Loading Animated Texture: " + key);
                
                if(_animatedTexturesContent.ContainsKey(key))
                    throw new DuplicateNameException($"The animated texture key: {key} was found more then once while parsing animated textures");

                value.Texture = _contentManager.Load<Texture2D>(value.Location);
                
                //color
                if (value.RandomColor == true)
                {
                    Random random = new Random();
                    

                    int r = random.Next(0, 255);
                    int g = random.Next(0,255);
                    int b = random.Next(0,255);
                    
                    value.ParsedColor = new Color(r,g,b,1f);
                    Console.WriteLine(value.ParsedColor);
                }
                else if (value.ColorConfig == null)
                {
                    value.ParsedColor = Color.White;
                }
                else
                {
                    string[] colorArray = value.ColorConfig.Split(",");

                    if (colorArray.Length != 4)
                        throw new Exception($"Color parsing failed for Animation texture: {key}");
                
                    int r = int.Parse(colorArray[0]);
                    int g = int.Parse(colorArray[1]);
                    int b = int.Parse(colorArray[2]);
                    float alpha = float.Parse(colorArray[3]);
                
                    value.ParsedColor = new Color(r,g,b,alpha);
                }
                
                _animatedTexturesContent.Add(key, value);
            }
        }
        
        #endregion
        
        #region ParseAtlasTexture

        private void ParseAtlasTextures(Dictionary<string,AtlasTexture> atlasConfig) 
        {
            foreach ((string key, AtlasTexture value) in atlasConfig)
            {
                Console.WriteLine("Loading Atlas Texture: " + key);
                
                if(_atlasTexturesContent.ContainsKey(key))
                    throw new DuplicateNameException($"The animated texture key: {key} was found more then once while parsing animated textures");

                if(!_textureContent.ContainsKey(value.TextureKey))
                    throw new KeyNotFoundException($"The key: \"{value.TextureKey}\" was not a valid key. Try confirming that the key exist in the Textures section of the json");
                    
                value.Texture = _textureContent[value.TextureKey];//Get reference to the correct texture
                
                _atlasTexturesContent.Add(key, value);
            }
        }
        
        #endregion
        
        #region ParseFonts
        
        /// <summary>
        /// Parses and loads the fonts.
        /// </summary>
        /// <param name="fontConfig"></param>
        private void ParseFonts(Dictionary<string,string> fontConfig)
        {
            foreach (var font in fontConfig)
            {
                Console.WriteLine("Loading font: " + font.Key);
                
                if(_textureContent.ContainsKey(font.Key))
                    throw new DuplicateNameException($"The font key: {font.Key} was found more then once while parsing fonts");
                
                _fontContent.Add(font.Key, _contentManager.Load<SpriteFont>(font.Value));
            }
        }
        
        #endregion
        
        #region ParseAudio
        /// <summary>
        /// Parses and loads the audio.
        /// </summary>
        private void ParseAudio()
        {
            //TODO
        }
        
        #endregion
    }

    #region Content Schema Classes
    
    /// <summary>
    /// Schema for what the content will look like.
    /// </summary>
    [UsedImplicitly]
    public class ContentJSONSchema
    {
        public Dictionary<string,string> Fonts { get; set; }

        
        public Dictionary<string,string> Audio { get; set; }
        
        public Dictionary<string,string> Textures { get; set; }
        
        public Dictionary<string,AtlasTexture> AtlasTextures { get; set; }
        
        public Dictionary<string,AnimatedTexture> AnimatedTextures { get; set; }

    }
    
    #endregion
    
}