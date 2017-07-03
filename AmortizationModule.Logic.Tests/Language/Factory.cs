using AmortizationModule.Logic.Language.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic.Language
{
    public static class Factory
    {
        public static AmortizationCreateHelper CreateAmortizationCreateHelper()
        {
            return new AmortizationCreateHelperImpl();
        }

        public static AmortizationAssertHelper CreateAmortizationAssertHelper()
        {
            return new AmortizationAssertHelperImpl();
        }

        public static AmortizationCommandHelper CreateAmortizationCommandHelper()
        {
            return new AmortizationCommandHelperImpl();
        }
    }
}
