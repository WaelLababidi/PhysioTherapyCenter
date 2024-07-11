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

namespace PhysioTherapyCenter.Models.ViewModels
{
    public class AppointmentViewModel
    {

        public string PatientId { get; set; }

        public string PatientName { get; set; }

        public string PhoneNumber_1 { get; set; }

        public string PhoneNumber_2 { get; set; }



        public string ClaimId { get; set; }

        public string ClaimDescription { get; set; }



        public DateTime? AppointmentDate { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public int? Amount { get; set; }

        public bool Confirmed { get; set; }

        public string Note { get; set; }

    }
}