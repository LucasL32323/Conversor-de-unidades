namespace Medidas.Core;

public enum Categoria
{
    Temperatura,
    Distancia,
    Massa,
}

public static class CategoriaExtensions
{
    public static string Descricao(this Categoria categoria) => categoria switch
    {
        Categoria.Temperatura => "Temperatura",
        Categoria.Distancia => "Distância",
        Categoria.Massa => "Massa",
        _ => categoria.ToString(),
    };
}
