# Updated for this solution's project/folder naming ("2b_DbContext")

# För att göra .ps1-filen körbar, kör följande kommando i PowerShell (Behöver bara köras första gången):
# Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

#If EFC tools needs update use:
#dotnet tool update --global dotnet-ef

# To execute:
# .\database-rebuild-all.ps1 databasename [sqlserver|mysql|postgresql] [docker|azure] [root|dbo|supusr|usr|gstusr] appsettingsFolder

# example:
# .\database-rebuild-all.ps1 sql-attraction-rating-app sqlserver docker root ..\0_AppWebApi
# .\database-rebuild-all.ps1 sql-attraction-rating-app sqlserver docker dbo ..\AppRazor
# .\database-rebuild-all.ps1 sql-attraction-rating-app sqlserver docker dbo ..\AppMvc

param(
    [Parameter(Mandatory=$true)]
    [string]$DatabaseName,

    [Parameter(Mandatory=$true)]
    [ValidateSet("sqlserver", "mysql", "postgresql")]
    [string]$DatabaseType,
    
    [Parameter(Mandatory=$true)]
    [ValidateSet("docker", "azure")]
    [string]$DeploymentTarget,

    [Parameter(Mandatory=$true)]
    [ValidateSet("root", "dbo", "supusr", "usr", "gstusr")]
    [string]$DefaultDataUser,

    [Parameter(Mandatory=$true)]
    [string]$AppSettingsFolder
)

#Set Database Context
switch ($DatabaseType) {
    "sqlserver"  { $DBContext = "SqlServerDbContext" }
    "mysql"      { $DBContext = "MySqlDbContext" }
    "postgresql" { $DBContext = "PostgresDbContext" }
}

$AppSettingsFolder = Resolve-Path $AppSettingsFolder
$AppSettingsPath = Join-Path $AppSettingsFolder "appsettings.json"

#set UseDataSetWithTag to "<db_name>.<db_type>.<env>" in appsettings.json
$pattern = '"UseDataSetWithTag"\s*:\s*"[^"]*"'
$replacement = '"UseDataSetWithTag": "' + $DatabaseName + '.' + $DatabaseType + '.' + $DeploymentTarget + '"'
(Get-Content -Path $AppSettingsPath) -replace $pattern, $replacement | Set-Content -Path $AppSettingsPath

#set DefaultDataUser in appsettings.json
$Content = Get-Content $AppSettingsPath -Raw
$UpdatedContent = $Content -replace '"DefaultDataUser":\s*"[^"]*"', ('"DefaultDataUser": "' + $DefaultDataUser + '"')
Set-Content $AppSettingsPath $UpdatedContent

if ($DeploymentTarget -eq "docker") {
    #drop any database
    $env:EFC_AppSettingsFolder = $AppSettingsFolder
    dotnet ef database drop -f -c $DBContext -p ../2b_DbContext -s ../2b_DbContext
}

#remove any migration
Remove-Item -Recurse -Force ../2b_DbContext/Migrations/$DBContext -ErrorAction SilentlyContinue

#make a full new migration
$env:EFC_AppSettingsFolder = $AppSettingsFolder
dotnet ef migrations add miInitial -c $DBContext -p ../2b_DbContext -s ../2b_DbContext -o Migrations/$DBContext

#update the database from the migration
$env:EFC_AppSettingsFolder = $AppSettingsFolder
dotnet ef database update -c $DBContext -p ../2b_DbContext -s ../2b_DbContext

#to initialize the database you need to run the sql scripts
#../2b_DbContext/SqlScripts/<db_type>/initDatabase.sql