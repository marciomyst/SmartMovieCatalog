[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string] $IssueUrl,

    [Parameter(Mandatory = $false)]
    [ValidateNotNullOrEmpty()]
    [string] $OutputDirectory = ".specify/inputs/github-issues",

    [Parameter(Mandatory = $false)]
    [switch] $Force
)

$ErrorActionPreference = "Stop"

function Test-CommandExists {
    param(
        [Parameter(Mandatory = $true)]
        [string] $CommandName
    )

    return $null -ne (Get-Command $CommandName -ErrorAction SilentlyContinue)
}

function Invoke-NativeCommand {
    param(
        [Parameter(Mandatory = $true)]
        [string] $Command,

        [Parameter(Mandatory = $false)]
        [string[]] $Arguments = @()
    )

    $output = & $Command @Arguments 2>&1
    $exitCode = $LASTEXITCODE

    if ($exitCode -ne 0) {
        throw "Command failed: $Command $($Arguments -join ' ')`n$output"
    }

    return $output
}

function Get-GitRepositoryRoot {
    if (-not (Test-CommandExists -CommandName "git")) {
        throw "Git is required. Install Git and run this script from a Git repository."
    }

    $root = Invoke-NativeCommand -Command "git" -Arguments @("rev-parse", "--show-toplevel")
    return ($root | Select-Object -First 1).Trim()
}

function Assert-RunningFromRepositoryRoot {
    $repoRoot = Get-GitRepositoryRoot
    $currentPath = (Resolve-Path ".").Path
    $resolvedRepoRoot = (Resolve-Path $repoRoot).Path

    if ($currentPath -ne $resolvedRepoRoot) {
        throw "Run this script from the repository root. Current path: $currentPath. Repository root: $resolvedRepoRoot."
    }

    return $resolvedRepoRoot
}

function Assert-SpecKitInitialized {
    if (-not (Test-Path ".specify")) {
        throw "Spec Kit is not initialized in this repository. Run: specify init --here --integration codex --script ps"
    }
}

function Assert-GitHubCliReady {
    if (-not (Test-CommandExists -CommandName "gh")) {
        throw "GitHub CLI 'gh' was not found. Install it and run 'gh auth login'."
    }

    try {
        Invoke-NativeCommand -Command "gh" -Arguments @("auth", "status") | Out-Null
    }
    catch {
        throw "GitHub CLI is not authenticated. Run 'gh auth login' and try again."
    }
}

function ConvertTo-MarkdownList {
    param(
        [Parameter(Mandatory = $false)]
        [object[]] $Items,

        [Parameter(Mandatory = $true)]
        [scriptblock] $Selector,

        [Parameter(Mandatory = $false)]
        [string] $EmptyText = "_None_"
    )

    if ($null -eq $Items -or $Items.Count -eq 0) {
        return $EmptyText
    }

    return ($Items | ForEach-Object { "- $(& $Selector $_)" }) -join "`n"
}

function ConvertTo-SafeFileName {
    param(
        [Parameter(Mandatory = $true)]
        [string] $Value
    )

    return ($Value -replace "[^a-zA-Z0-9._-]", "-").ToLowerInvariant()
}

function Redact-SensitiveContent {
    param(
        [Parameter(Mandatory = $false)]
        [AllowNull()]
        [string] $Content
    )

    if ([string]::IsNullOrWhiteSpace($Content)) {
        return $Content
    }

    $redacted = $Content

    $patterns = @(
        @{
            Pattern = "(?i)(GEMINI_API_KEY|SONAR_TOKEN|SNYK_TOKEN|GH_TOKEN|GITHUB_TOKEN|OPENAI_API_KEY|API_KEY|TOKEN|PASSWORD|SECRET)\s*[:=]\s*['""]?[^'"",\s]+['""]?"
            Replacement = '$1=[REDACTED]'
        },
        @{
            Pattern = "(?i)(Password|Pwd)\s*=\s*[^;`\r`\n]+"
            Replacement = '$1=[REDACTED]'
        },
        @{
            Pattern = "ghp_[A-Za-z0-9_]{20,}"
            Replacement = "[REDACTED_GITHUB_TOKEN]"
        },
        @{
            Pattern = "github_pat_[A-Za-z0-9_]{20,}"
            Replacement = "[REDACTED_GITHUB_PAT]"
        },
        @{
            Pattern = "sk-[A-Za-z0-9_-]{20,}"
            Replacement = "[REDACTED_API_KEY]"
        },
        @{
            Pattern = "(?s)-----BEGIN [A-Z ]*PRIVATE KEY-----.*?-----END [A-Z ]*PRIVATE KEY-----"
            Replacement = "[REDACTED_PRIVATE_KEY]"
        }
    )

    foreach ($entry in $patterns) {
        $redacted = [regex]::Replace($redacted, $entry.Pattern, $entry.Replacement)
    }

    return $redacted
}

