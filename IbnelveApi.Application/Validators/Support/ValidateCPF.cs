using IbnelveApi.Application.Extensions;

namespace IbnelveApi.Application.Validators.Support;

/// <summary>
/// Classe para validação de CPF
/// ATUALIZADA: Melhorias na validação e tratamento de caracteres especiais
/// </summary>
public static class ValidateCPF
{
    /// <summary>
    /// Valida se o CPF é válido
    /// </summary>
    /// <param name="cpf">CPF com ou sem formatação</param>
    /// <returns>True se o CPF for válido</returns>
    public static bool BeValidCPF(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        // Remove caracteres especiais automaticamente
        cpf = cpf.RemoveSpecialCharacters();

        // Verifica se tem 11 dígitos
        if (cpf.Length != 11)
            return false;

        // Verifica se todos os dígitos são iguais (CPFs inválidos conhecidos)
        if (cpf.All(c => c == cpf[0]))
            return false;

        // Verifica se contém apenas números
        if (!cpf.All(char.IsDigit))
            return false;

        // Validação do primeiro dígito verificador
        var sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += int.Parse(cpf[i].ToString()) * (10 - i);
        }

        var remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;

        if (int.Parse(cpf[9].ToString()) != digit1)
            return false;

        // Validação do segundo dígito verificador
        sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += int.Parse(cpf[i].ToString()) * (11 - i);
        }

        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;

        return int.Parse(cpf[10].ToString()) == digit2;
    }

    /// <summary>
    /// Formata o CPF para exibição (xxx.xxx.xxx-xx)
    /// </summary>
    /// <param name="cpf">CPF sem formatação</param>
    /// <returns>CPF formatado</returns>
    public static string FormatCPF(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return string.Empty;

        cpf = cpf.RemoveSpecialCharacters();

        if (cpf.Length != 11)
            return cpf;

        return $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
    }

    /// <summary>
    /// Remove formatação do CPF (deixa apenas números)
    /// </summary>
    /// <param name="cpf">CPF com formatação</param>
    /// <returns>CPF apenas com números</returns>
    public static string UnformatCPF(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return string.Empty;

        return cpf.RemoveSpecialCharacters();
    }

    /// <summary>
    /// Verifica se o CPF está no formato correto (com ou sem formatação)
    /// </summary>
    /// <param name="cpf">CPF a ser verificado</param>
    /// <returns>True se o formato estiver correto</returns>
    public static bool IsValidFormat(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        // Formato sem máscara: 11 dígitos
        if (cpf.All(char.IsDigit) && cpf.Length == 11)
            return true;

        // Formato com máscara: xxx.xxx.xxx-xx
        var cpfPattern = @"^\d{3}\.\d{3}\.\d{3}-\d{2}$";
        return System.Text.RegularExpressions.Regex.IsMatch(cpf, cpfPattern);
    }
}

