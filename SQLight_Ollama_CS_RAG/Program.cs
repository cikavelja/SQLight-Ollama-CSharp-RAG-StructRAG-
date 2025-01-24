using Microsoft.Data.Sqlite;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.Sqlite;

// Ensure SQLite database file exists
const string databasePath = "movies.db";
if (!File.Exists(databasePath))
{
    File.Create(databasePath).Dispose();
}

// Initialize the SQLite vector store with a connection string
using var connection = new SqliteConnection($"Data Source={databasePath};");

// Open the SQLite connection
await connection.OpenAsync();

connection.EnableExtensions(true);
connection.LoadExtension("./extensions/vec0.dll");


// Create the SQLite vector store
var vectorStore = new SqliteVectorStore(connection);

// Ensure the connection is properly initialized
var movies = vectorStore.GetCollection<ulong, MovieVector<ulong>>("movies");
await movies.CreateCollectionIfNotExistsAsync();

var movieData = MovieFactory<ulong>.GetMovieVectorList();

// Get the embeddings generator and generate embeddings for movies
IEmbeddingGenerator<string, Embedding<float>> generator =
    new OllamaEmbeddingGenerator(new Uri("http://localhost:11434/"), "all-minilm:l6-v2");

foreach (var movie in movieData)
{
    movie.Vector = await generator.GenerateEmbeddingVectorAsync(movie.Description);
    Console.WriteLine($"Upserting movie: {movie.Title}");
    await movies.UpsertAsync(movie);
}

// Perform the search
//var query = "A movie about ogres and dragons";
var query = "A movie about lions";

var queryEmbedding = await generator.GenerateEmbeddingVectorAsync(query);
var searchOptions = new VectorSearchOptions()
{
    Top = 2,
    VectorPropertyName = "Vector"
};

var results = await movies.VectorizedSearchAsync(queryEmbedding, searchOptions);
await foreach (var result in results.Results)
{
    Console.WriteLine($"Title: {result.Record.Title}");
    Console.WriteLine($"Description: {result.Record.Description}");
    Console.WriteLine($"Score: {result.Score}");
    Console.WriteLine();
}

// Movie, MovieVector, and MovieFactory classes remain unchanged

public class MovieFactory<T>
{
    public static List<Movie<T>> GetMovieList()
    {
        var movieData = new List<Movie<T>>()
        {
            new Movie<T>
            {
                Key = 0,
                Title = "Lion King",
                Description = "The Lion King is a classic Disney animated film that tells the story of a young lion named Simba who embarks on a journey to reclaim his throne as the king of the Pride Lands after the tragic death of his father."
            },
            new Movie<T>
            {
                Key = 1,
                Title = "Inception",
                Description = "Inception is a science fiction film directed by Christopher Nolan that follows a group of thieves who enter the dreams of their targets to steal information."
            },
            new Movie<T>
            {
                Key = 2,
                Title = "The Matrix",
                Description = "The Matrix is a science fiction film directed by the Wachowskis that follows a computer hacker named Neo who discovers that the world he lives in is a simulated reality created by machines."
            },
            new Movie<T>
            {
                Key = 3,
                Title = "Shrek",
                Description = "Shrek is an animated film that tells the story of an ogre named Shrek who embarks on a quest to rescue Princess Fiona from a dragon and bring her back to the kingdom of Duloc."
            }
        };
        return movieData;
    }

    public static List<MovieVector<T>> GetMovieVectorList()
    {
        var movieData = GetMovieList();
        var movieVectorData = new List<MovieVector<T>>();
        foreach (var movie in movieData)
        {
            movieVectorData.Add(new MovieVector<T>
            {
                Key = movie.Key,
                Title = movie.Title,
                Description = movie.Description
            });
        }
        return movieVectorData;
    }
}

public class Movie<T>
{
    public ulong Key { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }
}

public class MovieVector<T>
{
    [VectorStoreRecordKey]
    public ulong Key { get; set; }

    [VectorStoreRecordData]
    public string Title { get; set; }

    [VectorStoreRecordData]
    public string Description { get; set; }

    [VectorStoreRecordVector(384, DistanceFunction.EuclideanDistance)]
    public ReadOnlyMemory<float> Vector { get; set; }
}
