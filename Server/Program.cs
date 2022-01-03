var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddMutationConventions()
    ;

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.MapGraphQL();
app.Run();

public static class Data
{
    public static List<Book> Books = new()
    {
        new("Title 1", "Author 1"),
        new("Title 2", "Author 2")
    };
}


public record Book(string Title, string Author);

public class Query
{
    public IEnumerable<Book> GetBooks()
        => Data.Books;
    public Book GetBookById(int id)
        => Data.Books[id];
}

public class Mutation
{
    public int AddBook(Book input)
    {
        Data.Books.Add(input);
        return Data.Books.Count - 1;
    }

    public Book UpdateBook(int id, Book book)
    {
        return Data.Books[id] = book;
    }
}
