using AmortizationModule.Logic.DTO.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic
{
    public class AmortizationLink
    {
        protected AmortizationTransaction transaction;

        public virtual bool TriggerRecalculationWithIRR
        {
            get
            {
                return false;
            }
        }

        public DateTime LinkDate
        {
            get
            {
                return transaction.TransactionDate;
            }
            set
            {
                this.transaction.TransactionDate = value;
            }
        }
        public AmortizationLink(AmortizationTransaction transaction)
        {
            this.transaction = transaction;
        }

        public virtual double GetPremiumDiscountAmount()
        {
            return 0;
        }

        public virtual double GetCashFlowAmount()
        {
            return 0;
        }

        public virtual double GetInstalmentAmount()
        {
            return 0;
        }
    }
}
