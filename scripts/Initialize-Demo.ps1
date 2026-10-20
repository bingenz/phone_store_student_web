param([switch]$Approved)
$ErrorActionPreference = 'Stop'
if (-not $Approved) { throw 'Chỉ chạy sau khi đã duyệt khởi động LocalDB và tạo SmartMobileStore_Dev. Thêm -Approved khi được duyệt.' }
$repoPath = Split-Path -Parent $PSScriptRoot
$localDbPath = (Get-Command SqlLocalDB.exe -ErrorAction Stop).Source
& $localDbPath start MSSQLLocalDB
if ($LASTEXITCODE -ne 0) { throw 'Không khởi động được LocalDB.' }
$existing = & sqlcmd -S '(localdb)\MSSQLLocalDB' -E -C -b -h -1 -W -Q "SET NOCOUNT ON; SELECT CASE WHEN DB_ID(N'SmartMobileStore_Dev') IS NULL THEN 0 ELSE 1 END"
if ($LASTEXITCODE -ne 0) { throw 'Không kiểm tra được database bằng Windows Authentication.' }
if (($existing -join '').Trim() -ne '0') { throw 'SmartMobileStore_Dev đã tồn tại. Dừng để xác minh, không migrate hoặc seed.' }
Push-Location $repoPath
$previousConnection = $env:ConnectionStrings__Store
try {
    $env:ConnectionStrings__Store = 'Server=(localdb)\MSSQLLocalDB;Database=SmartMobileStore_Dev;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=5'
    dotnet tool restore
    if ($LASTEXITCODE -ne 0) { throw 'Tool restore thất bại.' }
    dotnet ef database update --project SmartMobileStore --connection $env:ConnectionStrings__Store
    if ($LASTEXITCODE -ne 0) { throw 'Migration thất bại; không seed.' }
    dotnet run --project SmartMobileStore --no-launch-profile -- --seed-demo
    if ($LASTEXITCODE -ne 0) { throw 'Seed thất bại. Không tự xóa database; xem lỗi trước khi chạy tiếp.' }
} finally {
    $env:ConnectionStrings__Store = $previousConnection
    Pop-Location
}
