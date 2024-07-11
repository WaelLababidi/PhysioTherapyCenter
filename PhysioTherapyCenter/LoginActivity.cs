using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PhysioTherapyCenter.Models.LoginViewModels;
using PhysioTherapyCenter.Models;
using PhysioTherapyCenter.Models.Entities;

namespace PhysioTherapyCenter
{
    //[Activity(Label = "LoginActivity")]
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class LoginActivity : Activity
    {



        EditText EmailEditText;
        EditText PasswordEditText;
        Button LoginButton;
        TextView ForgetPasswordTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);




            EmailEditText = FindViewById<EditText>(Resource.Id.txtEmail);
            PasswordEditText = FindViewById<EditText>(Resource.Id.txtPassword);
            LoginButton = FindViewById<Button>(Resource.Id.login_btn);
            ForgetPasswordTextView = FindViewById<TextView>(Resource.Id.forget_password_textView);

            LoginButton.Click += LoginBtn_Click;

            var user = Constants.DatabaseManager.GetDefaultUser();

            if (user != null)
            {
                EmailEditText.Text = user.Email;
                PasswordEditText.Text = user.Password;

                //if (user.ValidateAccessToken())
                //{
                //    Constants.ApplicationUser = user;
                //    Constants.Token = user.AccessToken;

                //    StartActivity(typeof(MainActivity));
                //    Finish();
                //}
            }

            ForgetPasswordTextView.Click += ForgetPasswordTextView_Click;

        }

        private void ForgetPasswordTextView_Click(object sender, EventArgs e)
        {
            //StartActivity(typeof(MainActivity));
        }

        [Obsolete]
        private async void LoginBtn_Click(object sender, EventArgs e)
        {
            var progressDialog = BuildProgressDialog("Waiting Login...");
            try
            {
                var model = new LoginViewModel();
                model.Email = EmailEditText.Text;
                model.Password = PasswordEditText.Text;

                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    ShowAlertMessage("Login Failed", "email or password is incorrect");
                    return;
                }
                else
                {
                    progressDialog.Show();

                    var result = await Constants.RestApiSerive.Login(model.Email, model.Password);

                    if (result != null)
                    {
                        var user = Constants.DatabaseManager.GetUser(model.Email);
                        if (user == null)
                        {
                            user = new ApplicationUser
                            {
                                Email = model.Email,
                                Password = model.Password,
                                AccessToken = result.access_token,
                                AccessTokenExpiryDate = result.expires
                            };
                            int id = Constants.DatabaseManager.InsertUser(user);
                            user.Id = id;
                        }
                        else
                        {
                            user.AccessToken = result.access_token;
                            user.AccessTokenExpiryDate = result.expires;
                            int id = Constants.DatabaseManager.UpdateUser(user);
                        }

                        Constants.ApplicationUser = user;

                        Constants.Token = result.access_token;
                        StartActivity(typeof(MainActivity));
                        Finish();
                    }
                    else
                    {
                        ShowAlertMessage("Login Failed", "email or password is incorrect");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowAlertMessage("Login Failed", "email or password is incorrect");
            }
            finally
            {
                progressDialog.Hide();
            }
        }

        private void ShowAlertMessage(string title, string message)
        {
            var dialog = new AlertDialog.Builder(this);
            var alert = dialog.Create();

            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetButton("Ok", (c, ev) =>
            {
                // Ok button click task  
            });
            alert.Show();
        }

        //[Obsolete]
        private ProgressDialog BuildProgressDialog(string message, bool Cancelable = false)
        {
            var progressDialog = new ProgressDialog(this);
            progressDialog.SetCancelable(Cancelable);
            progressDialog.SetMessage(message);
            return progressDialog;
        }

    }


    #region Comment

    //private async void LoginBtn_Click(object sender, EventArgs e)
    //{
    //    var model = new LoginViewModel();
    //    model.Email = EmailEditText.Text;
    //    model.Password = PasswordEditText.Text;
    //    HttpClient client = new HttpClient();
    //    var url = string.Format("{0}/token", Constants.Server_Url);
    //    var uri = new Uri(url);
    //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



    //    var content = new FormUrlEncodedContent(new[]
    //    {
    //            new KeyValuePair<string, string>("grant_type", "password"),
    //            new KeyValuePair<string, string>("username", model.Email),
    //            new KeyValuePair<string, string>("password", model.Password),
    //        });

    //    HttpResponseMessage response = await client.PostAsync(url, content);

    //    //HttpResponseMessage response = await client.GetAsync(uri, content);
    //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
    //    {
    //        var json = await response.Content.ReadAsStringAsync();
    //        var result = JsonConvert.DeserializeObject<AccessToken>(json);

    //        if (result != null)
    //        {
    //            Constants.Token = result.access_token;
    //            StartActivity(typeof(PatientsActivity));
    //        }
    //    }
    //    else
    //    {

    //    }

    //}


    //private async void LoginBtn_Click(object sender, EventArgs e)
    //{
    //    HttpClient client = new HttpClient();
    //    var uri = new Uri(string.Format("http://xamarinlogin.azurewebsites.net/api/Login?username=" + txtusername.Text + "&password=" + txtPassword.Text));
    //    HttpResponseMessage response; ;
    //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //    response = await client.GetAsync(uri);
    //    if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
    //    {
    //        var errorMessage1 = response.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1]
    //        {
    //        '"'
    //        });
    //        Toast.MakeText(this, errorMessage1, ToastLength.Long).Show();
    //    }
    //    else
    //    {
    //        var errorMessage1 = response.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1]
    //        {
    //        '"'
    //        });
    //        Toast.MakeText(this, errorMessage1, ToastLength.Long).Show();
    //    }
    //}
    //private void Btncreate_Click(object sender, EventArgs e)
    //{
    //    StartActivity(typeof(NewUserActivity));
    //}


    #endregion

}