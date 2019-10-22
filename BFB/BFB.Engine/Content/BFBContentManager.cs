using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using JetBrains.Annotations;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;

namespace BFB.Engine.Content
{
    public class BFBContentManager
    {

        #region Properties

        private readonly ContentManager _contentManager;
        
        //Dictionary<contentKey, Content>
        private readonly  Dictionary<string, Texture2D> _textureContent;
        private readonly Dictionary<string, AnimatedTexture> _animatedTexturesContent;
        private readonly  Dictionary<string, SpriteFont> _fontContent;
        private readonly  Dictionary<string, Song> _audioContent;//Probably in future when ready to use this create a wrapper class
        
        
        #endregion
        
        #region Constructor

        public BFBContentManager(ContentManager contentManager)
        {
            _contentManager = contentManager;
            
            _textureContent = new Dictionary<string, Texture2D>();
            _animatedTexturesContent = new Dictionary<string, AnimatedTexture>();
            _fontContent = new Dictionary<string, SpriteFont>();
            _audioContent = new  Dictionary<string, Song>();
            
            //TODO in future add groups so groups can be loaded and unloaded together
        }
        
        #endregion
        
        #region AddTexture

        public void AddTexture(string textureKey, Texture2D texture)
        {
            if (!_textureContent.ContainsKey(textureKey))
                _textureContent.Add(textureKey, texture);
        }
        
        #endregion
        
        #region GetTexture

        public Texture2D GetTexture(string textureKey)
        {
            return _textureContent.ContainsKey(textureKey) ? _textureContent[textureKey] : throw new KeyNotFoundException($"The textureKey: {textureKey} was not found.");
        }
        
        #endregion

        #region UnloadTexture

        public void UnloadTexture(string textureKey)
        {
            if (_textureContent.ContainsKey(textureKey)) 
                return;
            
            _textureContent[textureKey].Dispose();
            _textureContent.Remove(textureKey);
        }

        
        #endregion
        
        #region AddAudio

        public void AddAudio(string audioKey, Song audio)
        {
            if (!_audioContent.ContainsKey(audioKey))
                _audioContent.Add(audioKey, audio);
        }
        
        #endregion
        
        #region GetAudio

        public Song GetAudio(string audioKey)
        {
            return _audioContent.ContainsKey(audioKey) ? _audioContent[audioKey] :  throw new KeyNotFoundException($"The audioKey: {audioKey} was not found.");
        }
        
        #endregion
        
        #region UnloadAudio

        public void UnloadAudio(string audioKey)
        {
            if (_audioContent.ContainsKey(audioKey)) 
                return;
            
            _audioContent[audioKey].Dispose();
            _audioContent.Remove(audioKey);
        }

        
        #endregion
        
        #region AddFont

        public void AddFont(string fontKey, SpriteFont font)
        {
            if (!_fontContent.ContainsKey(fontKey))
                _fontContent.Add(fontKey, font);
        }
        
        #endregion
        
        #region getFont

        public SpriteFont GetFont(string fontKey)
        {
            return _fontContent.ContainsKey(fontKey) ? _fontContent[fontKey] :  throw new KeyNotFoundException($"The fontKey: {fontKey} was not found.");
        }
        
        #endregion
        
        #region UnloadFont

        public void UnloadFont(string fontKey)
        {
            if (_fontContent.ContainsKey(fontKey)) 
                return;
            
            _fontContent.Remove(fontKey);
        }

        
        #endregion
        
        #region AddAnimatedTexture

        public void AddAnimatedTexture(string textureKey, AnimatedTexture texture)
        {
            if (!_animatedTexturesContent.ContainsKey(textureKey))
                _animatedTexturesContent.Add(textureKey, texture);
        }
        
        #endregion
        
        #region GetAnimatedTexture

        public AnimatedTexture GetAnimatedTexture(string textureKey)
        {
            return _animatedTexturesContent.ContainsKey(textureKey) ? _animatedTexturesContent[textureKey] : throw new KeyNotFoundException($"The animated texture Key: {textureKey} was not found.");
        }
        
        #endregion

        #region UnloadAnimatedTexture

        public void UnloadAnimatedTexture(string textureKey)
        {
            if (_animatedTexturesContent.ContainsKey(textureKey)) 
                return;
            
            _animatedTexturesContent[textureKey].Texture.Dispose();
            _animatedTexturesContent.Remove(textureKey);
        }

        
        #endregion
        
        #region ParseContent

        public void ParseContent()
        {
            string json;
            
            //Get file for Parsing
            using (StreamReader r = new StreamReader("content.json"))
            {
                json = r.ReadToEnd();
            }
            
            ContentFileSchema content = JsonConvert.DeserializeObject<ContentFileSchema>(json);
            
            //Parse texture
            ParseTextures(content.Textures);
            
            //Parse fonts
            ParseFonts(content.Fonts);
            
            //Parse animated textures
            ParseAnimatedTextures(content.AnimatedTextures);
            
            //Parse audio
            ParseAudio(/*TODO*/);
        }
        
        #endregion
        
        #region ParseTextures

        private void ParseTextures(IEnumerable<ContentGroupSchema> textures)
        {
            foreach (ContentGroupSchema texture in textures)
            {
                Console.WriteLine("Loading Texture: " + texture.Key);
                
                if(_textureContent.ContainsKey(texture.Key))
                    throw new DuplicateNameException($"The texture key: {texture.Key} was found more then once while parsing textures");
                
                _textureContent.Add(texture.Key, _contentManager.Load<Texture2D>(texture.Location));
            }
        }
        
        #endregion
        
        #region ParseAnimatedTextures

        private void ParseAnimatedTextures(List<AnimatedTexture> textures)
        {
            foreach (AnimatedTexture texture in textures)
            {
                Console.WriteLine("Loading Animated Texture: " + texture.Key);
                
                if(_animatedTexturesContent.ContainsKey(texture.Key))
                    throw new DuplicateNameException($"The animated texture key: {texture.Key} was found more then once while parsing animated textures");

                texture.Texture = _contentManager.Load<Texture2D>(texture.Location);
                
                _animatedTexturesContent.Add(texture.Key, texture);

            }
        }
        
        #endregion
        
        #region ParseFonts

        private void ParseFonts(List<ContentGroupSchema> fonts)
        {
            foreach (ContentGroupSchema font in fonts)
            {
                Console.WriteLine("Loading font: " + font.Key);
                
                if(_textureContent.ContainsKey(font.Key))
                    throw new DuplicateNameException($"The font key: {font.Key} was found more then once while parsing fonts");
                
                _fontContent.Add(font.Key, _contentManager.Load<SpriteFont>(font.Location));
            }
        }
        
        #endregion
        
        #region ParseAudio

        private void ParseAudio()
        {
            //TODO
        }
        
        #endregion
    }

    #region Content Schema Classes
    
    [UsedImplicitly]
    public class ContentFileSchema
    {
        [UsedImplicitly]

        public List<ContentGroupSchema> Fonts { get; set; }
        
        [UsedImplicitly]

        public List<ContentGroupSchema> Textures { get; set; }
        
        [UsedImplicitly]

        public List<ContentGroupSchema> Audio { get; set; }

        [UsedImplicitly]

        public List<AnimatedTexture> AnimatedTextures { get; set; }

    }
    
    [UsedImplicitly]

    public class ContentGroupSchema
    {
        [UsedImplicitly]

        public string Key { get; set; }

        [UsedImplicitly]

        public string Location { get; set; }
    }


    
    #endregion
    
}