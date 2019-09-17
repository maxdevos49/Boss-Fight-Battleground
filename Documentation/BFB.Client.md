# BFB Client

In depth documentation on how the game client will work or does work and api endpoints.

Table of Contents:

<!-- @import "[TOC]" {cmd="toc" depthFrom=1 depthTo=6 orderedList=false} -->

<!-- code_chunk_output -->

- [BFB Client](#bfb-client)
  - [Adding a new Scene](#adding-a-new-scene)
  - [Adding Content](#adding-content)

<!-- /code_chunk_output -->

## Adding a new Scene
1. Create a new class in the BFB.Client Scenes folder that extends the type `Scene`.
2. Extending the Scene class should look roughly like this:
   
```csharp
public class ExampleScene : Scene
{
    public ExampleScene() : base(nameof(ExampleScene)) {}

    public override void Init()
    {
        //Init stuff here
    }

     public override void Load()
    {
        //Load stuff here
    }

    public override void Unload()
    {
        //Unload stuff here
    }

    public override void Update(GameTime gameTime)
    {
        //Update here
    }

    public override void Draw(GameTime gameTime, SpriteBatch graphics)
    {
        //Draw here
    }

}
```

1. The class name "ExampleScene" can be replaced with what ever the scene is to be named.

2. All method except the constructor are optional and do not not need to be implemented but would probably be silly to not have atleast a Draw and/or Update method. 

3. To register the scenes with the game go to the `Main.cs` file and find the `Initilize` Method.

4. Add the scene to the array of scenes inside the method call to `SceneManager.AddScenes()`. It will look like this: 

```csharp

SceneManager.AddScene(new Scene[] {
    new DebugScene(),//This scene should already exist
    new ExampleScene()
});

```

## Adding Content

1. First add the content to the correct folder in the BFB.Pipline project. 
2. Open the Monogame Pipeline Tool and open the Content.mgcb file from the BFB.Pipeline project.
3. Build the content from the Pipeline Tool.
4. Right click the newly built content in the Pipeline tool and select show output.
5. Copy the .xnb file to the correct folder in the BFB.Client project
6. You are now finished.