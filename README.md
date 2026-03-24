## Sobre o Projeto

API em **.NET 8**, adota **DDD (domain Drive Design)** para gerenciamento de despesas pessoais e **MySql** para banco de dados. O objetivo é permitir o usuário registrar suas despesas com título, valor, dia e hora e observação.
Arquitetura baseada em **REST**, com métodos **HTTP** e documentação via **Swagger**.
Pacotes utilizados:
- **AutoMapper**: Mapeamento entre objetos de domínio e requisição/resposta;
- **Shoudly**: Testes de unidade;
- **EntityFramework**: ORM para interações com Banco de Dados.

![hero-image]

### Features

- **Domain-Drive Design (DDD)**: Facilita manutenção do domínio da Aplicação;
- **Testes de Unidade**: Utilização de Shoudly para testes unitários e funcionalidades;
- **Geração de Relatórios**: Exportação de Relatórios de Despesas em **Excel ou PDF**;
- **RESTful API e Swagger**: Interface documentada para integração e testes;

### Construído com

![badge-dot-net] ![badge-windows] ![badge-visual-studio] ![badge-mysql] ![badge-swagger]

## Getting Started
Siga os passos para rodar a aplicação

### Requisitos
- Visual Studio 2022+
- Windows 10+ ou Linux/MacOS com [.NET SDK][dot-net-sdk]
- [MySQL Server][mysql]

### Instalação
1. Clonar o Repositório:
   ```sh
   git clone https://github.com/oliveirah12/CashFlow.git
   ```
3. Preencher Infos no `appsettings.Development.json`
4. Executar 









<!-- Links -->
[dot-net-sdk]: https://dotnet.microsoft.com/pt-br/download/dotnet/8.0
[mysql]: https://www.mysql.com/downloads/

<!-- Images -->
[hero-image]: images/heroimage.png

<!-- Badges -->
[badge-dot-net]: https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge
[badge-windows]: https://img.shields.io/badge/Windows-0078D4?logo=windows&logoColor=fff&style=for-the-badge
[badge-visual-studio]: https://img.shields.io/badge/Visual%20Studio-5C2D91?logo=visualstudio&logoColor=fff&style=for-the-badge
[badge-mysql]: https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=fff&style=for-the-badge
[badge-swagger]: https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=000&style=for-the-badge





