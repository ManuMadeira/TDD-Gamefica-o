using System;

namespace Gamification.Domain.Exceptions;

public class ElegibilidadeNaoAtendidaException : Exception
{
    public ElegibilidadeNaoAtendidaException(string? message = null) : base(message ?? "Elegibilidade não atendida.") { }
}

public class BadgeJaConcedidaException : Exception
{
    public BadgeJaConcedidaException(string? message = null) : base(message ?? "Badge já concedida.") { }
}

public class ConfiguracaoInvalidaException : Exception
{
    public ConfiguracaoInvalidaException(string? message = null) : base(message ?? "Configuração inválida.") { }
}
