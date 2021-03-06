using Labb3Extra.Managers;
using Labb3Extra.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Labb3Extra.ViewModel
{
    internal class UserProfileViewModel : ObservableObject
    {
        private readonly NavigationManager _navigationManager;
        private readonly UserManager _userManager;
        private Store _store = new();

        private readonly Managers.MongoDB _db = new("Store");
        private IMongoDatabase _database;
        public ObservableCollection<Product> Products { get; set; } = new();

        public ObservableCollection<Product> ActiveUserCart { get; set; } = new();

        public ObservableCollection<string> TypeOfProducts { get; set; } = new();

        public ObservableCollection<Product> FilteredProducts { get; set; }
        public RelayCommand StartViewCommand { get; }
        public RelayCommand ShopViewCommand { get; }
        public RelayCommand CheckOutCommand { get; }
        public RelayCommand SeeSumCommand { get; }

        public UserProfileViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            _navigationManager = navigationManager;
            _userManager = userManager;
            _activeUser = _userManager.ActiveUser;

            StartViewCommand = new RelayCommand(() => GoToStartView());
            ShopViewCommand = new RelayCommand(() => GoToShopView());
            CheckOutCommand = new RelayCommand(() => CheckOut());
            SeeSumCommand = new RelayCommand(() => SumCount());
            LoadActiveUserCart();
            LoadProdDatabase();
            
        }

   

        private User _activeUser;

        public User ActiveUser
        {
            get => _activeUser;
            set => SetProperty(ref _activeUser, value);
        }

        private Product _product;

        public Product ChosenProduct
        {
            get => _product;
            set
            {
                if (_product != value)
                {
                    _product = value;
                    OnPropertyChanged(nameof(ChosenProduct));
                }
            }
        }

        private string _priceTotal;

        public string PriceTotal
        {
            get => _priceTotal;
            set => SetProperty(ref _priceTotal, value);
        }

        public async Task CheckOut()
        {
            await _store.CheckOutUser(_userManager.ActiveUser);
        }
        public void GoToStartView()
        {
            _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _userManager);
        }

        public void GoToShopView()
        {
            _navigationManager.CurrentViewModel = new ShopViewModel(_navigationManager, _userManager);
        }

        //Räknar ut total summan för produkterna som finns i den aktiva kundens kundvagn.
        public string SumCount()
        {
            double sum = 0;
            foreach (var product in ActiveUserCart)
            {
                sum += product.Count * product.Price;
            }
            PriceTotal = $"{sum} US dollars";
            return PriceTotal;
        }

        //Hämtar den aktiva kundens produkter som dem har lagt i sin kundvagn
        public void LoadActiveUserCart()
        {
            foreach (var product in _userManager.ActiveUser.Cart)
            {
                ActiveUserCart.Add(product);
            }
        }

        //Hämtar dem olika produkterna som finns i mongodatabasen.
        public void LoadProdDatabase()
        {
            var db = new MongoClient();
            _database = db.GetDatabase("Store");
            var productCollection = _database.GetCollection<Product>("Products").AsQueryable().ToList();

            foreach (var product in productCollection)
            {
                Products.Add(product);
            }
        }

    }
}