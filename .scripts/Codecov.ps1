# $ErrorActionPreference = "Stop"

# Save the current location.
$CurrentDir = $(Get-Location).Path;
Write-Host "CurrentDir: " $CurrentDir

# Get location of powershell file
Write-Host "PSScriptRoot: " $PSScriptRoot

# we know this script is located in the .scripts\ folder of the root.
$RootDir = [IO.Path]::GetFullPath( (join-path $PSScriptRoot "..\") )
Write-Host "ROOT: " $RootDir

$nugetToolsDir = "C:\NuGetTools"
$codecovExe = ""
Get-ChildItem -Recurse ($nugetToolsDir) | Where-Object {$_.Name -like "codecov.exe"} | % { $codecovExe = $_.FullName};

if (! ( Test-Path $codecovExe )) 
{
  Write-Warning "$codecovExe  DOES NOT EXISTS"
}

Write-Host "CodeCov.exe " $codecovExe

$outputOpenCoverXmlFile = (join-path $RootDir "coverage-dotnet.xml")
iex "$codecovExe -f $outputOpenCoverXmlFile"