using System.Text.RegularExpressions;

public class Validaciones
{
    // Valida si el campo es un correo electrónico válido
    public static bool EsCorreoValido(string correo)
    {
        if (string.IsNullOrEmpty(correo))
            return false;

        string patronCorreo = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(correo, patronCorreo);
    }

    // Valida si el texto coincide con un patrón (para nombres, direcciones, etc.)
    public static bool EsTextoValido(string texto, string patron)
    {
        if (string.IsNullOrEmpty(texto))
            return false;

        return Regex.IsMatch(texto, patron);
    }
}
