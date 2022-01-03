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
}
