using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Services;

namespace MySpot.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IClock, Clock>()
                .AddScoped<IReservationsService, ReservationsService>();
            return serviceCollection;
        }
    }
}
