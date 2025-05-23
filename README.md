# MyMvcApp CRUD Application Deployment Guide

This guide provides step-by-step instructions for deploying the MyMvcApp CRUD Application to Azure using an ARM template and automating the deployment with a GitHub Actions pipeline.

## Prerequisites

- An active Azure subscription
- Azure CLI installed ([Download here](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli))
- GitHub repository containing your MyMvcApp source code
- Sufficient permissions to create resources in your Azure subscription

## ARM Template Overview

The provided ARM template (`deploy.json`) defines the following Azure resources:

- **App Service Plan** (`Microsoft.Web/serverfarms`): Hosts the web app.
- **Web App** (`Microsoft.Web/sites`): The Azure App Service where your ASP.NET Core MVC app will be deployed.

### Template Files

- `deploy.json`: The ARM template defining the infrastructure.
- `deploy.parameters.json`: Parameters file for customizing deployment (web app name, location, SKU).

## Deploying with Azure CLI

1. **Register the Microsoft.Web Resource Provider (if not already registered):**
   ```powershell
   az provider register --namespace Microsoft.Web
   ```

2. **Create a Resource Group (if needed):**
   ```powershell
   az group create --name <your-resource-group> --location <azure-region>
   ```

3. **Deploy the ARM Template:**
   ```powershell
   az deployment group create \
     --resource-group <your-resource-group> \
     --template-file deploy.json \
     --parameters @deploy.parameters.json
   ```

4. **Find Your Web App URL:**
   After deployment, your app will be available at:
   ```
   https://<webAppName>.azurewebsites.net
   ```
   (Replace `<webAppName>` with the value from your parameters file.)

## GitHub Actions Pipeline for CI/CD

To automate deployment, add a GitHub Actions workflow file (e.g., `.github/workflows/azure-deploy.yml`) to your repository. Below is a sample workflow:

```yaml
name: Deploy ASP.NET Core App to Azure Web App

on:
  push:
    branches:
      - master

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Set up .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'
      - name: Build
        run: dotnet build MyMvcApp.csproj --configuration Release
      - name: Publish
        run: dotnet publish MyMvcApp.csproj --configuration Release --output ./publish
      - name: 'Deploy to Azure Web App'
        uses: azure/webapps-deploy@v3
        with:
          app-name: ${{ secrets.AZURE_WEBAPP_NAME }}
          publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
          package: ./publish
```

### Notes:
- Set the following GitHub repository secrets:
  - `AZURE_WEBAPP_NAME`: Name of your Azure Web App (from `deploy.parameters.json`).
  - `AZURE_WEBAPP_PUBLISH_PROFILE`: Export this from the Azure Portal (Web App > Get publish profile).
- Adjust paths and project names as needed for your solution structure.

## Troubleshooting

- **Resource Not Found Errors:**
  - Ensure the `Microsoft.Web` provider is registered.
  - Deploy to a resource group, not at the subscription level.
- **File Lock Errors During Build:**
  - Stop any running instances of the app before rebuilding.
- **App Not Starting:**
  - Check the Azure Portal for logs and configuration issues.

## Additional Resources
- [Azure Resource Manager Templates Documentation](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/overview)
- [Deploy ASP.NET Core apps to Azure App Service](https://docs.microsoft.com/en-us/azure/app-service/quickstart-dotnetcore)
- [GitHub Actions for Azure](https://github.com/Azure/actions)

---

This README provides a complete guide for deploying and automating your MyMvcApp CRUD Application on Azure. For further customization, refer to the official Azure documentation.
