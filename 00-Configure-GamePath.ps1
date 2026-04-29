[CmdletBinding()]
param()

$ErrorActionPreference = "Stop"

if (-not $PSVersionTable -or -not $PSVersionTable.PSVersion -or $PSVersionTable.PSVersion.Major -lt 5) {
    throw "This setup script requires Windows PowerShell 5.1+ or PowerShell 7+."
}

try {
    $utf8NoBom = [System.Text.UTF8Encoding]::new($false)
    [Console]::InputEncoding = $utf8NoBom
    [Console]::OutputEncoding = $utf8NoBom
    $global:OutputEncoding = $utf8NoBom
}
catch {
}

try {
    if (Get-Command chcp.com -ErrorAction SilentlyContinue) {
        & chcp.com 65001 > $null
    }
}
catch {
}

$projectDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$localPropsPath = Join-Path $projectDir "local.props"

Write-Host ""
Write-Host "BaseLibToRitsu package setup"
Write-Host "This writes local.props beside the project to resolve sts2.dll / 0Harmony.dll / Steamworks.NET.dll."
Write-Host ""

$gamePath = Read-Host "Enter Slay the Spire 2 install path"
if ([string]::IsNullOrWhiteSpace($gamePath)) {
    throw "Game path cannot be empty."
}

$gamePath = $gamePath.Trim().Trim('"')
if (-not (Test-Path -LiteralPath $gamePath -PathType Container)) {
    throw "Game path does not exist: $gamePath"
}

$dataDir = Join-Path $gamePath "data_sts2_windows_x86_64"
$requiredFiles = @(
    "sts2.dll",
    "0Harmony.dll",
    "Steamworks.NET.dll"
)

$missingFiles = @(
    $requiredFiles | Where-Object {
        -not (Test-Path -LiteralPath (Join-Path $dataDir $_) -PathType Leaf)
    }
)

if (-not (Test-Path -LiteralPath $dataDir -PathType Container)) {
    Write-Warning "Game data directory was not found: $dataDir"
}
elseif ($missingFiles.Count -gt 0) {
    Write-Warning ("These prerequisite DLLs were not found under " + $dataDir + ": " + ($missingFiles -join ", "))
}

$escapedGamePath = [System.Security.SecurityElement]::Escape($gamePath)
$escapedDataDir = [System.Security.SecurityElement]::Escape($dataDir)
$content = @"
<Project>
  <PropertyGroup>
    <Sts2Dir>$escapedGamePath</Sts2Dir>
    <Sts2DataDir>$escapedDataDir</Sts2DataDir>
  </PropertyGroup>
</Project>
"@

[System.IO.File]::WriteAllText($localPropsPath, $content, [System.Text.UTF8Encoding]::new($false))

Write-Host ""
Write-Host "Wrote local.props:"
Write-Host "  $localPropsPath"
Write-Host ""
Write-Host "You can build the project now."
Read-Host "Press Enter to exit"