using AmortizationModule.Logic.DTO.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic
{
    public interface AmortizationCalculator
    {
        void Initialize(AmortizationInput input, AmortizationInitiation initiation);
        double CalculateAccumulatedAmortization(AmortizationInitiation initiation, DateTime calculationDate);
    }
}
