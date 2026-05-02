$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
Set-Location $repoRoot

dotnet build .\SmartMovieCatalog.slnx

Push-Location .\frontend
try {
    npm test -- --watch=false
}
finally {
    Pop-Location
}
