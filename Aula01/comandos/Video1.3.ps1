Write-Output "Criando Azure Function LOCAL para consulta de carrinho..."
Set-Location C:\Users\Nitro5\Documents\GitHub\curso-3393-planodeaula\Aula01\depois
Set-Location MarceloLojinhaApp
func new --name GetCarrinho --template "HTTP trigger" --authlevel "anonymous"

Write-Output "Modificando texto de boas-vindas..."
$file = 'HttpExample.cs'
$find = 'response.WriteString("Welcome to Azure Functions!");'
$replace = 'response.WriteString("Bem-vindos ao Curso Azure Functions!");'
(Get-Content $file).replace($find, $replace) | Set-Content $file

# func start

Write-Output "Fazendo login no Azure... (verifique o navegador)"
az login
Start-Sleep -Seconds 5

Write-Output "Criando grupo de recursos..."
az group create --resource-group curso-azure-functions --location eastus
Start-Sleep -Seconds 10

Write-Output "Criando Storage Account"
az storage account create --name marcelostorage --location eastus --resource-group curso-azure-functions --sku Standard_LRS --allow-blob-public-access false
Start-Sleep -Seconds 10

Write-Output "Criando Function App na nuvem..."
az functionapp create --resource-group curso-azure-functions --consumption-plan-location eastus --runtime dotnet-isolated --functions-version 4 --name MarceloLojinhaApp --storage-account marcelostorage
Start-Sleep -Seconds 20

Write-Output "Publicando Function App local na nuvem..."
func azure functionapp publish MarceloLojinhaApp
