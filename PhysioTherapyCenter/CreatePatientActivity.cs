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
using FR.Ganfra.Materialspinner;
using Newtonsoft.Json;
using PhysioTherapyCenter.Models;
using PhysioTherapyCenter.Models.Adapters;
using PhysioTherapyCenter.Models.Entities;
using PhysioTherapyCenter.Models.Fragments;
using PhysioTherapyCenter.Models.Interfaces;
using SharedEntities;

namespace PhysioTherapyCenter
{
    //[Activity(Label = "PatientsActivity")]
    [Activity(Label = "Create Patient", Theme = "@style/AppTheme.NoActionBar")]
    public class CreatePatientActivity : BaseDrawerActivity
    {
        private EditText edit_text_patient_name;
        private EditText edit_text_patient_arabic_name;
        private EditText edit_text_patient_phone_number_1;
        private EditText edit_text_patient_phone_number_2;
        private EditText edit_text_patient_creation_date;
        private EditText edit_text_patient_dob;
        private EditText edit_text_guarantor;

        protected Button SubmitButton;


        private PatientViewModel entity = new PatientViewModel { };

        private RadioButton radio_button_male;
        private RadioButton radio_button_female;

        private SearchGuarantorDialogFragment searchGuarantorDialogFragment;
        private FragmentManager fragmentManager;

        private List<GuarantorViewModel> guarantors;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.create_patient);

            Init();

            navigationView.SetCheckedItem(Resource.Id.nav_patients);

            edit_text_patient_name = FindViewById<EditText>(Resource.Id.edit_text_patient_name);
            edit_text_patient_arabic_name = FindViewById<EditText>(Resource.Id.edit_text_patient_arabic_name);
            edit_text_patient_phone_number_1 = FindViewById<EditText>(Resource.Id.edit_text_patient_phone_number_1);
            edit_text_patient_phone_number_2 = FindViewById<EditText>(Resource.Id.edit_text_patient_phone_number_2);
            edit_text_patient_creation_date = FindViewById<EditText>(Resource.Id.edit_text_patient_creation_date);
            edit_text_patient_dob = FindViewById<EditText>(Resource.Id.edit_text_patient_dob);
            edit_text_guarantor = FindViewById<EditText>(Resource.Id.edit_text_guarantor);
            //genders = new List<string>() { "Male", "Female" };
            //genders_ids_list = new List<string>() { "7a09be33ebad453b8c3a5830255af72b", "8b6d6b7ccff944d4b807ed3942ebc63d" };


            guarantors = new List<GuarantorViewModel>()
            {
                new GuarantorViewModel{ Value="0",Text="GS"},
                new GuarantorViewModel{ Value="1",Text="IS"},
                new GuarantorViewModel{ Value="2",Text="Coop"},
                new GuarantorViewModel{ Value="3",Text="GlobeMed"},
                new GuarantorViewModel{ Value="4",Text="Nssf"},
                new GuarantorViewModel{ Value="5",Text="Private"},
            };

            //guarantors = new List<string>() { "GS", "IS", "Coop", "GlobeMed", "Nssf", "Private" };
            //guarantors_ids_list = new List<string>() { "0", "1", "2", "3", "4", "5" };


            SubmitButton = FindViewById<Button>(Resource.Id.button_create_patient);
            SubmitButton.Click += SubmitButton_Click;


            radio_button_male = FindViewById<RadioButton>(Resource.Id.radio_button_male);
            //radio_button_male.Checked = entity.GenderId == "7a09be33ebad453b8c3a5830255af72b";

            radio_button_female = FindViewById<RadioButton>(Resource.Id.radio_button_female);
            //radio_button_female.Checked = entity.GenderId == "8b6d6b7ccff944d4b807ed3942ebc63d";

            edit_text_patient_creation_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            edit_text_patient_creation_date.LongClick += (s, e) =>
            {
                var frag = DatePickerFragment.NewInstance(delegate (DateTime onDateSelected) { edit_text_patient_creation_date.Text = onDateSelected.ToString("dd/MM/yyyy"); }, ConvertToDateTime(edit_text_patient_creation_date.Text));
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };


            entity.GenderId = "7a09be33ebad453b8c3a5830255af72b";
            radio_button_male.Click += radio_button_male_Click;
            radio_button_female.Click += radio_button_female_Click;


            edit_text_guarantor.Click += delegate
            {
                searchGuarantorDialogFragment = new SearchGuarantorDialogFragment(guarantors, SearchGuarantorDialogFragment_CallBackAction);
                searchGuarantorDialogFragment.Show(FragmentManager, "SEARCH_GUARANTOR_DIALOG_TAG");
            };

