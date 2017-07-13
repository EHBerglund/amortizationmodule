using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    public class LoanCostLink : AmortizationLink
    {
        public LoanCostLink(AmortizationTransaction transaction) : base(transaction)
        {
        }

        public override double GetCashFlowAmount()
        {
            return -transaction.Quantity * transaction.Rate;
        }

        public override double GetPremiumDiscountAmount()
        {
            return -transaction.Quantity * transaction.Rate;
        }

        public override bool TriggerRecalculationWithIRR => true;
    }
}
