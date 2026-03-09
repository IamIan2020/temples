# Temple Membership System - Deploy Script
# Usage: Run PowerShell as Administrator, then: .\deploy.ps1

param(
    [string]$PublishPath = "D:\publish\temples-api",
    [string]$SiteName = "Temples"
)

$ErrorActionPreference = "Stop"
$ProjectRoot = $PSScriptRoot

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Temple System - Auto Deploy" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# 1. Backup web.config
$webConfigBackup = $null
$webConfigPath = "$PublishPath\web.config"
if (Test-Path $webConfigPath) {
    Write-Host "[1/6] Backup web.config..." -ForegroundColor Yellow
    $webConfigBackup = Get-Content $webConfigPath -Raw
    Write-Host "      OK" -ForegroundColor Green
} else {
    Write-Host "[1/6] No web.config to backup (first deploy)" -ForegroundColor Gray
}

# 2. Stop IIS site
Write-Host "[2/6] Stop IIS site $SiteName..." -ForegroundColor Yellow
try {
    Stop-WebSite -Name $SiteName -ErrorAction SilentlyContinue
    Stop-WebAppPool -Name $SiteName -ErrorAction SilentlyContinue
    Start-Sleep -Seconds 2
    Write-Host "      OK" -ForegroundColor Green
} catch {
    Write-Host "      Skip (site may not be running)" -ForegroundColor Gray
}

# 3. Publish backend
Write-Host "[3/6] Publish backend..." -ForegroundColor Yellow
$apiProject = "$ProjectRoot\src\Temples.Api"
dotnet publish $apiProject -c Release -o $PublishPath --nologo -v q
if ($LASTEXITCODE -ne 0) {
    Write-Host "      Backend publish FAILED!" -ForegroundColor Red
    exit 1
}
Write-Host "      OK" -ForegroundColor Green

# 4. Build frontend + copy
Write-Host "[4/6] Build frontend..." -ForegroundColor Yellow
$frontendPath = "$ProjectRoot\frontend"
Push-Location $frontendPath
$ErrorActionPreference = "Continue"
npm run build 2>&1 | ForEach-Object { $_.ToString() } | Out-Null
$buildExit = $LASTEXITCODE
$ErrorActionPreference = "Stop"
if ($buildExit -ne 0) {
    Pop-Location
    Write-Host "      Frontend build FAILED!" -ForegroundColor Red
    exit 1
}
Pop-Location

Write-Host "      Copy frontend to wwwroot..." -ForegroundColor Yellow
$wwwroot = "$PublishPath\wwwroot"
if (Test-Path $wwwroot) {
    Remove-Item "$wwwroot\*" -Recurse -Force
}
New-Item -ItemType Directory -Path $wwwroot -Force | Out-Null
Copy-Item "$frontendPath\dist\*" $wwwroot -Recurse -Force
Write-Host "      OK" -ForegroundColor Green

# 5. Restore web.config
Write-Host "[5/6] Restore web.config..." -ForegroundColor Yellow
if ($webConfigBackup) {
    Set-Content $webConfigPath -Value $webConfigBackup -NoNewline
    Write-Host "      OK (custom settings restored)" -ForegroundColor Green
} else {
    Write-Host "      Using default web.config" -ForegroundColor Gray
}

# Ensure logs directory exists
New-Item -ItemType Directory -Path "$PublishPath\logs" -Force | Out-Null

# 6. Start IIS site
Write-Host "[6/6] Start IIS site $SiteName..." -ForegroundColor Yellow
try {
    Start-WebAppPool -Name $SiteName
    Start-WebSite -Name $SiteName
    Write-Host "      OK" -ForegroundColor Green
} catch {
    Write-Host "      Start FAILED: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  Deploy Complete!" -ForegroundColor Green
Write-Host "  https://ianx75.tplinkdns.com" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Green
