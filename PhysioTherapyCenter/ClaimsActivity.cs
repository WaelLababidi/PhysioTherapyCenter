using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Com.Toptoche.Searchablespinnerlibrary;
using FR.Ganfra.Materialspinner;
using Newtonsoft.Json;
using PhysioTherapyCenter.Models;
using PhysioTherapyCenter.Models.Adapters;
using PhysioTherapyCenter.Models.Entities;
using PhysioTherapyCenter.Models.Fragments;
using PhysioTherapyCenter.Models.Interfaces;
using PhysioTherapyCenter.Models.ViewModels;
using SharedEntities;

namespace PhysioTherapyCenter
{
    //[Activity(Label = "PatientsActivity")]
    [Activity(Label = "Claims", Theme = "@style/AppTheme.NoActionBar")]
    public class ClaimsActivity : BaseDrawerActivity
    {

        #region Private Properties

        private TextView text_view_patient;
        private ListView listview;

        private ArrayAdapter patientsAdapter;

        private ClaimAdapter claimsAdapter;

        private List<PatientViewModel> patients = new List<PatientViewModel>();

        private List<ClaimInfo> claims = new List<ClaimInfo>();

        //private MaterialSpinner spinner_guarantor;
        //private List<string> guarantors;
        //private List<string> guarantors_ids_list;
        //private ArrayAdapter GuarantorsAdapter;

        private SearchPatientDialogFragment searchPatientDialogFragment;


        private string PatientId;


        private FloatingActionButton fab_create_is_claim;
        private FloatingActionButton fab_create_gs_claim;

        private FloatingActionButton fab_create_coop_claim;
        private FloatingActionButton fab_create_nssf_claim;
        private FloatingActionButton fab_create_globemed_claim;
        private FloatingActionButton fab_create_private_claim;

        private FloatingActionButton fab_main;
        private View bg_fab_menu;
        private static bool isFabOpen;



        #endregion

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.claims);

            Init();

            navigationView.SetCheckedItem(Resource.Id.nav_claims);

            var progressDialog = new ProgressDialog(this);

            progressDialog.SetCancelable(false);
            progressDialog.SetMessage("Loading Data...");

            progressDialog.Show();


            //spinner_guarantor = FindViewById<MaterialSpinner>(Resource.Id.spinner_patient_guarantor_instituation);
            text_view_patient = FindViewById<TextView>(Resource.Id.text_view_patient);
            listview = FindViewById<ListView>(Resource.Id.claims_list_View);
            listview.ItemClick += listview_ItemClick;

            //guarantors = new List<string>() { "All", "GS", "IS", "Coop", "GlobeMed", "Nssf", "Private" };
            //guarantors_ids_list = new List<string>() { "-1", "0", "1", "2", "3", "4", "5" };
            //GuarantorsAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, guarantors);
            //GuarantorsAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            //spinner_guarantor.Adapter = GuarantorsAdapter;
            //spinner_guarantor.ItemSelected += spinner_guarantor_ItemSelected;
            patients = await Patients();


            text_view_patient.Click += (s, e) =>
            {
                searchPatientDialogFragment = new SearchPatientDialogFragment(patients, SearchPatientDialogFragment_CallBackAction);
                searchPatientDialogFragment.Show(this.FragmentManager, "SEARCH_PATIENT_DIALOG_TAG");

            };

            claimsAdapter = new ClaimAdapter(this, claims);
            listview.Adapter = claimsAdapter;

            progressDialog.Hide();





            fab_main = FindViewById<FloatingActionButton>(Resource.Id.fab_main);
            fab_main.Click += (o, e) =>
            {
                if (isFabOpen)
                {
                    CloseFabMenu();
                }
                else
                {
                    ShowFabMenu();
                }
            };

            bg_fab_menu = FindViewById<View>(Resource.Id.bg_fab_menu);
            bg_fab_menu.Click += (o, e) => CloseFabMenu();

            fab_create_is_claim = FindViewById<FloatingActionButton>(Resource.Id.fab_create_is_claim);

            fab_create_is_claim.Click += fab_create_is_claim_Click;

            fab_create_gs_claim = FindViewById<FloatingActionButton>(Resource.Id.fab_create_gs_claim);

            fab_create_gs_claim.Click += fab_create_gs_claim_Click;

            fab_create_coop_claim = FindViewById<FloatingActionButton>(Resource.Id.fab_create_coop_claim);
            fab_create_coop_claim.Click += fab_create_coop_claim_Click;

            fab_create_nssf_claim = FindViewById<FloatingActionButton>(Resource.Id.fab_create_nssf_claim);
            fab_create_nssf_claim.Click += fab_create_nssf_claim_Click;

            fab_create_globemed_claim = FindViewById<FloatingActionButton>(Resource.Id.fab_create_globemed_claim);

            fab_create_private_claim = FindViewById<FloatingActionButton>(Resource.Id.fab_create_private_claim);