function Test-ContainsPotentialSecret {
    param(
        [Parameter(Mandatory = $false)]
        [AllowNull()]
        [string] $Content
    )

    if ([string]::IsNullOrWhiteSpace($Content)) {
        return $false
    }

    $secretPatterns = @(
        "(?i)GEMINI_API_KEY\s*[:=]",
        "(?i)SONAR_TOKEN\s*[:=]",
        "(?i)SNYK_TOKEN\s*[:=]",
        "(?i)GH_TOKEN\s*[:=]",
        "(?i)GITHUB_TOKEN\s*[:=]",
        "(?i)OPENAI_API_KEY\s*[:=]",
        "(?i)PASSWORD\s*[:=]",
        "(?i)SECRET\s*[:=]",
        "(?i)(Password|Pwd)\s*=",
        "ghp_[A-Za-z0-9_]{20,}",
        "github_pat_[A-Za-z0-9_]{20,}",
        "sk-[A-Za-z0-9_-]{20,}",
        "-----BEGIN [A-Z ]*PRIVATE KEY-----"
    )

    foreach ($pattern in $secretPatterns) {
        if ($Content -match $pattern) {
            return $true
        }
    }

    return $false
}

function Get-UniqueOutputPath {
    param(
        [Parameter(Mandatory = $true)]
        [string] $BasePath,

        [Parameter(Mandatory = $true)]
        [bool] $ForceOverwrite
    )

    if (-not (Test-Path $BasePath)) {
        return $BasePath
    }

    if ($ForceOverwrite) {
        return $BasePath
    }

    $directory = Split-Path $BasePath -Parent
    $fileNameWithoutExtension = [System.IO.Path]::GetFileNameWithoutExtension($BasePath)
    $extension = [System.IO.Path]::GetExtension($BasePath)
    $timestamp = Get-Date -Format "yyyyMMdd-HHmmss"

    $newPath = Join-Path $directory "$fileNameWithoutExtension-$timestamp$extension"

    Write-Warning "Output file already exists. Writing to a timestamped file instead: $newPath"

    return $newPath
}

$repositoryRoot = Assert-RunningFromRepositoryRoot
Assert-SpecKitInitialized
Assert-GitHubCliReady

$issuePattern = "^https://github\.com/(?<owner>[^/]+)/(?<repo>[^/]+)/issues/(?<number>\d+)(?:[/?#].*)?$"

if ($IssueUrl -notmatch $issuePattern) {
    throw "Invalid GitHub issue URL. Expected: https://github.com/owner/repository/issues/123"
}

$owner = $Matches["owner"]
$repo = $Matches["repo"]
$number = $Matches["number"]
$repository = "$owner/$repo"

Write-Host "Repository root: $repositoryRoot"
Write-Host "Fetching GitHub issue #$number from $repository..."

$issueJson = Invoke-NativeCommand -Command "gh" -Arguments @(
    "issue",
    "view",
    $number,
    "--repo",
    $repository,
    "--json",
    "title,body,number,url,state,labels,assignees,milestone,comments,createdAt,updatedAt"
)

$issueJsonText = $issueJson -join "`n"

