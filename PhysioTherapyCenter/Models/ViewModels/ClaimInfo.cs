using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.ViewModels
{
    public class ClaimInfo
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string Guarantor { get; set; }
        public string ClaimId { get; set; }
        public string ClaimNumber { get; set; }
        public string ClaimType { get; set; }
        public DateTime ApprovalDate { get; set; }
        public int OfficialAmount { get; set; }
        public int UnOfficialAmount { get; set; }
        public bool Stated { get; set; }
    }
}