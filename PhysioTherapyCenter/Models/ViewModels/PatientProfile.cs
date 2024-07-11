
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApi.Models.ViewModels
{
    public class PatientProfile 
    {
        public PatientProfile()
        {
            PersonalInfo = new PatientPersonalInfo();
            Claims = new List<ClaimInfo>();
        }
        public PatientPersonalInfo PersonalInfo { get; set; }
        public List<ClaimInfo> Claims { get; set; }

    }
}