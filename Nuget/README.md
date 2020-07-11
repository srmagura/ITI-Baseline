To publish an update of a package:

0. MANUALLY build RELEASE version from SLN

1. Update `.nuspec` file

    - At least change the version number
    - Add new assemblies, dependencies, etc.

2. Run `publish.ps1 <version>`

---

Packages can then be installed from the source `\\SERV-VM-00\iti\NuGet`.
