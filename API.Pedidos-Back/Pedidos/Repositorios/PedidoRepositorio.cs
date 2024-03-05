using Dapper;
using DTI.Pedidos.DbContexto;
using DTI.Pedidos.Entidades;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DTI.Pedidos.Repositorios;

public interface IPedidoRepositorio
{
    Task<IEnumerable<Pedido>> ObterPedidos();
    Task<Pedido> ObterPedido(int id);
    Task InserirPedido(Pedido pedido);
    Task AlterarPedido(Pedido pedido);
    Task DeletarPedido(int id);
    Task<IEnumerable<Pedido>> ObterPedidosCozinha();
    Task<IEnumerable<Pedido>> ObterPedidosCopa();
    Task AtualizarBebidaPronta(int pedidoId);
    Task AtualizarPratosProntos(int pedidoId);
}

public class PedidoRepositorio : SQLiteDbContext, IPedidoRepositorio
{
    private readonly IDbContext _dbContext;

    public PedidoRepositorio(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #region ObterPedidos 

    public async Task<IEnumerable<Pedido>> ObterPedidos()
    {
        IEnumerable<Pedido> pedidos = await _connection.QueryAsync<Pedido>(
                sql: $@"
                        SELECT
	                        {nameof(Pedido.Id)},
	                        {nameof(Pedido.NomeSolicitante)},
	                        {nameof(Pedido.Mesa)},
	                        {nameof(Pedido.Data)},
	                        {nameof(Pedido.BebidasProntas)},
	                        {nameof(Pedido.PratosProntos)},
	                        {nameof(Pedido.PedidoConcluido)}
                        FROM
	                        {nameof(Pedido)}
                        ORDER BY
                            {nameof(Pedido.Id)} DESC
                ");

        foreach (Pedido pedido in pedidos)
        {
            pedido.Bebida = await ObterPedidoBebida(pedido.Id);
            pedido.Prato = await ObterPedidoPrato(pedido.Id);
        }

        return pedidos;
    }

    public async Task<IEnumerable<Pedido>> ObterPedidosCozinha()
    {
        IEnumerable<Pedido> pedidos = await _connection.QueryAsync<Pedido>(
                sql: $@"
                        SELECT
	                        {nameof(Pedido.Id)},
	                        {nameof(Pedido.NomeSolicitante)},
	                        {nameof(Pedido.Mesa)},
	                        {nameof(Pedido.Data)},
	                        {nameof(Pedido.BebidasProntas)},
	                        {nameof(Pedido.PratosProntos)},
	                        {nameof(Pedido.PedidoConcluido)}
                        FROM
	                        {nameof(Pedido)}
                        WHERE 
                            {nameof(Pedido.PratosProntos)} = 0
                        ORDER BY
                            {nameof(Pedido.Id)}
                ");

        foreach (Pedido pedido in pedidos)
        {
            pedido.Prato = await ObterPedidoPrato(pedido.Id);
        }

        return pedidos;
    }

    public async Task<IEnumerable<Pedido>> ObterPedidosCopa()
    {
        IEnumerable<Pedido> pedidos = await _connection.QueryAsync<Pedido>(
                sql: $@"
                        SELECT
	                        {nameof(Pedido.Id)},
	                        {nameof(Pedido.NomeSolicitante)},
	                        {nameof(Pedido.Mesa)},
	                        {nameof(Pedido.Data)},
	                        {nameof(Pedido.BebidasProntas)},
	                        {nameof(Pedido.PratosProntos)},
	                        {nameof(Pedido.PedidoConcluido)}
                        FROM
	                        {nameof(Pedido)}
                        WHERE 
                            {nameof(Pedido.BebidasProntas)} = 0
                        ORDER BY
                            {nameof(Pedido.Id)}
                ");

        foreach (Pedido pedido in pedidos)
        {
            pedido.Bebida = await ObterPedidoBebida(pedido.Id);
        }

        return pedidos;
    }

    public async Task<Pedido> ObterPedido(int id)
    {
        Pedido pedido = await _connection.QueryFirstOrDefaultAsync<Pedido>(
                sql: $@"
                        SELECT
	                        {nameof(Pedido.Id)},
	                        {nameof(Pedido.NomeSolicitante)},
	                        {nameof(Pedido.Mesa)},
	                        {nameof(Pedido.Data)},
	                        {nameof(Pedido.BebidasProntas)},
	                        {nameof(Pedido.PratosProntos)},
	                        {nameof(Pedido.PedidoConcluido)}
                        FROM
	                        {nameof(Pedido)}
                        WHERE
                            {nameof(Pedido.Id)} = @ID;
                ",
                param: new
                {
                    ID = id
                });

        if (pedido == null)
            return null;

        pedido.Bebida = await ObterPedidoBebida(pedido.Id);
        pedido.Prato = await ObterPedidoPrato(pedido.Id);

        return pedido;
    }

    private async Task<PedidoBebida> ObterPedidoBebida(int pedidoId)
    {
        return await _connection.QueryFirstOrDefaultAsync<PedidoBebida>(
                sql: $@"
                        SELECT
	                        {nameof(PedidoBebida.Id)},
	                        {nameof(PedidoBebida.IdPedido)},
	                        {nameof(PedidoBebida.Nome)},
	                        {nameof(PedidoBebida.Valor)},
	                        {nameof(PedidoBebida.Quantidade)}
                        FROM
	                        {nameof(PedidoBebida)}
                        WHERE
                            {nameof(PedidoBebida.IdPedido)} = @PEDIDO_ID;
                ",
                param: new
                {
                    PEDIDO_ID = pedidoId
                });
    }

    private async Task<PedidoPrato> ObterPedidoPrato(int pedidoId)
    {
        return await _connection.QueryFirstOrDefaultAsync<PedidoPrato>(
                sql: $@"
                        SELECT
	                        {nameof(PedidoPrato.Id)},
	                        {nameof(PedidoPrato.IdPedido)},
	                        {nameof(PedidoPrato.Nome)},
	                        {nameof(PedidoPrato.Valor)},
	                        {nameof(PedidoPrato.Quantidade)}
                        FROM
	                        {nameof(PedidoPrato)}
                        WHERE
                            {nameof(PedidoPrato.IdPedido)} = @PEDIDO_ID;
                ",
                param: new
                {
                    PEDIDO_ID = pedidoId
                });
    }

    #endregion ObterPedidos

    #region INSERT

    public async Task InserirPedido(Pedido pedido)
    {
        pedido.Id = await _connection.ExecuteScalarAsync<int>(
            sql: $@"
                   INSERT INTO
                        {nameof(Pedido)}(
	                       {nameof(Pedido.NomeSolicitante)},
	                       {nameof(Pedido.Mesa)},
	                       {nameof(Pedido.Data)}
                       )
                       VALUES(
	                       @{nameof(Pedido.NomeSolicitante)},
	                       @{nameof(Pedido.Mesa)},
	                       @{nameof(Pedido.Data)}                                                   
                       );

                    SELECT last_insert_rowid();
                ",
            param: pedido);           

        pedido.Bebida.IdPedido = pedido.Id;
        await InserirPedidoBebida(pedido.Bebida);

        pedido.Prato.IdPedido = pedido.Id;
        await InserirPedidoPrato(pedido.Prato);
    }

    public async Task InserirPedidoBebida(PedidoBebida pedidoBebida)
    {
        await _connection.ExecuteAsync(
            sql: $@"
                   INSERT INTO
                        {nameof(PedidoBebida)}(
	                       {nameof(PedidoBebida.IdPedido)},
	                       {nameof(PedidoBebida.Nome)},
	                       {nameof(PedidoBebida.Valor)},
	                       {nameof(PedidoBebida.Quantidade)}
                       )
                       VALUES(
	                       @{nameof(PedidoBebida.IdPedido)},
	                       @{nameof(PedidoBebida.Nome)},
	                       @{nameof(PedidoBebida.Valor)}            ,
	                       @{nameof(PedidoBebida.Quantidade)}                                                   
                       );
                ",
            param: pedidoBebida);
    }

    public async Task InserirPedidoPrato(PedidoPrato pedidoPrato)
    {
        await _connection.ExecuteAsync(
            sql: $@"
                   INSERT INTO
                        {nameof(PedidoPrato)}(
	                       {nameof(PedidoPrato.IdPedido)},
	                       {nameof(PedidoPrato.Nome)},
	                       {nameof(PedidoPrato.Valor)},
	                       {nameof(PedidoPrato.Quantidade)}
                       )
                       VALUES(
	                       @{nameof(PedidoPrato.IdPedido)},
	                       @{nameof(PedidoPrato.Nome)},
	                       @{nameof(PedidoPrato.Valor)}            ,
	                       @{nameof(PedidoPrato.Quantidade)}                                                   
                       );
                ",
            param: pedidoPrato);
    }

    #endregion INSERT

    public async Task AlterarPedido(Pedido pedido)
    {
        await _connection.ExecuteAsync(
            sql: $@"
                  UPDATE
                     {nameof(Pedido)}
                  SET     
	                 {nameof(Pedido.BebidasProntas)} = @{nameof(Pedido.BebidasProntas)},
	                 {nameof(Pedido.PratosProntos)} = @{nameof(Pedido.PratosProntos)},
	                 {nameof(Pedido.PedidoConcluido)} = @{nameof(Pedido.PedidoConcluido)} 
                  WHERE
                     {nameof(Pedido.Id)} = @{nameof(Pedido.Id)};
            ",
            param: pedido);
    }

    public async Task AtualizarBebidaPronta(int pedidoId)
    {
        await _connection.ExecuteAsync(
            sql: $@"
                  UPDATE
                     {nameof(Pedido)}
                  SET     
	                 {nameof(Pedido.BebidasProntas)} = 1
                  WHERE
                     {nameof(Pedido.Id)} = @PEDIDO_ID;
            ",
            param: new { PEDIDO_ID = pedidoId });
    }

    public async Task AtualizarPratosProntos(int pedidoId)
    {
        await _connection.ExecuteAsync(
            sql: $@"
                  UPDATE
                     {nameof(Pedido)}
                  SET     
	                 {nameof(Pedido.PratosProntos)} = 1
                  WHERE
                     {nameof(Pedido.Id)} = @PEDIDO_ID;
            ",
            param: new { PEDIDO_ID = pedidoId });
    }

    public async Task DeletarPedido(int id)
    {
        await _connection.QueryAsync<Pedido>(
            sql: $@"
                  DELETE FROM
                     {nameof(Pedido)}
                  WHERE
                      {nameof(Pedido.Id)} = @ID;",
            param: new
            {
                ID = id
            });
    }
}
