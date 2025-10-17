# TDD-Gamefica-o
Atividade proposta pelo professor em aula
 
 # 🏆 Gamification System

Sistema de gamificação desenvolvido em .NET 9 com arquitetura limpa e testes unitários.

## 🏗️ Estrutura de Projetos

```
Gamification/
├── src/
│   └── Gamification.Domain/          # Domínio principal (Entities, Value Objects, Ports)
├── tests/
│   └── Gamification.Domain.Tests/    # Testes unitários do domínio
└── Gamification.sln                  # Solução principal
```

## 📋 Domínio - Awards (Conquistas)

### Estrutura do Domínio
- **Models**: Entidades e objetos de valor
- **Policies**: Regras de negócio e políticas
- **Ports**: Interfaces para adapters externos

### Ports Implementados
- `IAwardsReadStore`: Operações de leitura
- `IAwardsWriteStore`: Operações de escrita  
- `IAwardsUnitOfWork`: Unidade de trabalho transacional

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
