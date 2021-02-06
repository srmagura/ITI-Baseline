
dotnet pack -c Release

SET VER=0.1.6

dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/Baseline/Audit/bin/Release/ITI.Baseline.Audit.%VER%.nupkg
dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/Baseline/Passwords/bin/Release/ITI.Baseline.Passwords.%VER%.nupkg
dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/Baseline/RequestTrace/bin/Release/ITI.Baseline.RequestTrace.%VER%.nupkg
dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/Baseline/Util/bin/Release/ITI.Baseline.Util.%VER%.nupkg
dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/Baseline/ValueObjects/bin/Release/ITI.Baseline.ValueObjects.%VER%.nupkg

dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/DDD/Application/bin/Release/ITI.DDD.Application.%VER%.nupkg
dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/DDD/Auth/bin/Release/ITI.DDD.Auth.%VER%.nupkg
dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/DDD/Core/bin/Release/ITI.DDD.Core.%VER%.nupkg
dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/DDD/Domain/bin/Release/ITI.DDD.Domain.%VER%.nupkg
dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/DDD/Infrastructure/bin/Release/ITI.DDD.Infrastructure.%VER%.nupkg
dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI/DDD/Logging/bin/Release/ITI.DDD.Logging.%VER%.nupkg
