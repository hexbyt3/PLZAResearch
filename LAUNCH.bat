@echo off
REM Launch PLZA Save Dumper

cd /d "%~dp0"

if not exist "bin\Release\net9.0-windows\PLZASaveDumper.exe" (
    echo Building application for first time...
    echo This will take about 5 seconds...
    echo.
    dotnet build -c Release
    if errorlevel 1 (
        echo.
        echo Build failed! Make sure .NET 9.0 SDK is installed.
        pause
        exit /b 1
    )
    echo.
    echo Build complete!
    echo.
)

start "" "bin\Release\net9.0-windows\PLZASaveDumper.exe"
