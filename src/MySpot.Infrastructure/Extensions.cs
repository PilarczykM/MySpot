using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Infrastructure.DAL;

// INFO: Internal class visible for MySpot.Tests.Unit
[assembly: InternalsVisibleTo("MySpot.Tests.Unit")]

namespace MySpot.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection serviceCollection
        )
        {
            serviceCollection.AddPostgres();
            // .AddSingleton<IWeeklyParkingSpotRepository, InMemoryWeeklyParkingSpotRepository>();
            return serviceCollection;
        }
    }
}
