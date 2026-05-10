param(
    [ValidateSet("domain", "application", "both")]
    [string] $Scope = "both"
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
Set-Location $repoRoot

dotnet tool restore

function Invoke-StrykerRun([string]$SourceProjectPath, [string]$TestProjectPath, [string]$ConfigFilePath)
{
    Push-Location $SourceProjectPath
    try {
        dotnet tool run dotnet-stryker -- --config-file $ConfigFilePath --test-project $TestProjectPath
    }
    finally {
        Pop-Location
    }
}

$domainSourceProject = Join-Path $repoRoot 'backend\src\SmartMovieCatalog.Domain'
$domainTestProject = Join-Path $repoRoot 'backend\tests\SmartMovieCatalog.Domain.Tests\SmartMovieCatalog.Domain.Tests.csproj'
$domainConfigFile = Join-Path $repoRoot 'backend\tests\SmartMovieCatalog.Domain.Tests\stryker-config.json'
$applicationSourceProject = Join-Path $repoRoot 'backend\src\SmartMovieCatalog.Application'
$applicationTestProject = Join-Path $repoRoot 'backend\tests\SmartMovieCatalog.Application.Tests\SmartMovieCatalog.Application.Tests.csproj'
$applicationConfigFile = Join-Path $repoRoot 'backend\tests\SmartMovieCatalog.Application.Tests\stryker-config.json'

switch ($Scope) {
    "domain" {
        Invoke-StrykerRun $domainSourceProject $domainTestProject $domainConfigFile
    }
    "application" {
        Invoke-StrykerRun $applicationSourceProject $applicationTestProject $applicationConfigFile
    }
    default {
        Invoke-StrykerRun $domainSourceProject $domainTestProject $domainConfigFile
        Invoke-StrykerRun $applicationSourceProject $applicationTestProject $applicationConfigFile
    }
}
