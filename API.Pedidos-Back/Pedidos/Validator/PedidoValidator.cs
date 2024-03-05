using DTI.Pedidos.Entidades;
using FluentValidation;

namespace DTI.Pedidos.Validator;

public class PedidoValidator : AbstractValidator<Pedido>
{
    public PedidoValidator()
    {
        RuleFor(l => l.NomeSolicitante)
            .Must(nome => !string.IsNullOrEmpty(nome))
            .WithMessage("O campo “NomeSolicitante” deverá estar preenchido");

        RuleFor(l => l.Mesa)
            .Must(nome => !string.IsNullOrEmpty(nome))
            .WithMessage("O campo “Mesa” deverá estar preenchido");
    }
}
