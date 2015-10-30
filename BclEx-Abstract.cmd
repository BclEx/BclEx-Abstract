@echo off
echo Building BclEx-Abstract:
PowerShell -Command ".\psake.ps1"

If Not "%NugetPackagesDir%" == "" xcopy .\Release\*.nupkg %NugetPackagesDir% /Y/Q
If Not "%NugetPackagesDir%" == "" del %NugetPackagesDir%\*.symbols.nupkg /Q
