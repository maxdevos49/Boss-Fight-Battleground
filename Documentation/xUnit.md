# xUnit Testing

How to use xUnit and tips and tricks you find along the way

# Table of Contents

- [Setup](#setup)

 
  ### Setup

  To create a xUnit test directory to coincide with your monogame directory, go through the following steps:

   1. Configure your Monogame project as you would normally. For this example, lets say we have the following directory:
    ```
     /myMonogame
     	/Game
     	myMonogame.sln
    ```
   2. In the directory with your .sln file, create a new directory labelled "/DirectoryName.Tests"
   ```
    /myMonogame
    	/Game
    	/Game.Tests
    	myMonogame.sln
    ```
    3. Go into the new /DirectoryName.Tests directory and run the command `dotnet new xunit` to add the correct dependencies for xUnit.
    4. Next, add your initial game directory as a dependency for the project by running `dotnet add reference ../DirectoryName/DirectoryName.csproj`
    5. Finally, go up one directory and add the new .Tests directory to the solution file with `dotnet sln add ./DirectoryName.Tests/DirectoryName.Tests.csproj`

 ---