using loja_produtos_api.Data.Dapper;

namespace loja_produtos_api.Extensions
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
