using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic.Language
{
    public interface AmortizationAssertHelper
    {
        void VerifyOutputTotalAccumulatedAmortizationEquals(string calculationDate, double accumulatedAmortization, AmortizationOutput output);
    }
}
