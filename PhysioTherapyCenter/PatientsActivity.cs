using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using PhysioTherapyCenter.Models;
using PhysioTherapyCenter.Models.Adapters;
using PhysioTherapyCenter.Models.Entities;
using PhysioTherapyCenter.Models.Fragments;
using SharedEntities;

namespace PhysioTherapyCenter
{
    //[Activity(Label = "PatientsActivity")]
    [Activity(Label = "Patients", Theme = "@style/AppTheme.NoActionBar")]
    public class PatientsActivity : BaseDrawerActivity
    {

        private ListView listview;
        private EditText editText;

        private List<PatientViewModel> patients;

        private PatientAdapter patientAdapter;

        private FragmentManager fragmentManager;
        private PatientsFragment patientsFragment;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.patients);

            Init();

            navigationView.SetCheckedItem(Resource.Id.nav_patients);

            listview = FindViewById<ListView>(Resource.Id.patients_list_View);

            editText = FindViewById<EditText>(Resource.Id.search_view_patients);

            editText.TextChanged += EditText_TextChanged;


            var progressDialog = new ProgressDialog(this);
            progressDialog.SetCancelable(false);
            progressDialog.SetMessage("Loading Data...");

            progressDialog.Show();


            patients = await Patients();

            patientAdapter = new PatientAdapter(this, patients);
            listview.Adapter = patientAdapter;
            listview.ItemLongClick += listview_ItemLongClick;
            listview.ItemClick += listview_ItemClick;
            progressDialog.Hide();


            FloatingActionButton create_new_patient_btn = FindViewById<FloatingActionButton>(Resource.Id.create_new_patient_btn);
            create_new_patient_btn.Click += create_new_patient_btnOnClick;



            fragmentManager = this.FragmentManager;
            patientsFragment = new PatientsFragment();


            Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

        }

        private void listview_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            Bundle args = new Bundle();
            args.PutString("patient_id", patientAdapter[e.Position].PatientId);
            args.PutString("patient_name", patientAdapter[e.Position].Name);
            patientsFragment.Arguments = args;

   
            patientsFragment.Show(fragmentManager, "patients_fragment");
        }

        private void listview_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {


     
        }



        private void create_new_patient_btnOnClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CreatePatientActivity));
            StartActivity(intent);
            Finish();
        }

        private void EditText_TextChanged(object sender, TextChangedEventArgs e)
        {
            //get the text from Edit Text            
            var searchText = editText.Text;

            if (!string.IsNullOrEmpty(searchText) && !string.IsNullOrWhiteSpace(searchText))
            {
                //Compare the entered text with List  
                var result = patients.Where(x => x.Name.ToLower().Contains(searchText.ToLower())).ToList();

                // bind the result with adapter  
                patientAdapter = new PatientAdapter(this, result);
                listview.Adapter = patientAdapter;
            }

        }





    }
}