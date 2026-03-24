## Sobre o Projeto

API em **.NET 8**, adota **DDD (domain Drive Design)** para gerenciamento de despesas pessoais e **MySql** para banco de dados. O objetivo é permitir o usuário registrar suas despesas com título, valor, dia e hora e observação.
Arquitetura baseada em **REST**, com métodos **HTTP** e documentação via **Swagger**.
Pacotes utilizados:
- **AutoMapper**: Mapeamento entre objetos de domínio e requisição/resposta;
- **Shoudly**: Testes de unidade;
- **EntityFramework**: ORM para interações com Banco de Dados.

### Features

- **Domain-Drive Design (DDD)**: Facilita manutenção do domínio da Aplicação;
- **Testes de Unidade**: Utilização de Shoudly para testes unitários e funcionalidades;
- **Geração de Relatórios**: Exportação de Relatórios de Despesas em **Excel ou PDF**;
- **RESTful API e Swagger**: Interface documentada para integração e testes;

## Getting Started
Siga os passos para rodar a aplicação

### Requisitos
- Visual Studio 2022+
- Windows 10+ ou Linux/MacOS com [.NET SDK](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
- [MySQL Server](https://www.mysql.com/downloads/)

### Instalação
1. Clonar o Repositório:
   ```sh
   git clone https://github.com/oliveirah12/CashFlow.git
   ```
3. Preencher Infos no `appsettings.Development.json`
4. Executar 
