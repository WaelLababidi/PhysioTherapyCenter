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
    [Activity(Label = "Edit Appointment", Theme = "@style/AppTheme.NoActionBar")]
    public class EditAppointmentActivity : BaseDrawerActivity
    {

        #region Private Properties

        private EditText text_view_patient;

        private EditText text_view_claim;

        private EditText edit_text_appointment_date;

        private EditText edit_text_start_time;

        private EditText edit_text_end_time;

        private EditText edit_text_amount;

        private EditText edit_text_note;
        private RadioButton radio_button_yes;
        private RadioButton radio_button_no;

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

        private TimePickerFragment start_timePickerFragment;
        private TimePickerFragment end_timePickerFragment;
        #endregion

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.edit_appointment);

            Init();

            navigationView.SetCheckedItem(Resource.Id.nav_calendar);

            var progressDialog = new ProgressDialog(this);

            progressDialog.SetCancelable(false);
            progressDialog.SetMessage("Loading Data...");

            progressDialog.Show();


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
            };

            edit_text_end_time = FindViewById<EditText>(Resource.Id.edit_text_end_time);
            edit_text_end_time.Click += (s, e) =>
            {
                var frag = TimePickerFragment.NewInstance(delegate (string time) { edit_text_end_time.Text = time; }, ConvertToTimespan(edit_text_end_time.Text));
                frag.Show(FragmentManager, TimePickerFragment.TAG);
            };
            //spinner_confirmed = FindViewById<MaterialSpinner>(Resource.Id.spinner_confirmed);
            edit_text_amount = FindViewById<EditText>(Resource.Id.edit_text_amount);
            edit_text_note = FindViewById<EditText>(Resource.Id.edit_text_note);
            text_view_patient = FindViewById<EditText>(Resource.Id.text_view_patient);
            text_view_claim = FindViewById<EditText>(Resource.Id.text_view_claim);
            SubmitButton = FindViewById<Button>(Resource.Id.button_save);



            radio_button_yes = FindViewById<RadioButton>(Resource.Id.radio_button_yes);
            radio_button_no = FindViewById<RadioButton>(Resource.Id.radio_button_no);

            radio_button_yes.Click += radio_button_yes_Click;
            radio_button_no.Click += radio_button_no_Click;

            //confirmed = new List<string>() { "Yes", "No" };
            //ConfirmedAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, confirmed);
            //ConfirmedAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            //spinner_confirmed.Adapter = ConfirmedAdapter;


            if (Intent.Extras != null)
            {

                var AppointmentId = Intent.Extras.GetString("AppointmentId");
                if (!string.IsNullOrEmpty(AppointmentId))
                {

                    var result = await GetAppointment(AppointmentId);
                    if (result != null)
                    {
                        claims = await PatientClaims(result.PatientId);
                        if (claims.Count > 0)
                        {
                            var Claim = claims.Where(x => x.ClaimId == result.ClaimId).FirstOrDefault();
                            if (Claim != null)
                            {
                                text_view_claim.Text = Claim.ApprovalDate.ToString("dd/MM/yyyy");
                                entity.ClaimId = Claim.ClaimId;
                            }
                            else
                            {
                                entity.ClaimId = string.Empty;

                            }
                        }
                        else
                        {
                            text_view_claim.Text = string.Format("Select Claim");
                            entity.ClaimId = string.Empty;
                        }


                        text_view_patient.Text = result.PatientName;

                        edit_text_appointment_date.Text = result.AppointmentDate.Value.ToString("dd/MM/yyyy");

                        edit_text_start_time.Text = result.StartTime.Value.ToString(@"hh\:mm");
                        edit_text_end_time.Text = result.EndTime.Value.ToString(@"hh\:mm");

                        edit_text_amount.Text = result.Amount == null ? string.Empty : result.Amount.Value.ToString();

                        edit_text_note.Text = result.Note;


                        radio_button_yes.Checked = result.Confirmed == true;
                        radio_button_no.Checked = result.Confirmed == false;

                        //spinner_confirmed.SetSelection(result.Confirmed ? 0 : 1);

                        entity.AppointmentId = result.AppointmentId;
                        entity.PatientId = result.PatientId;
                        entity.ClaimId = result.ClaimId;

                        entity.LastEdited = result.LastEdited;

                    }
                    else
                    {
                        entity.PatientId = string.Empty;
                        text_view_patient.Text = string.Format("Select Patient");

                        entity.ClaimId = string.Empty;
                        text_view_claim.Text = string.Format("Select Claim");
                    }
                }
            }



            edit_text_start_time.LongClick += (s, e) =>
            {
                var frag = TimePickerFragment.NewInstance(delegate (string time) { edit_text_start_time.Text = time; }, ConvertToTimespan(edit_text_start_time.Text));
                frag.Show(FragmentManager, TimePickerFragment.TAG);
            };

            edit_text_end_time.LongClick += (s, e) =>
            {
                var frag = TimePickerFragment.NewInstance(delegate (string time) { edit_text_end_time.Text = time; }, ConvertToTimespan(edit_text_end_time.Text));

                frag.Show(FragmentManager, TimePickerFragment.TAG);

            };





            patients = await Patients();


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


            text_view_claim.Click += (s, e) =>
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



            SubmitButton.Click += SubmitButton_Click;

            progressDialog.Hide();



            Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_delete, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            int id = item.ItemId;

            if (id == Resource.Id.action_delete)
            {
                var AppointmentId = entity.AppointmentId;
                DeleteAppointmentAsync(AppointmentId);
            }

            return true;
        }

        private async Task DeleteAppointmentAsync(string AppointmentId)
        {

            var isDeleted = false;


            if (!string.IsNullOrEmpty(AppointmentId))
            {
                var progressDialog = new ProgressDialog(this);
                progressDialog.SetCancelable(false);
                progressDialog.SetMessage("Deleting Data...");

                progressDialog.Show();

                var url = string.Format("{0}/api/appointments/{1}", Constants.Server_Url, AppointmentId);

                var result = await Constants.RestApiSerive.DeleteResponse<ServerResponse<string>>(url);


                progressDialog.Hide();

                Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                Android.App.AlertDialog alert = dialog.Create();

                if (result != null)
                {
                    if (result.valid)
                    {
                        isDeleted = true;

                        alert.SetTitle("Success!");
                        alert.SetMessage(result.value);
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

                    if (isDeleted)
                    {
                        var intent = new Intent(this, typeof(CalendarActivity));
                        StartActivity(intent);
                        Finish();
                    }

                });
                alert.Show();
            }


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
            }
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



        private void radio_button_yes_Click(object sender, EventArgs e)
        {
            entity.Confirmed = true;
        }

        private void radio_button_no_Click(object sender, EventArgs e)
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
            var result = await Constants.RestApiSerive.PutResponse<ServerResponse<string>>(url, jsonString);

            progressDialog.Hide();

            if (result != null)
            {
                if (result.valid)
                {

                    //edit_text_appointment_date.Text = string.Empty;
                    //edit_text_start_time.Text = string.Empty;
                    //edit_text_end_time.Text = string.Empty;
                    //edit_text_amount.Text = string.Empty;
                    //edit_text_note.Text = string.Empty;

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

        List<string> ValidateModel()
        {

            var result = new List<string>();

            if (string.IsNullOrEmpty(entity.AppointmentId)) { result.Add("Appointment is Required"); }

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