@echo off

REM Check if running in GitHub Actions environment (Enterprise edition)
if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\Common7\Tools\VsDevCmd.bat" (
    call "%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\Common7\Tools\VsDevCmd.bat"
) else (
    REM Fall back to Community edition if running locally
    if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Community\Common7\Tools\VsDevCmd.bat" (
        call "%ProgramFiles%\Microsoft Visual Studio\2022\Community\Common7\Tools\VsDevCmd.bat"
    ) else (
        echo Visual Studio 2022 not found.
        exit /b 1
    )
)

REM Call rc.exe
rc.exe /r ".\ComicRack\myressources.rc"
