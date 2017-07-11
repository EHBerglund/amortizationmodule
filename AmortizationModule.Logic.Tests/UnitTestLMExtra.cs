using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AmortizationModule.Logic.Language;
using AmortizationModule.Logic.DTO.Internal;
using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    [TestClass]
    public class TestLoanExtra
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

        private AmortizationInput SetUpLoanExtra()
        {

            AmortizationInput Input = new AmortizationInput();

            Input.UserInput = BuildHelper.CreateUserInput(PositionSeq: 1, CalculationDate: "15.08.2020");

            Input.AmortizationSecurity = BuildHelper.Security(
            SecuritySeq: 1,
            SecurityType: 23,
            MaturityDate: "15.08.2021",
            Floater: false,
            Currency: "NOK");

            Input.Settings = BuildHelper.CreateSettings(
            Method: 1,
            OutputAggregated: true,
            InterestMethod: 1);

            Input.AmortizationSecurity.Instalments = new Dictionary<DateTime, double>() {
                { new DateTime(2017,11,15), 0.0625},
                { new DateTime(2018,02,15), 0.0625},
                { new DateTime(2018,05,15), 0.0625},
                { new DateTime(2018,08,15), 0.0625},
                { new DateTime(2018,11,15), 0.0625},
                { new DateTime(2019,02,15), 0.0791666666666667},
                { new DateTime(2019,05,15), 0.0608333333333333},
                { new DateTime(2019,08,15), 0.0608333333333333},
                { new DateTime(2019,11,15), 0.0608333333333333},
                { new DateTime(2020,02,15), 0.0608333333333333},
                { new DateTime(2020,05,15), 0.0608333333333333},
                { new DateTime(2020,08,15), 0.0608333333333333},
                { new DateTime(2020,11,15), 0.0608333333333333},
                { new DateTime(2021,02,15), 0.0608333333333333},
                { new DateTime(2021,05,15), 0.0608333333333333},
                { new DateTime(2021,08,15), 0.0608333333333333},
             };

            Input.AmortizationSecurity.InterestTerms = new Dictionary<DateTime, double> {
                { new DateTime(2017,11,15), 0.00970410958904109},
                { new DateTime(2018,02,15), 0.00909760273972603},
                { new DateTime(2018,05,15), 0.00821421232876712},
                { new DateTime(2018,08,15), 0.00788458904109589},
                { new DateTime(2018,11,15), 0.00727808219178082},
                { new DateTime(2019,02,15), 0.00667157534246575},
                { new DateTime(2019,05,15), 0.00571083333333333},
                { new DateTime(2019,08,15), 0.005313},
                { new DateTime(2019,11,15), 0.00472266666666667},
                { new DateTime(2020,02,15), 0.00413233333333333},
                { new DateTime(2020,05,15), 0.003465},
                { new DateTime(2020,08,15), 0.00295166666666667},
                { new DateTime(2020,11,15), 0.00236133333333333},
                { new DateTime(2021,02,15), 0.001771},
                { new DateTime(2021,05,15), 0.00114216666666667},
                { new DateTime(2021,08,15), 0.000590333333333333},

            };

            Input.InterestRates = BuildHelper.CreateInterestRates(new Dictionary<string, double>(){
                { "15.08.2017",0.0385}});

            Input.AmortizationTransactions = new List<AmortizationTransaction>(){
                BuildHelper.Transaction("15.08.2017",67,1,"V-01",1200000,1,1,"NOK",1),
                BuildHelper.Transaction("15.08.2017",116,1,"V-02",1000,1,2,"NOK",1),
                BuildHelper.Transaction("15.11.2017",68,1,"V-03",75000,1,3,"NOK",1),
                BuildHelper.Transaction("15.11.2017",9,1,"V-04",11644.9315068493,1,4,"NOK",1),
                BuildHelper.Transaction("15.02.2018",68,1,"V-05",75000,1,5,"NOK",1),
                BuildHelper.Transaction("15.02.2018",9,1,"V-06",10917.1232876712,1,6,"NOK",1),
                BuildHelper.Transaction("15.05.2018",68,1,"V-07",75000,1,7,"NOK",1),
                BuildHelper.Transaction("15.05.2018",9,1,"V-08",9857.05479452055,1,8,"NOK",1),
                BuildHelper.Transaction("15.08.2018",68,1,"V-09",75000,1,9,"NOK",1),
                BuildHelper.Transaction("15.08.2018",9,1,"V-10",9461.50684931507,1,10,"NOK",1),
                BuildHelper.Transaction("15.11.2018",68,1,"V-11",75000,1,11,"NOK",1),
                BuildHelper.Transaction("15.11.2018",9,1,"V-12",8733.69863013699,1,12,"NOK",1),
                BuildHelper.Transaction("15.02.2019",68,1,"V-13",95000,1,13,"NOK",1),
                BuildHelper.Transaction("15.02.2019",9,1,"V-14",8005.8904109589,1,14,"NOK",1),
                BuildHelper.Transaction("15.05.2019",68,1,"V-15",73000,1,15,"NOK",1),
                BuildHelper.Transaction("15.05.2019",9,1,"V-16",6853,1,16,"NOK",1),
                BuildHelper.Transaction("15.08.2019",68,1,"V-17",73000,1,17,"NOK",1),
                BuildHelper.Transaction("15.08.2019",9,1,"V-18",6375.6,1,18,"NOK",1),
                BuildHelper.Transaction("15.11.2019",68,1,"V-19",73000,1,19,"NOK",1),
                BuildHelper.Transaction("15.11.2019",9,1,"V-20",5667.2,1,20,"NOK",1),
                BuildHelper.Transaction("15.02.2020",68,1,"V-21",73000,1,21,"NOK",1),
                BuildHelper.Transaction("15.02.2020",9,1,"V-22",4958.8,1,22,"NOK",1),
                BuildHelper.Transaction("15.05.2020",68,1,"V-23",73000,1,23,"NOK",1),
                BuildHelper.Transaction("15.05.2020",9,1,"V-24",4158,1,24,"NOK",1),
                BuildHelper.Transaction("15.08.2020",68,1,"V-25",73000,1,25,"NOK",1),
                BuildHelper.Transaction("15.08.2020",9,1,"V-26",3542,1,26,"NOK",1),
                BuildHelper.Transaction("15.11.2020",68,1,"V-27",73000,1,27,"NOK",1),
                BuildHelper.Transaction("15.11.2020",9,1,"V-28",2833.6,1,28,"NOK",1),
                BuildHelper.Transaction("15.02.2021",68,1,"V-29",73000,1,29,"NOK",1),
                BuildHelper.Transaction("15.02.2021",9,1,"V-30",2125.2,1,30,"NOK",1),
                BuildHelper.Transaction("15.05.2021",68,1,"V-31",73000,1,31,"NOK",1),
                BuildHelper.Transaction("15.05.2021",9,1,"V-32",1370.6,1,32,"NOK",1),
                BuildHelper.Transaction("15.08.2021",68,1,"V-33",73000,1,33,"NOK",1),
                BuildHelper.Transaction("15.08.2021",9,1,"V-34",708.4,1,34,"NOK",1)
            };

            return Input;

        }

        [TestMethod]
        public void TestLoansExtra()
        {
            AmortizationInput input = SetUpLoanExtra();
            AmortizationOutput output = CommandHelper.GenerateAmortizationOutput(input);
            AssertHelper.VerifyOutputTotalAccumulatedAmortizationEquals("15.08.2020", 37.2, output);
        }
    }
}
