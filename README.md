# ITI.Baseline

## Publishing

Each project is its own NuGet package.

0. Install https://github.com/microsoft/artifacts-credprovider  
1. Update the version number in every `.csproj` file using Replace-All on (for example) `<Version>2.1.0</Version>`.  
2. In PowerShell run

   ```pwsh
   ./Publish-NuGetPackages.ps1 x.x.x
   ```

   where `x.x.x` is the version number.
