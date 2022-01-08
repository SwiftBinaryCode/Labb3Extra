using Microsoft.Toolkit.Mvvm.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.ObjectModel;

namespace Labb3Extra.Model
{
    internal class User : ObservableObject
    {
        //Propertys
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement]
        public string Username { get; set; }

        [BsonElement]
        public string Password { get; set; }

        [BsonElement]
        public ObservableCollection<Product> Cart { get; set; }

        public User()

        {
            Cart = new ObservableCollection<Product>();
        }

        public override string ToString()
        {
            return $"{Username}";
        }
    }
}