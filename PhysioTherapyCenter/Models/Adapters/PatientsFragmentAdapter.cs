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

namespace PhysioTherapyCenter.Models.Adapters
{
    public class PatientsFragmentAdapter : BaseAdapter<PatientsFragmentViewModel>
    {


        private List<PatientsFragmentViewModel> _Items;
        private Context _Context;


        public PatientsFragmentAdapter(Context Context, List<PatientsFragmentViewModel> Items)
        {
            _Items = Items;
            _Context = Context;
        }


        public override PatientsFragmentViewModel this[int position] => _Items[position];


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
                row = LayoutInflater.From(_Context).Inflate(Resource.Layout.listview_row_fragment_patients, null, false);
            }


            ImageView imageView_icon = row.FindViewById<ImageView>(Resource.Id.item_icon);
            imageView_icon.SetImageResource(_Items[position].Icon) ;


            TextView textView_name = row.FindViewById<TextView>(Resource.Id.item_name);
            textView_name.Text = _Items[position].Name;


            return row;
        }
    }
}