﻿using System;
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
            transactions = GenerateAmortizationOutputTransactions(initiations);
            return transactions;
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

        private InitiationCategorizer[] GetInitiationCategorizerRules()
        {
            InitiationCategorizer[] categorizers = new InitiationCategorizer[1];
            categorizers[0] = new PurchaseBondCategorizer();
            foreach (InitiationCategorizer categorizer in categorizers)
            {
                categorizer.Initialize();
            }
            return categorizers;
        }

        private LinkCategorizer[] GetLinkCategorizerRules()
        {
            LinkCategorizer[] categorizers = new LinkCategorizer[3];
            categorizers[0] = new PremiumDiscountCategorizer();
            categorizers[1] = new InterestCategorizer();
            categorizers[2] = new InstalmentCategorizer();
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
                double accumulated = calculator.CalculateAccumulatedAmortization(initiation, calculationDate);
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
