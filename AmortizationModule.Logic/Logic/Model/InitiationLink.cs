using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    public class InitiationLink : AmortizationLink
    {
        public InitiationLink(AmortizationTransaction transaction) : base(transaction)
        {
        }

        public override double GetInstalmentAmount()
        {
            return -TransactionSetup.QuantityEffect(transaction);
        }

        public override double GetCashFlowAmount()
        {
            return -TransactionSetup.QuantityEffect(transaction);
        }
    }
}
