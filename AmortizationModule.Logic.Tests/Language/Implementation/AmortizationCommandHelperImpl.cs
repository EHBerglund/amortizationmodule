using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.Internal;

namespace AmortizationModule.Logic.Language.Implementation
{
    public class AmortizationCommandHelperImpl : AmortizationCommandHelper
    {
        public AmortizationOutput GenerateAmortizationOutput(AmortizationInput input)
        {
            AmortizationLogic logic = AmortizationFactory.CreateAmortizationLogic();
            return logic.GetAmortizationOutput(input);
        }
    }
}
