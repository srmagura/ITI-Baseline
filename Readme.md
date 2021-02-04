# Publishing
Each project is its own NuGet package.

0. Install https://github.com/microsoft/artifacts-credprovider

1. Update version number in the project properties.
2. In PowerShell, do:
```
cd <the-project>
dotnet pack -c Release
dotnet nuget push --source "iti-azure-devops" --api-key az --interactive <path-to-nupkg>
```