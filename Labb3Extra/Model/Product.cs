using Microsoft.Toolkit.Mvvm.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Labb3Extra.Model
{
    public class Product : ObservableObject
    {
        private readonly IMongoDatabase _database;

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement]
        public string NameOfProduct { get; set; }

        [BsonElement]
        public string TypeOfProduct { get; set; }

        [BsonElement]
        public string Image { get; set; }

        [BsonElement]
        public int Price { get; set; }

        [BsonElement]
        public int PriceTotal { get; set; }

        [BsonElement]
        public int Count { get; set; }

        public Product()
        {
            var db = new MongoClient();
            _database = db.GetDatabase("Store");
            _database.GetCollection<Managers.MongoDB>("Products");
        }

        public override string ToString()
        {
            return $"{NameOfProduct}";
        }

        public Product Copy()
        {
            var productCopy = new Product();
            productCopy.Id = Id;
            productCopy.NameOfProduct = NameOfProduct;
            productCopy.TypeOfProduct = TypeOfProduct;
            productCopy.Price = Price;
            productCopy.Count = 0;
            productCopy.Image = Image;
            return productCopy;
        }
    }
}