using Labb3Extra.Managers;
using Labb3Extra.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3Extra.ViewModel
{
    class UserProfileViewModel : ObservableObject
    {
        private readonly NavigationManager _navigationManager;
        private readonly UserManager _userManager;
        //List of the users chosen products
        public ObservableCollection<Product> Products { get; set; } = new();
        public ObservableCollection<Product> ActiveUserCart { get; set; } = new();
        //navigation manager and usermanager
        //Active user to see onl there list of products
        //List of Store

        //Relay commands 
        //Startview,shopview and exit view

        //Kunstruktor

        //Propertys
        //Activeuser and product,selected produkt


        //Methods
        //loadproducts
        //CheckoutActiveUSer
        //showSum

        public UserProfileViewModel(NavigationManager navigationManager, UserManager userManager)
            {
                _navigationManager = navigationManager;
                _userManager = userManager;
            }
    }
}
