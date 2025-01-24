
# SQLite Retrieval Augmented Generation (RAG) and Vector Search With C# and Ollama

[![Watch the video](https://img.youtube.com/vi/UjUVy0oB9YE/RAG.jpg)](https://youtu.be/UjUVy0oB9YE)


This project demonstrates a simple movie recommendation system using the **Semantic Kernel** and the **SQLite Vector Store** connector. It leverages vector embeddings to store and query movie data based on textual descriptions and performs similarity searches to provide relevant recommendations.

## Features

- **SQLite Vector Store**: Uses SQLite with the vector extension (`vec0`) to store and query movie embeddings.
- **Embedding Generation**: Generates vector embeddings for movie descriptions using the **OllamaEmbeddingGenerator**.
- **Vectorized Search**: Supports similarity-based search to recommend movies based on a user-provided query.
- **C# Implementation**: Fully implemented in C# using the Microsoft Semantic Kernel library.

## Prerequisites

1. .NET 9.0 or later.

2. A SQLite build with the `vec0` module (vector extension) enabled.

3. NuGet Packages:

   - `Microsoft.Data.Sqlite`
   - `Microsoft.Extensions.AI.Ollama`
   - `Microsoft.Extensions.AI`
   - `Microsoft.Extensions.VectorData`
   - `Microsoft.SemanticKernel.Connectors.Sqlite`

4. **Ollama Embedding Generator** running locally (e.g., at `http://localhost:11434/`).

### Installing and Using MiniLM-L6-v2 in Ollama

To install and use the MiniLM-L6-v2 (or all-MiniLM-L6-v2) model in Ollama, follow these steps:

1. **Ensure Ollama is Installed**

   - If Ollama is not installed, download and install it from the Ollama website.
   - Ensure you have the latest version (v0.1.26 or newer) to support MiniLM-L6-v2.

2. **Verify Model Availability**

   - Ollama provides built-in support for embedding models, including MiniLM-L6-v2. The model might be referred to as `all-minilm:l6-v2` in Ollama.
   - Visit Ollama's model library or search using their CLI/API to confirm:
     ```bash
     ollama list
     ```

3. **Install the Model**

   - Use Ollama's CLI command to download and install the model:
     ```bash
     ollama pull all-minilm:l6-v2
     ```
     This command will fetch the all-MiniLM-L6-v2 model into your Ollama environment.

## Setup Instructions

### 1. Clone the Repository

```bash
git clone <repository-url>
cd <repository-folder>
```

### 2. Install Dependencies

Restore NuGet packages:

```bash
dotnet restore
```

### 3. Ensure SQLite Vector Support

- Download and install a SQLite version that includes the `vec0` module.SQLite doesn't support vector search out-of-the-box. The SQLite extension should be loaded first to enable vector search capability. The current implementation of the SQLite connector is compatible with the sqlite-vec vector search extension.

4. Configure the Project

- SQLite doesn't support vector search out-of-the-box. The SQLite extension should be loaded first to enable vector search capability. The current implementation of the SQLite connector is compatible with the sqlite-vec vector search extension.
- Ensure the SQLite database file (`movies.db`) exists, or it will be automatically created on the first run. Update the database connection string in the code if necessary.

### 5. Run the Application

Build and run the application:

```bash
dotnet run
```

## How It Works

1. **Movie Data Initialization**:

   - A list of movies with titles and descriptions is initialized.
   - Vector embeddings are generated for each movie description.

2. **Data Storage**:

   - Embeddings and movie metadata are stored in the SQLite vector store.

3. **Query and Recommendations**:

   - The user provides a query (e.g., "A family-friendly movie with ogres and dragons").
   - The query is converted to an embedding and searched against the stored movie vectors.
   - The top matching movies are retrieved and displayed.

## Example Output

Example query: *"A family-friendly movie with ogres and dragons."*

Sample result:

```
Title: Shrek
Description: Shrek is an animated film that tells the story of an ogre named Shrek who embarks on a quest to rescue Princess Fiona from a dragon and bring her back to the kingdom of Duloc.
Score: 0.95
```

## Troubleshooting

1. **SQLite Error: 'no such module: vec0'**:

   - Ensure you are using a SQLite version with vector extension support.
   - Specify the full path to the `vec0` module using `connection.LoadExtension("path/to/vec0");`.

2. **Embedding Generator Issues**:

   - Verify the **OllamaEmbeddingGenerator** is running and accessible at the specified URL.

## Contributing

Feel free to contribute to this project by submitting issues or pull requests. Suggestions and improvements are always welcome!

## License

This project is licensed under the [MIT License](LICENSE).
