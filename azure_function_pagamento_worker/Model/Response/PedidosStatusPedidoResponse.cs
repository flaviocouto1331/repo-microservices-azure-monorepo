namespace azure_function_pagamento_worker.Model.Response
{
    public class PedidosStatusPedidoResponse
    {
        public bool status { get; set; }
        public string message { get; set; } = null!;
        public Guid Id { get; set; }
    }
}
