using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PhysioTherapyCenter.Models.Entities;
using PhysioTherapyCenter.Models.ViewModels;
using SharedEntities;

namespace PhysioTherapyCenter.Models.Adapters
{
    public class CalendarAppointmentsAdapter : BaseAdapter<AppointmentViewModel>
    {


        private List<AppointmentViewModel> _Items;
        private Context _Context;


        public CalendarAppointmentsAdapter(Context Context, List<AppointmentViewModel> Items)
        {
            _Items = Items;
            _Context = Context;
        }


        public override AppointmentViewModel this[int position] => _Items[position];


        public override int Count => _Items.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(_Context).Inflate(Resource.Layout.listview_row_calendar_appointments, null, false);
            }

            TextView textView_patient_name = row.FindViewById<TextView>(Resource.Id.patient_name);
            textView_patient_name.Text = _Items[position].PatientName;

            TextView textView_start_time = row.FindViewById<TextView>(Resource.Id.start_time);
            textView_start_time.Text = _Items[position].StartTime.Value.ToString(@"hh\:mm");

            TextView textView_end_time = row.FindViewById<TextView>(Resource.Id.end_time);
            textView_end_time.Text = _Items[position].EndTime.Value.ToString(@"hh\:mm");

          
            if (!_Items[position].Confirmed)
            {
                textView_patient_name.SetTextColor(Color.ParseColor("red"));
                textView_start_time.SetTextColor(Color.ParseColor("red"));
                textView_end_time.SetTextColor(Color.ParseColor("red"));
            }

            return row;
        }
    }
}