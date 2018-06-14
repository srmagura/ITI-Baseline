
cls

nuget pack Iti.Baseline.nuspec

@echo off
for /f %%i in ('dir /b/a-d/od/t:c *.nupkg') do set LAST=%%i
@echo on

nuget add %LAST% -source \\Serv-vm-00\iti\Nuget

nuget list -source \\Serv-vm-00\iti\Nuget

pause

