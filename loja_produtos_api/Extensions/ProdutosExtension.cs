using loja_produtos_api.Data.Repositories;
using loja_produtos_api.Services;

namespace loja_produtos_api.Extensions
{
    public static class ProdutosExtension
    {
        public static IServiceCollection AddProdutosExtension(this IServiceCollection services) 
        {
            services.AddScoped<IProdutosRepository, ProdutosRepository>();
            services.AddTransient<IProdutosService, ProdutosService>();
            return services;
        }
    }
}
