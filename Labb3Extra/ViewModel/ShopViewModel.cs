using Labb3Extra.Managers;
using Labb3Extra.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Labb3Extra.ViewModel
{
    internal class ShopViewModel : ObservableObject
    {
        private NavigationManager _navigationManager;
        private UserManager _userManager = new();
        public ObservableCollection<Product> Products { get; set; } = new();
        public ObservableCollection<string> TypeOfProducts { get; set; } = new();
        public ObservableCollection<Product> FilteredProducts { get; set; }
        public ObservableCollection<Product> ActiveUserCart { get; set; } = new();

        private readonly Managers.MongoDB _db = new("Store");

        private Product _product;
        private IMongoDatabase _database;

        public RelayCommand AddToCartCommand { get; }
        public RelayCommand ResetListCommand { get; }
        public RelayCommand GoToUserProfileCommand { get; }

        //Kunstruktor
        public ShopViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            _navigationManager = navigationManager;
            _userManager = userManager;
            ActiveUserCart = _userManager.ActiveUser.Cart;
            ResetListCommand = new RelayCommand(() => Resetproducts());
            GoToUserProfileCommand = new RelayCommand(() => GoToUserProfile());
            AddToCartCommand = new RelayCommand(() => AddProdToCart());
            FilteredProducts = Products;
            LoadProdDatabase();
            GetTypeOfProdfromDatabase();
            
        }

        public void GoToUserProfile()
        {
            _navigationManager.CurrentViewModel = new UserProfileViewModel(_navigationManager, _userManager);
        }

        private int _count;

        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        private string _image;

        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public Product ChosenProduct
        {
            get => _product;
            set
            {
                SetProperty(ref _product, value);
                if (_product != value)
                { 
                    _product = value;
                    OnPropertyChanged(nameof(ChosenProduct));                  
                    Image = _product.Image; 
                }
            }
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
                OnPropertyChanged(nameof(_product));

              
            }
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

        public void AddProdToCart()
        {
            if (Count == 0 || ChosenProduct == null)
            {
                MessageBox.Show("Please choose an amount you would like to add to your cart", "Error", MessageBoxButton.OK);
            }

            var ProductsFound = ActiveUserCart.FirstOrDefault(p => p.Id == ChosenProduct.Id);

            if (ProductsFound != null)
            {
                MessageBox.Show($"You have added {Count} {ChosenProduct} to your cart");
                _userManager.ActiveUser.Cart = ActiveUserCart;
                ProductsFound.Count += Count;
                _product.Count -= Count;
                _db.UpsertRecord("Users", _userManager.ActiveUser);
                _db.UpsertProduct("Products", ChosenProduct);
                Count = 0;
               

                return;
            }

            var productCopy = ChosenProduct.Copy();
            productCopy.Count = Count;
            ActiveUserCart.Add(productCopy);
            _product.Count -= Count;
            MessageBox.Show($"You have added {Count} {ChosenProduct} to your cart");
            _userManager.ActiveUser.Cart = ActiveUserCart;
            _db.UpsertRecord("Users", _userManager.ActiveUser);
            _db.UpsertProduct("Products", ChosenProduct);
            Count = 0;
            
        }

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

        public void GetTypeOfProdfromDatabase()
        {
            var db = new MongoClient();
            _database = db.GetDatabase("Store");
            var collection = _database.GetCollection<Product>("Products");
            var typeofprod = collection.AsQueryable().Select(p => p.TypeOfProduct).Distinct();

            foreach (var items in typeofprod)
            {
                TypeOfProducts.Add(items);
            }
        }

      
    }
}