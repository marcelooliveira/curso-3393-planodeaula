# Criar as variáveis de ambiente no Windows:
# CosmosDB_URI
# CosmosDB_KEY
# CosmosDB_CONNECTIONSTRING

Write-Output "Criando Azure Function LOCAL para consulta de carrinho..."
Set-Location C:\Users\Nitro5\Documents\GitHub\curso-3393-planodeaula\Aula01\depois
Set-Location MarceloLojinhaApp
func new --name Carrinho --template "HTTP trigger" --authlevel "anonymous"

# Write-Output "Fazendo login no Azure... (verifique o navegador)"
# az login
# Start-Sleep -Seconds 5

Write-Output "Publicando Function App local na nuvem..."
func azure functionapp publish MarceloLojinhaApp
