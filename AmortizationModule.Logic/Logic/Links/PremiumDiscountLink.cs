using AmortizationModule.Logic.DTO.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic
{
    public class PremiumDiscountLink : AmortizationLink
    {
        public PremiumDiscountLink(AmortizationTransaction transaction)
            : base(transaction)
        {

        }

        public override double GetPremiumDiscountAmount()
        {
            double premium = (transaction.Rate - 1) * transaction.Quantity;
            List<int> reverseTypes = new List<int>() {4, 67};
            List<int> regularTypes = new List<int>() {43, 40};
            if (reverseTypes.Contains(transaction.TransactionType))
                return -premium;
            if (regularTypes.Contains(transaction.TransactionType))
                return premium;
            return 0;
        }

        public override double GetCashFlowAmount()
        {
            return GetPremiumDiscountAmount();
        }
    }
}
