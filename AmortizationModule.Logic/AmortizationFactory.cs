using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic
{
    public static class AmortizationFactory
    {
        public static AmortizationLogic CreateAmortizationLogic()
        {
            return new AmortizationLogicImpl();
        }

        static AmortizationOutput CreateAmortizationOutput()
        {
            return new AmortizationOutputImpl();
        }
    }
}
