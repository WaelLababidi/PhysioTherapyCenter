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
    public class RankAdapter : BaseAdapter<RankViewModel>
    {


        private List<RankViewModel> _Items;
        private Context _Context;


        public RankAdapter(Context Context, List<RankViewModel> Items)
        {
            _Items = Items;
            _Context = Context;
        }


        public override RankViewModel this[int position] => _Items[position];


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
                row = LayoutInflater.From(_Context).Inflate(Resource.Layout.listview_row_rank, null, false);
            }

            TextView textView_rank_arabic_name = row.FindViewById<TextView>(Resource.Id.rank_arabic_name);
            textView_rank_arabic_name.Text = _Items[position].ArabicMilitaryRankName;


            return row;
        }
    }
}