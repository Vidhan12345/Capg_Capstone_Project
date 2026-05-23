param(
    [Parameter(Mandatory=$true)]
    [string]$StaticWebAppName,
    [Parameter(Mandatory=$true)]
    [string]$BuildOutputPath,
    [Parameter(Mandatory=$false)]
    [string]$ResourceGroup = ""
)

Write-Host "Deploying frontend to Azure Static Web Apps: $StaticWebAppName"

# Using SWA CLI for deployment
# Requires: npm install -g @azure/static-web-apps-cli
$deploymentToken = $env:DEPLOYMENT_TOKEN

if (-not $deploymentToken) {
    Write-Error "DEPLOYMENT_TOKEN environment variable is required"
    exit 1
}

swa deploy $BuildOutputPath `
    --deployment-token $deploymentToken `
    --app-name $StaticWebAppName

Write-Host "Frontend deployment completed successfully"
