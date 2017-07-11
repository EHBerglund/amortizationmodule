using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    public class RecalculateByInterestChangeLink : AmortizationLink
    {
        public RecalculateByInterestChangeLink(AmortizationTransaction transaction) : base(transaction)
        {
        }
        public override bool TriggerRecalculation => true;
    }
}
