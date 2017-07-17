using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AmortizationModule.Logic.Language;
using AmortizationModule.Logic.DTO.Internal;
using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    [TestClass]
    public class TestLoan1
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

        private AmortizationInput SetUpLoan1()
        {
            AmortizationInput Input = new AmortizationInput();

            Input.UserInput = BuildHelper.CreateUserInput(PositionSeq: 1, CalculationDate: "30.04.2022");

            Input.AmortizationSecurity = BuildHelper.Security(
            SecuritySeq: 1,
            SecurityType: 23,
            MaturityDate: "30.04.2027",
            Floater: false,
            Currency: "NOK");

            Input.Settings = BuildHelper.CreateSettings(
            Method: 1,
            OutputAggregated: true,
            InterestMethod: 1);

            Input.AmortizationSecurity.Instalments = new List<Installment>() {
                new Installment(new DateTime(2018,04,30), 0.1, new DateTime(2017,04,30)),
                new Installment(new DateTime(2019,04,30), 0.1, new DateTime(2017, 04, 30)),
                new Installment(new DateTime(2020,04,30), 0.1, new DateTime(2017, 04, 30)),
                new Installment(new DateTime(2021,04,30), 0.1, new DateTime(2017, 04, 30)),
                new Installment(new DateTime(2022,04,30), 0.1, new DateTime(2017, 04, 30)),
                new Installment(new DateTime(2023,04,30), 0.1, new DateTime(2017, 04, 30)),
                new Installment(new DateTime(2024,04,30), 0.1, new DateTime(2017, 04, 30)),
                new Installment(new DateTime(2025,04,30), 0.1, new DateTime(2017, 04, 30)),
                new Installment(new DateTime(2026,04,30), 0.1, new DateTime(2017, 04, 30)),
                new Installment(new DateTime(2027,04,30), 0.1, new DateTime(2017, 04, 30)),
            };

            Input.AmortizationSecurity.InterestTerms = new Dictionary<DateTime, double> {
                { new DateTime(2018,04,30), 0.03},
                { new DateTime(2019,04,30), 0.03},
                { new DateTime(2020,04,30), 0.03},
                { new DateTime(2021,04,30), 0.03},
                { new DateTime(2022,04,30), 0.03},
                { new DateTime(2023,04,30), 0.03},
                { new DateTime(2024,04,30), 0.03},
                { new DateTime(2025,04,30), 0.03},
                { new DateTime(2026,04,30), 0.03},
                { new DateTime(2027,04,30), 0.03},
            };

            Input.InterestRates = BuildHelper.CreateInterestRates(new Dictionary<string, double>(){
                { "30.04.2017",0.03}});

            Input.AmortizationTransactions = new List<AmortizationTransaction>(){
                BuildHelper.Transaction("30.04.2017",67,1,"V-01",90000,1,1,"NOK",1),
                BuildHelper.Transaction("30.04.2017",116,1,"V-02",2000,1,2,"NOK",1),
                BuildHelper.Transaction("30.04.2018",68,1,"V-03",9000,1,3,"NOK",1),
                BuildHelper.Transaction("30.04.2018",9,1,"V-04",2700,1,4,"NOK",1),
                BuildHelper.Transaction("30.04.2018",65,1,"V-05",100,1,5,"NOK",1),
                BuildHelper.Transaction("30.04.2019",68,1,"V-06",9000,1,6,"NOK",1),
                BuildHelper.Transaction("30.04.2019",9,1,"V-07",2430,1,7,"NOK",1),
                BuildHelper.Transaction("30.04.2019",65,1,"V-08",100,1,8,"NOK",1),
                BuildHelper.Transaction("30.04.2020",68,1,"V-09",9000,1,9,"NOK",1),
                BuildHelper.Transaction("30.04.2020",9,1,"V-10",2160,1,10,"NOK",1),
                BuildHelper.Transaction("30.04.2020",65,1,"V-11",100,1,11,"NOK",1),
                BuildHelper.Transaction("30.04.2021",68,1,"V-12",9000,1,12,"NOK",1),
                BuildHelper.Transaction("30.04.2021",9,1,"V-13",1890,1,13,"NOK",1),
                BuildHelper.Transaction("30.04.2021",65,1,"V-14",100,1,14,"NOK",1),
                BuildHelper.Transaction("30.04.2022",68,1,"V-15",9000,1,15,"NOK",1),
                BuildHelper.Transaction("30.04.2022",9,1,"V-16",1620,1,16,"NOK",1),
                BuildHelper.Transaction("30.04.2022",65,1,"V-17",100,1,17,"NOK",1),
                BuildHelper.Transaction("30.04.2023",68,1,"V-18",9000,1,18,"NOK",1),
                BuildHelper.Transaction("30.04.2023",9,1,"V-19",1350,1,19,"NOK",1),
                BuildHelper.Transaction("30.04.2023",65,1,"V-20",100,1,20,"NOK",1),
                BuildHelper.Transaction("30.04.2024",68,1,"V-21",9000,1,21,"NOK",1),
                BuildHelper.Transaction("30.04.2024",9,1,"V-22",1080,1,22,"NOK",1),
                BuildHelper.Transaction("30.04.2024",65,1,"V-23",100,1,23,"NOK",1),
                BuildHelper.Transaction("30.04.2025",68,1,"V-24",9000,1,24,"NOK",1),
                BuildHelper.Transaction("30.04.2025",9,1,"V-25",810,1,25,"NOK",1),
                BuildHelper.Transaction("30.04.2025",65,1,"V-26",100,1,26,"NOK",1),
                BuildHelper.Transaction("30.04.2026",68,1,"V-27",9000,1,27,"NOK",1),
                BuildHelper.Transaction("30.04.2026",9,1,"V-28",540,1,28,"NOK",1),
                BuildHelper.Transaction("30.04.2026",65,1,"V-29",100,1,29,"NOK",1),
                BuildHelper.Transaction("30.04.2027",68,1,"V-30",9000,1,30,"NOK",1),
                BuildHelper.Transaction("30.04.2027",9,1,"V-31",270,1,31,"NOK",1),
                BuildHelper.Transaction("30.04.2027",65,1,"V-32",100,1,32,"NOK",1)
            };
            return Input;
        }

        [TestMethod]
        public void TestLoans1()
        {
            AmortizationInput input = SetUpLoan1();
            AmortizationOutput output = CommandHelper.GenerateAmortizationOutput(input);
            AssertHelper.VerifyOutputTotalAccumulatedAmortizationEquals("30.04.2022", 1424, output);
        }
    }

}

