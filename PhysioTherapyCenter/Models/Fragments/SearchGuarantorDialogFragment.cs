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
    public class SearchGuarantorDialogFragment : DialogFragment
    {

        private SearchView sv;
        private ListView lv;
        private GuarantorAdapter adapter;

        private List<GuarantorViewModel> Items;

        private Action<GuarantorViewModel> CallBackAction;

        public SearchGuarantorDialogFragment(List<GuarantorViewModel> items, Action<GuarantorViewModel> callBackAction)
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

            adapter = new GuarantorAdapter(this.Activity, Items);
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
                adapter = new GuarantorAdapter(this.Activity, Items.Where(x => x.Text.ToLower().Contains(e.NewText.ToLower())).ToList());
                lv.Adapter = adapter;
            }

            //adapter.Filter.InvokeFilter(e.NewText);
        }
    }
}