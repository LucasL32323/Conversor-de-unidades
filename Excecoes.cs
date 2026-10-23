namespace Medidas.Core;

public sealed class UnidadeDesconhecidaException(string simbolo)
    : Exception($"Unidade desconhecida: '{simbolo}'.")
{
    public string Simbolo { get; } = simbolo;
}

public sealed class ConversaoIncompativelException(string mensagem) : Exception(mensagem);

public sealed class ValorInvalidoException(string mensagem) : Exception(mensagem);
