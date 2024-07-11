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
    [Activity(Label = "Create Coop Claim", Theme = "@style/AppTheme.NoActionBar")]
    public class CreateCoopClaimActivity : BaseDrawerActivity
    {
        private EditText text_view_patient;
        private EditText edit_text_claim_guarantor;
        private EditText text_view_relationship;
        private EditText text_view_doctor;
        private EditText edit_text_diagnosis_description;

        private EditText edit_text_approval_date;
        private EditText edit_text_visit_date;
        private EditText edit_text_max_sessions_count;


        private RadioButton radio_session_place_clinic;
        private RadioButton radio_session_place_home;

        private RadioButton radio_button_yes;
        private RadioButton radio_button_no;


        private List<PatientViewModel> patients = new List<PatientViewModel>();
        private List<DoctorViewModel> doctors = new List<DoctorViewModel>();

        protected Button SubmitButton;

        private List<string> relationships = new List<string>();

        private CoopPatientClaim entity = new CoopPatientClaim { };


        private SearchPatientDialogFragment searchPatientDialogFragment;
        private SearchDoctorDialogFragment searchDoctorDialogFragment;
        private SearchDialogFragment searchDialogFragment;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.create_coop_claim);

            Init();

            navigationView.SetCheckedItem(Resource.Id.nav_claims);



            text_view_patient = FindViewById<EditText>(Resource.Id.text_view_patient);
            edit_text_claim_guarantor = FindViewById<EditText>(Resource.Id.edit_text_claim_guarantor);
            text_view_relationship = FindViewById<EditText>(Resource.Id.text_view_relationship);
            text_view_doctor = FindViewById<EditText>(Resource.Id.text_view_doctor);
            edit_text_diagnosis_description = FindViewById<EditText>(Resource.Id.edit_text_diagnosis_description);


            edit_text_approval_date = FindViewById<EditText>(Resource.Id.edit_text_approval_date);
            edit_text_visit_date = FindViewById<EditText>(Resource.Id.edit_text_visit_date);
            edit_text_max_sessions_count = FindViewById<EditText>(Resource.Id.edit_text_max_sessions_count);


            radio_session_place_clinic = FindViewById<RadioButton>(Resource.Id.radio_session_place_clinic);
            radio_session_place_home = FindViewById<RadioButton>(Resource.Id.radio_session_place_home);


            radio_button_yes = FindViewById<RadioButton>(Resource.Id.radio_button_yes);
            radio_button_no = FindViewById<RadioButton>(Resource.Id.radio_button_no);

            SubmitButton = FindViewById<Button>(Resource.Id.button_create);



            var progressDialog = new ProgressDialog(this);
            progressDialog.SetCancelable(false);
            progressDialog.SetMessage("Loading Data...");

            progressDialog.Show();


            patients = await Patients();

            doctors = await Doctors();

            progressDialog.Hide();

            relationships = new List<string>() { "والدة", "والد", "زوج", "زوجة", "ابن", "ابنة", "اخ", "اخت", "نفسه", "نفسها" };

            text_view_patient.Click += delegate
            {
                searchPatientDialogFragment = new SearchPatientDialogFragment(patients, SearchPatientDialogFragment_CallBackAction);
                searchPatientDialogFragment.Show(FragmentManager, "SEARCH_PATIENT_DIALOG_TAG");
            };
            text_view_patient.FocusChange += (s, e) =>
            {
                if (e.HasFocus)
                {
                    searchPatientDialogFragment = new SearchPatientDialogFragment(patients, SearchPatientDialogFragment_CallBackAction);
                    searchPatientDialogFragment.Show(FragmentManager, "SEARCH_PATIENT_DIALOG_TAG");
                }
            };




            text_view_relationship.Click += delegate
            {
                searchDialogFragment = new SearchDialogFragment(relationships, SearchDialogFragment_CallBackAction);
                searchDialogFragment.Show(FragmentManager, "SEARCH_RELATIONSHIP_DIALOG_TAG");
            };
            text_view_relationship.FocusChange += (s, e) =>
            {
                if (e.HasFocus)
                {
                    searchDialogFragment = new SearchDialogFragment(relationships, SearchDialogFragment_CallBackAction);
                    searchDialogFragment.Show(FragmentManager, "SEARCH_RELATIONSHIP_DIALOG_TAG");
                }
            };
            text_view_doctor.Click += delegate
            {
                searchDoctorDialogFragment = new SearchDoctorDialogFragment(doctors, SearchDoctorDialogFragment_CallBackAction);
                searchDoctorDialogFragment.Show(FragmentManager, "SEARCH_DOCTOR_DIALOG_TAG");
            };
            text_view_doctor.FocusChange += (s, e) =>
            {
                if (e.HasFocus)
                {
                    searchDoctorDialogFragment = new SearchDoctorDialogFragment(doctors, SearchDoctorDialogFragment_CallBackAction);
                    searchDoctorDialogFragment.Show(FragmentManager, "SEARCH_DOCTOR_DIALOG_TAG");
                }
            };

            SubmitButton.Click += SubmitButton_Click;


            entity.Stated = false;
            radio_button_yes.Checked = entity.Stated == true;
            radio_button_no.Checked = entity.Stated == false;


            radio_button_yes.Click += radio_button_yes_Click;
            radio_button_no.Click += radio_button_no_Click;



            radio_session_place_clinic.Checked = entity.SessionsPlace == "Clinic";
            radio_session_place_home.Checked = entity.SessionsPlace == "Home";

            radio_session_place_clinic.Click += radio_session_place_clinic_Click;
            radio_session_place_home.Click += radio_session_place_home_Click;

            entity.ApprovalDate = DateTime.Now;
            edit_text_approval_date.Text = entity.ApprovalDate.Value.ToString("dd/MM/yyyy");
            edit_text_approval_date.LongClick += (s, e) =>
            {
                var frag = DatePickerFragment.NewInstance(delegate (DateTime onDateSelected) { edit_text_approval_date.Text = onDateSelected.ToString("dd/MM/yyyy"); }, ConvertToDateTime(edit_text_approval_date.Text));
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            entity.VisitDate = DateTime.Now;
            edit_text_visit_date.Text = entity.VisitDate.Value.ToString("dd/MM/yyyy");
            edit_text_visit_date.LongClick += (s, e) =>
            {
                var frag = DatePickerFragment.NewInstance(delegate (DateTime onDateSelected) { edit_text_visit_date.Text = onDateSelected.ToString("dd/MM/yyyy"); }, ConvertToDateTime(edit_text_visit_date.Text));
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

          


            entity.MaxSessionsCount = 10;
            edit_text_max_sessions_count.Text = entity.MaxSessionsCount.ToString();
            //edit_text_code.Text = entity.Code.ToString();




            Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

        }


        private void SearchDialogFragment_CallBackAction(string item)
        {
            if (item != null)
            {
                text_view_relationship.Text = item;
                entity.Relationship = item;
            }
            else
            {
                text_view_relationship.Text = string.Format("Select Relationship");
                entity.Relationship = string.Empty;
            }
        }


        private void radio_button_yes_Click(object sender, EventArgs e)
        {
            entity.Stated = true;
        }

        private void radio_button_no_Click(object sender, EventArgs e)
        {
            entity.Stated = false;
        }


        private void radio_session_place_clinic_Click(object sender, EventArgs e)
        {
            entity.SessionsPlace = "Clinic";

        }

        private void radio_session_place_home_Click(object sender, EventArgs e)
        {
            entity.SessionsPlace = "Home";
        }

        private async void SearchPatientDialogFragment_CallBackAction(PatientViewModel patient)
        {
            if (patient != null)
            {
                text_view_patient.Text = patient.Name.ToString();
                entity.PatientId = patient.PatientId;
            }
            else
            {
                text_view_patient.Text = string.Format("Select Patient");
                entity.PatientId = string.Empty;

            }
        }


        private void SearchDoctorDialogFragment_CallBackAction(DoctorViewModel doctor)
        {
            if (doctor != null)
            {
                text_view_doctor.Text = doctor.EnglishDoctorName;
                entity.DoctorId = doctor.DoctorId;
            }
            else
            {
                text_view_doctor.Text = string.Format("Select Doctor");
                entity.DoctorId = string.Empty;
            }
        }


        protected void Radio_Button_Yes_Click(View view)
        {
            entity.Stated = true;
        }

        protected void Radio_Button_No_Click(View view)
        {
            entity.Stated = false;

        }

        protected async void SubmitButton_Click(object sender, EventArgs e)
        {
            entity.Guarantor = edit_text_claim_guarantor.Text;
            entity.DiagnosisDescription = edit_text_diagnosis_description.Text;
            entity.ApprovalDate = ConvertToDateTime(edit_text_approval_date.Text);
            entity.VisitDate = ConvertToDateTime(edit_text_visit_date.Text);
            entity.MaxSessionsCount = ConvertToNumber(edit_text_max_sessions_count.Text);


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

            var url = string.Format("{0}/api/CoopPatientClaims", Constants.Server_Url);

            var jsonString = JsonConvert.SerializeObject(entity);
            var result = await Constants.RestApiSerive.PostResponse<ServerResponse<string>>(url, jsonString);

            progressDialog.Hide();

            if (result != null)
            {
                if (result.valid)
                {
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
                    var intent = new Intent(this, typeof(ClaimsActivity));
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
                var intent = new Intent(this, typeof(ClaimsActivity));
                StartActivity(intent);
                Finish();
                //base.OnBackPressed();
            }
        }

        List<string> ValidateModel()
        {

            var result = new List<string>();

            if (string.IsNullOrEmpty(entity.PatientId)) { result.Add("Patient is required"); }
            if (string.IsNullOrEmpty(entity.Guarantor)) { result.Add("Guarantor is required"); }
            if (string.IsNullOrEmpty(entity.Relationship)) { result.Add("Relationship is required"); }
            if (string.IsNullOrEmpty(entity.DoctorId)) { result.Add("Doctor is required"); }
            if (string.IsNullOrEmpty(entity.DiagnosisDescription)) { result.Add("Diagnosis is required"); }

            if (entity.ApprovalDate == null) { if (!string.IsNullOrEmpty(edit_text_approval_date.Text)) result.Add("Enter Valid Approval Date"); else result.Add("Approval Date is required"); }

            if (entity.MaxSessionsCount == null) { if (!string.IsNullOrEmpty(edit_text_max_sessions_count.Text)) result.Add("Enter Valid Max Sessions Count"); else result.Add("Max Sessions Count is required"); }


            return result;
        }


    }
}