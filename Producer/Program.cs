using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IMongoClient, MongoClient>(provider =>
        {
            var connString = context.Configuration.GetConnectionString("MongoDB");
            return new MongoClient(connString);
        });
        services.AddTransient(provider => provider.GetRequiredService<IMongoClient>().GetDatabase("transactions"));
    })
    .Build();

await host.RunAsync();
