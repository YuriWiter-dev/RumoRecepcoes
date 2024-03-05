using DTI.Pedidos.Entidades;
using DTI.Pedidos.Repositorios;
using DTI.Pedidos.Servicos;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DTI.Pedidos.Tests;

public class PedidoServicosTest
{
    private readonly Mock<IPedidoRepositorio> _pedidoRepositorioMock;

    public PedidoServicosTest()
    {
        _pedidoRepositorioMock = new Mock<IPedidoRepositorio>();
    }

    [Fact]
    private async Task DeveRetornarListaDePedidos()
    {
        _pedidoRepositorioMock.Setup(_ => _.ObterPedidos()).ReturnsAsync(
              new List<Pedido>()
               {
                    new()
                    {
                        Id = 1,
                        NomeSolicitante = "Yuri Witer!",
                        Data = DateTime.Now,
                    },
                    new()
                    {
                        Id = 1,
                        NomeSolicitante = "Yan Witer!",
                        Data = new DateTime(year: 2024, month: 01, day: 29),
                    }
               });

        PedidoServico pedidoServico = new(_pedidoRepositorioMock.Object);
        IEnumerable<Pedido> result = await pedidoServico.ObterPedidos();
        Assert.IsAssignableFrom<IEnumerable<Pedido>>(result);
        Assert.NotNull(result);
        Assert.True(result.Count() > 0);
    }

    [Fact]
    private async Task NaoDeveRetornarListaDePedidos()
    {
        _pedidoRepositorioMock.Setup(_ => _.ObterPedidos()).ReturnsAsync(new List<Pedido>());
        PedidoServico pedidoServico = new(_pedidoRepositorioMock.Object);
        IEnumerable<Pedido> result = await pedidoServico.ObterPedidos();
        Assert.IsAssignableFrom<IEnumerable<Pedido>>(result);
        Assert.NotNull(result);
        Assert.True(result.Count() == 0);
    }

    [Fact]
    private async Task DeveRetornarPedidoPorId()
    {
        _pedidoRepositorioMock.Setup(_ => _.ObterPedido(It.IsAny<int>())).ReturnsAsync(
              new Pedido()
              {
                  Id = 1,
                  NomeSolicitante = "Yuri Witer!",
                  Data = DateTime.Now,
              });

        PedidoServico pedidoServico = new(_pedidoRepositorioMock.Object);
        Pedido result = await pedidoServico.ObterPedido(1);
        Assert.IsAssignableFrom<Pedido>(result);
        Assert.NotNull(result);
        Assert.True(result != null);
    }

    [Fact]
    private async Task NaoDeveRetornarPedidoPorId()
    {
        PedidoServico pedidoServico = new(_pedidoRepositorioMock.Object);
        Pedido result = await pedidoServico.ObterPedido(1);
        Assert.True(result == null);
    }

    [Fact]
    private async Task DeveInserirPedido()
    {
        _pedidoRepositorioMock.Setup(_ => _.InserirPedido(It.IsAny<Pedido>()));
        PedidoServico pedidoServico = new(_pedidoRepositorioMock.Object);
        await pedidoServico.InserirPedido
            (new()
            {
                NomeSolicitante = "Yuri Witer!",
                Data = DateTime.Now,
            });
    }

    [Fact]
    private async Task DeveAlterarPedido()
    {
        _pedidoRepositorioMock.Setup(_ => _.AlterarPedido(It.IsAny<Pedido>()));
        PedidoServico pedidoServico = new(_pedidoRepositorioMock.Object);
        await pedidoServico.AlterarPedido
            (new()
            {
                Id = 1,
                NomeSolicitante = "Yuri Witer!",
                Data = DateTime.Now,
            });
    }

    [Fact]
    private async Task DeveDeletarPedido()
    {
        _pedidoRepositorioMock.Setup(_ => _.DeletarPedido(It.IsAny<int>()));
        PedidoServico pedidoServico = new(_pedidoRepositorioMock.Object);
        await pedidoServico.DeletarPedido(1);
    }
}