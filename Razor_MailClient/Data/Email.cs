using System;
using System.ComponentModel.DataAnnotations;

namespace Razor_MailClient.Data
{
    public class Email
    {
        public int ID { get; set; }

        [Display(Name = "To")]
        public string TO { get; set; }

        [Display(Name = "From")]
        public string FROM { get; set; }

        [Display(Name = "Subject")]
        public string SUBJECT { get; set; }

        [Display(Name = "Body")]
        public string BODY { get; set; }

        [Display(Name = "Date")]
        public DateTime DATE_RECEIVED { get; set; }

        public bool Selected { get; set; }
    }
}
