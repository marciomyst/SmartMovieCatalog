$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
Set-Location $repoRoot

dotnet run --project .\backend\src\SmartMovieCatalog.Api\SmartMovieCatalog.Api.csproj
