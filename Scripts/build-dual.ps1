param(
    [ValidateSet("all", "0.107.1", "0.113.0")]
    [string]$Target = "all",
    [switch]$SkipPck,
    [switch]$SkipDeploy
)

$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $PSScriptRoot
$projectFile = Join-Path $projectRoot "StsModBloodywolf.csproj"
$loaderProject = Join-Path $projectRoot "loader\StsModBloodywolf.Loader.csproj"
$manifestSource = Join-Path $projectRoot "StsModBloodywolf.json"
$workshopRoot = Join-Path $projectRoot "artifacts\workshop"
$packageRoot = Join-Path $workshopRoot "StsModBloodywolf"
$buildOutput = Join-Path $projectRoot ".godot\mono\temp\bin\Release\StsModBloodywolf.dll"
$loaderOutput = Join-Path $projectRoot "loader\bin\Release\net9.0\StsModBloodywolf.Loader.dll"
$pckName = "StsModBloodywolf.pck"
$packagePck = Join-Path $packageRoot $pckName
$godotLog = Join-Path $workshopRoot "godot-export.log"
$stableRefs = Join-Path $projectRoot "refs\game\0.107.1"
$betaRefs = Join-Path $projectRoot "refs\game\0.113.0"
$gameRoot = "D:\Program Files (x86)\Steam\steamapps\common\Slay the Spire 2"
$uploaderRoot = "D:\ModUploader-win-x64\StsModBloodywolf"
$uploaderContent = Join-Path $uploaderRoot "content"
$uploaderWorkshopConfig = Join-Path $projectRoot "uploader\workshop.json"

function Invoke-Checked {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Name,
        [Parameter(Mandatory = $true)]
        [scriptblock]$Command
    )

    Write-Host ""
    Write-Host "==> $Name" -ForegroundColor Cyan
    & $Command
    if ($LASTEXITCODE -ne 0) {
        throw "$Name failed with exit code $LASTEXITCODE"
    }
}

function Get-ArtifactDirectory {
    param([string]$ApiCompat)

    $relative = if ($ApiCompat -eq "0.107.1") {
        "lib\StsModBloodywolf.Compat.0.107.1"
    } else {
        "lib\StsModBloodywolf"
    }
    return Join-Path $packageRoot $relative
}

function Reset-Package {
    if (Test-Path -LiteralPath $packageRoot) {
        Remove-Item -LiteralPath $packageRoot -Recurse -Force
    }
    New-Item -ItemType Directory -Path $packageRoot -Force | Out-Null
}

function Build-TargetArtifact {
    param([string]$ApiCompat)

    Invoke-Checked "Restore $ApiCompat" {
        dotnet restore $projectFile -p:Sts2ApiCompat=$ApiCompat
    }
    Invoke-Checked "Build $ApiCompat" {
        dotnet build $projectFile -c Release --no-restore --nologo `
            -p:Sts2ApiCompat=$ApiCompat `
            -p:CopyModOnBuild=false
    }

    if (-not (Test-Path -LiteralPath $buildOutput)) {
        throw "Build output not found: $buildOutput"
    }

    $artifactDir = Get-ArtifactDirectory $ApiCompat
    New-Item -ItemType Directory -Path $artifactDir -Force | Out-Null
    Copy-Item -LiteralPath $buildOutput -Destination (Join-Path $artifactDir "StsModBloodywolf.dll") -Force
}

function Build-Loader {
    Invoke-Checked "Build Loader" {
        dotnet build $loaderProject -c Release --nologo -p:Sts2RefDir=$stableRefs
    }
    if (-not (Test-Path -LiteralPath $loaderOutput)) {
        throw "Loader output not found: $loaderOutput"
    }
    Copy-Item -LiteralPath $loaderOutput -Destination (Join-Path $packageRoot "StsModBloodywolf.dll") -Force
}

function Write-PackageManifest {
    param([string[]]$Targets)

    $manifest = [System.IO.File]::ReadAllText(
        $manifestSource,
        [System.Text.Encoding]::UTF8
    ) | ConvertFrom-Json

    $artifactEntries = foreach ($apiCompat in ($Targets | Sort-Object)) {
        $relativeDir = if ($apiCompat -eq "0.107.1") {
            "lib/StsModBloodywolf.Compat.0.107.1"
        } else {
            "lib/StsModBloodywolf"
        }
        [pscustomobject]@{
            min_game_version = $apiCompat
            pck_path = $pckName
            dll_path = "$relativeDir/StsModBloodywolf.dll"
        }
    }

    $manifest.has_pck = $false
    $manifest.min_game_version = ($Targets | Sort-Object | Select-Object -First 1)
    $manifest | Add-Member -NotePropertyName "StsModBloodywolf" -NotePropertyValue (
        [pscustomobject]@{ artifacts = @($artifactEntries) }
    ) -Force
    $manifest.PSObject.Properties.Remove("pck_name")

    $manifestText = $manifest | ConvertTo-Json -Depth 12
    [System.IO.File]::WriteAllText(
        (Join-Path $packageRoot "StsModBloodywolf.json"),
        $manifestText + [Environment]::NewLine,
        (New-Object System.Text.UTF8Encoding($false))
    )
}

