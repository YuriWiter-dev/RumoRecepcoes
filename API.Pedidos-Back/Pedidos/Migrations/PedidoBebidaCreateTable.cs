using DTI.Pedidos.Entidades;
using FluentMigrator;

namespace DTI.Pedidos.Migrations;

[Migration(2024_03_03_01)]
public class PedidoBebidaCreateTable : Migration
{
    public override void Up()
    {
        Create.Table(nameof(PedidoBebida))
            .WithColumn(nameof(PedidoBebida.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(PedidoBebida.IdPedido)).AsInt32()
            .WithColumn(nameof(PedidoBebida.Nome)).AsString(150)
            .WithColumn(nameof(PedidoBebida.Valor)).AsDecimal()
            .WithColumn(nameof(PedidoBebida.Quantidade)).AsInt32();
    }

    public override void Down()
    {
    }
}
