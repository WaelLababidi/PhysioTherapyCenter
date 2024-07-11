using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace PhysioTherapyCenter.Models.Entities
{
    public class ApplicationUser
    {
        [PrimaryKey]
        public int Id { get; set; }

        //public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string AccessToken { get; set; }

        public DateTime AccessTokenExpiryDate { get; set; }

        public bool ValidateAccessToken()
        {
            var Today = DateTime.Now;
            if (string.IsNullOrEmpty(AccessToken) || AccessTokenExpiryDate == null || AccessTokenExpiryDate < Today)
                return false;
            return true;
        }

    }
}