param(
    [Parameter(Mandatory=$true)]
    [string]$ResourceGroup,
    [Parameter(Mandatory=$true)]
    [string]$AppServiceName,
    [Parameter(Mandatory=$true)]
    [string]$PackagePath,
    [Parameter(Mandatory=$false)]
    [string]$ConnectionString = ""
)

Write-Host "Deploying backend to Azure App Service: $AppServiceName"

# Deploy using zip deploy
$publishUrl = "https://$AppServiceName.scm.azurewebsites.net/api/zipdeploy"
$credentials = "$($AppServiceName)`:$($env:DEPLOYMENT_PASSWORD)"
$encodedCreds = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes($credentials))

Invoke-RestMethod -Uri $publishUrl -Method POST `
    -Headers @{Authorization = "Basic $encodedCreds"} `
    -ContentType "multipart/form-data" `
    -InFile $PackagePath

Write-Host "Deployment completed successfully"

# Apply EF Core migrations if connection string provided
if ($ConnectionString) {
    Write-Host "Applying EF Core migrations..."
    Invoke-RestMethod -Uri "https://$AppServiceName.azurewebsites.net/api/migrate" -Method POST
    Write-Host "Migrations applied"
}
