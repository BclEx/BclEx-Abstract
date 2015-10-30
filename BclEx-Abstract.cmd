@echo off
echo Building BclEx-Abstract:
PowerShell -Command ".\psake.ps1"

xcopy .\Release\*.nupkg \\degsapp01.degdarwin.com\d$\_APPLICATION\NUGET.live\_Secure\Packages /Y/Q