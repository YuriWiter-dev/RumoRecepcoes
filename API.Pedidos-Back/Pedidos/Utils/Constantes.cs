using FluentValidation.Results;
using System;
using System.Linq;

namespace DTI.Pedidos.Utils;

public static class Constantes
{
    public static string CaminhoBancoDeDados = $"{AppDomain.CurrentDomain.BaseDirectory}DbDtiPedido.sqlite";

    public static void ValidarRequest(ValidationResult validationResult)
    {
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(validationResult.Errors.FirstOrDefault().ErrorMessage);
        }
    }
}
