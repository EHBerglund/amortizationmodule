using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AmortizationModule.Logic.Language;
using AmortizationModule.Logic.DTO.Internal;
using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    [TestClass]
    public class TestLoanFloat
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

        private AmortizationInput SetUpLoanFloat()
        {

            AmortizationInput Input = new AmortizationInput();

            Input.UserInput = BuildHelper.CreateUserInput(PositionSeq: 1, CalculationDate: "15.07.2024");

            Input.AmortizationSecurity = BuildHelper.Security(
            SecuritySeq: 1,
            SecurityType: 23,
            MaturityDate: "15.01.2025",
            Floater: true,
            Currency: "NOK");

            Input.Settings = BuildHelper.CreateSettings(
            Method: 1,
            OutputAggregated: true,
            InterestMethod: 1);

            Input.AmortizationSecurity.Instalments = new Dictionary<DateTime, double>() {
                { new DateTime(2017,07,15), 0.0625},
                { new DateTime(2018,01,15), 0.0625},
                { new DateTime(2018,07,15), 0.0625},
                { new DateTime(2019,01,15), 0.0625},
                { new DateTime(2019,07,15), 0.0625},
                { new DateTime(2020,01,15), 0.0625},
                { new DateTime(2020,07,15), 0.0625},
                { new DateTime(2021,01,15), 0.0625},
                { new DateTime(2021,07,15), 0.0625},
                { new DateTime(2022,01,15), 0.0625},
                { new DateTime(2022,07,15), 0.0625},
                { new DateTime(2023,01,15), 0.0625},
                { new DateTime(2023,07,15), 0.0625},
                { new DateTime(2024,01,15), 0.0625},
                { new DateTime(2024,07,15), 0.0625},
                { new DateTime(2025,01,15), 0.0625},
            };

            Input.AmortizationSecurity.InterestTerms = new Dictionary<DateTime, double> {
                { new DateTime(2017,07,15), 0.0190917808219178},
                { new DateTime(2018,01,15), 0.0181952054794521},
                { new DateTime(2018,07,15), 0.0167053082191781},
                { new DateTime(2019,01,15), 0.0157691780821918},
                { new DateTime(2019,07,15), 0.0143188356164384},
                { new DateTime(2020,01,15), 0.0133431506849315},
                { new DateTime(2020,07,15), 0.0126215753424658},
                { new DateTime(2021,01,15), 0.0114842465753425},
                { new DateTime(2021,07,15), 0.0100417808219178},
                { new DateTime(2022,01,15), 0.00893219178082192},
                { new DateTime(2022,07,15), 0.00753133561643836},
                { new DateTime(2023,01,15), 0.00638013698630137},
                { new DateTime(2023,07,15), 0.0050208904109589},
                { new DateTime(2024,01,15), 0.00382808219178082},
                { new DateTime(2024,07,15), 0.00252431506849315},
                { new DateTime(2025,01,15), 0.00127602739726027},
           };

            Input.InterestRates = BuildHelper.CreateInterestRates(new Dictionary<string, double>(){
                { "15.01.2017",0.0385},
                { "15.07.2020",0.0405}});

            Input.AmortizationTransactions = new List<AmortizationTransaction>(){
                BuildHelper.Transaction("15.01.2017",67,1,"V-01",500000,1,1,"NOK",1),
                BuildHelper.Transaction("15.01.2017",116,1,"V-02",5000,1,2,"NOK",1),
                BuildHelper.Transaction("15.07.2017",68,1,"V-03",31250,1,3,"NOK",1),
                BuildHelper.Transaction("15.07.2017",9,1,"V-04",9545.8904109589,1,4,"NOK",1),
                BuildHelper.Transaction("15.01.2018",68,1,"V-05",31250,1,5,"NOK",1),
                BuildHelper.Transaction("15.01.2018",9,1,"V-06",9097.60273972603,1,6,"NOK",1),
                BuildHelper.Transaction("15.07.2018",68,1,"V-07",31250,1,7,"NOK",1),
                BuildHelper.Transaction("15.07.2018",9,1,"V-08",8352.65410958904,1,8,"NOK",1),
                BuildHelper.Transaction("15.01.2019",68,1,"V-09",31250,1,9,"NOK",1),
                BuildHelper.Transaction("15.01.2019",9,1,"V-10",7884.58904109589,1,10,"NOK",1),
                BuildHelper.Transaction("15.07.2019",68,1,"V-11",31250,1,11,"NOK",1),
                BuildHelper.Transaction("15.07.2019",9,1,"V-12",7159.41780821918,1,12,"NOK",1),
                BuildHelper.Transaction("15.01.2020",68,1,"V-13",31250,1,13,"NOK",1),
                BuildHelper.Transaction("15.01.2020",9,1,"V-14",6671.57534246575,1,14,"NOK",1),
                BuildHelper.Transaction("15.07.2020",68,1,"V-15",31250,1,15,"NOK",1),
                BuildHelper.Transaction("15.07.2020",9,1,"V-16",6310.78767123288,1,16,"NOK",1),
                BuildHelper.Transaction("15.01.2021",68,1,"V-17",31250,1,17,"NOK",1),
                BuildHelper.Transaction("15.01.2021",9,1,"V-18",5742.12328767123,1,18,"NOK",1),
                BuildHelper.Transaction("15.07.2021",68,1,"V-19",31250,1,19,"NOK",1),
                BuildHelper.Transaction("15.07.2021",9,1,"V-20",5020.8904109589,1,20,"NOK",1),
                BuildHelper.Transaction("15.01.2022",68,1,"V-21",31250,1,21,"NOK",1),
                BuildHelper.Transaction("15.01.2022",9,1,"V-22",4466.09589041096,1,22,"NOK",1),
                BuildHelper.Transaction("15.07.2022",68,1,"V-23",31250,1,23,"NOK",1),
                BuildHelper.Transaction("15.07.2022",9,1,"V-24",3765.66780821918,1,24,"NOK",1),
                BuildHelper.Transaction("15.01.2023",68,1,"V-25",31250,1,25,"NOK",1),
                BuildHelper.Transaction("15.01.2023",9,1,"V-26",3190.06849315068,1,26,"NOK",1),
                BuildHelper.Transaction("15.07.2023",68,1,"V-27",31250,1,27,"NOK",1),
                BuildHelper.Transaction("15.07.2023",9,1,"V-28",2510.44520547945,1,28,"NOK",1),
                BuildHelper.Transaction("15.01.2024",68,1,"V-29",31250,1,29,"NOK",1),
                BuildHelper.Transaction("15.01.2024",9,1,"V-30",1914.04109589041,1,30,"NOK",1),
                BuildHelper.Transaction("15.07.2024",68,1,"V-31",31250,1,31,"NOK",1),
                BuildHelper.Transaction("15.07.2024",9,1,"V-32",1262.15753424658,1,32,"NOK",1),
                BuildHelper.Transaction("15.01.2025",68,1,"V-33",31250,1,33,"NOK",1),
                BuildHelper.Transaction("15.01.2025",9,1,"V-34",638.013698630137,1,34,"NOK",1)
            };

            return Input;  
        }

        [TestMethod]
        public void TestLoansFloat()
        {
            AmortizationInput input = SetUpLoanFloat();
            AmortizationOutput output = CommandHelper.GenerateAmortizationOutput(input);
            AssertHelper.VerifyOutputTotalAccumulatedAmortizationEquals("15.07.2024", 80.27, output);
        }
    }
}
