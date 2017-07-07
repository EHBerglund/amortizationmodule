using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic
{
    public class AmortizationLink
    {
        private DateTime transactionDate;

        public DateTime TransactionDate { get => transactionDate; set => transactionDate = value; }

        public AmortizationLink(DateTime transactionDate)
        {
            this.transactionDate = transactionDate;
        }

        public virtual double GetPremiumDiscountAmount()
        {
            return 0;
        }

        public virtual double GetCashFlowAmount()
        {
            return 0;
        }
    }
}
