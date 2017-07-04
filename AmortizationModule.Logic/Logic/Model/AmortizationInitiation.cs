using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic
{
    public abstract class AmortizationInitiation
    {
        private DateTime transactionDate;

        public DateTime TransactionDate { get => transactionDate; set => transactionDate = value; }

        private List<AmortizationLink> links;
        public AmortizationInitiation(DateTime transactionDate)
        {
            this.TransactionDate = transactionDate;
        }

        public void AddLink(AmortizationLink link)
        {
            if (links == null)
                links = new List<AmortizationLink>();
            links.Add(link);
        }
    }
}