            fab_create_private_claim.Click += fab_create_private_claim_Click;

            Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

        }



        private void fab_create_private_claim_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CreatePrivateClaimActivity));
            StartActivity(intent);
            Finish();
        }

        private void fab_create_is_claim_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CreateISClaimActivity));
            StartActivity(intent);
            Finish();
        }

        private void fab_create_gs_claim_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CreateGSClaimActivity));
            StartActivity(intent);
            Finish();
        }


        private void fab_create_nssf_claim_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CreateNssfClaimActivity));
            StartActivity(intent);
            Finish();
        }


        private void fab_create_coop_claim_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CreateCoopClaimActivity));
            StartActivity(intent);
            Finish();
        }
        private void ShowFabMenu()
        {
            isFabOpen = true;
            fab_create_is_claim.Visibility = ViewStates.Visible;
            fab_create_gs_claim.Visibility = ViewStates.Visible;

            fab_create_coop_claim.Visibility = ViewStates.Visible;
            fab_create_nssf_claim.Visibility = ViewStates.Visible;
            fab_create_globemed_claim.Visibility = ViewStates.Visible;
            fab_create_private_claim.Visibility = ViewStates.Visible;

            bg_fab_menu.Visibility = ViewStates.Visible;

            //fab_main.Animate().Rotation(135f);
            bg_fab_menu.Animate().Alpha(1f);

            fab_create_is_claim.Animate().TranslationY(-Resources.GetDimension(Resource.Dimension.standard_60)).Rotation(0f);
            fab_create_gs_claim.Animate().TranslationY(-Resources.GetDimension(Resource.Dimension.standard_120)).Rotation(0f);

            fab_create_coop_claim.Animate().TranslationY(-Resources.GetDimension(Resource.Dimension.standard_180)).Rotation(0f);
            fab_create_nssf_claim.Animate().TranslationY(-Resources.GetDimension(Resource.Dimension.standard_240)).Rotation(0f);
            fab_create_globemed_claim.Animate().TranslationY(-Resources.GetDimension(Resource.Dimension.standard_300)).Rotation(0f);
            fab_create_private_claim.Animate().TranslationY(-Resources.GetDimension(Resource.Dimension.standard_360)).Rotation(0f);


        }
        private void CloseFabMenu()
        {
            isFabOpen = false;
            fab_main.Animate().Rotation(0f);
            bg_fab_menu.Animate().Alpha(0f);
            fab_create_is_claim.Animate().TranslationY(0f).Rotation(90f).SetListener(new FabAnimatorListener(bg_fab_menu, fab_create_is_claim, fab_create_gs_claim, fab_create_coop_claim, fab_create_nssf_claim, fab_create_globemed_claim, fab_create_private_claim));
            fab_create_gs_claim.Animate().TranslationY(0f).Rotation(90f);

            fab_create_coop_claim.Animate().TranslationY(0f).Rotation(90f);
            fab_create_nssf_claim.Animate().TranslationY(0f).Rotation(90f);
            fab_create_globemed_claim.Animate().TranslationY(0f).Rotation(90f);
            fab_create_private_claim.Animate().TranslationY(0f).Rotation(90f);


        }



        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }




        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                var intent = new Intent(this, typeof(DashboardActivity));
                StartActivity(intent);
                Finish();
            }
        }


        private void spinner_guarantor_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            FillInListView(PatientId);
        }


        private void listview_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Type type = null;
            var claim = claimsAdapter[e.Position];

            switch (claim.ClaimType.ToLower())
            {
                case "private":
                    type = typeof(EditPrivateClaimActivity);
                    break;
                case "is":
                    type = typeof(EditISClaimActivity);
                    break;
                case "gs":
                    type = typeof(EditGSClaimActivity);
                    break;
                case "nssf":
                    type = typeof(EditNssfClaimActivity);
                    break;
                case "coop":
                    type = typeof(EditCoopClaimActivity);
                    break;
                default:
                    break;
            }

            if (type != null)
            {
                var intent = new Intent(this, type);
                intent.PutExtra("ClaimId", claim.ClaimId);
                StartActivity(intent);
                Finish();
            }

        }


        //private void listview_ItemClick(object sender, EventArgs e)
        //{
        //    [e.].Guarantor
        //    //View listItem = (View)sender;
        //    //var height = listItem.MeasuredHeight;
        //    //listview.LayoutParameters.Height = claims.Count * height;

        //}



        private async void SearchPatientDialogFragment_CallBackAction(PatientViewModel patient)
        {
            if (patient != null)
            {
                text_view_patient.Text = patient.Name.ToString();
                PatientId = patient.PatientId;
                FillInListView(PatientId);

            }
        }

        private async void FillInListView(string PatientId)
        {

            var progressDialog = new ProgressDialog(this);
            progressDialog.SetCancelable(false);
            progressDialog.SetMessage("Loading Data...");

            progressDialog.Show();

            claims = await PatientClaims(PatientId);


            //var guarantor = guarantors_ids_list[spinner_guarantor.SelectedItemPosition];


            //if (!guarantor.Equals("-1"))
            //{
            //    claims = claims
            //        .Where(x => x.ClaimType.Equals(guarantors[spinner_guarantor.SelectedItemPosition], StringComparison.OrdinalIgnoreCase))
            //        .OrderByDescending(x => x.ApprovalDate)
            //        .ToList();
            //}

            claimsAdapter = new ClaimAdapter(this, claims);
            listview.Adapter = claimsAdapter;
            //int height = 122;
            //listview.LayoutParameters.Height = (claims.Count * height) + height * 2;

            progressDialog.Hide();
        }


        private class FabAnimatorListener : Java.Lang.Object, Animator.IAnimatorListener
        {

            private View[] viewsToHide;


            public FabAnimatorListener(params View[] viewsToHide)
            {
                this.viewsToHide = viewsToHide;

            }
            public void OnAnimationCancel(Animator animation)
            {
            }

            public void OnAnimationEnd(Animator animation)
            {
                if (!isFabOpen)
                {
                    foreach (var view in viewsToHide)
                    {
                        view.Visibility = ViewStates.Gone;

                    }
                }
            }

            public void OnAnimationRepeat(Animator animation)
            {
            }

            public void OnAnimationStart(Animator animation)
            {
            }
        }
    }




}