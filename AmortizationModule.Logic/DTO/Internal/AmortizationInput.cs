using AmortizationModule.Logic.DTO.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic.DTO.Internal
{
    public class AmortizationInput
    {
        public List<AmortizationCorrection> AmortizationCorrections { get; set; } 
        public List<AmortizationTransaction> AmortizationTransactions { get; set; }
        public AmortizationSecurity AmortizationSecurity { get; set; }
        public List<InterestRate> InterestRates { get; set; }
        public AmortizationSettings Settings { get; set; }
        public UserAmortizationInput UserInput { get; set; }
    }
}
