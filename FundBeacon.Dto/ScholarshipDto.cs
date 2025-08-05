using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundBeacon.Dto
{
    public class ScholarshipDto
    {
        public string Title { get; set; }
        public string About { get; set; }
        public string Eligibility { get; set; }
        public DateTime Deadline { get; set; }
        public string Benefits { get; set; }
        public string DocumentsRequired { get; set; }
        public string HowCanYouApply { get; set; }
        public string ContactUs { get; set; }
        public string Disclaimer { get; set; }
        public int ProviderId { get; set; }
    }
}
