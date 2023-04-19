using JobPortal.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class ApplicantData
    {
        [Key]
        public int Id { get; set; }
        public int PreviousExperience { get; set; }
        public JobPortalUser user { get; set; }
        public JobDetails JobApplied { get; set; }
    }
}
