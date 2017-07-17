using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    public class RecalculateByExtraordinaryInstallmentLink : AmortizationLink
    {
        public override bool TriggerRecalculationWithOldIRR => true;
        public RecalculateByExtraordinaryInstallmentLink(AmortizationTransaction transaction) : base(transaction)
        {
        }
    }
}
