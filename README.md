# Estacionei 🚗
API desenvolvida para controlar as entradas e saídas de veículos em um estacionamento. A aplicação permite o cadastro de clientes, veículos com seu respectivo tipo, configuração de tarifas por hora com base no tipo de veículo, além de registrar entradas e saídas.

Atualmente, a API está em fase de desenvolvimento, mas os **endpoints** listados abaixo já estão em pleno funcionamento.

## Como Utilizar?

1. **Instalar o .NET SDK**.
2. **Usar uma IDE**: Recomenda-se utilizar o Visual Studio.
3. **Clonar o projeto**.
4. **Instalar o SQL Server**: Futuramente, irei adicionar a imagem junto com a aplicação em container.
5. **Rodar o seguinte comando via Console de Gerenciador de Pacotes**:
   ```bash
   dotnet ef database update

6 - Aguarde a criação do banco e suas tabelas.   
7 - Dentro do codigo, altere o arquivo appsettings.Development.json -> ConnectionString  -> DefaultConnection -> Alterando apenas a instancia do SQL
``` json
"Server=InstanciaSQL;Database=DB_Estacionei;Trusted_Connection=True; TrustServerCertificate=True;"
```
8 - De f5 e se divirta.

A API está em desenvolvimento e estou sempre trazendo melhorias. Qualquer dica ou dúvida, basta me procurar via LinkedIn. Agradeço pela compreensão!
# Endpoints

## Clientes 🙎
Todos os Enpoints que são voltados ao cadastro ou atualização o telefone é obrigatorio e também precisa ser passado um telefone valido no seguinte formato 1799999999  
### **GET - Todos os Clientes**
Endpoint utilizado para recuperar todos os clientes e seus respectivos veículos. É possível utilizar parâmetros de paginação via URL, como o tamanho da página e o número da página desejada.

- **URL**: `GET https://servidor:porta/api/Clientes?PageNumber=1&PageSize=10`

#### Exemplo de retorno:
```json
[
  {
    "clienteId": 3,
    "clienteNome": "string",
    "clienteTelefone": "17999999999",
    "veiculosCliente": []
  },
  {
    "clienteId": 1003,
    "clienteNome": "ASSASA",
    "clienteTelefone": "17999999999",
    "veiculosCliente": [
      {
        "veiculoPlaca": "string",
        "veiculoModelo": "string",
        "tipoVeiculo": 1
      }
    ]
  }
]
```
### **GET - Cliente por ID**
Endpoint utilizado para recuperar um cliente específico pelo seu ID. Não possui paginação.

- **URL**: `GET https://servidor:porta/api/Clientes/{id}`

#### Exemplo de retorno:
```json
{
  "clienteId": 1008,
  "clienteNome": "Dexter",
  "clienteTelefone": "1799999999",
  "veiculosCliente": [
    {
      "veiculoPlaca": "CXC5422",
      "veiculoModelo": "Uno",
      "tipoVeiculo": 1
    },
    {
      "veiculoPlaca": "HJSK22",
      "veiculoModelo": "Gol",
      "tipoVeiculo": 1
    }
  ]
```
### **POST - Criar Cliente**
Este endpoint é utilizado para o cadastro de clientes, sendo obrigatório informar um nome e um telefone. Caso queira cadastrar um veículo junto ao cliente, basta incluí-lo no corpo da requisição.

- **URL**: `POST https://servidor:porta/api/Clientes`

#### Exemplo de request:
```json
{
  "clienteNome": "string",
  "clienteTelefone": "string",
  "Vehicle": {
    "veiculoPlaca": "string",
    "veiculoModelo": "string",
    "tipoVeiculo": 1
  }
}
```
### **PUT - Atualizar Cliente**
Este endpoint é utilizado para atualizar o cadastro de um cliente. Neste endpoint, não é possível alterar o Vehicle do cliente, sendo obrigatório o preenchimento do telefone.

- **URL**: `PUT https://servidor:porta/api/Clientes/{id}`

#### Exemplo de request:
```json
{
  "clienteNome": "string",
  "clienteTelefone": "string"
}
