using Labb3Extra.Managers;
using Labb3Extra.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDB.Driver;
using System;
using System.Collections.ObjectModel;

namespace Labb3Extra.ViewModel
{
    internal class UserProfileViewModel : ObservableObject
    {
        private readonly NavigationManager _navigationManager;
        private readonly UserManager _userManager;
        private Store _store = new();
        private readonly Managers.MongoDB _db = new("Store");
        private IMongoDatabase _database;

        //List of the users chosen products
        public ObservableCollection<Product> Products { get; set; } = new();

        public ObservableCollection<Product> ActiveUserCart { get; set; } = new();

        public UserProfileViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            _navigationManager = navigationManager;
            _userManager = userManager;
            _activeUser = _userManager.ActiveUser;

            StartViewCommand = new RelayCommand(() => GoToStartView());
            ShopViewCommand = new RelayCommand(() => GoToShopView());
        }

        public RelayCommand StartViewCommand { get; }
        public RelayCommand ShopViewCommand { get; }
        public RelayCommand CheckOutCommand { get; }
        //Relay commands
        //Startview,shopview and exit view

        public void GoToStartView()
        {
            _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _userManager);
        }

        public void GoToShopView()
        {
            _navigationManager.CurrentViewModel = new ShopViewModel(_navigationManager, _userManager);
        }

        public async void CheckOut()
        {
            await _store.CheckOutUser(_userManager.ActiveUser);
        }

        //Kunstruktor

        //Propertys
        //Activeuser and product,selected produkt
        private User _activeUser;

        public User ActiveUser
        {
            get => _activeUser;
            set => SetProperty(ref _activeUser, value);
        }

        private Product _selectedProduct;

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; }
        }

        private Double _sum;

        public Double Sum
        {
            get { return _sum; }
            set { _sum = value; }
        }

        public void LoadActiveUserCart()
        {
        }

        public void LoadProdcuts()
        {
        }

        public void SumCount()
        {
        }
    }
}