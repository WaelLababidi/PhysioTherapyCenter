using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
using Newtonsoft.Json;
using PhysioTherapyCenter.Models;
using SharedEntities;

namespace PhysioTherapyCenter
{
    //[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class BaseActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        protected NavigationView navigationView;

        //protected override void OnCreate(Bundle savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);
        //    SetContentView(Resource.Layout.activity_main);
        //    Init();
        //}

        protected void Init()
        {
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            //FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            //fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);


            var ApplicationUser = Constants.ApplicationUser.Email;
            View headerView = navigationView.GetHeaderView(0);
            TextView navUsername = (TextView)headerView.FindViewById(Resource.Id.application_user_email);
            navUsername.Text = Constants.ApplicationUser.Email;



        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
                //base.OnBackPressed();
            }
        }

        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.menu_main, menu);
        //    return true;
        //}



        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void create_new_appointment_btnOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }




        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;



            if (id == Resource.Id.nav_home)
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();

            }
            else if (id == Resource.Id.nav_patients)
            {
                //StartActivity(typeof(PatientsActivity));
                //Finish();
                var intent = new Intent(this, typeof(PatientsActivity));
                StartActivity(intent);
                Finish();

            }
            else if (id == Resource.Id.nav_calendar)
            {
                var intent = new Intent(this, typeof(CalendarActivity));
                StartActivity(intent);
                Finish();
            }
            else if (id == Resource.Id.nav_claims)
            {

            }
            else if (id == Resource.Id.nav_reports)
            {

            }
            else if (id == Resource.Id.nav_doctors)
            {

            }
            else if (id == Resource.Id.nav_insurance_company)
            {

            }
            else if (id == Resource.Id.nav_physical_therapists)
            {

            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }




        //---------------------------------------------------------------------------------



        protected DateTime? ConvertToDateTime(string str)
        {
            try
            {
                return DateTime.ParseExact(str, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            }
            catch (Exception)
            {
            }

            return null;
        }

        protected TimeSpan? ConvertToTimespan(string str)
        {
            try
            {
                return TimeSpan.Parse(str, CultureInfo.InvariantCulture);

            }
            catch (Exception)
            {
            }

            return null;
        }

        protected int? ConvertToNumber(string str)
        {
            try
            {
                return Int32.Parse(str);

            }
            catch (Exception)
            {
            }

            return null;
        }

        //---------------------------------------------------------------------------------


        protected async Task<List<PatientViewModel>> Patients(bool GetAll = true, bool INCLUDING_CLAIMS = false)
        {
            var result = new List<PatientViewModel>();
            try
            {

                using (var client = new HttpClient())
                {

                    //string url = "http://localhost:1397/api/patients/?GetAll=true";


                    string url = string.Format("{0}/api/patients/?GetAll={1}&INCLUDING_CLAIMS={2}", Constants.Server_Url, GetAll, INCLUDING_CLAIMS);
                    var uri = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);

                    HttpResponseMessage response;
                    response = await client.GetAsync(uri);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<List<PatientViewModel>>(json);

                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {


            }
            return result;

        }

        protected async Task<List<ClaimInfo>> PatientAppointments(string PatientId)
        {
            var result = new List<ClaimInfo>();
            try
            {

                using (var client = new HttpClient())
                {

                    string url = string.Format("{0}/api/patientappointments/{1}", Constants.Server_Url, PatientId);
                    var uri = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);

                    HttpResponseMessage response;
                    response = await client.GetAsync(uri);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<List<ClaimInfo>>(json);

                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {


            }
            return result;

        }



        protected async Task<AppointmentViewModel> GetAppointment(string AppointmentId)
        {
            AppointmentViewModel result = null;
            try
            {

                using (var client = new HttpClient())
                {

                    string url = string.Format("{0}/api/appointments/{1}", Constants.Server_Url, AppointmentId);
                    var uri = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);

                    HttpResponseMessage response;
                    response = await client.GetAsync(uri);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<AppointmentViewModel>(json);

                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {


            }
            return result;

        }
    }
}

