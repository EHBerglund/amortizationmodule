using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic.BusinessLogic
{
    public class CashFlowNotSuitableForIRRCalculation : Exception
    {
        public CashFlowNotSuitableForIRRCalculation(string msg)
            : base(msg)
        {

        }
    }
}
