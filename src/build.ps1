Param(
	[Parameter(Mandatory=$True)]  
	[string]$apiKey
)

Write-Host "Pack package..." 
.\..\tools\NuGet\NuGet.exe pack RGendarme.nuspec
Write-Host "Pack package done."

Write-Host "Push package..."
.\..\tools\NuGet\NuGet.exe push *.nupkg -ApiKey $apiKey -Source https://resharper-plugins.jetbrains.com
Write-Host "Push package...done."

Write-Host "Remove temporary nuget package..." -NoNewline
Remove-Item *.nupkg
Write-Host "done."