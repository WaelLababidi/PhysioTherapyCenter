using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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
    [Activity(Label = "Create Appointment", Theme = "@style/AppTheme.NoActionBar")]
    public class CreateAppointmentActivity : BaseDrawerActivity
    {
        private EditText text_view_patient;
        private EditText text_view_claim;

        private EditText edit_text_appointment_date;
        private EditText edit_text_start_time;
        private EditText edit_text_end_time;
        private EditText edit_text_amount;
        private EditText edit_text_note;

        private RadioButton radio_button_yes;
        private RadioButton radio_button_no;

        //private SearchableSpinner spinner_patients;
        //private SearchableSpinner spinner_claims;
        //private MaterialSpinner spinner_confirmed;
        //private List<string> confirmed;


        //private ArrayAdapter ConfirmedAdapter;
        private ArrayAdapter patientsAdapter;
        private ArrayAdapter claimsAdapter;

        private List<PatientViewModel> patients = new List<PatientViewModel>();
        private List<ClaimInfo> claims = new List<ClaimInfo>();

        protected Button SubmitButton;


        private AppointmentViewModel entity = new AppointmentViewModel { };

        private SearchPatientDialogFragment searchPatientDialogFragment;
        private SearchClaimDialogFragment searchClaimDialogFragment;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.create_appointment);

            Init();

            navigationView.SetCheckedItem(Resource.Id.nav_calendar);




            //spinner_patients = FindViewById<SearchableSpinner>(Resource.Id.spinner_patients);
            //spinner_patients.ItemSelected += spinner_patients_ItemSelected;
            //spinner_patients.SetTitle("Select Patient...");


            //spinner_claims = FindViewById<SearchableSpinner>(Resource.Id.spinner_claims);
            //spinner_claims.ItemSelected += spinner_claims_ItemSelected;
            //spinner_claims.SetTitle("Select Claim...");




            edit_text_appointment_date = FindViewById<EditText>(Resource.Id.edit_text_appointment_date);
            edit_text_appointment_date.LongClick += (s, e) =>
            {
                var frag = DatePickerFragment.NewInstance(delegate (DateTime onDateSelected) { edit_text_appointment_date.Text = onDateSelected.ToString("dd/MM/yyyy"); }, ConvertToDateTime(edit_text_appointment_date.Text));
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            edit_text_start_time = FindViewById<EditText>(Resource.Id.edit_text_start_time);
            edit_text_start_time.Click += (s, e) =>
            {
                var frag = TimePickerFragment.NewInstance(delegate (string time) { edit_text_start_time.Text = time; }, ConvertToTimespan(edit_text_start_time.Text));
                frag.Show(FragmentManager, TimePickerFragment.TAG);

                //TimePickerFragment frag = new TimePickerFragment(delegate (string time)
                //{
                //    edit_text_start_time.Text = time;
                //});
                //frag.Show(FragmentManager, TimePickerFragment.TAG);
            };

            edit_text_end_time = FindViewById<EditText>(Resource.Id.edit_text_end_time);
            edit_text_end_time.Click += (s, e) =>
            {
                var frag = TimePickerFragment.NewInstance(delegate (string time) { edit_text_end_time.Text = time; }, ConvertToTimespan(edit_text_end_time.Text));

                frag.Show(FragmentManager, TimePickerFragment.TAG);

                //TimePickerFragment frag = new TimePickerFragment(delegate (string time)
                //{
                //    edit_text_end_time.Text = time;
                //});
                //frag.Show(FragmentManager, TimePickerFragment.TAG);
            };



            edit_text_amount = FindViewById<EditText>(Resource.Id.edit_text_amount);


            edit_text_note = FindViewById<EditText>(Resource.Id.edit_text_note);

            //spinner_confirmed = FindViewById<MaterialSpinner>(Resource.Id.spinner_confirmed);
            //confirmed = new List<string>() { "Yes", "No" };
            //ConfirmedAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, confirmed);
            //ConfirmedAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            //spinner_confirmed.Adapter = ConfirmedAdapter;



            var progressDialog = new ProgressDialog(this);
            progressDialog.SetCancelable(false);
            progressDialog.SetMessage("Loading Data...");

            progressDialog.Show();


            patients = await Patients();

            //patientsAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, patients.Select(x => x.Name).ToList());
            //spinner_patients.Adapter = patientsAdapter;



            progressDialog.Hide();


            text_view_patient = FindViewById<EditText>(Resource.Id.text_view_patient);
            text_view_patient.Click += delegate
           {
               searchPatientDialogFragment = new SearchPatientDialogFragment(patients, SearchPatientDialogFragment_CallBackAction);
               searchPatientDialogFragment.Show(FragmentManager, "SEARCH_PATIENT_DIALOG_TAG");
               //delegate (PatientViewModel patient)
               //{
               //    if (patient != null)
               //    {
               //        text_view_patient.Text = patient.Name.ToString();
               //        entity.PatientId = patient.PatientId;

               //        progressDialog = new ProgressDialog(this);
               //        progressDialog.SetCancelable(false);
               //        progressDialog.SetMessage("Loading Data...");

               //        progressDialog.Show();

               //        claims = PatientAppointments(patient.PatientId).Result;
               //        if (claims.Count > 0)
               //        {
               //            text_view_claim.Text = claims.Select(x => x.ApprovalDate.ToString()).FirstOrDefault();
               //        }

               //        progressDialog.Hide();
               //    }

               //});
           };
            text_view_patient.FocusChange += (s, e) =>
            {
                if (e.HasFocus)
                {
                    searchPatientDialogFragment = new SearchPatientDialogFragment(patients, SearchPatientDialogFragment_CallBackAction);
                    searchPatientDialogFragment.Show(FragmentManager, "SEARCH_PATIENT_DIALOG_TAG");
                }
            };


            text_view_claim = FindViewById<EditText>(Resource.Id.text_view_claim);
            text_view_claim.Click += delegate
            {
                searchClaimDialogFragment = new SearchClaimDialogFragment(claims, SearchClaimDialogFragment_CallBackAction);
                searchClaimDialogFragment.Show(FragmentManager, "SEARCH_CLAIM_DIALOG_TAG");

            };
            text_view_claim.FocusChange += (s, e) =>
            {
                if (e.HasFocus)
                {
                    searchClaimDialogFragment = new SearchClaimDialogFragment(claims, SearchClaimDialogFragment_CallBackAction);
                    searchClaimDialogFragment.Show(FragmentManager, "SEARCH_CLAIM_DIALOG_TAG");
                }
            };


            if (Intent.Extras != null)
            {

                var AppointmentDate = Intent.Extras.GetString("AppointmentDate");
                if (!string.IsNullOrEmpty(AppointmentDate))
                {
                    edit_text_appointment_date.Text = AppointmentDate;
                }
            }




            SubmitButton = FindViewById<Button>(Resource.Id.button_create);
            SubmitButton.Click += SubmitButton_Click;

            radio_button_yes = FindViewById<RadioButton>(Resource.Id.radio_button_yes);
            radio_button_no = FindViewById<RadioButton>(Resource.Id.radio_button_no);


            entity.Confirmed = true;
            radio_button_yes.Checked = entity.Confirmed == true;
            radio_button_no.Checked = entity.Confirmed == false;


            radio_button_yes.Click += radio_button_yes_Click;
            radio_button_no.Click += radio_button_no_Click;

            Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

        }

        private void radio_button_yes_Click(object sender, EventArgs e)
        {
            entity.Confirmed = true;
        }

        private void radio_button_no_Click(object sender, EventArgs e)
        {
            entity.Confirmed = false;
        }

        private async void SearchPatientDialogFragment_CallBackAction(PatientViewModel patient)
        {
            if (patient != null)
            {
                text_view_patient.Text = patient.Name.ToString();
                entity.PatientId = patient.PatientId;

                var progressDialog = new ProgressDialog(this);
                progressDialog.SetCancelable(false);
                progressDialog.SetMessage("Loading Data...");

                progressDialog.Show();

                claims = await PatientClaims(patient.PatientId);
                if (claims.Count > 0)
                {
                    var firstClaim = claims.FirstOrDefault();
                    text_view_claim.Text = firstClaim.ApprovalDate.ToString("dd/MM/yyyy");
                    entity.ClaimId = firstClaim.ClaimId;
                }
                else
                {
                    text_view_claim.Text = string.Format("Select Claim");
                    entity.ClaimId = string.Empty;
                }
                progressDialog.Hide();
            }
            else
            {
                text_view_patient.Text = string.Format("Select Patient");
                entity.PatientId = string.Empty;
                text_view_claim.Text = string.Format("Select Claim");
                entity.ClaimId = string.Empty;
            }
        }


        private void SearchClaimDialogFragment_CallBackAction(ClaimInfo claim)
        {
            if (claim != null)
            {
                text_view_claim.Text = claim.ApprovalDate.ToString("dd/MM/yyyy");
                entity.ClaimId = claim.ClaimId;
            }
            else
            {
                text_view_claim.Text = string.Format("Select Claim");
                entity.ClaimId = string.Empty;
            }
        }

        ////private async void spinner_patients_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        ////{


        ////    var progressDialog = new ProgressDialog(this);
        ////    progressDialog.SetCancelable(false);
        ////    progressDialog.SetMessage("Loading Data...");

        ////    progressDialog.Show();
        ////    var PatientId = patients[e.Position].PatientId;

        ////    claims = await PatientAppointments(PatientId);

        ////    claimsAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, claims.Select(x => string.Format("{0} - {1}", x.ApprovalDate.ToString("dd/MM/yyyy"), x.ClaimType)).ToList());
        ////    spinner_claims.Adapter = claimsAdapter;

        ////    entity.PatientId = PatientId;

        ////    progressDialog.Hide();

        ////    //View view = (View)sender;
        ////    //Snackbar.Make(view, patients[e.Position].Name, Snackbar.LengthLong)
        ////    //    .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        ////}

        private async void spinner_claims_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (claims.Count > 0)
            {
                var ClaimId = claims[e.Position].ClaimId;
                entity.ClaimId = ClaimId;
            }

        }

        protected void Radio_Button_Yes_Click(View view)
        {
            entity.Confirmed = true;
        }

        protected void Radio_Button_No_Click(View view)
        {
            entity.Confirmed = false;

        }

        protected async void SubmitButton_Click(object sender, EventArgs e)
        {

            entity.StartTime = ConvertToTimespan(edit_text_start_time.Text);
            entity.EndTime = ConvertToTimespan(edit_text_end_time.Text);

            entity.AppointmentDate = ConvertToDateTime(edit_text_appointment_date.Text);
            entity.Note = edit_text_note.Text;
            entity.Amount = ConvertToNumber(edit_text_amount.Text);

            //entity.Confirmed = spinner_confirmed.SelectedItem.ToString().Equals("yes", StringComparison.OrdinalIgnoreCase) ? true : false;

            var errors = ValidateModel();

            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog alert = dialog.Create();

            if (errors.Count > 0)
            {
                alert.SetTitle("Missing Info");
                alert.SetMessage(string.Join(Constants.NewLine, errors));
                alert.SetButton("Ok", (c, ev) =>
                {
                    // Ok button click task  
                });
                alert.Show();

                return;
            }


            var progressDialog = new ProgressDialog(this);
            progressDialog.SetCancelable(false);
            progressDialog.SetMessage("Sending Data...");

            progressDialog.Show();

            var url = string.Format("{0}/api/appointments", Constants.Server_Url);

            var jsonString = JsonConvert.SerializeObject(entity);
            var result = await Constants.RestApiSerive.PostResponse<ServerResponse<string>>(url, jsonString);

            progressDialog.Hide();

            if (result != null)
            {
                if (result.valid)
                {

                    //edit_text_appointment_date.Text = string.Empty;
                    edit_text_start_time.Text = string.Empty;
                    edit_text_end_time.Text = string.Empty;
                    edit_text_amount.Text = string.Empty;
                    edit_text_note.Text = string.Empty;

                    alert.SetTitle("Success!");
                    alert.SetMessage(result.value);
                    SuccessBtnAction = true;
                }
                else
                {
                    alert.SetTitle("Faild!");
                    alert.SetMessage(result.value);
                }
            }
            else
            {
                alert.SetTitle("Faild!");
                alert.SetMessage("Server could not be reached");
            }

            alert.SetButton("Ok", (c, ev) =>
            {
                if (SuccessBtnAction)
                {
                    var intent = new Intent(this, typeof(CalendarActivity));
                    StartActivity(intent);
                    Finish();
                }
            });
            alert.Show();

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
                var intent = new Intent(this, typeof(CalendarActivity));
                StartActivity(intent);
                Finish();
                //base.OnBackPressed();
            }
        }

        List<string> ValidateModel()
        {

            var result = new List<string>();

            if (string.IsNullOrEmpty(entity.PatientId)) { result.Add("Patient is required"); }

            if (string.IsNullOrEmpty(entity.ClaimId)) { result.Add("Claim is required"); }

            if (entity.AppointmentDate == null) { result.Add("Appointment Date is required"); }


            if (entity.StartTime == null) { result.Add("Start Time is required"); }

            if (entity.EndTime == null) { result.Add("End Time is required"); }

            //if (entity.Amount == null) { result.Add("Amount is required"); }


            return result;
        }


    }
}