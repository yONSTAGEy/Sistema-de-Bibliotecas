namespace Biblioteca.Helpers;
public static class SelecaoHelper
{
    public static T? SelecionarPorNumero<T>(List<T> itens, int escolha) where T : class
    {
        if (escolha >= 1 && escolha <= itens.Count)
       {
           return itens[escolha - 1];
       }
       return null;
    }
}