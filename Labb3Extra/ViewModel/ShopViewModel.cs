using Labb3Extra.Managers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3Extra.ViewModel
{
    class ShopViewModel:ObservableObject
    {
        private NavigationManager _navigationManager;
        private UserManager _userManager = new();

        //Lista Av produkter,Lista av olika produkttyp,
        //Lista av produkter som active user har valt

        //Navigation manager och usermanger

        //Två relay commands 
        //Lägg till i kundvagnen
        //Gå till userprofilevyn

        //Kunstruktor 
        public ShopViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            _navigationManager = navigationManager;
            _userManager = userManager;
            
        }



        //Propertys
        //Isfalse
        //Amount
        //An observablecollection of the items
        //chosen producttype
        //chosenproduct


        //Methods
        //AddProduct to cart
        //LoadProducts into the list 
        //Getthe differnt product types
        
        



    }
}
