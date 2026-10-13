using card_api.Data.Repositories;
using card_api.Service;

namespace card_api.Extensions
{
    public static class ConsultaCartaoExtension
    {
        public static IServiceCollection AddConsultaCartaoExtension(this IServiceCollection services) 
        {
            services.AddScoped<IConsultaCartaoRepository, ConsultaCartaoRepository>();
            services.AddTransient<IConsultaCartaoService, ConsultaCartaoService>();
            return services;
        }
    }
}
