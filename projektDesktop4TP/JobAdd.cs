using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektDesktop4TP
{
    public class JobAdd
    {
        public int Id { get; set; }
        public string JobName { get; set; }
        public string AgreementType { get; set; }
        public string WorkType { get; set; }
        public decimal Payment { get; set; }
        public string WorkDays { get; set; }
        public string WorkHours { get; set; }
        public string Responsibilities { get; set; }
        public string Requirements { get; set; }
        public string Benefits { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
