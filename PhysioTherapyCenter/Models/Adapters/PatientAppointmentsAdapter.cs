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
using PhysioTherapyCenter.Models.ViewModels;
using SharedEntities;

namespace PhysioTherapyCenter.Models.Adapters
{
    public class PatientAppointmentsAdapter : BaseAdapter<AppointmentViewModel>
    {


        private List<AppointmentViewModel> _Items;
        private Context _Context;


        public PatientAppointmentsAdapter(Context Context, List<AppointmentViewModel> Items)
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
                row = LayoutInflater.From(_Context).Inflate(Resource.Layout.listview_row_appointment, null, false);
            }


            TextView textView_appointment_date = row.FindViewById<TextView>(Resource.Id.appointment_date);
            textView_appointment_date.Text = _Items[position].AppointmentDate.Value.ToString("dd/MM/yyyy");

            TextView textView_start_time = row.FindViewById<TextView>(Resource.Id.start_time);
            textView_start_time.Text = _Items[position].StartTime.Value.ToString(@"hh\:mm");

            TextView textView_end_time = row.FindViewById<TextView>(Resource.Id.end_time);
            textView_end_time.Text = _Items[position].EndTime.Value.ToString(@"hh\:mm");

            return row;
        }
    }
}