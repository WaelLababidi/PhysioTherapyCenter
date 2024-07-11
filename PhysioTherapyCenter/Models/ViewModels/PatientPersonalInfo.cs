using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models.ViewModels
{
    public class PatientPersonalInfo
    {

        public string PatientId { get; set; }

        public int PatientNumber { get; set; }

        public string Name { get; set; }

        public string ArabicName { get; set; }

        public string PhoneNumber_1 { get; set; }

        public string PhoneNumber_2 { get; set; }

        public string GuarantorInstituation { get; set; }
    }
}