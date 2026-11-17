using System.Linq;

namespace SistemaBiblioteca.API.Validators;

public static class CpfValidator
{
    public static bool Valido(string cpf)
    {
        if(cpf.Length != 11)
        {
            return false;
        }

        if(cpf.Distinct().Count() == 1)
        {
            return false;
        }

        var soma = 0;

        for(int i = 0; i <= 8; i++)
        {
            int numero = cpf[i] - '0';

            soma += numero * (10 - i);
        }

        var resto = soma % 11;
        int digito1;

        if(resto < 2)
        {
            digito1 = 0;
        }
        else
        {
            digito1 = 11 - resto;
        }

        if(digito1 != cpf[9] - '0')
        {
            return false;
        }

        int soma2 = 0;
        for(int i = 0; i <= 9; i++)
        {
            int numero2 = cpf[i] - '0';

            soma2 += numero2 * (11 - i);
        }
        var resto2 = soma2 % 11;
        int digito2;
        if(resto2 < 2)
        {
            digito2 = 0;
        }
        else
        {
            digito2 = 11 - resto2;
        }

        if(digito2 != cpf[10] - '0')
        {
            return false;
        }
        return true;
    }
}