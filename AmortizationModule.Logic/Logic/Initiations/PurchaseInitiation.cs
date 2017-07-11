using AmortizationModule.Logic.DTO.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic
{
    public class PurchaseInitiation : AmortizationInitiation
    {
        public PurchaseInitiation(AmortizationTransaction transaction)
            : base(transaction)
        {

        }

        
    }
}
