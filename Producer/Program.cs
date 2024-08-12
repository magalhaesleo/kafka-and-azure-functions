using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration((context, config) =>
    {
        if (context.HostingEnvironment.IsDevelopment())
            config.AddUserSecrets<Program>();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IMongoClient>(new MongoClient(context.Configuration.GetConnectionString("MongoDB")));
        services.AddTransient(provider => provider.GetRequiredService<IMongoClient>().GetDatabase("transactions"));
    })
    .Build();

await host.RunAsync();
