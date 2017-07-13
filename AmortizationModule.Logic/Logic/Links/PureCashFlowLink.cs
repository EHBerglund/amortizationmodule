using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    public class PureCashFlowLink : AmortizationLink
    {
        private double amount;
        public PureCashFlowLink(AmortizationTransaction transaction, bool positive) : base(transaction)
        {
            if (positive)
            {
                amount = transaction.Rate * transaction.Quantity; 
            }
            else
            {
                amount = -transaction.Rate * transaction.Quantity;
            }
        }

        public override double GetCashFlowAmount()
        {
            return amount;
        }
    }
}
