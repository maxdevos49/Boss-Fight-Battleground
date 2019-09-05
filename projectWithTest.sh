#!/bin/bash

#Creates a C# project with unit test integrated
# -p | --project : The project type
# -n | --name    : The project name
# Example: projectWithTest.sh -p mvc -n MyMVCProject

#Defaults
projectName="newProjectWithTest";
project="console";

#Get the arguments
while [ "$1" != "" ]; do
    case $1 in
         -n | --name )
            shift;
            projectName=$1;
        ;;
        -p | --project )
            shift;
            project=$1;
        ;;
    esac
    shift;
done

echo;
echo "Creating a new $project project named $projectName";

#make root directory
mkdir $projectName;
cd $projectName;

#make the solution
dotnet new sln;

#create the source project
dotnet new $project -o "$projectName";

#add the project to the solution
echo "Adding ./$projectName/$projectName.csproj to the solution";
dotnet sln add ./$projectName/$projectName.csproj;

#create the test project
echo "Creating test named $projectName.Test";
dotnet new xunit -o "$projectName.Test";

#Add the source project reference to the test project
cd "$projectName.Test"
dotnet add reference "../$projectName/$projectName.csproj";
cd ../

#add the test project to the solution
dotnet sln add "./$projectName.Test/$projectName.Test.csproj";

cd ..

echo;
echo "Project creation has completed. Run test with the command 'dotnet test' and run the project with 'dotnet run'";
echo;

