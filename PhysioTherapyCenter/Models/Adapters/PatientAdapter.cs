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
using SharedEntities;

namespace PhysioTherapyCenter.Models.Adapters
{
    public class PatientAdapter : BaseAdapter<PatientViewModel>
    {


        private List<PatientViewModel> _Items;
        private Context _Context;


        public PatientAdapter(Context Context, List<PatientViewModel> Items)
        {
            _Items = Items;
            _Context = Context;
        }


        public override PatientViewModel this[int position] => _Items[position];


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
                row = LayoutInflater.From(_Context).Inflate(Resource.Layout.listview_row_patient, null, false);
            }

            TextView textView_patient_name = row.FindViewById<TextView>(Resource.Id.patient_name);
            textView_patient_name.Text = _Items[position].Name;

            TextView textView_patient_guarantor = row.FindViewById<TextView>(Resource.Id.patient_guarantor);
            textView_patient_guarantor.Text = _Items[position].GuarantorInstituation;


            return row;
        }


        //public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        //{

        //    View row = convertView;

        //    if (row == null)
        //    {
        //        row = LayoutInflater.From(_Context).Inflate(Resource.Layout.listview_row_patient, null, false);
        //    }

        //    TextView textView_patient_name = row.FindViewById<TextView>(Resource.Id.patient_name);
        //    textView_patient_name.Text = _Items[position].Name;

        //    TextView textView_patient_guarantor = row.FindViewById<TextView>(Resource.Id.patient_guarantor);
        //    textView_patient_guarantor.Text = _Items[position].GuarantorInstituation;


        //    return row;
        //}
    }
}