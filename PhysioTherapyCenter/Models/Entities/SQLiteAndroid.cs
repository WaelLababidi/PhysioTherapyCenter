using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PhysioTherapyCenter.Models.Entities;
using SQLite;



namespace PhysioTherapyCenter.Models.Entities
{
    public class SQLiteAndroid : ISQLite
    {


        public SQLiteAndroid()
        {

        }
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "PhysioTherapyCenter.db3";

            var documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);


            var path = Path.Combine(documentPath, sqliteFilename);

            return new SQLiteConnection(path);
        }
    }
}