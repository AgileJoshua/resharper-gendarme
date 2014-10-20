@echo off

set apiKey=%1
if "%apiKey%" == "" (
	echo 'apiKey is not set'
	pause
	exit 
)

REM set config=Release
set config=Debug
set nuget=..\tools\NuGet\NuGet.exe

echo '[1/4] Restore nuget packages...'
%nuget% restore RGendarme\RGendarme.sln
echo '[1/4] Restore nuget packages...done.'

echo '[2/4] Build RGendarme project...'
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild RGendarme\RGendarme.sln /t:Rebuild /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false
echo '[2/4] Build RGendarme project...done.'

echo '[3/4] Pack nuget package...'
%nuget% pack RGendarme.nuspec
echo '[3/4] Pack nuget package...done.'

echo '[4/4] Publish nuget package...'
%nuget% push *.nupkg %apiKey% -Source https://resharper-plugins.jetbrains.com
echo '[4/4] Publish nuget package...done.'

pause