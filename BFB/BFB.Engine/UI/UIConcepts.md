```csharp

class SomeModel
{
    public string filter { get; set; }

    public List<SomeListItem> items { get; set; }
}

class SomeListItem
{
    public Texture2D Image { get; set; }

    public string Title { get; set; }
    
    public string Subtitle { get; set; }
}

struct MyView : View
{
    //Figure out how to bind state to views model
    public SomeModel model;

    //possible probably with many static methods that return instances
    public UIStructure Body = UI.Create(SomeModel model, (v) => {

        v.TextBox(m => m.filter)
            .Background(Color.Red)
            .Color(Color.Red)
            .Font(Font.Subtle);

        v.List(m => m.Items, (item) => {
            v.Image(m => m.Image).AspectRatio(1)
            v.VStack((){
                v.Text(m => item.Title)
                    .Font(Font.Title);
                v.Text(m => item.Subtitle)
                    .Font(Font.Subtitle)
                    .Color(Color.Color);
            });
        });
    });
}

```

Basicaly makes a view that has an input at the top of the view followed by a list container. Each row(horizontal stacked by default) contains a image and then a vertical stack displaying two text components with modifiers to change default appearance of the components.