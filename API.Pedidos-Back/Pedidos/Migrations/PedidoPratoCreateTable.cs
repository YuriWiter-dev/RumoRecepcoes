using DTI.Pedidos.Entidades;
using FluentMigrator;

namespace DTI.Pedidos.Migrations;

[Migration(2024_03_03_02)]
public class PedidoPratoCreateTable : Migration
{
    public override void Up()
    {
        Create.Table(nameof(PedidoPrato))
            .WithColumn(nameof(PedidoPrato.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(PedidoPrato.IdPedido)).AsInt32()
            .WithColumn(nameof(PedidoPrato.Nome)).AsString(150)
            .WithColumn(nameof(PedidoPrato.Valor)).AsDecimal()
            .WithColumn(nameof(PedidoPrato.Quantidade)).AsInt32();
    }

    public override void Down()
    {
    }
}
