using DTI.Pedidos.Entidades;
using DTI.Pedidos.Repositorios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DTI.Pedidos.Servicos;

public interface IPedidoServico
{
    Task<IEnumerable<Pedido>> ObterPedidos();
    Task<IEnumerable<Pedido>> ObterPedidosCozinha();
    Task<IEnumerable<Pedido>> ObterPedidosCopa();
    Task<Pedido> ObterPedido(int id);
    Task InserirPedido(Pedido pedido);
    Task AlterarPedido(Pedido pedido);
    Task DeletarPedido(int id);
    Task AtualizarBebidaPronta(int id);
    Task AtualizarPratosProntos(int id);
}

public class PedidoServico : IPedidoServico
{
    private readonly IPedidoRepositorio _pedidoRepositorio;

    public PedidoServico(IPedidoRepositorio pedidoRepositorio)
    {
        _pedidoRepositorio = pedidoRepositorio;
    }

    public async Task<IEnumerable<Pedido>> ObterPedidos()
    {
        return await _pedidoRepositorio.ObterPedidos(); 
    }

    public async Task<IEnumerable<Pedido>> ObterPedidosCozinha()
    {
        return await _pedidoRepositorio.ObterPedidosCozinha();
    }

    public async Task<IEnumerable<Pedido>> ObterPedidosCopa()
    {
        return await _pedidoRepositorio.ObterPedidosCopa();
    }

    public async Task<Pedido> ObterPedido(int id)
    {
        return await _pedidoRepositorio.ObterPedido(id);
    }

    public async Task InserirPedido(Pedido pedido)
    {
        await _pedidoRepositorio.InserirPedido(pedido);
    }

    public async Task AlterarPedido(Pedido pedido)
    {
        await _pedidoRepositorio.AlterarPedido(pedido);
    }

    public async Task AtualizarBebidaPronta(int id)
    {
        await _pedidoRepositorio.AtualizarBebidaPronta(id);
    }

    public async Task AtualizarPratosProntos(int id)
    {
        await _pedidoRepositorio.AtualizarPratosProntos(id);
    }

    public async Task DeletarPedido(int id)
    {
        await _pedidoRepositorio.DeletarPedido(id);
    }
}
