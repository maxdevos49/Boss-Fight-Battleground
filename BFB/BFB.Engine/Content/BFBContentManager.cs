using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace BFB.Engine.Content
{
    public class BFBContentManager
    {

        #region Properties

        private readonly ContentManager _contentManager;
        
        //Dictionary<contentKey, Content>
        private readonly  Dictionary<string, Texture2D> _textureContent;//Create a custom Texture2d wrapper class to hold animation details
        private readonly  Dictionary<string, SpriteFont> _fontContent;
        private readonly  Dictionary<string, Song> _audioContent;//Probably in future when ready to use this create a wrapper class
        
        
        #endregion
        
        #region Constructor

        public BFBContentManager(ContentManager contentManager)
        {
            _contentManager = contentManager;
            
            _textureContent = new Dictionary<string, Texture2D>();
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
        
        #region ParseContent

        public void ParseContent()
        {
            //TODO fills in textures, audio, and fonts, and anything else that may be important
        }
        
        #endregion

    }
}