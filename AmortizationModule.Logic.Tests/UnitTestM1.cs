using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AmortizationModule.Logic.Language;
using AmortizationModule.Logic.DTO.Internal;
using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    [TestClass]
    public class TestSimpleBond2
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


        private AmortizationInput SetUpSimpleBond2()
        {
            AmortizationInput Input = new AmortizationInput();

            Input.UserInput = BuildHelper.CreateUserInput(PositionSeq: 1, CalculationDate: "04.09.2015");

            Input.AmortizationSecurity = BuildHelper.Security(
            SecuritySeq: 1,
            SecurityType: 2,
            MaturityDate: "03.12.2015",
            Floater: false,
            Currency: "NOK");

            Input.AmortizationSecurity.Instalments = new Dictionary<DateTime, double>
            {
                {new DateTime(2017,1,1), 4}
            };

            Input.AmortizationSecurity.InterestTerms = new Dictionary<DateTime, double>
            {

            };

            Input.Settings = BuildHelper.CreateSettings(
            Method: 1,
            OutputAggregated: true);

            Input.InterestRates = BuildHelper.CreateInterestRates(new Dictionary<string, double>(){


            {"03.01.2015",0.018}
                });



        Input.AmortizationTransactions = new List<AmortizationTransaction>(){
        BuildHelper.Transaction("03.01.2015",4,1,"V-01",100000000,0.944444444444444,1,"NOK",1),
        BuildHelper.Transaction("03.02.2015",9,1,"V-02",279000,1,2,"NOK",1),
        BuildHelper.Transaction("05.03.2015",9,1,"V-03",270000,1,3,"NOK",1),
        BuildHelper.Transaction("06.04.2015",9,1,"V-04",288000,1,4,"NOK",1),
        BuildHelper.Transaction("02.05.2015",9,1,"V-05",234000,1,5,"NOK",1),
        BuildHelper.Transaction("03.06.2015",9,1,"V-06",288000,1,6,"NOK",1),
        BuildHelper.Transaction("05.07.2015",9,1,"V-07",288000,1,7,"NOK",1),
        BuildHelper.Transaction("03.08.2015",9,1,"V-08",261000,1,8,"NOK",1),
        BuildHelper.Transaction("04.09.2015",9,1,"V-09",288000,1,9,"NOK",1),
        BuildHelper.Transaction("04.10.2015",9,1,"V-10",270000,1,10,"NOK",1),
        BuildHelper.Transaction("03.11.2015",9,1,"V-11",270000,1,11,"NOK",1),
        BuildHelper.Transaction("03.12.2015",9,1,"V-12",270000,1,12,"NOK",1),
        BuildHelper.Transaction("03.12.2015",68,1,"V-13",180000000,1,13,"NOK",1)};

    return Input;

        }

        private AmortizationInput buildUpTest()
        {
            AmortizationInput Input = new AmortizationInput();

            Input.UserInput = BuildHelper.CreateUserInput(PositionSeq: 1, CalculationDate: "30.06.2016");

            Input.AmortizationSecurity = BuildHelper.Security(
            SecuritySeq: 1,
            SecurityType: 2,
            MaturityDate: "30.09.2016",
            Floater: true,
            Currency: "NOK");

            Input.Settings = BuildHelper.CreateSettings(
            Method: 1,
            OutputAggregated: true,
            InterestMethod: 1);

            Input.AmortizationSecurity.Instalments = new Dictionary<DateTime, double>() {
{ new DateTime(2016,09,30), 100000},
};

            Input.AmortizationSecurity.InterestTerms = new Dictionary<DateTime, double> {
{ new DateTime(2016,03,31), 0},
{ new DateTime(2016,06,30), 0},
{ new DateTime(2016,09,30), 0},
};

            Input.InterestRates = BuildHelper.CreateInterestRates(new Dictionary<string, double>(){
{ "01.01.2016",0.023},
{ "31.03.2016",0.04},
{ "30.06.2016",0.03}});

            Input.AmortizationTransactions = new List<AmortizationTransaction>(){
BuildHelper.Transaction("01.01.2016",4,1,"V-01",100000,1.01,1,"NOK",1),
BuildHelper.Transaction("30.06.2016",68,1,"V-02",10000,1,2,"NOK",1),
BuildHelper.Transaction("30.09.2016",68,1,"V-03",90000,1,3,"NOk",1)};

            return Input;
        }

        [TestMethod]
        public void TestSimpleBonds2()
        {
            AmortizationInput input = SetUpSimpleBond2();
            AmortizationOutput output = CommandHelper.GenerateAmortizationOutput(input);
            AssertHelper.VerifyOutputTotalAccumulatedAmortizationEquals("04.09.2015", 970999, output);
        }

    }

}


