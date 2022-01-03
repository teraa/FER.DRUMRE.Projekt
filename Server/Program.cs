using HotChocolate.Data;
using MongoDB.Bson;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

var mongoOptions = builder.Configuration.GetOptions<MongoOptions>();

builder.Services
    .AddMongo(mongoOptions.Uri, mongoOptions.DbName)
    .AddMongoCollection<Book>()
    // GQL
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddMutationConventions()
    .AddMongoDbFiltering()
    .AddMongoDbSorting()
    .AddMongoDbProjections()
    .AddMongoDbPagingProviders()
    .BindRuntimeType<ObjectId, IdType>()
    .AddTypeConverter<string, ObjectId>(x => ObjectId.Parse(x))
    .AddTypeConverter<ObjectId, string>(x => x.ToString());

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.MapGraphQL();
app.Run();

public class Query
{
    // [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IExecutable<Book> GetBooks([Service] IMongoCollection<Book> collection)
        => collection.AsExecutable();

    [UseFirstOrDefault]
    public IExecutable<Book> GetBookById(
        [Service] IMongoCollection<Book> collection,
        ObjectId id)
        => collection.Find(x => x.Id == id).AsExecutable();
}

public class Mutation
{
    public Book AddBook(
        [Service] IMongoCollection<Book> collection,
        BookInput book)
    {
        var doc = new Book
        {
            Author = book.Author,
            Title = book.Title,
        };

        collection.InsertOne(doc);
        return doc;
    }

    public Book UpdateBook(
        [Service] IMongoCollection<Book> collection,
        ObjectId id,
        BookInput book)
    {
        var doc = new Book
        {
            Id = id,
            Author = book.Author,
            Title = book.Title,
        };

        collection.ReplaceOne(x => x.Id == id, doc);
        return doc;
    }
}
