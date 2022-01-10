using Microsoft.Toolkit.Mvvm.ComponentModel;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Labb3Extra.Model
{
    internal class Product : ObservableObject
    {
        private readonly IMongoDatabase _database;

        public ObjectId Id { get; set; }
        public string NameOfProduct { get; set; }

        public string TypeOfProduct { get; set; }

        public string Image { get; set; }

        public double Price { get; set; }

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