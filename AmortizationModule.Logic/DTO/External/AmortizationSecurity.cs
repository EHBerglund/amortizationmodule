﻿using System;
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
        private List<Installment> instalments;
        public List<Installment> Instalments
        {
            get
            {
                return instalments == null ? new List<Installment>() : instalments;
            }
            set
            {
                instalments = value;
            }
        }
        private Dictionary<DateTime, double> interestTerms;
        public Dictionary<DateTime, double> InterestTerms
        {
            get
            {
                return interestTerms == null ? new Dictionary<DateTime, double>() : interestTerms;
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
