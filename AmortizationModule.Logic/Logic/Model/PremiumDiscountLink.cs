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
            : base(transaction.TransactionDate)
        {

        }
    }
}
