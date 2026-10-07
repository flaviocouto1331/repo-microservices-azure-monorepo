using loja_cliente_api.Data.Repositories;
using loja_cliente_api.Services;

namespace loja_cliente_api.Extensions
{
    public static class ClienteExtesion
    {
        public static IServiceCollection AddClienteExtesion(this IServiceCollection services)
        {
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddTransient<IClienteService, ClienteService>();
            return services;
        }
    }
}
