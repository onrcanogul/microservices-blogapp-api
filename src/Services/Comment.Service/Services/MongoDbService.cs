using Comment.Service.Services.Abstractions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace Comment.Service.Services
{
    public class MongoDbService(IConfiguration configuration) : IMongoDbService
    {
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            IMongoDatabase database = GetDatabase();
            return database.GetCollection<T>(collectionName);
        }

        public IMongoDatabase GetDatabase(string databaseName = "CommentDB")
        {
            MongoClient mongoClient = new(connectionString);
            return mongoClient.GetDatabase(databaseName);
        }

        private string connectionString => configuration.GetConnectionString("mongodb")!;
    }
}
