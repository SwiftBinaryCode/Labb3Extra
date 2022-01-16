using Labb3Extra.Managers;
using Labb3Extra.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDB.Driver;
using System;
using System.Windows;

namespace Labb3Extra.ViewModel
{
    internal class StartViewModel : ObservableObject
    {
        private readonly NavigationManager _navigationManager;
        private readonly UserManager _userManager;
        private readonly IMongoDatabase _database;
        private readonly Managers.MongoDB _db = new("Store");
        public RelayCommand AddUserCommand { get; }
        public RelayCommand LogInCommand

        {
            get
            {
                return new RelayCommand(() =>
                {
                    LogInCheck();
                    CheckRegisteredUsers(ActiveUser);
                    AdminLogIn();
                });
            }
        }

        public StartViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            _navigationManager = navigationManager;
            _userManager = userManager;
            ActiveUser = _userManager.ActiveUser;

            AddUserCommand = new RelayCommand(() => RegisterNewUser());

            var dbClient = new MongoClient();
            _database = dbClient.GetDatabase("Store");
            _database.GetCollection<Managers.MongoDB>("Users");
            _database.GetCollection<Managers.MongoDB>("Admin");
        }

   
        private User ActiveUser { get; set; }

        private string _newUsername;

        public string NewUsername
        {
            get => _newUsername;
            set => SetProperty(ref _newUsername, value);
        }

        private string _newUserPassword;

        public string NewUserPassword
        {
            get => _newUserPassword;
            set => SetProperty(ref _newUserPassword, value);
        }

        private string _username;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password;

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public void GoToUserProfile()
        {
            _navigationManager.CurrentViewModel = new UserProfileViewModel(_navigationManager, _userManager);
        }

        private void GoToAdminView()
        {
            _navigationManager.CurrentViewModel = new AdminViewModel(_navigationManager, _userManager);
        }

        //Registrerar nya användare och kollar om fälten är tomma.
        public void RegisterNewUser()
        {
            if (String.IsNullOrWhiteSpace(NewUsername) || String.IsNullOrWhiteSpace(NewUserPassword))
            {
                MessageBox.Show("Username and password fields are empty", "", MessageBoxButton.OK);
                return;
            }

            _db.InsertNew("Users", new User { Username = NewUsername, Password = NewUserPassword });
            MessageBox.Show("User Created", "Congrats", MessageBoxButton.OK);
            EmptyLogIn();
        }

        //Rensar fälten som finns i vyn.
        public void EmptyLogIn()
        {
            NewUsername = null;
            NewUserPassword = null;
            Username = null;
            Password = null;
        }
        
        //Kollar igenom collectionen av Users i mongo databasen och kollar om användarnamnet finns.
        public void CheckRegisteredUsers(User activeUser)
        {
            var collection = _database.GetCollection<User>("Users");
            bool foundUser = collection.Find(s => s.Username == Username).Any();
            if (foundUser)
            {
                ActiveUser = collection.Find(s => s.Username == Username).Single();
                _userManager.ActiveUser = ActiveUser;

                if (Password == _userManager.ActiveUser.Password)
                {
                    MessageBox.Show($"Welcome {Username}", "Welcome", MessageBoxButton.OK);
                    GoToUserProfile();
                    return;
                }
                else if (Password != _userManager.ActiveUser.Password)

                {
                    MessageBox.Show("Wrong Password", "Try Again", MessageBoxButton.OK);
                    EmptyLogIn();
                }
            }
        }
        
        //Kollar igenom collectionen av Admins i mongo databasen för att se om användarnamnet finns.
        public void AdminLogIn()
        {
            var Admincollection = _database.GetCollection<User>("Admin");
            bool adminFound = Admincollection.Find(a => a.Username == Username).Any();

            if (adminFound)
            {
                MessageBox.Show($"You have logged in as {Username}", "Welcome Admin", MessageBoxButton.OK);
                EmptyLogIn();
                GoToAdminView();
            }
        }

        //Kollar om användarnamnet som används är admin eller en vanlig user.
        public void LogInCheck()
        {
            var collection = _database.GetCollection<User>("Users");
            bool foundUser = collection.Find(s => s.Username == Username).Any();

            var Admincollection = _database.GetCollection<User>("Admin");
            bool adminFound = Admincollection.Find(a => a.Username == Username).Any();

            if (!foundUser && !adminFound)
            {
                MessageBox.Show("Sorry no log in details found", "Error", MessageBoxButton.OK);
                EmptyLogIn();
            }
        }
    }
}