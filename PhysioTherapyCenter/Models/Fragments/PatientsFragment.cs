using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using PhysioTherapyCenter.Models.Adapters;
using PhysioTherapyCenter.Models.ViewModels;

namespace PhysioTherapyCenter.Models.Fragments
{
    public class PatientsFragment : DialogFragment
    {

        private ListView listview;
        private List<PatientsFragmentViewModel> Items;
        private PatientsFragmentAdapter adapter;


        private string patientId;
        private string patient_name;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Items = new List<PatientsFragmentViewModel>
            {
                new PatientsFragmentViewModel{Name="Appointments" , Icon=Resource.Drawable.baseline_calendar_today_24 },
                new PatientsFragmentViewModel{Name="Claims" , Icon=Resource.Drawable.baseline_rate_review_24 },
                new PatientsFragmentViewModel{Name="Edit Info", Icon= Resource.Drawable.baseline_edit_24 },
                //new PatientsFragmentViewModel{Name="Delete Info", Icon= Resource.Drawable.baseline_delete_24 },
            };

            this.SetStyle(DialogFragmentStyle.Normal, Resource.Style.dialog);//Android.Resource.Style.ThemeHoloLight


            //this.SetStyle(DialogFragment, Resource.Style.dialog);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.listview_fragment_patients, container, false);


            listview = view.FindViewById<ListView>(Resource.Id.patients_fragment_list_View);

            adapter = new PatientsFragmentAdapter(this.Activity, Items);
            listview.Adapter = adapter;

            listview.ItemClick += listview_ItemClick;

            if (Arguments != null)
            {
                patientId = Arguments.GetString("patient_id");
                patient_name = Arguments.GetString("patient_name");


                //int mParam2 = Arguments.GetInt("paramInt");
            }

            Dialog.SetTitle(!string.IsNullOrEmpty(patient_name) ? patient_name : "Select an Action");


            return view;

        }

        private void listview_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
           
            if (e.Position == 2)
            {
                //var intent = new Intent(this.Context, typeof(StartActivity));
                var intent = new Intent(Activity, typeof(EditPatientActivity));

                intent.PutExtra("patientId", patientId);
                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this.Activity, patientId, ToastLength.Long).Show();
            }


        }
    }
}