using Labb3Extra.Managers;
using Labb3Extra.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

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

        //Constructor
        public AdminViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            _navigationManager = navigationManager;
            _userManager = userManager;
            StartViewCommand = new RelayCommand(GoToStartView);
            AddProductCommand = new RelayCommand(AddProdToDatabase);
            ResetListCommand = new RelayCommand(Resetproducts);
            LoadProdDatabase();
            FilteredProducts = Products;
            GetTypeOfProdfromDatabase();
            ActiveUser = _userManager.ActiveUser;
        }

        public User ActiveUser { get; set; }

        public void GoToStartView()
        {
            _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _userManager);
        }

        //Propertys

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
                    _product = value;
                    OnPropertyChanged(nameof(ChosenProduct));
                    Image = _product.Image;
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
        //restet filter products
        public void Resetproducts()
        {
            FilteredProducts = Products;
            OnPropertyChanged(nameof(FilteredProducts));

        }
        private void FilterProductList()
        {
            FilteredProducts = new (Products.Where(p => p.TypeOfProduct == _chosenProductType));
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

        //methods
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

        //AddProdcuts
        public void AddProdToDatabase()
        {
            _db.InsertNew("Products", new Product { NameOfProduct = NameOfProduct, Price = Price, Count = Count, TypeOfProduct = TypeOfProduct, Image = Image });
            MessageBox.Show("Product Added", "Added", MessageBoxButton.OK);
            Products.Clear();
            LoadProdDatabase();
            emptyBoxes();
        }

        public void emptyBoxes()
        {
            NameOfProduct = null;
            TypeOfProduct = null;
            Count = 0;
            Price = 0;
            Image = null;
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