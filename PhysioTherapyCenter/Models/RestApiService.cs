using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using PhysioTherapyCenter.Models.LoginViewModels;

namespace PhysioTherapyCenter.Models
{
    public class RestApiService
    {

        private HttpClient client;


        public AccessToken accessToken { get; set; }

        public RestApiService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        }

        public async Task<AccessToken> Login(string Email, string Password)
        {
            //System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            var postData = new List<KeyValuePair<string, string>>();

            var grant_type = "password";

            postData.Add(new KeyValuePair<string, string>("username", Email));
            postData.Add(new KeyValuePair<string, string>("password", Password));
            postData.Add(new KeyValuePair<string, string>("grant_type", grant_type));

            var content = new FormUrlEncodedContent(postData);

            var response = await PostResponse<AccessToken>(Constants.Login_Url, content);

            if (response != null)
            {
                accessToken = response;
            }
            return response;
        }

        public async Task<T> PostResponse<T>(string webUrl, FormUrlEncodedContent content) where T : class
        {
            try
            {
                var response = await client.PostAsync(webUrl, content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<T>(jsonResult);
                    return responseJson;
                }
            }
            catch (Exception ex)
            {
            }

            return null;

        }

        public async Task<T> PostResponse<T>(string webUrl, string jsonString) where T : class
        {
            try
            {
                var token = accessToken != null ? accessToken.access_token : string.Empty;

                var ContentType = "application/json";
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.PostAsync(webUrl, new StringContent(jsonString, Encoding.UTF8, ContentType));

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<T>(jsonResult);
                    return responseJson;
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        public async Task<T> GetResponse<T>(string webUrl) where T : class
        {
            try
            {
                var token = accessToken != null ? accessToken.access_token : string.Empty;
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var uri = new Uri(webUrl);
                var response = await client.GetAsync(uri);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<T>(jsonResult);
                    return responseJson;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }


        public async Task<T> PutResponse<T>(string webUrl, string jsonString) where T : class
        {
            try
            {
                var token = accessToken != null ? accessToken.access_token : string.Empty;

                var ContentType = "application/json";
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.PutAsync(webUrl, new StringContent(jsonString, Encoding.UTF8, ContentType));

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<T>(jsonResult);
                    return responseJson;
                }
            }
            catch (Exception)
            {
            }

            return null;
        }


        public async Task<T> DeleteResponse<T>(string webUrl) where T : class
        {
            try
            {
                var token = accessToken != null ? accessToken.access_token : string.Empty;

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.DeleteAsync(webUrl);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<T>(jsonResult);
                    return responseJson;
                }
            }
            catch (Exception)
            {
            }

            return null;
        }
    }
}