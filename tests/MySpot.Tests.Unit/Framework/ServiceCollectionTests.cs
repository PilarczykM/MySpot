using Microsoft.Extensions.DependencyInjection;

namespace MySpot.Tests.Unit.Framework
{
    public class ServiceCollectionTests
    {
        // Dependencies should be registered in the same lifetime.
        // If object depend on other implementation order mather.
        [Fact]
        public void Test_AddTransient()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IMessenger, Messanger>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var messanger1 = serviceProvider.GetRequiredService<IMessenger>();
            messanger1.Send();
            var messanger2 = serviceProvider.GetRequiredService<IMessenger>();
            messanger2.Send();

            messanger1.ShouldNotBeNull();
            messanger2.ShouldNotBeNull();
            messanger1.ShouldNotBe(messanger2);
        }

        [Fact]
        public void Test_AddSingleton()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IMessenger, Messanger>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var messanger1 = serviceProvider.GetRequiredService<IMessenger>();
            messanger1.Send();
            var messanger2 = serviceProvider.GetRequiredService<IMessenger>();
            messanger2.Send();

            messanger1.ShouldNotBeNull();
            messanger2.ShouldNotBeNull();
            messanger1.ShouldBe(messanger2);
        }

        [Fact]
        public void Test_AddScoped()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<IMessenger, Messanger>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            IMessenger messanger1;
            IMessenger messanger2;

            using (var scope = serviceProvider.CreateScope())
            {
                messanger1 = scope.ServiceProvider.GetRequiredService<IMessenger>();
                messanger1.Send();
                messanger2 = scope.ServiceProvider.GetRequiredService<IMessenger>();
                messanger2.Send();

                messanger1.ShouldNotBeNull();
                messanger2.ShouldNotBeNull();
                // In the same scope services are the same.
                messanger1.ShouldBe(messanger2);
            }

            messanger2 = serviceProvider.GetRequiredService<IMessenger>();
            // In different scope services are not the same.
            messanger1.ShouldNotBe(messanger2);
        }

        private interface IMessenger
        {
            void Send();
        }

        private class Messanger : IMessenger
        {
            private readonly Guid _id = Guid.NewGuid();

            public void Send()
            {
                Console.WriteLine($"Sending new message1... [{_id}]");
            }
        }
    }
}
