using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.External;
using AmortizationModule.Logic.DTO.Internal;

namespace AmortizationModule.Logic
{
    public class InstalmentCategorizer : LinkCategorizer
    {
        private List<int> installmentTransactions;
        public void Initialize()
        {
            installmentTransactions = new List<int>()
            {
                44, 68
            };
        }

        public List<AmortizationInitiation> ProcessTransaction(List<AmortizationInitiation> initiations, AmortizationTransaction transaction, AmortizationInput input)
        {
            if (IsInstalment(transaction))
            {
                initiations = LinkMapper.CoupleLinkToInitiation(initiations, new InstallmentLink(transaction), input.Settings);
            }
            return initiations;
        }

        private bool IsInstalment(AmortizationTransaction transaction)
        {
            return installmentTransactions.Contains(transaction.TransactionType);
        }
    }
}
