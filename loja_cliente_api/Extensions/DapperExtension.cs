using loja_cliente_api.Data.Dapper;

namespace loja_cliente_api.Extensions
{
    public static class DapperExtension
    {
        public static IServiceCollection AddDapperExtension(this IServiceCollection services)
        {
            services.AddScoped<DapperContext>();
            return services;
        }
    }
}
