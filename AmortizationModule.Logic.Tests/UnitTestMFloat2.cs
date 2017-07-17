using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AmortizationModule.Logic.Language;
using AmortizationModule.Logic.DTO.Internal;
using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    [TestClass]
    public class TestBondsFloat2
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

        private AmortizationInput SetUpBondsFloat2()
        {

                AmortizationInput Input = new AmortizationInput();

                Input.UserInput = BuildHelper.CreateUserInput(PositionSeq: 1, CalculationDate: "17.04.2018");

                Input.AmortizationSecurity = BuildHelper.Security(
                SecuritySeq: 1,
                SecurityType: 2,
                MaturityDate: "15.10.2018",
                Floater: true,
                Currency: "NOK");

                Input.Settings = BuildHelper.CreateSettings(
                Method: 1,
                OutputAggregated: true,
                InterestMethod: 1);

                Input.AmortizationSecurity.Instalments = new Dictionary<DateTime, double>() {
                    { new DateTime(2018,10,15), 1},
                };

            Input.AmortizationSecurity.InterestTerms = new Dictionary<DateTime, double> {
                { new DateTime(2014,10,16), 0.0277260273972603},
                { new DateTime(2015,04,11), 0.0266712328767123},
                { new DateTime(2015,10,20), 0.0289315068493151},
                { new DateTime(2016,04,21), 0.0277260273972603},
                { new DateTime(2016,10,18), 0.0295890410958904},
                { new DateTime(2017,04,15), 0.0294246575342466},
                { new DateTime(2017,10,17), 0.0304109589041096},
                { new DateTime(2018,04,17), 0.0299178082191781},
                { new DateTime(2018,10,15), 0.0297534246575342},
            };


            Input.InterestRates = BuildHelper.CreateInterestRates(new Dictionary<string, double>(){
                    { "15.04.2014",0.055},
                    { "21.04.2016",0.06}
                });

                Input.AmortizationTransactions = new List<AmortizationTransaction>(){
                    BuildHelper.Transaction("15.04.2014",4,1,"V-01",500000,1.02,1,"NOK",1),
                    BuildHelper.Transaction("16.10.2014",9,1,"V-02",13863.0136986301,1,2,"NOK",1),
                    BuildHelper.Transaction("11.04.2015",9,1,"V-03",13335.6164383562,1,3,"NOK",1),
                    BuildHelper.Transaction("20.10.2015",9,1,"V-04",14465.7534246575,1,4,"NOK",1),
                    BuildHelper.Transaction("21.04.2016",9,1,"V-05",13863.0136986301,1,5,"NOK",1),
                    BuildHelper.Transaction("18.10.2016",9,1,"V-06",13561.6438356164,1,6,"NOK",1),
                    BuildHelper.Transaction("15.04.2017",9,1,"V-07",13486.301369863,1,7,"NOK",1),
                    BuildHelper.Transaction("17.10.2017",9,1,"V-08",13938.3561643836,1,8,"NOK",1),
                    BuildHelper.Transaction("17.04.2018",9,1,"V-09",13712.3287671233,1,9,"NOK",1),
                    BuildHelper.Transaction("15.10.2018",9,1,"V-10",13636.9863013699,1,10,"NOK",1),
                    BuildHelper.Transaction("15.10.2018",68,1,"V-11",500000,1,11,"NOK",1)
                };

                return Input;
            }

            [TestMethod]
            public void TestBondFloat2()
            {
                AmortizationInput input = SetUpBondsFloat2();
                AmortizationOutput output = CommandHelper.GenerateAmortizationOutput(input);
                AssertHelper.VerifyOutputTotalAccumulatedAmortizationEquals("17.04.2018", -8779.586253, output);
            }
    }

}