function Export-PackagePck {
    $godotSettings = Join-Path $projectRoot ".vscode\settings.json"
    $godotExe = $env:GODOT_EXE
    if (-not $godotExe -and (Test-Path -LiteralPath $godotSettings)) {
        $settings = [System.IO.File]::ReadAllText(
            $godotSettings,
            [System.Text.Encoding]::UTF8
        ) | ConvertFrom-Json
        $godotExe = $settings.'godotTools.editorPath.godot4'
    }
    if (-not $godotExe -or -not (Test-Path -LiteralPath $godotExe)) {
        throw "Godot executable not found. Set GODOT_EXE or .vscode/settings.json."
    }

    Remove-Item -LiteralPath $packagePck -Force -ErrorAction SilentlyContinue
    & $godotExe --headless --recovery-mode --path $projectRoot `
        --export-pack "Windows Desktop" $packagePck *> $godotLog

    $deadline = (Get-Date).AddSeconds(60)
    while (-not (Test-Path -LiteralPath $packagePck) -and (Get-Date) -lt $deadline) {
        Start-Sleep -Milliseconds 500
    }

    if (-not (Test-Path -LiteralPath $packagePck)) {
        Get-Content -LiteralPath $godotLog -Tail 80 -ErrorAction SilentlyContinue
        $rootPck = Join-Path $projectRoot $pckName
        if (Test-Path -LiteralPath $rootPck) {
            Write-Host "PCK export failed; reusing existing root PCK." -ForegroundColor Yellow
            Copy-Item -LiteralPath $rootPck -Destination $packagePck -Force
            return
        }
        throw "PCK export did not produce: $packagePck"
    }

    Write-Host "PCK exported: $packagePck" -ForegroundColor Green
    Copy-Item -LiteralPath $packagePck -Destination (Join-Path $projectRoot $pckName) -Force
}

function Use-ExistingPck {
    $rootPck = Join-Path $projectRoot $pckName
    if (-not (Test-Path -LiteralPath $rootPck)) {
        throw "PCK export skipped and no existing PCK found at $rootPck"
    }
    Copy-Item -LiteralPath $rootPck -Destination $packagePck -Force
}

function Deploy-CurrentVersion {
    param([string[]]$Targets)

    if ($SkipDeploy) {
        Write-Host "Local deployment skipped." -ForegroundColor Yellow
        return
    }
    if (Get-Process -Name "SlayTheSpire2" -ErrorAction SilentlyContinue) {
        throw "SlayTheSpire2 is running. Close the game before deployment."
    }

    $releaseInfoPath = Join-Path $gameRoot "release_info.json"
    if (-not (Test-Path -LiteralPath $releaseInfoPath)) {
        throw "Game release info not found: $releaseInfoPath"
    }

    $releaseInfo = [System.IO.File]::ReadAllText(
        $releaseInfoPath,
        [System.Text.Encoding]::UTF8
    ) | ConvertFrom-Json
    $currentVersion = $releaseInfo.version.TrimStart("v")
    if ($Targets -notcontains $currentVersion) {
        Write-Host "Current game version $currentVersion is not among built targets; deployment skipped." -ForegroundColor Yellow
        return
    }

    $destination = Join-Path $gameRoot "mods\StsModBloodywolf"
    if (Test-Path -LiteralPath $destination) {
        Remove-Item -LiteralPath $destination -Recurse -Force
    }
    Copy-Item -LiteralPath $packageRoot -Destination $destination -Recurse -Force
    Write-Host "Deployed unified loader package to $destination" -ForegroundColor Green
}

function Sync-Uploader {
    if (-not (Test-Path -LiteralPath $uploaderRoot)) {
        Write-Host "ModUploader workspace not found; sync skipped." -ForegroundColor Yellow
        return
    }

    if (Test-Path -LiteralPath $uploaderContent) {
        Remove-Item -LiteralPath $uploaderContent -Recurse -Force
    }
    Copy-Item -LiteralPath $packageRoot -Destination $uploaderContent -Recurse -Force

    if (Test-Path -LiteralPath $uploaderWorkshopConfig) {
        Copy-Item -LiteralPath $uploaderWorkshopConfig `
            -Destination (Join-Path $uploaderRoot "workshop.json") -Force
    }
    Write-Host "Synced unified package to ModUploader workspace." -ForegroundColor Green
}

New-Item -ItemType Directory -Path $workshopRoot -Force | Out-Null
[System.IO.File]::WriteAllText((Join-Path $workshopRoot ".gdignore"), "")
[System.IO.File]::WriteAllText((Join-Path $projectRoot "refs\.gdignore"), "")

$targets = if ($Target -eq "all") { @("0.107.1", "0.113.0") } else { @($Target) }
Reset-Package

foreach ($apiCompat in $targets) {
    Build-TargetArtifact $apiCompat
}

if ($Target -eq "all") {
    # Leave the project restored/built against stable for Godot export and IDE consistency.
    Invoke-Checked "Restore default package state 0.107.1" {
        dotnet restore $projectFile -p:Sts2ApiCompat=0.107.1
    }
    Invoke-Checked "Build default package state 0.107.1" {
        dotnet build $projectFile -c Release --no-restore --nologo `
            -p:Sts2ApiCompat=0.107.1 `
            -p:CopyModOnBuild=false
    }
    Copy-Item -LiteralPath $buildOutput `
        -Destination (Join-Path (Get-ArtifactDirectory "0.107.1") "StsModBloodywolf.dll") -Force
}

Build-Loader
Write-PackageManifest $targets

if ($SkipPck) {
    Use-ExistingPck
} else {
    Export-PackagePck
}

Deploy-CurrentVersion $targets
Sync-Uploader

Write-Host ""
Write-Host "Unified loader package completed." -ForegroundColor Green
Write-Host "  Package: $packageRoot"
Get-ChildItem -LiteralPath $packageRoot -Recurse -File | ForEach-Object {
    Write-Host "  $($_.FullName.Substring($packageRoot.Length + 1))"
}
