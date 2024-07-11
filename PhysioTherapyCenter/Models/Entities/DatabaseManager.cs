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
    public class DatabaseManager
    {
        static object locker = new object();

        SQLiteConnection database;

        public DatabaseManager()
        {
            var sqLiteAndroid = new SQLiteAndroid();

            database = sqLiteAndroid.GetConnection();

            if (database != null)
            {


                var tables = database.TableMappings.ToList();


                if (!tables.Any(x => x.TableName == "ApplicationUser"))
                {
                    database.CreateTable<ApplicationUser>();
                }
            }

        }

        public ApplicationUser GetUser(string Email, string Password)
        {
            lock (locker)
            {
                if (database.Table<ApplicationUser>().Count() == 0)
                {
                    return null;
                }

                var users = database.Table<ApplicationUser>().ToList();
                return users.Where(x => x.Email.Equals(Email, StringComparison.OrdinalIgnoreCase) && x.Password.Equals(Password))
                   .FirstOrDefault();
            }
        }

        public ApplicationUser GetUserById(int id)
        {
            lock (locker)
            {
                if (database.Table<ApplicationUser>().Count() == 0)
                {
                    return null;
                }

                return database.Table<ApplicationUser>().Where(x => x.Id == id).FirstOrDefault();
            }
        }


        public ApplicationUser GetDefaultUser()
        {
            lock (locker)
            {
                if (database.Table<ApplicationUser>().Count() == 0)
                {
                    return null;
                }

                var users = database.Table<ApplicationUser>().ToList();
                return users.FirstOrDefault();
            }
        }

        public ApplicationUser GetUser(string Email)
        {
            lock (locker)
            {
                if (database.Table<ApplicationUser>().Count() == 0)
                {
                    return null;
                }

                var users = database.Table<ApplicationUser>().ToList();
                return users
                    .Where(x => x.Email.Equals(Email, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
            }
        }

        public int InsertUser(ApplicationUser entity)
        {
            lock (locker)
            {
                return database.Insert(entity);
            }
        }

        public int UpdateUser(ApplicationUser entity)
        {
            lock (locker)
            {
                return database.Update(entity);
            }
        }

    }
}