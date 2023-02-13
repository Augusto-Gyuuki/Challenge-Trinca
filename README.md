# Challenge.Trinca

appSettings.json contém as configurações para a integração com o Azure Cosmos DB e as configurações para a mensagem de outbox.

## CosmosDbSettings

- `AccountEndpoint`: Este é o endpoint da conta do Azure Cosmos DB. Deve ser preenchido com o valor correspondente do Azure Key Vault.
- `AccountKey`: Este é a chave de conta do Azure Cosmos DB. Deve ser preenchido com o valor correspondente do Azure Key Vault.
- `DatabaseName`: Este é o nome da base de dados do Azure Cosmos DB.

## OutboxMessageSettings

- `BackgroundIntevalInSeconds`: Este é o intervalo de tempo em segundos entre as verificações de mensagens na fila de outbox.
- `RetryCount`: Este é o número de tentativas de envio de mensagens da fila de outbox.
- `RetryWaitTimeInSeconds`: Este é o tempo de espera em segundos entre as tentativas de envio de mensagens da fila de outbox.
- `MessagesTakeCount`: Este é o número de mensagens que devem ser lidas da fila de outbox em cada verificação.
