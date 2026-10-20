param(
    [string]$AppDirectory = $PSScriptRoot,
    [ValidateRange(1024, 65535)][int]$Port = 5182,
    [switch]$CreateAdmin
)
$ErrorActionPreference = 'Stop'
$executable = Join-Path $AppDirectory 'SmartMobileStore.exe'
if (-not (Test-Path -LiteralPath $executable)) {
    throw 'Khong thay SmartMobileStore.exe. Hay chay script trong thu muc publish hoac truyen -AppDirectory.'
}
$previousConnection = $env:ConnectionStrings__Store
$previousEnvironment = $env:ASPNETCORE_ENVIRONMENT
Push-Location $AppDirectory
try {
    if ([string]::IsNullOrWhiteSpace($previousConnection)) {
        $server = '(localdb)\MSSQLLocalDB'
        $sqlcmd = Get-Command sqlcmd -ErrorAction SilentlyContinue
        if ($sqlcmd) {
            function Test-DemoDatabase([string]$Candidate) {
                # Windows PowerShell 5.1 surfaces native stderr as ErrorRecord.
                # A failed alias probe must reach the read-only fallback check.
                $ErrorActionPreference = 'Continue'
                $result = & $sqlcmd.Source -S $Candidate -E -C -l 3 -b -h -1 -W -Q "SET NOCOUNT ON; SELECT COUNT(*) FROM sys.databases WHERE name=N'SmartMobileStore_Dev' AND state_desc=N'ONLINE';" 2>&1
                return ($LASTEXITCODE -eq 0 -and (($result | Out-String).Trim() -eq '1'))
            }
            if (-not (Test-DemoDatabase $server)) {
                # Some tool sessions cannot resolve the LocalDB alias used by Visual Studio.
                # Read the current endpoint; never save a transient pipe in appsettings.
                $logPath = Join-Path $env:LOCALAPPDATA 'Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\error.log'
                $endpoint = $null
                if (Test-Path -LiteralPath $logPath) {
                    $line = Select-String -LiteralPath $logPath -Pattern 'connection on \[\s*(\\\\\.\\pipe\\LOCALDB#[^\]]+)\]' | Select-Object -Last 1
                    if ($line) { $endpoint = 'np:' + $line.Matches[0].Groups[1].Value.Trim() }
                }
                if ($endpoint -and (Test-DemoDatabase $endpoint)) { $server = $endpoint }
                else { throw 'Khong ket noi duoc SmartMobileStore_Dev hien co. Kiem tra LocalDB/tai khoan Windows; khong xoa file DB hay khoi tao de len du lieu.' }
            }
        }
        $env:ConnectionStrings__Store = "Server=$server;Database=SmartMobileStore_Dev;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=5"
    }
    $env:ASPNETCORE_ENVIRONMENT = 'Production'
    if ($CreateAdmin) {
        & $executable --create-admin
    } else {
        Write-Host "Mo trinh duyet: http://localhost:$Port"
        Write-Host 'Giu cua so nay mo. Nhan Ctrl+C de dung. Du lieu luu trong SQL tren may nay.'
        & $executable --urls "http://localhost:$Port"
    }
    if ($LASTEXITCODE -ne 0) { throw "Ung dung dung voi ma loi $LASTEXITCODE." }
} finally {
    $env:ConnectionStrings__Store = $previousConnection
    $env:ASPNETCORE_ENVIRONMENT = $previousEnvironment
    Pop-Location
}
