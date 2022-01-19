using Labb3Extra.Managers;
using Labb3Extra.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Labb3Extra.ViewModel
{
    internal class AdminViewModel : ObservableObject
    {
        private NavigationManager _navigationManager;
        private UserManager _userManager;
        private IMongoDatabase _database;
        private Product _product;
        private readonly Managers.MongoDB _db = new("Store");
        public ObservableCollection<string> TypeOfProducts { get; set; } = new();
        public ObservableCollection<Product> Products { get; set; } = new();
        public ObservableCollection<Product> FilteredProducts { get; set; }

        public RelayCommand StartViewCommand { get; }
        public RelayCommand AddProductCommand { get; }
        public RelayCommand ResetListCommand { get; }

        public RelayCommand EmptyTextBoxCommand { get; }





        public AdminViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            _navigationManager = navigationManager;
            _userManager = userManager;
            StartViewCommand = new RelayCommand(GoToStartView);
            AddProductCommand = new RelayCommand(AddProdToDatabase);
            ResetListCommand = new RelayCommand(Resetproducts);
            EmptyTextBoxCommand = new RelayCommand(EmptyBoxes);


            LoadProdDatabase();
            FilteredProducts = Products;
            GetTypeOfProdfromDatabase();
            ActiveUser = _userManager.ActiveUser;
        }

        public User ActiveUser { get; set; }

        private string _nameOfProduct;

        public string NameOfProduct
        {
            get => _nameOfProduct; set => SetProperty(ref _nameOfProduct, value);
        }

        public Product ChosenProduct
        {
            get => _product;
            set
            {
                if (_product != value)
                {
                    _product = value ?? new Product();

                    OnPropertyChanged(nameof(ChosenProduct));

                    //Gör det möjligt för admin att trycka på en produkt i listan och få fram informationen i textboxarna.Ifall det är svårt att läsa information i listan om varje produkt.
                    //Sedan kan hen trycka på clear fields för att tömma fälten igen.
                    NameOfProduct = ChosenProduct.NameOfProduct;
                    Price = ChosenProduct.Price;
                    TypeOfProduct = ChosenProduct.TypeOfProduct;
                    Image = ChosenProduct.Image;
                    Count = ChosenProduct.Count;
                    
                }
            }
        }

        private int _price;

        public int Price
        {
            get => _price; set => SetProperty(ref _price, value);
        }

        private string _typeOfProduct;

        public string TypeOfProduct
        {
            get => _typeOfProduct; set => SetProperty(ref _typeOfProduct, value);
        }

        private string _chosenProductType;

        public string ChosenProductType
        {
            get => _chosenProductType;
            set
            {
                SetProperty(ref _chosenProductType, value);
                FilterProductList();
                OnPropertyChanged(nameof(FilteredProducts));
            }
        }

        private int _priceTotal;

        public int PriceTotal
        {
            get => _priceTotal;
            set => SetProperty(ref _priceTotal, value);

          
        }


        private int _count;

        public int Count
        {
            get => _count; set => SetProperty(ref _count, value);
        }

        private string _image;

        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public void GoToStartView()
        {
            _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _userManager);
        }

        
        public void Resetproducts()
        {
            FilteredProducts = Products;
            OnPropertyChanged(nameof(FilteredProducts));
        }

        private void FilterProductList()
        {
            FilteredProducts = new(Products.Where(p => p.TypeOfProduct == _chosenProductType));
        }

        //Hämtar dem olika produkterna från mongo databasen.
        public void LoadProdDatabase()
        {
            var db = new MongoClient();
            _database = db.GetDatabase("Store");
            var collection = _database.GetCollection<Product>("Products").AsQueryable().ToList();

            foreach (var product in collection)
            {

                Products.Add(product);
               
            }

        }

       

        //Lägger till produkter i mongo databasen, och hämtar den nya produkttypen man har lagt till så att man slipper logga in och ut.
        public void AddProdToDatabase()
        {
            if (String.IsNullOrWhiteSpace(NameOfProduct) || String.IsNullOrWhiteSpace(TypeOfProduct) || Price == 0 || Count == 0)
                
            {
                MessageBox.Show("Please enter values", "Error", MessageBoxButton.OK);
                EmptyBoxes();
            }

            PriceTotal = Price * Count;

            _db.InsertNew("Products", new Product { NameOfProduct = NameOfProduct, Price = Price, Count = Count, TypeOfProduct = TypeOfProduct, Image = Image, PriceTotal = PriceTotal });
            MessageBox.Show("Product Added", "Added", MessageBoxButton.OK);
           

            Products.Clear();
            LoadProdDatabase();
            GetTypeOfProdfromDatabase();
            EmptyBoxes();

        }

        //Rensar fälten i vyn
        public void EmptyBoxes()
        {
            NameOfProduct = null;
            TypeOfProduct = null;
            Count = 0;
            Price = 0;
            PriceTotal = 0;
            Image = null;
        }

        //Hämtar dem olika produkt typerna från mongo databasen.
        public void GetTypeOfProdfromDatabase()
        {
            var db = new MongoClient();
            _database = db.GetDatabase("Store");
            var collection = _database.GetCollection<Product>("Products");
            var typeofprod = collection.AsQueryable().Select(p => p.TypeOfProduct).Distinct();

            foreach (var items in typeofprod)
            {
                if (TypeOfProduct == TypeOfProduct)
                {
                    TypeOfProducts.Remove(items);
                }

                TypeOfProducts.Add(items);

            }

        }
    }
}