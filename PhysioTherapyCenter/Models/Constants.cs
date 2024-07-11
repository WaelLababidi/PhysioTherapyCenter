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
using PhysioTherapyCenter.Models.Entities;

namespace PhysioTherapyCenter.Models
{
    public static class Constants
    {

        private static RestApiService _RestApiSerive;
        private static DatabaseManager _DatabaseManager;


        public static bool isDev = false;

        //public static string Server_Url = isDev ? string.Format("http://192.168.1.104:1397") : string.Format("http://fidalababidiwebapi.smartestsolutions.net");
        public static string Server_Url = isDev ? string.Format("http://192.168.0.2:1205") : string.Format("http://89.17.125.225:6666");

        public static string Login_Url = string.Format("{0}/token", Server_Url);

        public static string Token;
        public static ApplicationUser ApplicationUser;




        public static RestApiService RestApiSerive
        {
            get
            {
                if (_RestApiSerive == null)
                {
                    _RestApiSerive = new RestApiService();
                }
                return _RestApiSerive;
            }
        }

        public static DatabaseManager DatabaseManager
        {
            get
            {
                if (_DatabaseManager == null)
                {
                    _DatabaseManager = new DatabaseManager();
                }
                return _DatabaseManager;
            }
        }

        public static string NewLine { get { return "\n"; } }
    }
}