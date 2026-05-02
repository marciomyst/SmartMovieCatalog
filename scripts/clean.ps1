$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot

$paths = @(
    "backend/src/SmartMovieCatalog.Api/bin",
    "backend/src/SmartMovieCatalog.Api/obj",
    "backend/src/SmartMovieCatalog.Application/bin",
    "backend/src/SmartMovieCatalog.Application/obj",
    "backend/src/SmartMovieCatalog.Contracts/bin",
    "backend/src/SmartMovieCatalog.Contracts/obj",
    "backend/src/SmartMovieCatalog.Domain/bin",
    "backend/src/SmartMovieCatalog.Domain/obj",
    "backend/src/SmartMovieCatalog.Infrastructure/bin",
    "backend/src/SmartMovieCatalog.Infrastructure/obj",
    "backend/tests/SmartMovieCatalog.Api.Tests/bin",
    "backend/tests/SmartMovieCatalog.Api.Tests/obj",
    "backend/tests/SmartMovieCatalog.Application.Tests/bin",
    "backend/tests/SmartMovieCatalog.Application.Tests/obj",
    "backend/tests/SmartMovieCatalog.Domain.Tests/bin",
    "backend/tests/SmartMovieCatalog.Domain.Tests/obj",
    "backend/tests/SmartMovieCatalog.Infrastructure.Tests/bin",
    "backend/tests/SmartMovieCatalog.Infrastructure.Tests/obj",
    "frontend/dist",
    "frontend/.angular/cache",
    "frontend/obj"
)

foreach ($relativePath in $paths) {
    $fullPath = Join-Path $repoRoot $relativePath
    $resolvedRoot = [System.IO.Path]::GetFullPath($repoRoot)
    $resolvedPath = [System.IO.Path]::GetFullPath($fullPath)

    if ($resolvedPath.StartsWith($resolvedRoot, [System.StringComparison]::OrdinalIgnoreCase) -and (Test-Path -LiteralPath $resolvedPath)) {
        Remove-Item -LiteralPath $resolvedPath -Recurse -Force
    }
}
