using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.Repositories;

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
            serviceCollection.AddSingleton<
                IWeeklyParkingSpotRepository,
                InMemoryWeeklyParkingSpotRepository
            >();
            return serviceCollection;
        }
    }
}
