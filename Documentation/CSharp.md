# CSharp

How to use C# and tips and tricks you find along the way

Table of Contents:


<!-- @import "[TOC]" {cmd="toc" depthFrom=1 depthTo=6 orderedList=false} -->

<!-- code_chunk_output -->

- [CSharp](#csharp)
  - [Threads](#threads)

<!-- /code_chunk_output -->



## Threads

Visual Example:

```csharp
//Thread t = new Thread(() => methodCall(someParameter))//If you need to pass in parameters 
Thread t = new Thread(methodCall)//No parameters needed
{
    Name="Thread Name for Debuging",
    IsBackground = true//Always set this because otherwise the thread may not end with the program
};
t.Start()

```

