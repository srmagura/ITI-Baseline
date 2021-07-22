param([string] $version)

if(!$version) {
	"Version parameter is required. Exiting..."
	exit 1
}

try {
	Push-Location $PSScriptRoot

	dotnet pack -c Release

	dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/Baseline/Audit/bin/Release/ITI.Baseline.Audit.$version.nupkg
	dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/Baseline/Passwords/bin/Release/ITI.Baseline.Passwords.$version.nupkg
	dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/Baseline/RequestTrace/bin/Release/ITI.Baseline.RequestTrace.$version.nupkg
	dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/Baseline/Util/bin/Release/ITI.Baseline.Util.$version.nupkg
	dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/Baseline/ValueObjects/bin/Release/ITI.Baseline.ValueObjects.$version.nupkg

	dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/DDD/Application/bin/Release/ITI.DDD.Application.$version.nupkg
	dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/DDD/Auth/bin/Release/ITI.DDD.Auth.$version.nupkg
	dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/DDD/Core/bin/Release/ITI.DDD.Core.$version.nupkg
	dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/DDD/Domain/bin/Release/ITI.DDD.Domain.$version.nupkg
	dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/DDD/Infrastructure/bin/Release/ITI.DDD.Infrastructure.$version.nupkg
	dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/DDD/Logging/bin/Release/ITI.DDD.Logging.$version.nupkg
} finally {
	Pop-Location
}