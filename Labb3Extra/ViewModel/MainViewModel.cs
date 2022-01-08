using Labb3Extra.Managers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Labb3Extra.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        private readonly NavigationManager _navigationManager;
        private readonly UserManager _userManager;
        public ObservableObject CurrentViewModel => _navigationManager.CurrentViewModel;

        public RelayCommand StartViewCommand { get; }
        public RelayCommand UserProfileViewCommand { get; }
        public RelayCommand AdminViewCommand { get; }
        public RelayCommand ShopViewCommand { get; }

        public MainViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            _navigationManager = navigationManager;
            _userManager = userManager;
            _navigationManager.CurrentViewModelChanged += CurrentViewModelChanged;
            StartViewCommand = new RelayCommand(GoToStartView);
            UserProfileViewCommand = new RelayCommand(GoToUserProfile);
            AdminViewCommand = new RelayCommand(GoToAdminView);
            ShopViewCommand = new RelayCommand(GoToShopView);
        }

        public void GoToStartView()
        {
            _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _userManager);
        }

        public void GoToUserProfile()
        {
            _navigationManager.CurrentViewModel = new UserProfileViewModel(_navigationManager, _userManager);
        }

        private void GoToAdminView()
        {
            _navigationManager.CurrentViewModel = new AdminViewModel(_navigationManager, _userManager);
        }

        private void GoToShopView()
        {
            _navigationManager.CurrentViewModel = new ShopViewModel(_navigationManager, _userManager);
        }

        private void CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}