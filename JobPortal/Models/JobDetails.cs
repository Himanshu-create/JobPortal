using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public enum JobDomain
    {
        WebDev,
        AppDev,
        Devops,
        Backend,
        Testing,
        QA,
        DataAnalytics
    }
    public class JobDetails
    {
        [Key]
        public int Id { get; set; }
        public string jobName { get; set; }
        public JobDomain Domain { get; set; }
        public DateTime LastDateOfRegistration { get; set; }
    }
}
