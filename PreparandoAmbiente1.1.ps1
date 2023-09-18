az cosmosdb create -n marcelo-account -g curso-azure-functions --default-consistency-level Session --locations regionName='eastus'

az cosmosdb sql database create -a marcelo-account -g curso-azure-functions -n lojinha

az cosmosdb sql container create -a marcelo-account -g curso-azure-functions -d lojinha -n Carrinho -p "/myPartitionKey"

# inserir 1 item no container Carrinho:

# {
#   "id": "fulano@detal.com.br",
#   "items": [
#     {
#       "ProductId": 4,
#       "Icon": "🍊",
#       "Description": "Tangerina (kg)",
#       "UnitPrice": 3.5,
#       "Quantity": 1,
#       "Total": 3.5,
#       "Id": 3
#     },
#     {
#       "ProductId": 13,
#       "Icon": "🍒",
#       "Description": "Cereja (kg)",
#       "UnitPrice": 3.5,
#       "Quantity": 3,
#       "Total": 10.5,
#       "Id": 2
#     },
#     {
#       "ProductId": 17,
#       "Icon": "🥥",
#       "Description": "Coco (un)",
#       "UnitPrice": 4.5,
#       "Quantity": 2,
#       "Total": 9,
#       "Id": 1
#     }
#   ]
# }