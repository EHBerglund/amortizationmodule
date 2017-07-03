using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AmortizationModule.Logic.Language;
using AmortizationModule.Logic.DTO.Internal;
using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    [TestClass]
    public class UnitTest1
    {
        AmortizationCreateHelper BuildHelper;
        AmortizationCommandHelper CommandHelper;
        AmortizationAssertHelper AssertHelper;
        [TestInitialize()]
        public void TestInitialize()
        {
            BuildHelper = Factory.CreateAmortizationCreateHelper();
            CommandHelper = Factory.CreateAmortizationCommandHelper();
            AssertHelper = Factory.CreateAmortizationAssertHelper();
        }

        private AmortizationInput SetUpSimpleBond()
        {
            AmortizationInput Input = new AmortizationInput();
            Input.UserInput = BuildHelper.CreateUserInput(PositionSeq:1, CalculationDate:"1.1.2016");

            Input.AmortizationSecurity =
                BuildHelper.Security(
                    SecuritySeq:1,
                    SecurityType:(int)SecurityTypeDefs.Bond,
                    MaturityDate:"30.09.2016",
                    Floater:true,
                    Currency:"NOK");

            Input.InterestRates = BuildHelper.CreateInterestRates(new Dictionary<string, double>()
            {
                { "1.1.2016", 0.02},
                { "31.3.2016", 0.04},
                { "30.06.2016", 0.03}
            });

            Input.Settings = BuildHelper.CreateSettings(
                Method:(int)AmortizationMethodDefs.EfficientRateMethod,
                OutputAggregated:true);

            Input.AmortizationTransactions = new List<AmortizationTransaction>()
            {
                BuildHelper.Transaction(TransactionSeq:1, TransactionDate:"1.1.2016",
                    TransactionType:(int)TransactionTypeDefs.Purchase, Position:1,
                    Voucher:"V-1", Quantity:100000,Rate:1.01, Currency:"NOK"),

                BuildHelper.Transaction(TransactionSeq:2, TransactionDate:"30.06.2016",
                    TransactionType:(int)TransactionTypeDefs.Payback, Position:1,
                    Voucher:"V-1", Quantity:10000, Rate:1, Currency:"NOK"),

                BuildHelper.Transaction("30.09.2016", 68, 1, "V-2",90000,3,1,"NOK")
            };

            return Input;
        }

        [TestMethod]
        public void TestSimpleBond()
        {
            AmortizationInput input = SetUpSimpleBond();
            AmortizationOutput output = CommandHelper.GenerateAmortizationOutput(input);
            AssertHelper.VerifyOutputTotalAccumulatedAmortizationEquals("30.06.2016", 2992, output);
        }
    }
}
