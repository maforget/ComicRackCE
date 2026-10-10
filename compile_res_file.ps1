<#
.SYNOPSIS
    Compiles a .rc file into a .res file using rc.exe, without hardcoding a
    Visual Studio version or edition.

.DESCRIPTION
    rc.exe isn't on PATH unless you're in a "Developer Command Prompt for VS".
    Instead of hardcoding a path like ".../2022/Enterprise/...", this script:

      1. Checks if rc.exe is already on PATH (fast path - e.g. when the
         GitHub Action 'ilammy/msvc-dev-cmd' already set up the environment).
      2. Otherwise, uses vswhere.exe (a stable path present since VS 2017,
         regardless of VS version/edition) to find ANY installed VS with the
         VC++ tools workload, then runs its VsDevCmd.bat to pick up rc.exe.

    This means the script keeps working when VS 2026 (or whatever comes
    next) is installed, with no edits required.

.PARAMETER RcFile
    Path to the .rc file to compile. Defaults to the ComicRack resources file.

.PARAMETER Arch
    Architecture to pass to VsDevCmd.bat. Defaults to x64.

.EXAMPLE
    .\Compile-Rc.ps1
    .\Compile-Rc.ps1 -RcFile ".\ComicRack\myressources.rc"
#>

param(
    [string]$RcFile = ".\ComicRack\myressources.rc",
    [string]$Arch = "x64"
)

$ErrorActionPreference = "Stop"

# Resolve RcFile relative to THIS script's folder, not the caller's current
# working directory. This matters because MSBuild's <Exec> task runs with
# the project directory as its working directory (not the solution dir, and
# not this script's dir), so a relative path here can silently point
# somewhere that doesn't exist when invoked from a PreBuild target.
if (-not [System.IO.Path]::IsPathRooted($RcFile)) {
    $RcFile = Join-Path $PSScriptRoot $RcFile
}
$RcFile = [System.IO.Path]::GetFullPath($RcFile)

Write-Host "Script root : $PSScriptRoot"
Write-Host "Working dir : $(Get-Location)"
Write-Host "Resolved rc : $RcFile"

if (-not (Test-Path $RcFile)) {
    throw "RC file not found: '$RcFile'. Check the -RcFile path (and that it's correct relative to the script's own folder, since that's what it's resolved against now)."
}

function Find-RcExe {

    # 1. Already on PATH? (e.g. ilammy/msvc-dev-cmd already ran in the workflow,
    #    or you're already in a Developer Command Prompt locally)
    $existing = Get-Command rc.exe -ErrorAction SilentlyContinue
    if ($existing) {
        return $existing.Source
    }

    # 2. Locate vswhere.exe - stable path since VS 2017, independent of
    #    year/edition, installed automatically alongside any VS install
    #    (and present by default on GitHub-hosted windows runners).
    $vswhere = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio\Installer\vswhere.exe"
    if (-not (Test-Path $vswhere)) {
        throw "vswhere.exe not found at '$vswhere'. Is Visual Studio installed?"
    }

    # Ask vswhere for the latest VS install that has the VC++ tools workload,
    # regardless of version (2019/2022/2026/...) or edition (Community/Pro/Enterprise/BuildTools).
    $vsPath = & $vswhere -latest -products * `
        -requires Microsoft.VisualStudio.Component.VC.Tools.x86.x64 `
        -property installationPath

    if (-not $vsPath) {
        throw "No Visual Studio installation with the VC++ tools workload was found."
    }

    $vsDevCmd = Join-Path $vsPath "Common7\Tools\VsDevCmd.bat"
    if (-not (Test-Path $vsDevCmd)) {
        throw "VsDevCmd.bat not found at '$vsDevCmd'."
    }

    # Run VsDevCmd.bat in a cmd.exe subshell (it's a batch file that mutates
    # env vars) and set the env vars for the process.
	$envDump = & cmd.exe /c "`"$vsDevCmd`" -arch=$Arch -no_logo && set"
	foreach ($line in $envDump) {
		if ($line -match '^([^=]+)=(.*)$') {
			Set-Item -Path "Env:$($Matches[1])" -Value $Matches[2]
		}
	}
	return (Get-Command rc.exe).Source
}

$rcExe = Find-RcExe
Write-Host "Using rc.exe: $rcExe"

& $rcExe /r "$RcFile"
if ($LASTEXITCODE -ne 0) {
    throw "rc.exe failed with exit code $LASTEXITCODE"
}

Write-Host "Compiled successfully: $RcFile"