@echo off
REM Run PLZASaveDumper

cd /d "%~dp0"

if not exist "bin\Release\net9.0-windows\PLZASaveDumper.exe" (
    echo ERROR: Application not built yet
    echo.
    echo Please run Build.bat first
    echo.
    pause
    exit /b 1
)

start "" "bin\Release\net9.0-windows\PLZASaveDumper.exe"
