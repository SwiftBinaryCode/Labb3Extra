using Microsoft.Toolkit.Mvvm.ComponentModel;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3Extra.Model
{
    class User:ObservableObject
    {
        //Propertys
        public ObjectId Id { get; set;}
        public string Username { get; set; }

        public string Password { get; set; }

        public ObservableCollection<Product> Cart { get; set; }

        public User()

        {

            Cart = new ObservableCollection<Product>();

        }
        public override string ToString()
        {
            return $"Du är inloggad som: {Username}";
        }



    }
}
