using DTI.Pedidos.Entidades;
using FluentMigrator;

namespace DTI.Pedidos.Migrations;

[Migration(2024_03_03_00)]
public class PedidoCreateTable : Migration
{
    public override void Up()
    {
        Create.Table(nameof(Pedido))
            .WithColumn(nameof(Pedido.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(Pedido.NomeSolicitante)).AsString(150)
            .WithColumn(nameof(Pedido.Mesa)).AsString(150)
            .WithColumn(nameof(Pedido.Data)).AsDateTime()
            .WithColumn(nameof(Pedido.BebidasProntas)).AsBoolean().WithDefaultValue(false)
            .WithColumn(nameof(Pedido.PratosProntos)).AsBoolean().WithDefaultValue(false)
            .WithColumn(nameof(Pedido.PedidoConcluido)).AsBoolean().WithDefaultValue(false);
    }

    public override void Down()
    {
    }
}
