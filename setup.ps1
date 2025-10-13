# Repository setup script for Windows PowerShell
# - Restores dotnet local tools from .config/dotnet-tools.json
# - Runs `dotnet husky install` to install Git hooks
# - Optionally verifies dotnet, git and husky availability

param(
    [switch]$SkipVerify
)

function Write-Section($msg) {
    Write-Host "`n==== $msg ==== `n" -ForegroundColor Cyan
}

Write-Section "Start repository setup"

# Ensure script runs from repository root (where this script lives)
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
Set-Location $scriptDir

# 1) Restore dotnet tools (reads .config/dotnet-tools.json)
Write-Host "Restoring dotnet tools (this reads .config/dotnet-tools.json)..." -ForegroundColor Yellow
if (Test-Path ".config\dotnet-tools.json") {
    dotnet tool restore
} else {
    Write-Host "No .config/dotnet-tools.json found. Skipping dotnet tool restore." -ForegroundColor DarkYellow
}

# 2) Ensure dotnet husky is installed as a local tool (best-effort)
# If project uses global dotnet tool install, skip installing here.

# 3) Run dotnet husky install to create .husky/_ and hook scripts
Write-Host "Installing husky hooks..." -ForegroundColor Yellow
try {
    dotnet husky install
    Write-Host "Husky install executed." -ForegroundColor Green
} catch {
    Write-Host "Failed to run 'dotnet husky install'. Make sure dotnet and Husky tool are installed (see README)." -ForegroundColor Red
}

if (-not $SkipVerify) {
    Write-Section "Verification"
    Write-Host "Checking git hooks existence (.husky folder)..." -ForegroundColor Yellow
    if (Test-Path ".husky" ) {
        Get-ChildItem -Path .husky -Force | ForEach-Object { Write-Host "  $_" }
        Write-Host "Husky hooks appear present." -ForegroundColor Green
    } else {
        Write-Host ".husky folder not found after install. You may need to run 'dotnet husky install' manually or ensure the Husky dotnet tool is available." -ForegroundColor Red
    }
}

Write-Section "Done"

Write-Host "Next steps: git config user.name/email if needed; run 'git status' to review changes; run 'dotnet format' if required by project." -ForegroundColor Cyan
