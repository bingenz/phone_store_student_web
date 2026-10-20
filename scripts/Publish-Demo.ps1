param([Parameter(Mandatory = $true)][string]$OutputDirectory)
$ErrorActionPreference = 'Stop'
$repoDirectory = Split-Path -Parent $PSScriptRoot
$target = [System.IO.Path]::GetFullPath($OutputDirectory)
if (Test-Path -LiteralPath $target) {
    throw 'Thu muc dich da ton tai. Chon thu muc moi de khong ghi de ban publish dang dung.'
}
Push-Location $repoDirectory
try {
    dotnet publish SmartMobileStore -c Release --output $target
    if ($LASTEXITCODE -ne 0) { throw 'Publish that bai.' }
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'Run-PublishedDemo.ps1') -Destination $target
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'Chay-Demo.cmd') -Destination $target
    Copy-Item -LiteralPath (Join-Path $repoDirectory 'docs\DEMO.md') -Destination (Join-Path $target 'HUONG_DAN.md')
    Write-Host "Da publish: $target"
} finally { Pop-Location }
