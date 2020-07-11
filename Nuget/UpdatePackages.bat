
@echo off

SET VERSION=%1

IF NOT "%VERSION%" == "" GOTO CONTINUE

echo.
echo USAGE: %0 {version}

GOTO :END


REM -------------------------------------------------------------------

:CONTINUE

@echo ==============================================
@echo VERSION = %VERSION%
@echo ==============================================

erase /Q *.nupkg

dotnet pack ..\Source\Iti.Baseline.sln -c Release -o . /p:Version=%VERSION%

REM --- dotnet nuget push *.nupkg -s \\Serv-vm-00\iti\Nuget

echo.
echo.
DIR *.nupkg
echo.
echo.

:END

@echo.
@echo ==============================================

@pause
