# $ErrorActionPreference = "Stop"

# Save the current location.
$CurrentDir = $(Get-Location).Path;
Write-Host "CurrentDir: " $CurrentDir

# Get location of powershell file
Write-Host "PSScriptRoot: " $PSScriptRoot

# we know this script is located in the .scripts\ folder of the root.
$RootDir = [IO.Path]::GetFullPath( (join-path $PSScriptRoot "..\") )
Write-Host "ROOT: " $RootDir

# $outputOpenCoverXmlFile = 'C:\projects\coverage-dotnet.xml'
$outputOpenCoverXmlFile = (join-path $RootDir "coverage-dotnet.xml")

# Should be release of debug (set by AppVeyor)
$build_version_orig = $env:APPVEYOR_BUILD_VERSION
$build_version_new = $build_version_orig
$env:APPVEYOR_BUILD_VERSION = "test"
#$build_version_new.Replace("+",".")

Write-Host "Orig build version: " $build_version_orig
Write-Host "New build version: " $env:APPVEYOR_BUILD_VERSION

codecov -f $outputOpenCoverXmlFile

Write-Host "Restore"
$env:APPVEYOR_BUILD_VERSION = $build_version_orig