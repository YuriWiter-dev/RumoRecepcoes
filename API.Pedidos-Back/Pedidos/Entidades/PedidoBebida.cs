namespace DTI.Pedidos.Entidades
{
    public class PedidoBebida
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public decimal Quantidade { get; set; }
    }
}
