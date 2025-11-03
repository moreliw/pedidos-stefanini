using FluentValidation;
using Pedidos.Application.DTOs;

namespace Pedidos.API.Validators;

public class CriarPedidoRequestValidator : AbstractValidator<CriarPedidoRequest>
{
    public CriarPedidoRequestValidator()
    {
        RuleFor(x => x.NomeCliente)
            .NotEmpty().MaximumLength(60);

        RuleFor(x => x.EmailCliente)
            .NotEmpty().MaximumLength(60).EmailAddress();

        RuleFor(x => x.Itens)
            .NotNull().NotEmpty().WithMessage("Pedido deve conter pelo menos um item.");

        RuleForEach(x => x.Itens)
            .SetValidator(new CriarItemPedidoRequestValidator());
    }
}

public class CriarItemPedidoRequestValidator : AbstractValidator<CriarItemPedidoRequest>
{
    public CriarItemPedidoRequestValidator()
    {
        RuleFor(x => x.IdProduto).GreaterThan(0);
        RuleFor(x => x.Quantidade).GreaterThan(0);
    }
}

public class AtualizarPedidoRequestValidator : AbstractValidator<AtualizarPedidoRequest>
{
    public AtualizarPedidoRequestValidator()
    {
        RuleFor(x => x.NomeCliente)
            .NotEmpty().MaximumLength(60);

        RuleFor(x => x.EmailCliente)
            .NotEmpty().MaximumLength(60).EmailAddress();

        RuleFor(x => x.Itens)
            .NotNull().NotEmpty().WithMessage("Pedido deve conter pelo menos um item.");

        RuleForEach(x => x.Itens)
            .SetValidator(new CriarItemPedidoRequestValidator());
    }
}