using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmortizationModule.Logic.DTO.Internal;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    public class AmortizationLogicImpl : AmortizationLogic
    {
        public AmortizationOutput GetAmortizationOutput(AmortizationInput input)
        {
            input = SetBasicMeasuresAndFilterIrrelevantTransactions(input);
            List<AmortizationTransactionOutput> outputTransactions = GenerateAmortizationTransactionsOutput(input);
            AmortizationOutput output = new AmortizationOutputImpl();

            return output;
        }

        private AmortizationInput SetBasicMeasuresAndFilterIrrelevantTransactions(AmortizationInput input)
        {
            AmortizationFilter filter = new AmortizationFilter();
            input = filter.SetBasicMeasuresAndFilterIrrelevantTransactions(input);
            return input;
        }

        private List<AmortizationTransactionOutput> GenerateAmortizationTransactionsOutput(AmortizationInput filteredInput)
        {
            List<AmortizationTransactionOutput> transactions = new List<AmortizationTransactionOutput>();
            List<AmortizationInitiation> initiations = CategorizeInitiations(filteredInput);
            initiations = CategorizeLinks(initiations, filteredInput);
            return transactions;
        }

        private List<AmortizationInitiation> CategorizeInitiations(AmortizationInput input)
        {
            List<AmortizationInitiation> initiations = new List<AmortizationInitiation>();
            InitiationCategorizer[] categorizers = GetInitiationCategorizerRules();
            foreach (AmortizationTransaction transaction in input.AmortizationTransactions)
            {
                foreach (InitiationCategorizer categorizer in categorizers)
                {
                    categorizer.ProcessTransaction(initiations, transaction, input);
                }
            }
            return initiations;
        }

        private List<AmortizationInitiation> CategorizeLinks(List<AmortizationInitiation> initiations, AmortizationInput input)
        {
            return initiations;
        }

        private InitiationCategorizer[] GetInitiationCategorizerRules()
        {
            InitiationCategorizer[] categorizers = new InitiationCategorizer[1];
            categorizers[0] = new PurchaseBondCategorizer();
            return categorizers;
        }
    }
}
