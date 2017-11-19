@echo off

IF DEFINED APPVEYOR (
    C:\ProgramData\chocolatey\lib\opencover.portable\tools\OpenCover.Console.exe -register:user -target:"c:\Program Files\dotnet\dotnet.exe" -targetargs:"test src\ImageHash.Test\ImageHash.Test.csproj -c Debug --no-build --logger \"trx;LogFileName=%APPVEYOR_BUILD_FOLDER%\TestResults.trx\" " -threshold:1 -oldStyle -returntargetcode -filter:"+[CoenM*]* -[*.Test]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -excludebyfile:*\*Designer.cs -hideskipped:All -mergebyhash -mergeoutput -output:.\coverage-dotnet.xml
) ELSE (
	echo No coverage without appveyor.
)