using AmortizationModule.Logic.DTO.External;
using AmortizationModule.Logic.DTO.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic
{
    public interface LinkCategorizer
    {
        void Initialize();
        List<AmortizationInitiation> ProcessTransaction(List<AmortizationInitiation> initiations, AmortizationTransaction transaction, AmortizationInput input);
    }
}
