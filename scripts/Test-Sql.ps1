param([switch]$Approved, [switch]$ReuseKnownTestDatabase)
$ErrorActionPreference = 'Stop'
if (-not $Approved) { throw 'Cần duyệt database SmartMobileStore_Test_20261020 trước khi thêm -Approved.' }
$previousApproval = $env:SMARTMOBILE_SQL_APPROVED
$previousReuse = $env:SMARTMOBILE_REUSE_APPROVED_TEST
Push-Location (Split-Path -Parent $PSScriptRoot)
try {
    $env:SMARTMOBILE_SQL_APPROVED = '1'
    $env:SMARTMOBILE_REUSE_APPROVED_TEST = $(if ($ReuseKnownTestDatabase) { '1' } else { '0' })
    dotnet test --logger 'console;verbosity=minimal'
    if ($LASTEXITCODE -ne 0) { throw 'Kiểm thử thất bại; xem kết quả ở trên. Database được giữ nguyên.' }
} finally {
    $env:SMARTMOBILE_SQL_APPROVED = $previousApproval
    $env:SMARTMOBILE_REUSE_APPROVED_TEST = $previousReuse
    Pop-Location
}
