# Define the path to Inno Setup 6
$innoSetupPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"

# Check if Inno Setup is installed
if (!(Test-Path $innoSetupPath)) {
    Write-Error "Inno Setup 6 is not installed in the expected location: $innoSetupPath"
    exit 1
}

# Define the path to the CurrentCommit.txt file
$currentCommitFile = ".\ComicRack\CurrentCommit.txt"

# Check if the CurrentCommit.txt file exists
if (!(Test-Path $currentCommitFile)) {
    Write-Error "The file $currentCommitFile does not exist."
    exit 1
}

# Read the content of the CurrentCommit.txt file and extract the first 7 characters
$commitHash = Get-Content -Path $currentCommitFile -ErrorAction Stop | Select-Object -First 1
if ($commitHash.Length -lt 7) {
    Write-Error "The commit hash in $currentCommitFile is too short."
    exit 1
}
$shortCommitHash = $commitHash.Substring(0, 7)

# Define the path to the Installer.iss file
$installerFile = ".\Installer.iss"

# Check if the Installer.iss file exists
if (!(Test-Path $installerFile)) {
    Write-Error "The file $installerFile does not exist."
    exit 1
}

# Define the custom parameters for Inno Setup
$appVersionParam = "/DMyAppVersion=nightly-$shortCommitHash"
$setupFileParam = "/DMyAppSetupFile=ComicRackCESetup_nightly"

# Output the parameters being used
Write-Output "Building installer with the following parameters:"
Write-Output "Inno Setup Path: $innoSetupPath"
Write-Output "Installer File: $installerFile"
Write-Output "App Version Parameter: $appVersionParam"
Write-Output "Setup File Parameter: $setupFileParam"

# Run Inno Setup to build the installer
try {
    Write-Output "Executing: & $innoSetupPath $installerFile $appVersionParam $setupFileParam"
    & $innoSetupPath $installerFile $appVersionParam $setupFileParam
    Write-Output "Installer built successfully."
} catch {
    Write-Error "Failed to build the installer. Error: $_"
    exit 1
}

