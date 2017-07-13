using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;
using AmortizationModule.Logic.DTO.Internal;

namespace AmortizationModule.Logic
{
    public class PremiumDiscountCategorizer : LinkCategorizer
    {
        public void Initialize()
        {
        }

        public List<AmortizationInitiation> ProcessTransaction(List<AmortizationInitiation> initiations, AmortizationTransaction transaction, AmortizationInput input)
        {
            if (transaction.Categorized)
                return initiations;

            if (TransactionIsPremiumDiscount(transaction))
            {
                initiations = LinkMapper.CoupleLinkToInitiation(initiations, new PremiumDiscountLink(transaction), input.Settings);
                transaction.Categorized = true;
            }
            return initiations;
        }

        private bool TransactionIsPremiumDiscount(AmortizationTransaction transaction)
        {
            return transaction.TransactionType == (int)TransactionTypeDefs.Premium
                || transaction.TransactionType == (int)TransactionTypeDefs.Discount;
        }
    }
}
