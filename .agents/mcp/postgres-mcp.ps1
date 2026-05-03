$ErrorActionPreference = "Stop"

function Get-PostgresUrlFromDocker {
    $docker = Get-Command docker -ErrorAction SilentlyContinue
    if ($null -eq $docker) {
        return $null
    }

    $envLines = & docker inspect smartmoviecatalog-postgres-1 --format '{{range .Config.Env}}{{println .}}{{end}}' 2>$null
    if ($LASTEXITCODE -ne 0 -or $null -eq $envLines) {
        return $null
    }

    $containerEnv = @{}
    foreach ($line in $envLines) {
        $parts = $line.Split("=", 2)
        if ($parts.Length -eq 2) {
            $containerEnv[$parts[0]] = $parts[1]
        }
    }

    $user = $containerEnv["POSTGRES_USER"]
    $password = $containerEnv["POSTGRES_PASSWORD"]
    $database = $containerEnv["POSTGRES_DB"]

    if ([string]::IsNullOrWhiteSpace($user) -or
        [string]::IsNullOrWhiteSpace($password) -or
        [string]::IsNullOrWhiteSpace($database)) {
        return $null
    }

    $escapedUser = [uri]::EscapeDataString($user)
    $escapedPassword = [uri]::EscapeDataString($password)
    $escapedDatabase = [uri]::EscapeDataString($database)

    return "postgresql://${escapedUser}:${escapedPassword}@localhost:5432/${escapedDatabase}"
}

$connectionString = $env:SMARTMOVIECATALOG_POSTGRES_URL
if ([string]::IsNullOrWhiteSpace($connectionString)) {
    $connectionString = Get-PostgresUrlFromDocker
}

if ([string]::IsNullOrWhiteSpace($connectionString)) {
    [Console]::Error.WriteLine("SMARTMOVIECATALOG_POSTGRES_URL is not set and smartmoviecatalog-postgres-1 could not provide PostgreSQL settings.")
    exit 1
}

& npx -y @modelcontextprotocol/server-postgres $connectionString
exit $LASTEXITCODE
