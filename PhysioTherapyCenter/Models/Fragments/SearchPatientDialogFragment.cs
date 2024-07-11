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
    public class SearchPatientDialogFragment : DialogFragment
    {

        private SearchView sv;
        private ListView lv;
        private PatientAdapter adapter;

        private List<PatientViewModel> Items;

        private Action<PatientViewModel> CallBackAction;

        public SearchPatientDialogFragment(List<PatientViewModel> items, Action<PatientViewModel> callBackAction)
        {
            Items = items;
            CallBackAction = callBackAction;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View rootView = inflater.Inflate(Resource.Layout.search_dialog_fragment, container, false);

            this.Dialog.SetTitle("Search...");

            //Items.Add("30000");
            //Items.Add("40000");
            //Items.Add("50000");
            //Items.Add("60000");
            //Items.Add("70000");

            sv = rootView.FindViewById<SearchView>(Resource.Id.sv);
            lv = rootView.FindViewById<ListView>(Resource.Id.lv);

            adapter = new PatientAdapter(this.Activity, Items);
            lv.Adapter = adapter;

            sv.QueryTextChange += sv_QueryTextChange;
            lv.ItemClick += lv_ItemClick;

            return rootView;

        }

        private void lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //View view = (View)sender;
            //Snackbar.Make(view, adapter[e.Position].PatientId, Snackbar.LengthLong)
            //    .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();

            CallBackAction(adapter[e.Position]);

            Dismiss();
        }

        private void sv_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewText))
            {
                adapter = new PatientAdapter(this.Activity, Items.Where(x => x.Name.ToLower().Contains(e.NewText.ToLower())).ToList());
                lv.Adapter = adapter;
            }

            //adapter.Filter.InvokeFilter(e.NewText);
        }
    }
}