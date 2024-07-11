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
    public class ClaimAdapter : BaseAdapter<ClaimInfo>
    {


        private List<ClaimInfo> _Items;
        private Context _Context;


        public ClaimAdapter(Context Context, List<ClaimInfo> Items)
        {
            _Items = Items;
            _Context = Context;
        }


        public override ClaimInfo this[int position] => _Items[position];


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
                row = LayoutInflater.From(_Context).Inflate(Resource.Layout.listview_row_patient_claim, null, false);
            }

            TextView textView_approval_date = row.FindViewById<TextView>(Resource.Id.approval_date);
            textView_approval_date.Text = _Items[position].ApprovalDate.ToString("dd/MM/yyyy");

            TextView textview_claim_type = row.FindViewById<TextView>(Resource.Id.claim_type);
            textview_claim_type.Text = _Items[position].ClaimType;


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