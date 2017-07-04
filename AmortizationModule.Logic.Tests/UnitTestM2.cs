using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AmortizationModule.Logic.Language;
using AmortizationModule.Logic.DTO.Internal;
using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    [TestClass]
    public class TestSimpleBond3
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


        private AmortizationInput SetUpSimpleBond3()
        {
            AmortizationInput Input = new AmortizationInput();

            Input.UserInput = BuildHelper.CreateUserInput(PositionSeq: 1, CalculationDate: "01.01.2014");

            Input.AmortizationSecurity = BuildHelper.Security(
            SecuritySeq: 1,
            SecurityType: 2,
            MaturityDate: "01.01.2020",
            Floater: false,
            Currency: "NOK");

            Input.Settings = BuildHelper.CreateSettings(
            Method: 1,
            OutputAggregated: true);


            Input.InterestRates = BuildHelper.CreateInterestRates(new Dictionary<string, double>(){
            { "01.01.2014",4.7}});



        Input.AmortizationTransactions = new List<AmortizationTransaction>(){
        BuildHelper.Transaction("01.01.2014",4,1,"V-01",1250000,0.96,1,"NOK",1),
        BuildHelper.Transaction("01.01.2015",9,1,"V-02",58750,1,2,"NOK",1),
        BuildHelper.Transaction("01.01.2016",9,1,"V-03",58750,1,3,"NOK",1),
        BuildHelper.Transaction("01.01.2017",9,1,"V-04",58750,1,4,"NOK",1),
        BuildHelper.Transaction("01.01.2018",9,1,"V-05",58750,1,5,"NOK",1),
        BuildHelper.Transaction("01.01.2019",9,1,"V-06",58750,1,6,"NOK",1),
        BuildHelper.Transaction("01.01.2020",9,1,"V-07",58750,1,7,"NOK",1),
        BuildHelper.Transaction("02.01.2020",68,1,"V-08",1250000,1,8,"NOK",1)};


            return Input;

        }

        [TestMethod]
        public void TestSimpleBonds3()
        {
            AmortizationInput input = SetUpSimpleBond3();
            AmortizationOutput output = CommandHelper.GenerateAmortizationOutput(input);
            AssertHelper.VerifyOutputTotalAccumulatedAmortizationEquals("01.01.2019", 8992.647015, output);
        }

    }

}

