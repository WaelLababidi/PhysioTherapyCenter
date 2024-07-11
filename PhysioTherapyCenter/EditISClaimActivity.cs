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
    [Activity(Label = "Edit IS Claim", Theme = "@style/AppTheme.NoActionBar")]
    public class EditISClaimActivity : BaseDrawerActivity
    {
        private EditText text_view_patient;
        private EditText edit_text_claim_number;
        private EditText edit_text_claim_guarantor;
        private EditText text_view_relationship;
        private EditText edit_text_military_number;
        private EditText text_view_rank;

        private RadioButton radio_session_place_clinic;
        private RadioButton radio_session_place_home;


        private EditText edit_text_approval_date;
        private EditText edit_text_session_start_date;
        private EditText edit_text_session_end_date;
        private EditText edit_text_receiving_invoice_date;
        private EditText edit_text_quantity;
        private EditText edit_text_official_amount;
        private EditText edit_text_visit_date;
        private EditText text_view_physical_therapists;
        private RadioButton radio_button_yes;
        private RadioButton radio_button_no;
        protected Button SubmitButton;

        private List<PatientViewModel> patients = new List<PatientViewModel>();
        private List<RankViewModel> militaryRanks = new List<RankViewModel>();
        private List<PhysicalTherapistViewModel> physicalTherapists = new List<PhysicalTherapistViewModel>();
        private List<string> relationships = new List<string>();

        private PatientClaim entity = new PatientClaim { };


        private SearchPatientDialogFragment searchPatientDialogFragment;
        private SearchRankDialogFragment searchRankDialogFragment;
        private SearchPhysicalTherapistDialogFragment searchPhysicalTherapistDialogFragment;
        private SearchDialogFragment searchDialogFragment;

        private ProgressDialog progressDialog;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.edit_is_claim);

            Init();

            navigationView.SetCheckedItem(Resource.Id.nav_claims);


            progressDialog = new ProgressDialog(this);
            progressDialog.SetCancelable(false);
            progressDialog.SetMessage("Loading Data...");
            progressDialog.Show();

            if (Intent.Extras != null)
            {
                var ClaimId = Intent.Extras.GetString("ClaimId");
                if (!string.IsNullOrEmpty(ClaimId))
                {
                    entity = await GetISClaim(ClaimId);

                    if (entity != null)
                    {

                    


                        text_view_patient = FindViewById<EditText>(Resource.Id.text_view_patient);
                        edit_text_claim_number = FindViewById<EditText>(Resource.Id.edit_text_claim_number);
                        edit_text_claim_guarantor = FindViewById<EditText>(Resource.Id.edit_text_claim_guarantor);
                        text_view_relationship = FindViewById<EditText>(Resource.Id.text_view_relationship);
                        edit_text_military_number = FindViewById<EditText>(Resource.Id.edit_text_military_number);
                        text_view_rank = FindViewById<EditText>(Resource.Id.text_view_rank);
                        radio_session_place_clinic = FindViewById<RadioButton>(Resource.Id.radio_session_place_clinic);
                        radio_session_place_home = FindViewById<RadioButton>(Resource.Id.radio_session_place_home);

                        edit_text_approval_date = FindViewById<EditText>(Resource.Id.edit_text_approval_date);
                        edit_text_session_start_date = FindViewById<EditText>(Resource.Id.edit_text_session_start_date);
                        edit_text_session_end_date = FindViewById<EditText>(Resource.Id.edit_text_session_end_date);
                        //edit_text_receiving_invoice_date = FindViewById<EditText>(Resource.Id.edit_text_receiving_invoice_date);
                        edit_text_quantity = FindViewById<EditText>(Resource.Id.edit_text_quantity);
                        edit_text_official_amount = FindViewById<EditText>(Resource.Id.edit_text_official_amount);
                        edit_text_visit_date = FindViewById<EditText>(Resource.Id.edit_text_visit_date);
                        text_view_physical_therapists = FindViewById<EditText>(Resource.Id.text_view_physical_therapists);

                        radio_button_yes = FindViewById<RadioButton>(Resource.Id.radio_button_yes);
                        radio_button_no = FindViewById<RadioButton>(Resource.Id.radio_button_no);


                        SubmitButton = FindViewById<Button>(Resource.Id.button_create);




                        patients = await Patients();
                        militaryRanks = await MilitaryRanks();
                        physicalTherapists = await PhysicalTherapists();
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
                        text_view_patient.Text = entity.PatientName;


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
                        text_view_relationship.Text = entity.ArabicRelationDescription;


                        text_view_rank.Click += delegate
                        {
                            searchRankDialogFragment = new SearchRankDialogFragment(militaryRanks, SearchRankDialogFragment_CallBackAction);
                            searchRankDialogFragment.Show(FragmentManager, "SEARCH_RANK_DIALOG_TAG");
                        };
                        text_view_rank.FocusChange += (s, e) =>
                        {
                            if (e.HasFocus)
                            {
                                searchRankDialogFragment = new SearchRankDialogFragment(militaryRanks, SearchRankDialogFragment_CallBackAction);
                                searchRankDialogFragment.Show(FragmentManager, "SEARCH_RANK_DIALOG_TAG");
                            }
                        };
                        text_view_rank.Text = entity.RankName;

                        text_view_physical_therapists.Click += delegate
                        {
                            searchPhysicalTherapistDialogFragment = new SearchPhysicalTherapistDialogFragment(physicalTherapists, SearchPhysicalTherapistDialogFragment_CallBackAction);
                            searchPhysicalTherapistDialogFragment.Show(FragmentManager, "SEARCH_PHYSICAL_THERAPIST_DIALOG_TAG");
                        };
                        text_view_physical_therapists.FocusChange += (s, e) =>
                        {
                            if (e.HasFocus)
                            {
                                searchPhysicalTherapistDialogFragment = new SearchPhysicalTherapistDialogFragment(physicalTherapists, SearchPhysicalTherapistDialogFragment_CallBackAction);
                                searchPhysicalTherapistDialogFragment.Show(FragmentManager, "SEARCH_PHYSICAL_THERAPIST_DIALOG_TAG");
                            }
                        };
                        text_view_physical_therapists.Text = entity.PhysicalTherapistName;

                        edit_text_claim_number.Text = entity.ClaimNumber;
                        edit_text_claim_guarantor.Text = entity.ArabicRelativeName;
                        edit_text_military_number.Text = entity.MilitaryIdNumber;


                        //text_view_relationship.Text = "نفسه";
                        //entity.ArabicRelationDescription = "نفسه";

                        SubmitButton.Click += SubmitButton_Click;

                        radio_button_yes.Checked = entity.Stated == true;
                        radio_button_no.Checked = entity.Stated == false;

                        radio_button_yes.Click += radio_button_yes_Click;
                        radio_button_no.Click += radio_button_no_Click;


                        radio_session_place_clinic.Checked = entity.SessionsPlace == "Clinic";
                        radio_session_place_home.Checked = entity.SessionsPlace == "Home";

                        radio_session_place_clinic.Click += radio_session_place_clinic_Click;
                        radio_session_place_home.Click += radio_session_place_home_Click;

                        if (entity.ApprovalDate_Update != null)
                            edit_text_approval_date.Text = entity.ApprovalDate_Update.Value.ToString("dd/MM/yyyy");
                        edit_text_approval_date.LongClick += (s, e) =>
                        {
                            var frag = DatePickerFragment.NewInstance(delegate (DateTime onDateSelected) { edit_text_approval_date.Text = onDateSelected.ToString("dd/MM/yyyy"); }, ConvertToDateTime(edit_text_approval_date.Text));
                            frag.Show(FragmentManager, DatePickerFragment.TAG);
                        };


                        if (entity.SessionsStartDate_Update != null)
                            edit_text_session_start_date.Text = entity.SessionsStartDate_Update.Value.ToString("dd/MM/yyyy");
                        edit_text_session_start_date.LongClick += (s, e) =>
                        {
                            var frag = DatePickerFragment.NewInstance(delegate (DateTime onDateSelected) { edit_text_session_start_date.Text = onDateSelected.ToString("dd/MM/yyyy"); }, ConvertToDateTime(edit_text_session_start_date.Text));
                            frag.Show(FragmentManager, DatePickerFragment.TAG);
                        };


                        if (entity.SessionsEndDate_Update != null)
                            edit_text_session_end_date.Text = entity.SessionsEndDate_Update.Value.ToString("dd/MM/yyyy");
                        edit_text_session_end_date.LongClick += (s, e) =>
                        {
                            var frag = DatePickerFragment.NewInstance(delegate (DateTime onDateSelected) { edit_text_session_end_date.Text = onDateSelected.ToString("dd/MM/yyyy"); }, ConvertToDateTime(edit_text_session_end_date.Text));
                            frag.Show(FragmentManager, DatePickerFragment.TAG);
                        };

                        if (entity.VisitDate != null)
                            edit_text_visit_date.Text = entity.VisitDate.Value.ToString("dd/MM/yyyy");
                        edit_text_visit_date.LongClick += (s, e) =>
                        {
                            var frag = DatePickerFragment.NewInstance(delegate (DateTime onDateSelected) { edit_text_visit_date.Text = onDateSelected.ToString("dd/MM/yyyy"); }, ConvertToDateTime(edit_text_visit_date.Text));
                            frag.Show(FragmentManager, DatePickerFragment.TAG);
                        };


                        edit_text_quantity.Text = entity.SessionsCount.ToString();
                        edit_text_official_amount.Text = entity.RequiredAmount.ToString();


                    }
                    else
                    {
                        var intent = new Intent(this, typeof(ClaimsActivity));
                        StartActivity(intent);
                        Finish();
                    }
                }
                else
                {
                    var intent = new Intent(this, typeof(ClaimsActivity));
                    StartActivity(intent);
                    Finish();
                }
            }


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
                var InternalSecurityPatientClaimId = entity.InternalSecurityPatientClaimId;
                DeleteISPatientClaimAsync(InternalSecurityPatientClaimId);
            }

            return true;
        }

        private async Task DeleteISPatientClaimAsync(string InternalSecurityPatientClaimId)
        {

            var isDeleted = false;


            if (!string.IsNullOrEmpty(InternalSecurityPatientClaimId))
            {
                var progressDialog = new ProgressDialog(this);
                progressDialog.SetCancelable(false);
                progressDialog.SetMessage("Deleting Data...");

                progressDialog.Show();

                var url = string.Format("{0}/api/isclaims/{1}", Constants.Server_Url, InternalSecurityPatientClaimId);

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
                        var intent = new Intent(this, typeof(ClaimsActivity));
                        StartActivity(intent);
                        Finish();
                    }

                });
                alert.Show();
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


        private void SearchPatientDialogFragment_CallBackAction(PatientViewModel item)
        {
            if (item != null)
            {
                text_view_patient.Text = item.Name.ToString();
                entity.PatientId = item.PatientId;
            }
            else
            {
                text_view_patient.Text = string.Format("Select Patient");
                entity.PatientId = string.Empty;

            }
        }


        private void SearchRankDialogFragment_CallBackAction(RankViewModel item)
        {
            if (item != null)
            {
                text_view_rank.Text = item.ArabicMilitaryRankName;
                entity.RankId = item.MilitaryRankId;
            }
            else
            {
                text_view_rank.Text = string.Format("Select Rank");
                entity.RankId = string.Empty;
            }
        }

        private void SearchPhysicalTherapistDialogFragment_CallBackAction(PhysicalTherapistViewModel item)
        {
            if (item != null)
            {
                text_view_physical_therapists.Text = item.EnglishName;
                entity.PhysicalTherapistId = item.PhysicalTherapistId;
            }
            else
            {
                text_view_physical_therapists.Text = string.Format("Select Therapist");
                entity.PhysicalTherapistId = string.Empty;
            }
        }


        private void SearchDialogFragment_CallBackAction(string item)
        {
            if (item != null)
            {
                text_view_relationship.Text = item;
                entity.ArabicRelationDescription = item;
            }
            else
            {
                text_view_relationship.Text = string.Format("Select Relationship");
                entity.ArabicRelationDescription = string.Empty;
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
            entity.ClaimNumber = edit_text_claim_number.Text;
            entity.ArabicRelativeName = edit_text_claim_guarantor.Text;
            entity.ArabicRelationDescription = text_view_relationship.Text;
            entity.MilitaryIdNumber = edit_text_military_number.Text;


            entity.ApprovalDate_Update = ConvertToDateTime(edit_text_approval_date.Text);
            entity.SessionsStartDate_Update = ConvertToDateTime(edit_text_session_start_date.Text);
            entity.SessionsEndDate_Update = ConvertToDateTime(edit_text_session_end_date.Text);

            entity.SessionsCount = ConvertToNumber(edit_text_quantity.Text);
            entity.RequiredAmount = ConvertToNumber(edit_text_official_amount.Text);
            entity.VisitDate = ConvertToDateTime(edit_text_visit_date.Text);

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


            progressDialog.SetMessage("Sending Data...");
            progressDialog.Show();

            var url = string.Format("{0}/api/isclaims", Constants.Server_Url);

            var jsonString = JsonConvert.SerializeObject(entity);
            var result = await Constants.RestApiSerive.PutResponse<ServerResponse<string>>(url, jsonString);

            progressDialog.Hide();

            if (result != null)
            {
                if (result.valid)
                {

                    //edit_text_prescription_date.Text = string.Empty;

                    //text_view_patient.Text = string.Empty;
                    //text_view_doctor.Text = string.Empty;
                    edit_text_claim_number.Text = string.Empty;
                    edit_text_visit_date.Text = string.Empty;
                    //edit_text_max_sessions_count.Text = string.Empty;


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

            if (string.IsNullOrEmpty(entity.ClaimNumber)) { result.Add("Claim# is required"); }

            if (string.IsNullOrEmpty(entity.ArabicRelativeName)) { result.Add("Guarantor is required"); }

            if (string.IsNullOrEmpty(entity.ArabicRelationDescription)) { result.Add("Relationship is required"); }

            if (string.IsNullOrEmpty(entity.MilitaryIdNumber)) { result.Add("Military# is required"); }

            if (string.IsNullOrEmpty(entity.RankId)) { result.Add("Rank is required"); }

            if (string.IsNullOrEmpty(entity.SessionsPlace)) { result.Add("Sessions Place is required"); }

            if (entity.SessionsCount == null) { result.Add("Sessions Count is required"); }

            if (entity.RequiredAmount == null) { result.Add("Official Amount is required"); }

            if (entity.ApprovalDate_Update == null) { if (!string.IsNullOrEmpty(edit_text_approval_date.Text)) result.Add("Enter Valid Approval Date"); else result.Add("Approval Date is required"); }

            if (entity.SessionsStartDate_Update == null) { if (!string.IsNullOrEmpty(edit_text_session_start_date.Text)) result.Add("Enter Valid Sessions Start Date"); else result.Add("Sessions Start Date is required"); }

            if (entity.SessionsEndDate_Update == null) { if (!string.IsNullOrEmpty(edit_text_session_end_date.Text)) result.Add("Enter Valid Sessions End Date"); else result.Add("Sessions End Date is required"); }

            if (entity.VisitDate == null) { if (!string.IsNullOrEmpty(edit_text_visit_date.Text)) result.Add("Enter Valid Visit Date"); else result.Add("Visit Date is required"); }

            if (string.IsNullOrEmpty(entity.PhysicalTherapistId)) { result.Add("Physical Therapist is required"); }

            return result;
        }


    }
}