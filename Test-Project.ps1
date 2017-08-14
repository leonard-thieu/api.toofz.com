[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [string]$Assembly
)

if (-Not(Test-Path Env:\PROJECT)) { throw 'The environment variable "PROJECT" is not set. Tests will not be run.' }
$project = $env:PROJECT
$configuration = $env:CONFIGURATION
if ($configuration -eq $null) { $configuration = 'Debug' }

[xml]$xml = Get-Content "$project.Tests\packages.config"
$version = ($xml.packages.package | ? { $_.id -eq 'OpenCover' }).version

$targetArgs = ".\$project.Tests\bin\$configuration\$project.Tests.dll"
if (Test-Path Env:\APPVEYOR) { $targetArgs = $targetArgs + ' /logger:AppVeyor' }

& "packages\OpenCover.$version\tools\OpenCover.Console.exe" `
    "-register:user" `
    "-target:vstest.console.exe" `
    "-targetargs:$targetArgs" `
    "-filter:+[$Assembly*]*" `
    "-excludebyattribute:*.ExcludeFromCodeCoverage*;*.GeneratedCodeAttribute*"
if ($LASTEXITCODE -ne 0) { $Host.SetShouldExit($LASTEXITCODE) }