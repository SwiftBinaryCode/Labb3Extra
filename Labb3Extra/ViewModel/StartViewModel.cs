using Labb3Extra.Managers;
using Labb3Extra.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MongoDB.Driver;
using System;
using System.Windows;

namespace Labb3Extra.ViewModel
{
    class StartViewModel : ObservableObject
    {
        //propertys  för att navigera och få activeuser
        private readonly NavigationManager _navigationManager;
        private readonly UserManager _userManager;
        private readonly IMongoDatabase _database;
        private readonly Managers.MongoDB _db = new("Store");



        //RelayCommands
        public RelayCommand AddUserCommand { get; }
        public RelayCommand LogInCommand { get; }
        public StartViewModel(NavigationManager navigationManager, UserManager userManager)
        {
            _navigationManager = navigationManager;
            _userManager = userManager;
            ActiveUser = _userManager.ActiveUser;
            AddUserCommand = new RelayCommand(RegisterNewUser);

            var dbClient = new MongoClient();
            _database = dbClient.GetDatabase("Store");


            _database.GetCollection<Managers.MongoDB>("Users");
            _database.GetCollection<Managers.MongoDB>("Admin");

        }

        public void GoToUserProfile()
        {
            _navigationManager.CurrentViewModel = new UserProfileViewModel(_navigationManager, _userManager);
        }

        private void GoToAdminView()
        {
            _navigationManager.CurrentViewModel = new AdminViewModel(_navigationManager, _userManager);
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

        //Metoder
        public void RegisterNewUser()
        {
            //while(true)
            //{
            //    if (String.IsNullOrWhiteSpace(NewUsername) || String.IsNullOrWhiteSpace(NewUserPassword))
            //    {
            //        MessageBox.Show("This field cant be empty pleae try again", "Woops", MessageBoxButton.OK);
            //        return;
            //    }
            //    break;

            //};


            _db.InsertNew("Users", new User { Username = NewUsername, Password = NewUserPassword });
            MessageBox.Show("User Created", "Congrats", MessageBoxButton.OK);
            EmptyLogIn();







        }
        public void EmptyLogIn()
        {


            NewUsername = null;
            NewUserPassword = null;
            Username = null;
            Password = null;
        }

        public void CheckRegisteredUsers(User activeUser)
        {
            //Get collection of users from database
            var collection = _database.GetCollection<User>("Users");
            bool findUser = collection.Find(s => s.Username == Username).Any();
            //Check if any of the usernames exist
            if (findUser)
            {
                ActiveUser = collection.Find(s => s.Username == Username).Single();


                if (Password == _userManager.ActiveUser.Password)
                {
                    MessageBox.Show("Welcome {Username}", "Welcome", MessageBoxButton.OK);
                    _userManager.ActiveUser = ActiveUser;
                    GoToUserProfile();
                    return;

                }
                while (Password != _userManager.ActiveUser.Password)
                {
                    MessageBox.Show("Opps", "Wrong Password", MessageBoxButton.OK);
                    EmptyLogIn();


                }
            }
        }
        public void AdminLogIn()
        {
            //Get the collection of admin users from the database
            var collection = _database.GetCollection<User>("Admin");
            //var AdminFound =



            //create a variable the checks if the admin exits
            //bool Find = adminCollection.Find(a => a.Username == Username).Any();
            //If statment that checks the og in and if its succesful they go to the admin view

            //if (AdminFound)
            //{
            //    MessageBox.Show($"You have logged in as {Username}", "Welcome Admin", MessageBoxButton.OK);
            //    Username = null;
            //    Password = null;
            //    GoToAdminView();
            //}

            EmptyLogIn();

        }



        //public void LogInCheck()
        //{
        //    //get both admin and users from the database
        //    var collection = _database.GetCollection<User>("Users");
        //    var foundUser = ;
        //    var Adminlocator = _database.GetCollection<User>("Admin");
        //    var foundAdmin =
        //    //If tatment to check if any of the users exits if not show a message

        //    if (!foundUser && !foundAdmin)
        //    {
        //        MessageBox.Show(
        //         "Sorry incorrect log in details try again.", "Error",
        //         MessageBoxButton.OK);

        //        EmptyLogIn();
        //    }


        //}

    }
}
