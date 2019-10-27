# BFB Client

In depth documentation on how the game engine works

Table of contents:


<!-- @import "[TOC]" {cmd="toc" depthFrom=1 depthTo=6 orderedList=false} -->

<!-- code_chunk_output -->

- [BFB Client](#bfb-client)
  - [Scene](#scene)
      - [About:](#about)
    - [Example](#example)
    - [Scene Manager](#scene-manager)

<!-- /code_chunk_output -->

## Scene

#### About:
Namespace: `using BFB.Engine.Scene`
Classes: 
  - Scene(Abstract)
  - SceneManager

Enums:
  - SceneStatus 
  

### Example
```csharp

class ExampleScene : Scene
{
    public ExampleScene() : base(nameof(ExampleScene)){}

    public override void Init()
    {
    }

     public override void Load()
    {
    }

     public override void Unload()
    {
    }

     public override void update()
    {
    }

     public override void Draw()
    {
    }
}

```

### Scene Manager

 Methods:
 - AddScene()
 - AddScenes()
 - StartScene()
 - LaunchScene()
 - StopScene()
 ...
 TODO