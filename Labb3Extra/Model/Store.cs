using Labb3Extra.Managers;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Labb3Extra.Model
{
    internal class Store
    {
        private UserManager _userManager = new();
        private readonly Managers.MongoDB _db = new("Store");

        public async Task CheckOutUser(User user)
        {
            _userManager.ActiveUser = user;
            user.Cart.Clear();
            await _db.UpsertRecord("Users", _userManager.ActiveUser);
            MessageBox.Show($"Thank You!", "Have a good day", MessageBoxButton.OK);
            Environment.Exit(0);
        }
    }
}