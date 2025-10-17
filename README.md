# TDD-Gameficacao
Repositório contendo a implementação parcial do módulo de Gamification (Awards) para atividade acadêmica.

## Visão geral
Este projeto implementa o domínio de um sistema de gamificação em .NET 9. As mudanças recentes incluíram a criação de value objects, entidades, exceções de domínio e testes unitários.

Tecnologias:
- .NET 9
- xUnit, Moq, FluentAssertions para testes

## Estrutura do repositório
```
Gamification/
├── src/
│   └── Gamification.Domain/
│       ├── Awards/
│       │   ├── Models/           # Entidades (Award, BadgeAward, RewardLog)
│       │   └── Ports/            # Interfaces (IAwardsReadStore, IAwardsWriteStore, IAwardsUnitOfWork)
│       ├── ValueObjects/         # BadgeSlug, XpAmount
│       └── Exceptions/           # Exceções de domínio
├── tests/
│   └── Gamification.Domain.Tests/
│       └── Awards/               # Testes de Models e Ports
```

## Principais arquivos adicionados/alterados
- src/Gamification.Domain/ValueObjects/BadgeSlug.cs
    - Value object imutável para identificador de badge (valida formato).

- src/Gamification.Domain/ValueObjects/XpAmount.cs
    - Value object imutável para quantidade de XP (não-negativo).

- src/Gamification.Domain/Exceptions/DomainExceptions.cs
    - Exceções de domínio: `ElegibilidadeNaoAtendidaException`, `BadgeJaConcedidaException`, `ConfiguracaoInvalidaException`.

- src/Gamification.Domain/Awards/Models/BadgeAward.cs
    - Entidade representando um badge concedido (chave natural: UserId + BadgeSlug). Aplicadas validações no construtor.

- src/Gamification.Domain/Awards/Models/RewardLog.cs
    - Entidade de auditoria com timestamp automático (`OccurredAt = DateTime.UtcNow`).

- src/Gamification.Domain/Awards/Models/Award.cs
    - Entidade existente (mantida), comentários traduzidos.

- tests/Gamification.Domain.Tests/Awards/Models/ValueObjectsTests.cs
    - Testes para `BadgeSlug`, `XpAmount` e `RewardLog`.

- tests/Gamification.Domain.Tests/Awards/Models/AwardTests.cs
    - Testes existentes (strings traduzidas onde aplicável).

- tests/Gamification.Domain.Tests/Awards/Ports/PortsTests.cs
    - Testes de portas (strings traduzidas onde aplicável).

## Como executar os testes (PowerShell)
```powershell
cd C:\Repositorios\DotNet\TDD-Gamefica-o\Gamification\Gamification
# Restaurar e build
dotnet restore; dotnet build
# Executar testes do projeto de testes
dotnet test .\tests\Gamification.Domain.Tests\Gamification.Domain.Tests.csproj
```


## 📋 Domínio - Awards (Conquistas)

### Estrutura do Domínio
- **Models**: Entidades e objetos de valor
- **Policies**: Regras de negócio e políticas
- **Ports**: Interfaces para adapters externos

### Ports Implementados

As interfaces (ports) implementadas no domínio de Awards e seus métodos principais:

- `IAwardsReadStore` (porta de leitura / Query side)
    - Task<Award?> GetByIdAsync(Guid awardId, CancellationToken cancellationToken = default)
        - Obtém uma premiação pelo seu Id.
    - Task<IReadOnlyList<Award>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        - Recupera todas as premiações de um usuário.
    - Task<bool> ExistsAsync(Guid awardId, CancellationToken cancellationToken = default)
        - Verifica existência de uma premiação.

- `IAwardsWriteStore` (porta de escrita / Command side)
    - Task CreateAsync(Award award, CancellationToken cancellationToken = default)
        - Cria uma nova premiação.
    - Task UpdateAsync(Award award, CancellationToken cancellationToken = default)
        - Atualiza uma premiação existente.
    - Task DeleteAsync(Guid awardId, CancellationToken cancellationToken = default)
        - Remove uma premiação pelo Id.

- `IAwardsUnitOfWork` (unidade de trabalho/transacional)
    - IAwardsReadStore ReadStore { get; }
        - Acesso às operações de leitura.
    - IAwardsWriteStore WriteStore { get; }
        - Acesso às operações de escrita.
    - Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        - Persiste alterações dentro da unidade de trabalho.
    - Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        - Inicia uma transação.
    - Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        - Confirma a transação.
    - Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        - Desfaz a transação.

## 🚀 Como Executar

### Pré-requisitos
- .NET 9.0 SDK
- Git

### Comandos Básicos

```bash
# Clone o repositório
git clone <url-do-repositorio>
cd Gamification

# Restaurar pacotes
dotnet restore

# Build da solução
dotnet build

# Executar testes
dotnet test

# Executar testes com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Modo watch para desenvolvimento
dotnet watch test
```

## 🧪 Testes

### Estrutura de Testes
```
Tests/
└── Gamification.Domain.Tests/
    └── Awards/
        ├── Models/           # Testes de entidades
        ├── Policies/         # Testes de políticas
        └── Ports/           # Testes de interfaces
```

### Executando Testes Específicos

```bash
# Executar todos os testes
dotnet test

# Executar testes com verbosidade
dotnet test --verbosity normal

# Executar testes de um namespace específico
dotnet test --filter "Gamification.Domain.Tests.Awards.Models"

# Executar um teste específico
dotnet test --filter "Should_Create_Award_With_Valid_Data"
```

## 📦 Dependências

### Domínio
- **.NET 9.0** - Runtime e SDK
- **Nullable Reference Types** - Habilitado

### Testes
- **xUnit** - Framework de testes
- **Moq** - Framework de mocking
- **FluentAssertions** - Asserts mais expressivos
- **coverlet.collector** - Cobertura de código

## 🔧 Configurações do Projeto

### Gamification.Domain
```xml
<TargetFramework>net9.0</TargetFramework>
<Nullable>enable</Nullable>
<ImplicitUsings>enable</ImplicitUsings>
```

### Gamification.Domain.Tests
```xml
<TargetFramework>net9.0</TargetFramework>
<Nullable>enable</Nullable>
<IsPackable>false</IsPackable>
```

## 💡 Padrões e Convenções

### Nomenclatura
- **Interfaces**: Prefixo `I` (ex: `IAwardsRepository`)
- **Testes**: Nome descritivo (ex: `Should_Create_Award_When_Valid`)
- **Namespaces**: Seguindo estrutura de pastas

### Arquitetura
- **Ports & Adapters**: Separação clara de concerns
- **Domain-Driven Design**: Foco no domínio de negócio
- **Test-Driven Development**: Desenvolvimento guiado por testes

## 🐛 Solução de Problemas

### Erros Comuns

**Problema**: Erro de referência do xUnit
```bash
# Solução: Restaurar e rebuild
dotnet clean
dotnet restore
dotnet build
```

**Problema**: Testes não encontrados
```bash
# Solução: Verificar filtros
dotnet test --list-tests
```
