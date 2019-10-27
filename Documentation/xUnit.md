# xUnit Testing

How to use xUnit and tips and tricks you find along the way

Table of Contents:

<!-- code_chunk_output -->

- [xUnit Testing](#xunit-testing)
  - [Setup](#setup)
  - [Assert Methods](#assert-methods)
  - [Setting up a Test Class](#setting-up-a-test-class)

<!-- /code_chunk_output -->


## Setup

1. Create a new project or skip this step if the project already exist.
2. Right click the solution in the project explorer.
3. Select the option `add->add new project`.
4. Select a xUnit Test Project under the `.NET Core -> Test` Section.
5. Select the .Net Core SDK to Use(Probably .NET Core 2.2).
6. Give the test project a name and then Select finish.
7. Unfold the newly created project and right click the dependicies folder.
8. Select edit references.
9. Select the project/projects you want to test and then click ok.
10. You should now be able to write test.    

## Assert Methods

```csharp

Assert.Equal();
Assert.True();
Assert.False();
Assert.NotNull();
Assert.NotEmpty();

```

## Setting up a Test Class

```csharp

public class TestExample
{

   [Fact]//This makes it a test
   public void TestTruth()
   {
      Assert.True(true);//Assert is the testing
   }

}

```
