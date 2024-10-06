# AeC - Sistema de Endereços

Este projeto é uma aplicação web ASP.NET MVC que permite a gestão de endereços, com funcionalidades de criação, edição, exclusão e exportação de dados. O projeto utiliza Entity Framework Core para acesso ao banco de dados e SQL Server como banco de dados.

## Requisitos

- [.NET SDK 6.0 ou superior](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (ou outro editor compatível com .NET)
- Ferramenta de gerenciamento de pacotes NuGet

## Passos para configurar e rodar o projeto

### Clonar o repositório

Clone o repositório do projeto para o seu ambiente local usando o seguinte comando:

git clone https://github.com/mtcamillo/AeC.git

### Configurar o banco de dados
   
SQL Server: Certifique-se de que o SQL Server está instalado e rodando na sua máquina.

Connection String: Abra o arquivo appsettings.json na raiz do projeto e edite a string de conexão da seguinte forma, com base na sua instância do SQL Server:

"ConnectionStrings": {
  "DefaultConnection": "Server=SEU_SERVIDOR_SQL;Database=AeC;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}

Substitua SEU_SERVIDOR_SQL pelo nome da sua instância do SQL Server.

### Aplicar as migrations e criar o banco de dados
   
O projeto utiliza Entity Framework Core para a gestão de migrations e criação de tabelas no banco de dados, não foi necessário scripts para criação de tabelas. Para aplicar as migrations e criar o banco de dados, siga os seguintes passos:

  1. Abra o Terminal ou Console do Gerenciador de Pacotes no Visual Studio.

  2. Execute o comando para garantir que todas as dependências estão instaladas:

    dotnet restore  

  3. Execute o comando para aplicar as migrations e criar as tabelas no banco de dados:

    dotnet ef database update
    
  Esse comando criará o banco de dados e aplicará todas as migrations necessárias.

### Rodar o projeto

Após configurar o banco de dados e aplicar as migrations, você pode rodar o projeto com o seguinte comando:

        dotnet run

Ou, se estiver usando o Visual Studio, pressione F5 para rodar o projeto em modo de depuração.


## Funcionalidades do sistema

Cadastro e login de usuário.

CRUD de Endereços: Criação, leitura, atualização e exclusão de endereços.

Exportação de Endereços: Exportação dos dados de endereços para CSV.

Busca de Endereço pelo CEP: Integração com a API ViaCEP para preenchimento automático dos campos do endereço ao buscar por um CEP.


### Tela de login
![login](https://github.com/user-attachments/assets/1618a72c-a0ee-4c1a-9095-4004062ca8b7)

### Tela de cadastro
![registro](https://github.com/user-attachments/assets/4b66d9b3-f594-42f7-a689-5d5ef3078edc)

### Endereços
![tela enderecos](https://github.com/user-attachments/assets/95fb054d-5559-4582-a8d4-b35824d66398)

### Adicionar endereço
![add novo endereco](https://github.com/user-attachments/assets/8c681a78-76f6-4dbe-b3bf-43804c8a0bd1)

### Editar endereço
![editar endereço](https://github.com/user-attachments/assets/78be8e1f-822c-4757-8167-2e950b6be95e)

### Excluir endereço
![exclusão endereço](https://github.com/user-attachments/assets/8a57b404-2e7e-4908-99db-09e7c50c7e7f)

### Arquivo CSV
![csv](https://github.com/user-attachments/assets/a0f17903-d085-4f70-b992-24213592b748)


