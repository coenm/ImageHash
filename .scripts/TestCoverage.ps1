# $ErrorActionPreference = "Stop"

# Save the current location.
$CurrentDir = $(Get-Location).Path;
Write-Host "CurrentDir: " $CurrentDir

# Get location of powershell file
Write-Host "PSScriptRoot: " $PSScriptRoot

# we know this script is located in the .scripts\ folder of the root.
$RootDir = [IO.Path]::GetFullPath( (join-path $PSScriptRoot "..\") )
Write-Host "ROOT: " $RootDir

# Expected OpenCover location appveyor.
$opencoverExe = 'C:\ProgramData\chocolatey\bin\OpenCover.Console.exe'
# Search for opencover in the chocolatery directory.
Get-ChildItem -Recurse ('C:\ProgramData\chocolatey\bin') | Where-Object {$_.Name -like "OpenCover.Console.exe"} | % { $opencoverExe = $_.FullName};

$dotnetExe = 'dotnet.exe'
$outputOpenCoverXmlFile = (join-path $RootDir "coverage-dotnet.xml")

# Should be release of debug (set by AppVeyor)
$configuration = $env:CONFIGURATION

Write-Host "(Environment) Configuration:" $configuration 
Write-Host "Location opencover.exe: " $opencoverExe
Write-Host "Location dotnet.exe: " $dotnetExe
Write-Host "Location xml coverage result: " $outputOpenCoverXmlFile

$dotnetTestArgs = '-c ' + $configuration + ' --no-build --filter Category!=StressTest --logger:trx' # ;LogFileName=' + $outputTrxFile
$opencoverFilter = "+[CoenM*]* -[*.Test]*"

pushd
cd ..
$testProjectLocations = Get-ChildItem -Recurse | Where-Object{$_.Name -like "*Test.csproj" } | % { $_.FullName }; # access $_.DirectoryName for the directory.
popd

Try
{
	ForEach ($testProjectLocation in $testProjectLocations)
	{
		Write-Host "Run tests for project " (Resolve-Path $testProjectLocation).Path;

		$command = "${opencoverExe} "`
            + "-threshold:1 "`
            + "-register:user "`
            + "-oldStyle "`
            + "-mergebyhash "`
            + "-mergeoutput "`
            + "-returntargetcode "`
            + "-hideskipped:All "`
            + "-excludebyfile:*\*Designer.cs "`
            + "-target:""${dotnetExe}"" "`
            + "-targetargs:""test ${testProjectLocation} ${dotnetTestArgs}"" "`
            + "-output:${outputOpenCoverXmlFile} "`
            + "-excludebyattribute:System.Diagnostics.DebuggerNonUserCodeAttribute "`
            + "-filter:""${opencoverFilter}"""
		
		Write-Output $command

		iex $command
		
		Write-Host "Command finished, ready for the next one"
	}
}
Finally
{
	Write-Output "Done testing.."
	popd
}