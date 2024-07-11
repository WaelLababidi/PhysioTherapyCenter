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

namespace PhysioTherapyCenter.Models
{
    public class ServerResponse<T> where T : class
    {

        public bool valid { get; set; }
        public T value { get; set; }
    }
}