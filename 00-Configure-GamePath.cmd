@echo off
setlocal
set "SCRIPT_DIR=%~dp0"

where pwsh.exe >nul 2>nul
if %errorlevel%==0 (
    pwsh.exe -NoProfile -File "%SCRIPT_DIR%00-Configure-GamePath.ps1"
) else (
    powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%SCRIPT_DIR%00-Configure-GamePath.ps1"
)

if errorlevel 1 (
    echo.
    echo Setup failed.
    pause
)