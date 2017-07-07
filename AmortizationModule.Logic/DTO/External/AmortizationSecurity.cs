using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic.DTO.External
{
    public class AmortizationSecurity
    {
        public int SecuritySeq { get; set; }
        public int SecurityType { get; set; }
        public DateTime MaturityDate { get; set; }
        public bool Floater { get; set; }
        public string Currency { get; set; }
        public bool IsIssue { get; set; }
        private Dictionary<DateTime, double> instalments;
        public Dictionary<DateTime, double> Instalments
        {
            get
            {
                return instalments == null ? new Dictionary<DateTime, double>() : instalments;
            }
            set
            {
                instalments = value;
            }
        }
        private List<DateTime> interestTerms;
        public List<DateTime> InterestTerms
        {
            get
            {
                return interestTerms == null ? new List<DateTime>() : interestTerms;
            }
            set
            {
                interestTerms = value;
            }
        }

        public AmortizationSecurity()
        {
            SecuritySeq = 1;
            SecurityType = 2;
            Floater = false;
            Currency = "NOK";
            IsIssue = false;
        }
    }
}
