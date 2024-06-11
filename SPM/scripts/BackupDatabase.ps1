param (
    [string]$ConfigFilePath
)

function Get-ConfigValue {
    param (
        [string]$ConfigFilePath,
        [string]$Section,
        [string]$Key
    )

    $ini = Get-Content $ConfigFilePath | Out-String
    $pattern = "(?ms)^\[$Section\](.*?)^\["
    $matches = [regex]::Matches($ini, $pattern)
    if ($matches.Count -eq 0) {
        $pattern = "(?ms)^\[$Section\](.*)"
        $matches = [regex]::Matches($ini, $pattern)
    }
    if ($matches.Count -gt 0) {
        $sectionContent = $matches[0].Groups[1].Value
        $keyPattern = "(?m)^\s*$Key\s*=\s*(.*?)\s*$"
        $keyMatches = [regex]::Matches($sectionContent, $keyPattern)
        if ($keyMatches.Count -gt 0) {
            return $keyMatches[0].Groups[1].Value.Trim()
        }
    }
    return $null
}

# Path to the MySQL options file
$configFilePath = $ConfigFilePath

# Read values from the config file
# $user = Get-ConfigValue -ConfigFilePath $configFilePath -Section "client" -Key "user"
# $password = Get-ConfigValue -ConfigFilePath $configFilePath -Section "client" -Key "password"
# $hostname = Get-ConfigValue -ConfigFilePath $configFilePath -Section "client" -Key "host"
$database = Get-ConfigValue -ConfigFilePath $configFilePath -Section "other" -Key "database"
$backupPath = Get-ConfigValue -ConfigFilePath $configFilePath -Section "other" -Key "backup_path"

if (-not (Test-Path $backupPath)) {
    Write-Error "Backup path does not exist: $backupPath"
    exit 1
}

$backupFile = Join-Path -Path $backupPath -ChildPath "$database-$(Get-Date -Format 'yyyyMMddHHmmss').sql"

# Full path to the mysqldump executable
# TODO: $mysqlPath = "$env:MYSQL_Commands/mysqldump.exe"
$mysqldumpPath = "C:\Program Files\MySQL\MySQL Server 8.0\bin\mysqldump.exe"

# Construct the mysqldump command arguments
# $args = @("--defaults-extra-file=$configFilePath", "$database")
$args = @("--login-path=client", "$database")

# Execute the command
Start-Process -FilePath $mysqldumpPath -ArgumentList $args -RedirectStandardOutput $backupFile -NoNewWindow -Wait

try {
    Start-Process -FilePath $mysqldumpPath -ArgumentList $args -RedirectStandardOutput $backupFile -NoNewWindow -Wait -ErrorAction Stop
    # Write-Output "Backup completed successfully: $backupFile"
}
catch {
    Write-Error "Backup failed: $_"
}
