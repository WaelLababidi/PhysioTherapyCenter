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
    public class GuarantorAdapter : BaseAdapter<GuarantorViewModel>
    {


        private List<GuarantorViewModel> _Items;
        private Context _Context;


        public GuarantorAdapter(Context Context, List<GuarantorViewModel> Items)
        {
            _Items = Items;
            _Context = Context;
        }


        public override GuarantorViewModel this[int position] => _Items[position];


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
                row = LayoutInflater.From(_Context).Inflate(Resource.Layout.listview_row_guarantor, null, false);
            }

            TextView textView_guarantor_name = row.FindViewById<TextView>(Resource.Id.textView_guarantor_name);
            textView_guarantor_name.Text = _Items[position].Text;


            return row;
        }
    }
}