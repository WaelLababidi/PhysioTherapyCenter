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
using Newtonsoft.Json;

namespace PhysioTherapyCenter.Models
{
    public class AccessToken
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public int expires_in { get; set; }

        public string userName { get; set; }

        [JsonProperty(".issued")]
        public DateTime issued { get; set; }

        [JsonProperty(".expires")]
        public DateTime expires { get; set; }

    }
}