# Jet Timer

A lightweight Windows overlay timer with dual independent countdowns.

Disclaimer: I have no idea if it is legal or not, I personally dont know. It has same functionality as your egg boiling timer. Use at your own risk, and dont sue me in US, I have no insurance there.

## Features

- **F1** - Start/restart Timer 1 (87 seconds)
- **F2** - Start/restart Timer 2 (87 seconds)
- Draggable transparent overlay window
- Always on top (works over fullscreen applications)
- Color-coded countdown:
  - 🟢 Green: > 30 seconds
  - 🟠 Orange: 10-30 seconds
  - 🔴 Red: < 10 seconds
- Auto-hide when timers complete

## Requirements

- Windows 10/11
- [.NET 6 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/6.0/runtime)

## Installation

1. Download `JetTimer.exe` from [Releases](../../releases)
2. Install [.NET 6 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/6.0/runtime) if not already installed
3. Run `JetTimer.exe`

## Building from Source

### Prerequisites
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

### Build Instructions

```bash
dotnet publish JetTimer.csproj -c Release -r win-x64
```

The executable will be in `bin\Release\net6.0-windows\win-x64\publish\JetTimer.exe`

## Usage

1. Run `JetTimer.exe`
2. Press **F1** to start Timer 1
3. Press **F2** to start Timer 2
4. Drag the window to reposition it
5. Both timers run independently

## Size

- Executable: **~150KB**
- Requires .NET 6 Runtime (~50MB one-time install)

## License

MIT
