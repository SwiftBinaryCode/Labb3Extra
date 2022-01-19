using Labb3Extra.Model;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Labb3Extra.Managers
{
    internal class MongoDB
    {
        private IMongoDatabase _database;

        public MongoDB(string database)
        {
            var dbClient = new MongoClient();
            _database = dbClient.GetDatabase(database);
        }

        public void InsertNew<T>(string dbCollection, T input)
        {
            var collection = _database.GetCollection<T>(dbCollection);
            collection.InsertOneAsync(input);
        }

        public Task UpsertRecord(string dbCollection, User user)
        {
            var collection = _database.GetCollection<User>(dbCollection);
            var filter = Builders<User>.Filter.Eq("_id", user.Id);
            return collection.ReplaceOneAsync(filter, user, new ReplaceOptions { IsUpsert = true });
        }

        public Task UpsertProduct(string dbCollection, Product product)
        {
            var collection = _database.GetCollection<Product>(dbCollection);
            var filter = Builders<Product>.Filter.Eq("_id", product.Id);
            return collection.ReplaceOneAsync(filter, product, new ReplaceOptions { IsUpsert = true });
        }

    }
}