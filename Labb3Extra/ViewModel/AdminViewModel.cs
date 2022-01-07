using Labb3Extra.Managers;
using Labb3Extra.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3Extra.ViewModel
{
    class AdminViewModel:ObservableObject
    {
        private NavigationManager _navigationManager;
        private UserManager _userManager;

        //List of producttypes,list of products
        //Navigation manager and user manager

        //ImongoDatbase
        //Mongodb database 

        //RelayCommands
        //Startview
        //AddProduct
        //remveProduct

        //Constructor
        public AdminViewModel(NavigationManager navigationManager, UserManager userManager)
        {

            _navigationManager = navigationManager;
            _userManager = userManager;
            
        }

        public User ActiveUser { get; set; }


        //Propertys
        //Nameofproduct,producttype,price,amount,chosenproduct
        //listof items,chodenproducttype

        //methods
        //LodProductsInto the listview
        //AddProdcuts
        //DeleteProducts
        //Get the different product types
        //Load Prodcuts types
        //Product filter

    }
}
