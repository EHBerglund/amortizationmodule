using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AmortizationModule.Logic.Language;
using AmortizationModule.Logic.DTO.Internal;
using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    [TestClass]
    public class RateChangeTest
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

        private AmortizationInput BuildUpExample1()
        {
            AmortizationInput Input = new AmortizationInput();

            Input.UserInput = BuildHelper.CreateUserInput(PositionSeq: 1, CalculationDate: "30.09.2016");

            Input.AmortizationSecurity = BuildHelper.Security(
            SecuritySeq: 1,
            SecurityType: 2,
            MaturityDate: "31.12.2016",
            Floater: true,
            Currency: "NOK");

            Input.Settings = BuildHelper.CreateSettings(
            Method: 1,
            OutputAggregated: true,
            InterestMethod: 1);

            Input.AmortizationSecurity.Instalments = new Dictionary<DateTime, double>() {
{ new DateTime(2016,12,31), 1},
};
            Input.AmortizationSecurity.InterestTerms = new Dictionary<DateTime, double> {
                { new DateTime(2016,03,31), 0.009863014},
                { new DateTime(2016,06,30), 0.009972603},
                { new DateTime(2016,09,30), 0.015123288},
                { new DateTime(2016,12,31), 0.015123288},
            };


            Input.InterestRates = BuildHelper.CreateInterestRates(new Dictionary<string, double>(){
                { "01.01.2016",0.04},
                { "30.06.2016",0.06}}
            );

            Input.AmortizationTransactions = new List<AmortizationTransaction>(){
BuildHelper.Transaction("01.01.2016",4,1,"V-01",100000,0.9,1,"NOK",1),
BuildHelper.Transaction("31.03.2016",9,1,"V-02",986.3014,1,2,"NOK",1),
BuildHelper.Transaction("30.06.2016",9,1,"V-03",997.2603,1,3,"NOk",1)};
            return Input;

        }

        [TestMethod]
        public void TestFloater()
        {
            AmortizationInput input = BuildUpExample1();
            AmortizationOutput output = CommandHelper.GenerateAmortizationOutput(input);
            AssertHelper.VerifyOutputTotalAccumulatedAmortizationEquals("30.09.2016", 7333, output);
        }
    }
}
