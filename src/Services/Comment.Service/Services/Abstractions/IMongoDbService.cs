using MongoDB.Driver;

namespace Comment.Service.Services.Abstractions
{
    public interface IMongoDbService
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
        IMongoDatabase GetDatabase(string databaseName);

    }
}
