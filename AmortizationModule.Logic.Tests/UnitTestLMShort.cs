using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AmortizationModule.Logic.Language;
using AmortizationModule.Logic.DTO.Internal;
using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    [TestClass]
    public class TestLoanShort
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

        private AmortizationInput SetUpLoanShort()
        {

            AmortizationInput Input = new AmortizationInput();

            Input.UserInput = BuildHelper.CreateUserInput(PositionSeq: 1, CalculationDate: "01.12.2017");

            Input.AmortizationSecurity = BuildHelper.Security(
            SecuritySeq: 1,
            SecurityType: 23,
            MaturityDate: "01.02.2018",
            Floater: false,
            Currency: "NOK");

            Input.Settings = BuildHelper.CreateSettings(
            Method: 1,
            OutputAggregated: true,
            InterestMethod: 1);

            Input.AmortizationSecurity.Instalments = new Dictionary<DateTime, double>() {
                { new DateTime(2017,09,01), 0.0833333333333333},
                { new DateTime(2017,10,01), 0.0833333333333333},
                { new DateTime(2017,11,01), 0.0833333333333333},
                { new DateTime(2017,12,01), 0.0833333333333333},
                { new DateTime(2018,01,01), 0.0833333333333333},
                { new DateTime(2018,02,01), 0.583333333333333},
            };

            Input.AmortizationSecurity.InterestTerms = new Dictionary<DateTime, double> {
                { new DateTime(2017,09,01), 0.00467123287671233},
                { new DateTime(2017,10,01), 0.00414383561643836},
                { new DateTime(2017,11,01), 0.00389269406392694},
                { new DateTime(2017,12,01), 0.00339041095890411},
                { new DateTime(2018,01,01), 0.00311415525114155},
                { new DateTime(2018,02,01), 0.00272488584474886},
            };


            Input.InterestRates = BuildHelper.CreateInterestRates(new Dictionary<string, double>(){
                { "01.08.2017",0.055}});

            Input.AmortizationTransactions = new List<AmortizationTransaction>(){
                BuildHelper.Transaction("01.08.2017",67,1,"V-01",50000,1,1,"NOK",1),
                BuildHelper.Transaction("01.08.2017",116,1,"V-02",1000,1,2,"NOK",1),
                BuildHelper.Transaction("01.09.2017",68,1,"V-03",4166.66666666667,1,3,"NOK",1),
                BuildHelper.Transaction("01.09.2017",9,1,"V-04",233.561643835616,1,4,"NOK",1),
                BuildHelper.Transaction("01.09.2017",65,1,"V-05",50,1,5,"NOK",1),
                BuildHelper.Transaction("01.10.2017",68,1,"V-06",4166.66666666667,1,6,"NOK",1),
                BuildHelper.Transaction("01.10.2017",9,1,"V-07",207.191780821918,1,7,"NOK",1),
                BuildHelper.Transaction("01.10.2017",65,1,"V-08",50,1,8,"NOK",1),
                BuildHelper.Transaction("01.11.2017",68,1,"V-09",4166.66666666667,1,9,"NOK",1),
                BuildHelper.Transaction("01.11.2017",9,1,"V-10",194.634703196347,1,10,"NOK",1),
                BuildHelper.Transaction("01.11.2017",65,1,"V-11",50,1,11,"NOK",1),
                BuildHelper.Transaction("01.12.2017",68,1,"V-12",4166.66666666667,1,12,"NOK",1),
                BuildHelper.Transaction("01.12.2017",9,1,"V-13",169.520547945206,1,13,"NOK",1),
                BuildHelper.Transaction("01.12.2017",65,1,"V-14",50,1,14,"NOK",1),
                BuildHelper.Transaction("01.01.2018",68,1,"V-15",4166.66666666667,1,15,"NOK",1),
                BuildHelper.Transaction("01.01.2018",9,1,"V-16",155.707762557078,1,16,"NOK",1),
                BuildHelper.Transaction("01.01.2018",65,1,"V-17",50,1,17,"NOK",1),
                BuildHelper.Transaction("01.02.2018",68,1,"V-18",29166.6666666667,1,18,"NOK",1),
                BuildHelper.Transaction("01.02.2018",9,1,"V-19",136.244292237443,1,19,"NOK",1),
                BuildHelper.Transaction("01.02.2018",65,1,"V-20",50,1,20,"NOK",1)
            };

            return Input;
        }

        [TestMethod]
        public void TestLoansShort()
        {
            AmortizationInput input = SetUpLoanShort();
            AmortizationOutput output = CommandHelper.GenerateAmortizationOutput(input);
            AssertHelper.VerifyOutputTotalAccumulatedAmortizationEquals("01.12.2017", 749.21, output);
        }
    }
}