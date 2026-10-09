using loja_pedidos_api.Data.Repositories;
using loja_pedidos_api.Services;

namespace loja_pedidos_api.Extensions
{
    public static class PedidosExtension
    {
        public static IServiceCollection AddPedidosExtension(this IServiceCollection services) 
        {
            services.AddScoped<IPedidosRepository, PedidosRepository>();
            services.AddTransient<IPedidosService, PedidosService>();
            return services;
        }
    }
}
