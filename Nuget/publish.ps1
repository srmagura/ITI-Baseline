Param(
	[Parameter(Mandatory=$true)] [string] $version
)

$nuget = "./nuget.exe"
$packageName = "Iti.Baseline"

try {
	Push-Location $PSScriptRoot 

	Invoke-Expression "$nuget pack $packageName.nuspec"
	Invoke-Expression "$nuget push $packageName.$version.nupkg -Source ITI"
	Invoke-Expression "$nuget push $packageName.$version.nupkg -Source azure-devops -ApiKey 'a50c1246-3345-4530-83b7-d450ab05ee48' -ConfigFile ../Source/nuget.config"
	""
	"Successfully published $packageName $version."
} finally {
	Pop-Location
}