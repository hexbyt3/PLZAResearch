@echo off
REM Build PLZASaveDumper WinForms Application

title Building PLZA Save Dumper

echo ========================================
echo Building PLZA Save Dumper
echo ========================================
echo.

cd /d "%~dp0"

echo Building Release version...
dotnet build -c Release

if errorlevel 1 (
    echo.
    echo ========================================
    echo Build FAILED
    echo ========================================
    pause
    exit /b 1
)

echo.
echo ========================================
echo Build completed successfully!
echo ========================================
echo.
echo Output location:
echo   bin\Release\net9.0-windows\PLZASaveDumper.exe
echo.
echo You can run the application now!
echo.
pause