            edit_text_guarantor.FocusChange += (s, e) =>
            {
                if (e.HasFocus)
                {
                    searchGuarantorDialogFragment = new SearchGuarantorDialogFragment(guarantors, SearchGuarantorDialogFragment_CallBackAction);
                    searchGuarantorDialogFragment.Show(FragmentManager, "SEARCH_GUARANTOR_DIALOG_TAG");
                }
            };


            Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

        }

        private void SearchGuarantorDialogFragment_CallBackAction(GuarantorViewModel item)
        {
            if (item != null)
            {
                edit_text_guarantor.Text = item.Text;
                entity.GuarantorInstituation = item.Value;
            }
            else
            {
                edit_text_guarantor.Text = string.Format("Select Guarantor");
                entity.GuarantorInstituation = string.Empty;

            }
        }

        private void radio_button_male_Click(object sender, EventArgs e)
        {
            entity.GenderId = "7a09be33ebad453b8c3a5830255af72b";
        }

        private void radio_button_female_Click(object sender, EventArgs e)
        {
            entity.GenderId = "8b6d6b7ccff944d4b807ed3942ebc63d";
        }



        protected async void SubmitButton_Click(object sender, EventArgs e)
        {

            entity.Name = edit_text_patient_name.Text;
            entity.ArabicName = edit_text_patient_arabic_name.Text;
            entity.PhoneNumber_1 = edit_text_patient_phone_number_1.Text;
            entity.PhoneNumber_2 = edit_text_patient_phone_number_2.Text;
            entity.CreationDate_Update = ConvertToDateTime(edit_text_patient_creation_date.Text);
            entity.DateOfBirth_Update = ConvertToDateTime(edit_text_patient_dob.Text);

            //entity.Gender = spinner_gender.SelectedItem.ToString();
            //entity.GenderId = genders_ids_list[spinner_gender.SelectedItemPosition];

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

            var url = string.Format("{0}/api/patients", Constants.Server_Url);

            var jsonString = JsonConvert.SerializeObject(entity);
            var result = await Constants.RestApiSerive.PostResponse<ServerResponse<string>>(url, jsonString);

            progressDialog.Hide();

            if (result != null)
            {
                if (result.valid)
                {

                    edit_text_patient_name.Text = string.Empty;
                    edit_text_patient_arabic_name.Text = string.Empty;
                    edit_text_patient_phone_number_1.Text = string.Empty;
                    edit_text_patient_phone_number_2.Text = string.Empty;
                    edit_text_patient_creation_date.Text = string.Empty;
                    edit_text_patient_dob.Text = string.Empty;

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
                    var intent = new Intent(this, typeof(PatientsActivity));
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
                var intent = new Intent(this, typeof(PatientsActivity));
                StartActivity(intent);
                Finish();
                //base.OnBackPressed();
            }
        }




        List<string> ValidateModel()
        {

            var result = new List<string>();

            if (string.IsNullOrEmpty(entity.Name)) { result.Add("Name is required"); }

            if (string.IsNullOrEmpty(entity.ArabicName)) { result.Add("Arabic Name is required"); }

            if (string.IsNullOrEmpty(entity.GenderId)) { result.Add("Gender is required"); }

            if (string.IsNullOrEmpty(entity.GuarantorInstituation)) { result.Add("Guarantor is required"); }

            if (string.IsNullOrEmpty(edit_text_patient_creation_date.Text))
            {
                result.Add("Creation Date is required");
            }
            else
            {
                if (entity.CreationDate_Update == null)
                {
                    result.Add("Enter a valid Creation Date");
                }
            }

            if (!string.IsNullOrEmpty(edit_text_patient_dob.Text))
            {
                if (entity.DateOfBirth_Update == null)
                {
                    result.Add("Enter a valid DOB Date");
                }
            }

            return result;
        }




        //List<string> _ValidateModel(PatientViewModel entity)
        //{

        //    var result = new List<string>();

        //    var editTextName = FindViewById<EditText>(Resource.Id.edit_text_patient_name);

        //    //if (string.IsNullOrEmpty(editTextName.Text))
        //    //{
        //    //    editTextName.SetHighlightColor(Color.Red);
        //    //    result.Add("Name is required");
        //    //}
        //    //else
        //    //{
        //    //    editTextName.requestFocus();
        //    //    editTextName.setError("FIELD CANNOT BE EMPTY");
        //    //}
        //    //var errorIcon = Resources.GetDrawable(Resource.Drawable.Icon);
        //    //var correctIcon = Resources.GetDrawable(Resource.Drawable.Icon_sy);

        //    //if (string.IsNullOrEmpty(editTextName.Text))
        //    //{
        //    //    //change icon to red
        //    //    editTextName.SetCompoundDrawablesWithIntrinsicBounds(Resource.Drawable.baseline_person_black_24, 0, Resource.Drawable.delete_mark_24, 0);
        //    //}
        //    //else
        //    //{
        //    //    //change icon to green
        //    //    editTextName.SetCompoundDrawablesWithIntrinsicBounds(Resource.Drawable.baseline_person_black_24, 0, Resource.Drawable.check_mark_24, 0);

        //    //}




        //    return result;
        //}
    }
}