using MongoDB.Driver;

public static class Extensions
{
    public static IServiceCollection AddMongo(this IServiceCollection services, string connectionString, string dbName)
    {
        var client = new MongoClient(connectionString);
        services.AddSingleton<IMongoClient>(client);
        services.AddScoped<IMongoDatabase>(s => client.GetDatabase(dbName));

        return services;
    }

    public static IServiceCollection AddMongoCollection<TCollection>(this IServiceCollection services)
    {
        return services.AddScoped<IMongoCollection<TCollection>>(sp =>
        {
            return sp.GetRequiredService<IMongoDatabase>().GetCollection<TCollection>(typeof(TCollection).Name);
        });
    }

    public static TOptions GetOptions<TOptions>(this IConfiguration configuration)
    {
        const string suffix = "Options";

        string name = typeof(TOptions).Name;
        int idx = name.LastIndexOf(suffix);

        if (name.Length - idx == suffix.Length)
            name = name[..idx];

        return configuration.GetRequiredSection(name).Get<TOptions>();
    }
}
