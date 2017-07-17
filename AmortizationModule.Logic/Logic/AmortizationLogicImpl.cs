using System;
using System.Collections.Generic;
using AmortizationModule.Logic.DTO.Internal;
using AmortizationModule.Logic.DTO.External;
using System.Linq;

namespace AmortizationModule.Logic
{
    public class AmortizationLogicImpl : AmortizationLogic
    {
        AmortizationInput input;

        public AmortizationOutput GetAmortizationOutput(AmortizationInput Input)
        {
            this.input = Input;
            SetBasicMeasuresAndFilterIrrelevantTransactions();
            AddFutureBondStructure();
            List<AmortizationTransactionOutput> outputTransactions = GenerateAmortizationTransactionsOutput();
            AmortizationOutput output = new AmortizationOutputImpl();
            output.AddTransactionsList(outputTransactions);
            return output;
        }

        private void SetBasicMeasuresAndFilterIrrelevantTransactions()
        {
            AmortizationFilter filter = new AmortizationFilter();
            input = filter.SetBasicMeasuresAndFilterIrrelevantTransactions(input);
        }

        private void AddFutureBondStructure()
        {
            BondStructure structure = new BondStructure();
            input = structure.AddInstalments(input);
            input = structure.AddInterestTerms(input);
        }

        private List<AmortizationTransactionOutput> GenerateAmortizationTransactionsOutput()
        {
            List<AmortizationTransactionOutput> transactions = new List<AmortizationTransactionOutput>();
            List<AmortizationInitiation> initiations = CategorizeInitiations();
            initiations = CategorizeLinks(initiations);
            initiations = GenerateRecalculationLinks(initiations);
            initiations = GenerateRecalculationLinksWithoutIRR(initiations);
            transactions = GenerateAmortizationOutputTransactions(initiations);
            return transactions;
        }

        private List<AmortizationInitiation> GenerateRecalculationLinksWithoutIRR(List<AmortizationInitiation> initiations)
        {
            List<DateTime> addedRecalculations = new List<DateTime>();
            foreach (AmortizationInitiation initiation in initiations)
            {
                foreach (Installment installment in input.AmortizationSecurity.Instalments.Where(i => i.RegisteredDate > initiation.TransactionDate).OrderBy(i=> i.RegisteredDate))
                {
                    if (!addedRecalculations.Contains(installment.InstallmentDate))
                    {
                        initiation.AddLink(new RecalculateByExtraordinaryInstallmentLink(new AmortizationTransaction()
                        {
                            TransactionDate = installment.InstallmentDate,
                        }));
                        addedRecalculations.Add(installment.InstallmentDate);
                    }
                }
            }
            return initiations;
        }

        private List<AmortizationInitiation> CategorizeInitiations()
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

        private List<AmortizationInitiation> CategorizeLinks(List<AmortizationInitiation> initiations)
        {
            LinkCategorizer[] categorizers = GetLinkCategorizerRules();
            foreach (AmortizationTransaction transaction in input.AmortizationTransactions)
            {
                foreach (LinkCategorizer categorizer in categorizers)
                {
                    initiations = categorizer.ProcessTransaction(initiations, transaction, input);
                }
            }
            return initiations;
        }

        private List<AmortizationInitiation> GenerateRecalculationLinks(List<AmortizationInitiation> initiations)
        {
            int position = input.UserInput.PositionSeq;
            string currency = input.AmortizationSecurity.Currency;

            List<InterestRate> rates = input.InterestRates;
            foreach (InterestRate rate in rates)
            {
                foreach (AmortizationInitiation initiation in initiations)
                {
                    initiation.AddLink(new RecalculateByInterestChangeLink(new AmortizationTransaction()
                    {
                        Currency = currency,
                        Position = position,
                        Categorized = true,
                        Quantity = 0,
                        Rate = 0,
                        TransactionSeq = -1,
                        TransactionType = -1,
                        TransactionDate = rate.Date
                    }));
                }
            }
            return initiations;
        }

        private InitiationCategorizer[] GetInitiationCategorizerRules()
        {
            InitiationCategorizer[] categorizers = new InitiationCategorizer[3];
            categorizers[0] = new PurchaseBondCategorizer();
            categorizers[1] = new LoanCategorizer();
            categorizers[2] = new IssueLoanCategorizer();
            foreach (InitiationCategorizer categorizer in categorizers)
            {
                categorizer.Initialize();
            }
            return categorizers;
        }

        private LinkCategorizer[] GetLinkCategorizerRules()
        {
            LinkCategorizer[] categorizers = new LinkCategorizer[6];
            categorizers[0] = new PremiumDiscountCategorizer();
            categorizers[1] = new InterestCategorizer();
            categorizers[2] = new InstalmentCategorizer();
            categorizers[3] = new LoanIncomeCategorizer();
            categorizers[4] = new LoanCostCategorizer();
            categorizers[5] = new PureCashFlowCategorizer();
            foreach (LinkCategorizer categorizer in categorizers)
            {
                categorizer.Initialize();
            }
            return categorizers;
        }

        private List<AmortizationTransactionOutput> GenerateAmortizationOutputTransactions(List<AmortizationInitiation> initiations)
        {
            int positionSeq = input.UserInput.PositionSeq;
            DateTime calculationDate = input.UserInput.CalculationDate;
            List<AmortizationTransactionOutput> output = new List<AmortizationTransactionOutput>();
            foreach (AmortizationInitiation initiation in initiations)
            {
                AmortizationCalculator calculator = AmortizationFactory.CreateAmortizationCalculator();
                calculator.Initialize(input, initiation);
                double quantity = 1;
                double accumulated = calculator.CalculateAccumulatedAmortization(calculationDate);
                double rate = Math.Abs(accumulated);
                int transactionType = accumulated > 0 ? (int)TransactionTypeDefs.AmortizationIncome : (int)TransactionTypeDefs.AmortizationCost;
                output.Add(new AmortizationTransactionOutput()
                    {
                        Quantity = quantity,
                        Rate = rate,
                        PositionSeq = positionSeq,
                        TransactionType = transactionType
                    });
            }
            return output;
        }
    }
}
