using System;
using System.Collections.Generic;
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
using PhysioTherapyCenter.Models;
using PhysioTherapyCenter.Models.Adapters;
using PhysioTherapyCenter.Models.ViewModels;
using SharedEntities;

namespace PhysioTherapyCenter
{
    //[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    [Activity(Label = "Calendar", Theme = "@style/AppTheme.NoActionBar")]
    public class CalendarActivity : BaseDrawerActivity
    {

        private List<AppointmentViewModel> Appointments;
        protected CalendarView Calendar;
        protected ListView appointments_list_view;
        private DateTime SelectedDate;

        protected CalendarAppointmentsAdapter calendarAppointmentsAdapter;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.calendar);

            Init();

            navigationView.SetCheckedItem(Resource.Id.nav_calendar);

            Calendar = FindViewById<CalendarView>(Resource.Id.calendar_view);
            Calendar.DateChange += Calendar_DateChange;
            appointments_list_view = FindViewById<ListView>(Resource.Id.appointments_list_View);

            appointments_list_view.ItemClick += appointments_list_view_ItemClick;
            SelectedDate = DateTime.Now;

            await LoadDailyAppointment(SelectedDate);



            FloatingActionButton create_new_appointment = FindViewById<FloatingActionButton>(Resource.Id.create_new_appointment_btn);
            create_new_appointment.Click += create_new_appointment_btnOnClick;


        }

        private void appointments_list_view_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            View listItem = (View)sender;
            var height = listItem.MeasuredHeight;

            var Appointment = calendarAppointmentsAdapter[e.Position];
            var intent = new Intent(this, typeof(EditAppointmentActivity));
            intent.PutExtra("AppointmentId", Appointment.AppointmentId);
            StartActivity(intent);
            Finish();

        }

        private void create_new_appointment_btnOnClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CreateAppointmentActivity));

            intent.PutExtra("AppointmentDate", SelectedDate.ToString("dd/MM/yyyy"));

            StartActivity(intent);
            Finish();
        }

        private async Task LoadDailyAppointment(DateTime Today)
        {

            Appointments = new List<AppointmentViewModel>();
            var progressDialog = new ProgressDialog(this);
            progressDialog.SetCancelable(false);
            progressDialog.SetMessage("Loading Data...");
            progressDialog.Show();

            var url = string.Format("{0}/api/appointments?DateFrom={1}&DateTo={2}", Constants.Server_Url, Today, Today);

            var result = await Constants.RestApiSerive.GetResponse<List<AppointmentViewModel>>(url);

            if (result != null)
            {
                result.ForEach(x => { Appointments.Add(x); });
            }

            calendarAppointmentsAdapter = new CalendarAppointmentsAdapter(this, Appointments);
            appointments_list_view.Adapter = calendarAppointmentsAdapter;

            int height = 250;
            appointments_list_view.LayoutParameters.Height = (Appointments.Count * height) + height * 2;
            progressDialog.Hide();
        }

        private async void Calendar_DateChange(object sender, CalendarView.DateChangeEventArgs e)
        {

            SelectedDate = new DateTime(e.Year, e.Month + 1, e.DayOfMonth);
            //var SelectedDate = new DateTime(2020, 12, 8);

            await LoadDailyAppointment(SelectedDate);

            //Toast.MakeText(this, SelectedDate.ToString(), ToastLength.Long).Show();

        }


        //public int GetItemHeightofListView(ListView listView)
        //{

        //    var adapter = listView.Adapter;


        //    int grossElementHeight = 0;
        //    if (adapter != null)
        //    {
        //        int totalHeight = 0;
        //        for (int i = 0; i < adapter.Count; i++)
        //        {
        //            View listItem = adapter.GetView(i, null, null);
        //            listItem.Measure(0, 0);
        //            totalHeight += listItem.Height;
        //        }
        //    }
        //    return grossElementHeight;
        //}
    }
}

