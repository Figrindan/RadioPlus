$ErrorActionPreference = "Stop"

$gameDir = "E:\SteamLibrary\steamapps\common\Easy Delivery Co"
$dll = "bin\Debug\netstandard2.1\RadioPlus.dll"

dotnet build
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Copy-Item $dll "$gameDir\BepInEx\plugins\" -Force
Write-Host "Deployed to $gameDir\BepInEx\plugins\" -ForegroundColor Green