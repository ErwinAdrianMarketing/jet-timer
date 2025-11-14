@echo off
cd /d "%~dp0"
echo Building Jet Timer...
echo Current directory: %CD%
echo.

dotnet publish JetTimer.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

echo.
echo Build complete!
echo Executable location: %CD%\bin\Release\net6.0-windows\win-x64\publish\JetTimer.exe
echo.
pause
