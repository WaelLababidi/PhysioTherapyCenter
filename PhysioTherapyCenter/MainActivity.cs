using System;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using PhysioTherapyCenter.Models;

namespace PhysioTherapyCenter
{
    //[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    [Activity(Label = "Dashboard", Theme = "@style/AppTheme.NoActionBar")]
    public class MainActivity : BaseDrawerActivity
    {
        //protected override void OnCreate(Bundle savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);
        //    SetContentView(Resource.Layout.activity_main);

        //    Init();

        //    navigationView.SetCheckedItem(Resource.Id.nav_home);

        //}

        public static readonly int PickImageId = 1000;
        private ImageView _imageView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Init();

            navigationView.SetCheckedItem(Resource.Id.nav_home);

            //_imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            //Button button = FindViewById<Button>(Resource.Id.MyButton);
            //button.Click += ButtonOnClick;
        }
        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
            {

                _imageView.SetImageURI(data.Data);
            }
        }

    }
}

