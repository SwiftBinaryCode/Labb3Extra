using Labb3Extra.Managers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3Extra.ViewModel
{
    class MainViewModel: ObservableObject
    {
        private readonly NavigationManager _navigationManager;
        private readonly UserManager _userManager;

        public ObservableObject CurrentViewModel => _navigationManager.CurrentViewModel;


        public MainViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            _navigationManager = navigationManager;
            _userManager = userManager;
            StartViewCommand = new RelayCommand(() => { _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _userManager); });
            UserProfileViewCommand = new RelayCommand(() => { _navigationManager.CurrentViewModel = new UserProfileViewModel(_navigationManager, _userManager); });
            AdminViewCommand = new RelayCommand(() => { _navigationManager.CurrentViewModel = new AdminViewModel(_navigationManager, _userManager); });
            ShopViewCommand = new RelayCommand(() => { _navigationManager.CurrentViewModel = new ShopViewModel(_navigationManager, _userManager); });

            _navigationManager.CurrentViewModelChanged += CurrentViewModelChanged;
        }

        public RelayCommand StartViewCommand { get; }
        public RelayCommand UserProfileViewCommand { get; }
        public RelayCommand AdminViewCommand { get; }
        public RelayCommand ShopViewCommand { get; }

        private void CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
