# Criar a pasta do projeto
mkdir api_minimal
cd api_minimal

# Criar o projeto API Minimal
dotnet new webapi -minimal -o .

# Adicionar pacotes do Entity Framework Core
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools

# Criar pastas para Models e Data
mkdir Models
mkdir Data

# Adicionar migration inicial
dotnet tool install --global dotnet-ef # Se necessário
dotnet ef migrations add InitialCreate --context AppDbContext

# Aplicar as migrations ao banco de dados
dotnet ef database update --context AppDbContext

# Executar o projeto
dotnet run