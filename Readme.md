# Publishing
Each project is its own NuGet package.

0. Install https://github.com/microsoft/artifacts-credprovider

1. For each project, update version number in the project properties.
2. In PowerShell run
```
./Update-NuGetPackages.ps1 x.x.x
```
where `x.x.x` is the version number.