if ([string]::IsNullOrWhiteSpace($issueJsonText)) {
    throw "GitHub CLI returned an empty response for issue #$number in $repository."
}

$issue = $issueJsonText | ConvertFrom-Json

New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null

$repositorySlug = ConvertTo-SafeFileName -Value "$owner-$repo"
$baseOutputPath = Join-Path $OutputDirectory "$repositorySlug-issue-$number.md"
$outputPath = Get-UniqueOutputPath -BasePath $baseOutputPath -ForceOverwrite ([bool]$Force)

$labelsMarkdown = ConvertTo-MarkdownList `
    -Items $issue.labels `
    -Selector { param($label) $label.name } `
    -EmptyText "_No labels_"

$assigneesMarkdown = ConvertTo-MarkdownList `
    -Items $issue.assignees `
    -Selector { param($assignee) $assignee.login } `
    -EmptyText "_No assignees_"

$milestoneTitle = if ($null -ne $issue.milestone -and -not [string]::IsNullOrWhiteSpace($issue.milestone.title)) {
    $issue.milestone.title
}
else {
    "_No milestone_"
}

$rawIssueBody = if (-not [string]::IsNullOrWhiteSpace($issue.body)) {
    $issue.body
}
else {
    "_No issue body provided._"
}

$securityConcernDetected = Test-ContainsPotentialSecret -Content $rawIssueBody
$issueBody = Redact-SensitiveContent -Content $rawIssueBody

$commentsMarkdown = if ($null -ne $issue.comments -and $issue.comments.Count -gt 0) {
    ($issue.comments | ForEach-Object {
        $author = if ($null -ne $_.author -and -not [string]::IsNullOrWhiteSpace($_.author.login)) {
            $_.author.login
        }
        else {
            "unknown"
        }

        $createdAt = if (-not [string]::IsNullOrWhiteSpace($_.createdAt)) {
            $_.createdAt
        }
        else {
            "unknown date"
        }

        $rawBody = if (-not [string]::IsNullOrWhiteSpace($_.body)) {
            $_.body
        }
        else {
            "_Empty comment._"
        }

        if (Test-ContainsPotentialSecret -Content $rawBody) {
            $script:securityConcernDetected = $true
        }

        $body = Redact-SensitiveContent -Content $rawBody

@"
### Comment by @$author

- Created: $createdAt

$body
"@
    }) -join "`n`n"
}
else {
    "_No comments_"
}

$securityConcernSection = if ($securityConcernDetected) {
@"
## Security Concern

Potential sensitive data was detected in the issue body or comments.

Sensitive values were replaced with `[REDACTED]`.

Do not copy raw credentials, tokens, API keys, connection strings, certificates, or private keys into specifications, plans, commits, logs, or pull requests.
"@
}
else {
    ""
}

$context = @"
# GitHub Issue Context

## Source

- Repository: $repository
- Issue: #$($issue.number)
- URL: $($issue.url)
- State: $($issue.state)
- Created: $($issue.createdAt)
- Updated: $($issue.updatedAt)
- Milestone: $milestoneTitle

## Title

$($issue.title)

## Labels

$labelsMarkdown

## Assignees

$assigneesMarkdown

$securityConcernSection

## Issue Body

$issueBody

## Comments

$commentsMarkdown

## Instructions for Spec Kit

Use this GitHub issue as the primary source of truth.

Convert the issue into a Spec Kit feature specification before creating the implementation plan.

Preserve:

- business goal;
- user stories;
- acceptance criteria;
- technical constraints;
- non-goals;
- dependencies;
- open questions.

If information is missing, add it under a clearly marked **Clarifications Needed** section instead of inventing requirements.

If the issue conflicts with existing project documentation, explicitly call out the conflict.

Prefer a small, incremental implementation plan aligned with the repository's existing architecture, folder structure, language, framework, and conventions.
"@

Set-Content -Path $outputPath -Value $context -Encoding UTF8

Write-Host "Issue context written to: $outputPath"

if ($securityConcernDetected) {
    Write-Warning "Potential sensitive data was detected and redacted."
}

return $outputPath