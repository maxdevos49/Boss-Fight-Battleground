using System;
using System.ComponentModel;
using System.Drawing;

namespace BFB.Engine.UI
{
    public class UIBuilder<TModel>
    {

        //Generic Model
        private TModel _model;
        
        //Initial UI Parameters
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        

        public UIBuilder()
        {
        }
        
        public UIBuilder(TModel model)
        {
            _model = model;
        }
        
        public void BuildView(Action<UIBuilder<TModel>> viewHandler)
        {
//            viewHandler
        }
        
        
    }
}