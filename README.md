# Identity Microservice

## Descrição

Este microserviço de Identity é o primeiro domínio do nosso sistema de e-commerce. Ele é responsável por gerenciar a autenticação e autorização de usuários, fornecendo uma base segura para todos os outros microserviços.

**Tecnologias Utilizadas:**

- .NET 8
- Entity Framework Core + Npgsql
- Evolve
- RabbitMQ
- Scalar API
- xUnit + Moq
- TestContainers
- JWT (Microsoft.IdentityModel.Tokens)

**Arquitetura:**

O projeto adota uma arquitetura baseada em DDD, Clean Architecture, CQRS e Repository Pattern, visando baixo acoplamento, testabilidade e escalabilidade.

## Funcionalidades Atuais

- **Registro de Usuário:** Criação de novas contas de usuário.
- **Login de Usuário:** Autenticação de usuários existentes.
- **Gerenciamento de Token JWT:** Geração e validação de tokens JWT para autenticação.
- **HATEOAS:** Respostas API com links navegáveis.

## Próximos Passos

- Integração com outros microserviços do sistema de e-commerce.
- Expansão das funcionalidades de gerenciamento de usuário (reset de senha, etc.).
