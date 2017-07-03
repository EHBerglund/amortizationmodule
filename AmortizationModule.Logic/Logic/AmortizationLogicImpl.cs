using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.Internal;

namespace AmortizationModule.Logic
{
    public class AmortizationLogicImpl : AmortizationLogic
    {
        public AmortizationOutput GetAmortizationOutput(AmortizationInput input)
        {
            return AmortizationFactory.CreateAmortizationOutput();
        }
    }
}
