$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
Set-Location $repoRoot

$envFile = Join-Path $repoRoot '.env'
if (Test-Path $envFile) {
    Get-Content $envFile | ForEach-Object {
        $line = $_.Trim()
        if (-not [string]::IsNullOrWhiteSpace($line) -and -not $line.StartsWith('#')) {
            $pair = $line -split '=', 2
            if ($pair.Length -eq 2) {
                $key = $pair[0].Trim()
                $value = $pair[1].Trim()
                if (-not [string]::IsNullOrWhiteSpace($key)) {
                    [System.Environment]::SetEnvironmentVariable($key, $value)
                }
            }
        }
    }
}

function Set-EnvironmentVariableIfMissing {
    param(
        [Parameter(Mandatory = $true)]
        [string] $Name,

        [AllowEmptyString()]
        [AllowNull()]
        [string] $Value
    )

    if ([string]::IsNullOrWhiteSpace([System.Environment]::GetEnvironmentVariable($Name)) -and
        -not [string]::IsNullOrWhiteSpace($Value)) {
        [System.Environment]::SetEnvironmentVariable($Name, $Value)
    }
}

Set-EnvironmentVariableIfMissing -Name 'Jwt__Issuer' -Value $env:JWT_ISSUER
Set-EnvironmentVariableIfMissing -Name 'Jwt__Audience' -Value $env:JWT_AUDIENCE
Set-EnvironmentVariableIfMissing -Name 'Jwt__SigningKey' -Value $env:JWT_SIGNING_KEY
Set-EnvironmentVariableIfMissing -Name 'Jwt__AccessTokenLifetimeMinutes' -Value $env:JWT_ACCESS_TOKEN_LIFETIME_MINUTES
Set-EnvironmentVariableIfMissing -Name 'AdminSeedUser__Email' -Value $env:ADMIN_SEED_EMAIL
Set-EnvironmentVariableIfMissing -Name 'AdminSeedUser__Password' -Value $env:ADMIN_SEED_PASSWORD
Set-EnvironmentVariableIfMissing -Name 'AdminSeedUser__Name' -Value $env:ADMIN_SEED_NAME

if ([string]::IsNullOrWhiteSpace($env:ConnectionStrings__DefaultConnection)) {
    $postgresHost = if ([string]::IsNullOrWhiteSpace($env:POSTGRES_HOST)) { 'localhost' } else { $env:POSTGRES_HOST }
    $postgresPort = if ([string]::IsNullOrWhiteSpace($env:POSTGRES_PORT)) { '5432' } else { $env:POSTGRES_PORT }
    $postgresDatabase = if ([string]::IsNullOrWhiteSpace($env:POSTGRES_DB)) { 'smart_movie_catalog' } else { $env:POSTGRES_DB }
    $postgresUser = if ([string]::IsNullOrWhiteSpace($env:POSTGRES_USER)) { 'smartmovie' } else { $env:POSTGRES_USER }

    if (-not [string]::IsNullOrWhiteSpace($env:POSTGRES_PASSWORD)) {
        [System.Environment]::SetEnvironmentVariable(
            'ConnectionStrings__DefaultConnection',
            "Host=$postgresHost;Port=$postgresPort;Database=$postgresDatabase;Username=$postgresUser;Password=$env:POSTGRES_PASSWORD")
    }
}

dotnet run --project .\backend\src\SmartMovieCatalog.Api\SmartMovieCatalog.Api.csproj
