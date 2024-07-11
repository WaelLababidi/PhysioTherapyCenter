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
    public class StringAdapter : BaseAdapter<string>
    {


        private List<string> _Items;
        private Context _Context;


        public StringAdapter(Context Context, List<string> Items)
        {
            _Items = Items;
            _Context = Context;
        }


        public override string this[int position] => _Items[position];


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
                row = LayoutInflater.From(_Context).Inflate(Resource.Layout.listview_row_string, null, false);
            }

            TextView textView_text = row.FindViewById<TextView>(Resource.Id.textView_text);
            textView_text.Text = _Items[position];

            return row;
        }
    }
}