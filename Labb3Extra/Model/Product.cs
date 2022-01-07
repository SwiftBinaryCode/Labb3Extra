using Microsoft.Toolkit.Mvvm.ComponentModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3Extra.Model
{
    class Product : ObservableObject
    {
       
        private readonly IMongoDatabase _database;

        public ObjectId Id { get; set; }
        public string NameOfProduct { get; set; }

        public string TypeOfProduct { get; set; }

        public double Price { get; set; }

        public int Amount { get; set; }

        public Product()
        {
            var db = new MongoClient();
            _database = db.GetDatabase("Store");
            _database.GetCollection<Managers.MongoDB>("Products");
        }
        //ToStringMethod

    }
}
