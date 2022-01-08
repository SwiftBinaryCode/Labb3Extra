using Labb3Extra.Managers;
using Labb3Extra.ViewModel;
using MongoDB.Driver;
using System.Windows;

namespace Labb3Extra
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationManager _navigationManager;
        private readonly UserManager _userManager;

        public App()
        {
            _navigationManager = new NavigationManager();
            _userManager = new UserManager();
        }

        private IMongoDatabase _database;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _userManager);
            var mainWindow = new MainViewModel(_navigationManager, _userManager);
            var startUpWindow = new MainWindow { DataContext = mainWindow };

            startUpWindow.Show();
        }
    }
}