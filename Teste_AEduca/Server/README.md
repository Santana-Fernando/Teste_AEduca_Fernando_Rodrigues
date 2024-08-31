# My ASP.NET Web API

Este projeto é uma Web API construída com ASP.NET Core. Este guia fornecerá as etapas necessárias para configurar e executar a aplicação localmente.

## Pré-requisitos

Antes de começar, você precisará ter instalado em sua máquina:

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Visual Studio](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (opcional, dependendo da configuração do banco de dados)

## Configuração do Projeto

### Extra. Rode o projeto pelo Docker

Ajuste a string de conexão

```bash
"DockerConnection": "Host=localhost;Port=5432;Database=Registration;Username=postgres;Password=senha"
```

Faça a mudança no Infra.IoC > DependenceInjection

```bash
services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DockerConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
```

Coloque as credenciais no arquivo docker-compose.override.yml

```bash
- ConnectionStrings__DockerConnection=Host=postgres;Port=5432;Database=Registration;Username=postgres;Password=senha
```

Abra o terminal na pasta do projeto e rode o comando

```bash
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```
