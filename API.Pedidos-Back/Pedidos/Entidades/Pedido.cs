using System;
using System.Collections.Generic;

namespace DTI.Pedidos.Entidades;

public class Pedido
{
    public int Id { get; set; }
    public string NomeSolicitante { get; set; }
    public string Mesa { get; set; }
    public DateTime Data { get; set; }
    public bool BebidasProntas { get; set; }
    public bool PratosProntos { get; set; }
    public bool PedidoConcluido { get; set; }
    public PedidoBebida Bebida { get; set; }
    public PedidoPrato Prato { get; set; }
}
