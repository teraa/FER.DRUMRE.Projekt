#pragma warning disable CS8618
using MongoDB.Bson;

public class Book
{
    public ObjectId Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
}

public record BookInput(string Title, string Author);
