using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;

namespace Labb3Extra.Managers
{
    internal class NavigationManager
    {
        public event Action CurrentViewModelChanged;

        private ObservableObject _currentViewModel;

        public ObservableObject CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}