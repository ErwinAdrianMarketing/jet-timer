# Building Jet Timer (C# Version)

## Prerequisites
- Visual Studio 2022 or later
- .NET 6.0 SDK or later

## Build Instructions

### Option 1: Using Visual Studio
1. Open `JetTimer.csproj` in Visual Studio
2. Press F5 to run or Ctrl+Shift+B to build
3. The executable will be in `bin\Debug\net6.0-windows\`

### Option 2: Using Command Line
```bash
# Build debug version
dotnet build

# Build release version
dotnet build -c Release

# Publish as single executable (RECOMMENDED - creates small ~60MB exe)
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

The final executable will be in:
`bin\Release\net6.0-windows\win-x64\publish\JetTimer.exe`

## Features
- **F1** - Start Jet #1 timer (87 seconds)
- **F2** - Start Jet #2 timer (87 seconds)
- Draggable window (click and drag anywhere on the overlay)
- Always on top
- Color-coded timers:
  - Green: > 30 seconds
  - Orange: 10-30 seconds
  - Red: < 10 seconds
  - Gray: Inactive

## Size Comparison
- **Electron version**: ~492 MB
- **C# version**: ~60 MB (single file) or ~15 MB (with runtime installed separately)
