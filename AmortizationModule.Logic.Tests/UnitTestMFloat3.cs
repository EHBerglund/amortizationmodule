using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AmortizationModule.Logic.Language;
using AmortizationModule.Logic.DTO.Internal;
using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    [TestClass]
    public class TestBondsFloat3
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

        private AmortizationInput SetUpBondsFloat3()
        {
            AmortizationInput Input = new AmortizationInput();
            Input.UserInput = BuildHelper.CreateUserInput(PositionSeq: 1, CalculationDate: "01.02.2014");

            Input.AmortizationSecurity = BuildHelper.Security(
                SecuritySeq: 1,
                SecurityType: 2,
                MaturityDate: "01.02.2017",
                Floater: true,
                Currency: "NOK");

            Input.Settings = BuildHelper.CreateSettings(
                Method: 1,
                OutputAggregated: true,
                InterestMethod: 1);

            Input.AmortizationSecurity.Instalments = new Dictionary<DateTime, double>
            {
                { new DateTime (2017,02,01), 1 },
            };

            Input.AmortizationSecurity.InterestTerms = new Dictionary<DateTime, double>
            {
                {new DateTime (2006,02,01), 0.06},
                {new DateTime (2007,02,01), 0.06},
                {new DateTime (2008,02,01), 0.06},
                {new DateTime (2009,02,01), 0.060164384},
                {new DateTime (2010,02,01), 0.06},
                {new DateTime (2011,02,01), 0.06},
                {new DateTime (2012,02,01), 0.06},
                {new DateTime (2013,02,01), 0.060164384},
                {new DateTime (2014,02,01), 0.06},
                {new DateTime (2015,02,01), 0.06},
                {new DateTime (2016,02,01), 0.06},
                {new DateTime (2017,02,01), 0.060164384},

            };

            Input.InterestRates = BuildHelper.CreateInterestRates(new Dictionary<string, double>{
                { "01.02.2005", 0.06},
                { "01.02.2010", 0.05},
                { "01.02.2012", 0.08},
            });

            Input.AmortizationTransactions = new List<AmortizationTransaction>()
            {
                BuildHelper.Transaction("01.02.2005", 4, 1, "V-01", 100000, 1.232,1,"NOK", 1),
                BuildHelper.Transaction("01.02.2006", 9, 1, "V-02", 6000, 1,2,"NOK", 1),
                BuildHelper.Transaction("01.02.2007", 9, 1, "V-03", 6000, 1,3,"NOK", 1),
                BuildHelper.Transaction("01.02.2008", 9, 1, "V-04", 6000, 1,4,"NOK", 1),
                BuildHelper.Transaction("01.02.2009", 9, 1, "V-05", 6016.438356,1,5,"NOK", 1),
                BuildHelper.Transaction("01.02.2010", 9, 1, "V-06", 6000, 1,6,"NOK", 1),
                BuildHelper.Transaction("01.02.2011", 9, 1, "V-07", 6000, 1,7,"NOK", 1),
                BuildHelper.Transaction("01.02.2012", 9, 1, "V-08", 6000, 1,8,"NOK", 1),
                BuildHelper.Transaction("01.02.2013", 9, 1, "V-09", 6016.438356, 1,9,"NOK", 1),
                BuildHelper.Transaction("01.02.2014", 9, 1, "V-10", 6000, 1,10,"NOK", 1),
                BuildHelper.Transaction("01.02.2015", 9, 1, "V-11", 6000, 1,11,"NOK", 1),
                BuildHelper.Transaction("01.02.2016", 9, 1, "V-12", 6000, 1,12,"NOK", 1),
                BuildHelper.Transaction("01.02.2017", 9, 1, "V-13", 6016.438356, 1,13,"NOK", 1),
                BuildHelper.Transaction("01.02.2017", 68, 1, "V-14", 100000, 1,14,"NOK", 1),
            };

            return Input;


        }
     
        [TestMethod]

        public void TestBondFloat3()
        {
            AmortizationInput input = SetUpBondsFloat3();
            AmortizationOutput output = CommandHelper.GenerateAmortizationOutput(input);
            AssertHelper.VerifyOutputTotalAccumulatedAmortizationEquals("01.02.2014", 16389.14865, output);
        }


    }
}
