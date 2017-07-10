using AmortizationModule.Logic.DTO.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmortizationModule.Logic
{
    public class TransactionSetup
    {
        public static double QuantityEffect(AmortizationTransaction transaction)
        {
            return QuantityEffectAsInteger(transaction.TransactionType)*transaction.Quantity;
        }
        private static int QuantityEffectAsInteger(AmortizationTransaction transaction)
        {
            return QuantityEffectAsInteger(transaction.TransactionType);
        }
        private static int QuantityEffectAsInteger(int TransactionType)
        {
            List<int> positiveEffects = new List<int>()
            {
                4, 30, 41, 44, 67
            };
            List<int> negativeEffects = new List<int>()
            {
                68, 63, 43, 40, 6
            };

            if (positiveEffects.Contains(TransactionType))
                return 1;
            if (negativeEffects.Contains(TransactionType))
                return -1;
            return 0;
        }
    }
}
