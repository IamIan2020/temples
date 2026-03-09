# 宮廟會員系統 - 一鍵部署腳本
# 用法：以系統管理員執行 PowerShell，輸入 .\deploy.ps1

param(
    [string]$PublishPath = "D:\publish\temples-api",
    [string]$SiteName = "Temples"
)

$ErrorActionPreference = "Stop"
$ProjectRoot = $PSScriptRoot

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  宮廟會員系統 - 自動部署" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# 1. 備份 web.config
$webConfigBackup = $null
$webConfigPath = "$PublishPath\web.config"
if (Test-Path $webConfigPath) {
    Write-Host "[1/6] 備份 web.config..." -ForegroundColor Yellow
    $webConfigBackup = Get-Content $webConfigPath -Raw
    Write-Host "      OK" -ForegroundColor Green
} else {
    Write-Host "[1/6] 無需備份 web.config（首次部署）" -ForegroundColor Gray
}

# 2. 停止 IIS 站台
Write-Host "[2/6] 停止 IIS 站台 $SiteName..." -ForegroundColor Yellow
try {
    Stop-WebSite -Name $SiteName -ErrorAction SilentlyContinue
    Stop-WebAppPool -Name $SiteName -ErrorAction SilentlyContinue
    Start-Sleep -Seconds 2
    Write-Host "      OK" -ForegroundColor Green
} catch {
    Write-Host "      跳過（站台可能未執行）" -ForegroundColor Gray
}

# 3. 後端 Publish
Write-Host "[3/6] 發布後端..." -ForegroundColor Yellow
$apiProject = "$ProjectRoot\src\Temples.Api"
dotnet publish $apiProject -c Release -o $PublishPath --nologo -v q
if ($LASTEXITCODE -ne 0) {
    Write-Host "      後端發布失敗！" -ForegroundColor Red
    exit 1
}
Write-Host "      OK" -ForegroundColor Green

# 4. 前端 Build + 複製
Write-Host "[4/6] 建置前端..." -ForegroundColor Yellow
$frontendPath = "$ProjectRoot\frontend"
Push-Location $frontendPath
npm run build --silent 2>&1 | Out-Null
if ($LASTEXITCODE -ne 0) {
    npm run build
    if ($LASTEXITCODE -ne 0) {
        Pop-Location
        Write-Host "      前端建置失敗！" -ForegroundColor Red
        exit 1
    }
}
Pop-Location

Write-Host "      複製前端檔案至 wwwroot..." -ForegroundColor Yellow
$wwwroot = "$PublishPath\wwwroot"
if (Test-Path $wwwroot) {
    Remove-Item "$wwwroot\*" -Recurse -Force
}
New-Item -ItemType Directory -Path $wwwroot -Force | Out-Null
Copy-Item "$frontendPath\dist\*" $wwwroot -Recurse -Force
Write-Host "      OK" -ForegroundColor Green

# 5. 還原 web.config
Write-Host "[5/6] 還原 web.config..." -ForegroundColor Yellow
if ($webConfigBackup) {
    Set-Content $webConfigPath -Value $webConfigBackup -NoNewline
    Write-Host "      OK（已還原自訂設定）" -ForegroundColor Green
} else {
    Write-Host "      使用預設 web.config" -ForegroundColor Gray
}

# 確保 logs 目錄存在
New-Item -ItemType Directory -Path "$PublishPath\logs" -Force | Out-Null

# 6. 啟動 IIS 站台
Write-Host "[6/6] 啟動 IIS 站台 $SiteName..." -ForegroundColor Yellow
try {
    Start-WebAppPool -Name $SiteName
    Start-WebSite -Name $SiteName
    Write-Host "      OK" -ForegroundColor Green
} catch {
    Write-Host "      啟動失敗：$($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  部署完成！" -ForegroundColor Green
Write-Host "  https://ianx75.tplinkdns.com" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Green
