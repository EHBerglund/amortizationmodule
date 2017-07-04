using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.External;
using AmortizationModule.Logic.DTO.Internal;

namespace AmortizationModule.Logic.Logic.Categorizers
{
    public class PremiumDiscountCategorizer : LinkCategorizer
    {
        public void Initialize()
        {
        }

        public void ProcessTransaction(List<AmortizationInitiation> initiations, AmortizationTransaction transaction, AmortizationInput input)
        {
            if (transaction.Categorized)
                return;

            if (TransactionIsPremiumDiscount(transaction))
            {
                initiations = LinkMapper.CoupleLinkToInitiation(initiations, new PremiumDiscountLink(transaction.TransactionDate), input.Settings);
                transaction.Categorized = true;
            }
        }

        private bool TransactionIsPremiumDiscount(AmortizationTransaction transaction)
        {
            return transaction.TransactionType == (int)TransactionTypeDefs.Premium
                || transaction.TransactionType == (int)TransactionTypeDefs.Discount;
        }
    }
}
