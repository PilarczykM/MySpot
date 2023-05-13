using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Services;
using MySpot.Core.Abstractions;

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
