using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using PhysioTherapyCenter.Models.Adapters;
using SharedEntities;

namespace PhysioTherapyCenter.Models.Fragments
{
    public class SearchClaimDialogFragment : DialogFragment
    {

        private SearchView sv;
        private ListView lv;
        private ClaimAdapter adapter;

        private List<ClaimInfo> Items;

        private Action<ClaimInfo> CallBackAction;

        public SearchClaimDialogFragment(List<ClaimInfo> items, Action<ClaimInfo> callBackAction)
        {
            Items = items;
            CallBackAction = callBackAction;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View rootView = inflater.Inflate(Resource.Layout.search_dialog_fragment, container, false);

            this.Dialog.SetTitle("Search...");

            sv = rootView.FindViewById<SearchView>(Resource.Id.sv);
            lv = rootView.FindViewById<ListView>(Resource.Id.lv);

            adapter = new ClaimAdapter(this.Activity, Items);
            lv.Adapter = adapter;

            sv.QueryTextChange += sv_QueryTextChange;
            lv.ItemClick += lv_ItemClick;

            return rootView;

        }

       

        private void lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            CallBackAction(adapter[e.Position]);

            Dismiss();
        }

        private void sv_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewText))
            {
                adapter = new ClaimAdapter(this.Activity, Items.Where(x => x.ApprovalDate.ToString("dd/MM/yyyy").ToLower().Contains(e.NewText.ToLower())).ToList());
                lv.Adapter = adapter;
            }

            //adapter.Filter.InvokeFilter(e.NewText);
        }
    }
}