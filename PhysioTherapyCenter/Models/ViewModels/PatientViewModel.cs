using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.ViewModels
{
    public class PatientViewModel
    {

        public PatientViewModel()
        {
            Claims = new List<ClaimInfo>();
        }

        public string PatientId { get; set; }

        public int PatientNumber { get; set; }

        public string Name { get; set; }

        public string ArabicName { get; set; }

        public string Gender { get; set; }

        public string GenderId { get; set; }

        public string PhoneNumber_1 { get; set; }

        public string PhoneNumber_2 { get; set; }

        public DateTime? CreationDate_Update { get; set; }

        public DateTime? DateOfBirth_Update { get; set; }

        public string GuarantorInstituation { get; set; }

        public List<ClaimInfo> Claims { get; set; }


    }
}