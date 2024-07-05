# CompressIconFolders.ps1

param (
    [string]$BaseDirectory
)

# Ensure the BaseDirectory parameter is provided
if (-not $BaseDirectory) {
    Write-Error "BaseDirectory parameter is required."
    exit 1
}

# Clean up the BaseDirectory path
$BaseDirectory = $BaseDirectory.Trim('"').TrimEnd('\')

# Print the BaseDirectory for debugging
Write-Output "BaseDirectory: $BaseDirectory"

# Get all subdirectories in the specified base directory
$iconDirectory = Join-Path -Path $BaseDirectory -ChildPath "Resources\Icons"

# Print the iconDirectory for debugging
Write-Output "iconDirectory: $iconDirectory"

if (-not (Test-Path $iconDirectory)) {
    Write-Error "The directory $iconDirectory does not exist."
    exit 1
}

# Use Git to check for changes in the Resources/Icons directory
$gitChangedFiles = git status --porcelain | Select-String -Pattern "^.+/Resources/Icons/"

$folders = Get-ChildItem -Path $iconDirectory -Directory

# Iterate through each folder
foreach ($folder in $folders) {
    # Define the full path to the folder and the destination zip file
    $folderPath = Join-Path -Path $folder.FullName -ChildPath "*.*"
    $destinationPath = "$($folder.FullName).zip"

    # Check if the zip file exists
    $zipExists = Test-Path $destinationPath

    # Determine if we need to update the zip file
    $updateRequired = $false
    if ($zipExists) {
        if ($gitChangedFiles.Count -gt 0) {
            foreach ($changedFile in $gitChangedFiles) {
                if ($changedFile -like "*$($folder.Name)/*") {
                    $updateRequired = $true
                    Write-Output "$destinationPath exists, but there are changes, updating it."
                    break
                }
            }
        }
    } else {
        $updateRequired = $true
        Write-Output "$destinationPath doesn't exists, need to create it."
    }
    
    if ($updateRequired) {
        # Compress the folder into a zip file
        Compress-Archive -Path $folderPath -DestinationPath $destinationPath -Force
        } else {
        Write-Output "$destinationPath is up to date. No action required."
    }

  
    # Check if the zip file was created successfully
    if (Test-Path $destinationPath) {
        # Delete the original folder if the zip file exists
        Remove-Item -Path $folder.FullName -Recurse -Force
    } else {
        Write-Error "Failed to create $destinationPath"
    } 
}
