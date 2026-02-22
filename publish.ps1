Import-Module './modules.psm1'

$build_platforms = "win-x64", "win-arm64", "linux-x64", "osx-arm64"

if (Test-Path "./publish")
{
    Remove-Item "./publish" -Recurse -Force
}

Set-Location "./Yohuke.LiveMarker"

foreach ($plat in $build_platforms)
{
    RunProcess "dotnet" "publish -o `"../publish/$plat`" -r $plat -c Release --self-contained true -p:PublishSingleFile=true -p:UseAppHost=true"
}

Set-Location ".."