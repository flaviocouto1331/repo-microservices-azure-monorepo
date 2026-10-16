using loja_pedidos_api.Messaging;

namespace loja_pedidos_api.Extensions
{
    public static class ServiceBusExtension
    {
        public static IServiceCollection AddServiceBusExtension(this IServiceCollection services)
        {
            services.AddScoped<IServiceBus, ServiceBus>();
            return services;
        }
    }
}
