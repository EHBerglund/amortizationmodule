using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AmortizationModule.Logic.Language;
using AmortizationModule.Logic.DTO.Internal;
using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    [TestClass]
    public class TestBondsSantander
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

        private AmortizationInput SetUpBondsSantander()
        {

            AmortizationInput Input = new AmortizationInput();

            Input.UserInput = BuildHelper.CreateUserInput(PositionSeq: 1, CalculationDate: "15.06.2016");

            Input.AmortizationSecurity = BuildHelper.Security(
            SecuritySeq: 1,
            SecurityType: 2,
            MaturityDate: "20.06.2018",
            Floater: false,
            Currency: "NOK");

            Input.Settings = BuildHelper.CreateSettings(
            Method: 1,
            OutputAggregated: true,
            InterestMethod: 2);

            Input.AmortizationSecurity.Instalments = new Dictionary<DateTime, double>() {
                { new DateTime(2018,06,20), 1},
            };

            Input.AmortizationSecurity.InterestTerms = new Dictionary<DateTime, double> {
                { new DateTime(2015,12,16), 0.003670903},
                { new DateTime(2016,03,16), 0.003670903},
                { new DateTime(2016,06,15), 0.003670903},
            };

            Input.InterestRates = BuildHelper.CreateInterestRates(new Dictionary<string, double>() {
            { "16.09.2015",0.014522253}});

            Input.AmortizationTransactions = new List<AmortizationTransaction>(){

                BuildHelper.Transaction("16.09.2015",4,1,"V-01",36000000,1.0033,1,"NOK",1),
                BuildHelper.Transaction("16.09.2015",70,1,"V-02",36000000,0.0033,2,"NOK",1),
                BuildHelper.Transaction("16.09.2015",4,1,"V-03",50000000,1.00317,3,"NOK",1),
                BuildHelper.Transaction("16.09.2015",70,1,"V-04",50000000,0.00317,4,"NOK",1),
                BuildHelper.Transaction("16.09.2015",18,1,"V-05",50000000,0.0000883334,5,"NOK",1),
                BuildHelper.Transaction("16.09.2015",4,1,"V-06",200000000,1.00327,6,"NOK",1),
                BuildHelper.Transaction("16.09.2015",18,1,"V-07",200000000,0.00022083335,7,"NOK",1),
                BuildHelper.Transaction("16.09.2015",70,1,"V-08",200000000,0.00327,8,"NOK",1),
                BuildHelper.Transaction("30.09.2015",29,1,"V-09",1,113685,9,"NOK",1),
                BuildHelper.Transaction("30.09.2015",20,1,"V-10",1,316400,10,"NOK",1),
                BuildHelper.Transaction("31.10.2015",29,1,"V-11",-1,113685,11,"NOK",1),
                BuildHelper.Transaction("31.10.2015",29,1,"V-12",1,505266.67,12,"NOK",1),
                BuildHelper.Transaction("31.10.2015",20,1,"V-13",-1,316400,13,"NOK",1),
                BuildHelper.Transaction("31.10.2015",20,1,"V-14",1,681050,14,"NOK",1),
                BuildHelper.Transaction("05.11.2015",4,1,"V-15",50000000,1.00174,15,"NOK",1),
                BuildHelper.Transaction("05.11.2015",70,1,"V-16",50000000,0.00174,16,"NOK",1),
                BuildHelper.Transaction("05.11.2015",18,1,"V-17",50000000,0.002385,17,"NOK",1),
                BuildHelper.Transaction("05.11.2015",29,1,"V-18",50000000,0.002385,18,"NOK",1),
                BuildHelper.Transaction("10.11.2015",4,1,"V-19",100000000,1.00219,19,"NOK",1),
                BuildHelper.Transaction("10.11.2015",70,1,"V-20",100000000,0.00219,20,"NOK",1),
                BuildHelper.Transaction("10.11.2015",18,1,"V-21",100000000,0.0025175,21,"NOK",1),
                BuildHelper.Transaction("10.11.2015",29,1,"V-22",100000000,0.0025175,22,"NOK",1),
                BuildHelper.Transaction("30.11.2015",20,1,"V-23",-1,681050,23,"NOK",1),
                BuildHelper.Transaction("30.11.2015",20,1,"V-24",1,1004040,24,"NOK",1),
                BuildHelper.Transaction("30.11.2015",29,1,"V-25",-1,876266.67,25,"NOK",1),
                BuildHelper.Transaction("30.11.2015",29,1,"V-26",1,1347966.67,26,"NOK",1),
                BuildHelper.Transaction("16.12.2015",9,1,"V-27",436000000,0.00401666667431193,27,"NOK",1),
                BuildHelper.Transaction("16.12.2015",29,1,"V-28",-1,1347966.67,28,"NOK",1),
                BuildHelper.Transaction("31.12.2015",29,1,"V-29",1,290666.67,29,"NOK",1),
                BuildHelper.Transaction("31.12.2015",20,1,"V-30",-1,1004040,30,"NOK",1),
                BuildHelper.Transaction("31.12.2015",20,1,"V-31",1,1106500,31,"NOK",1),
                BuildHelper.Transaction("31.12.2015",118,1,"V-32",1,11719.83,32,"NOK",1),
                BuildHelper.Transaction("31.12.2015",118,1,"V-33",1,5085.59,33,"NOK",1),
                BuildHelper.Transaction("31.12.2015",118,1,"V-34",1,68773.81,34,"NOK",1),
                BuildHelper.Transaction("31.12.2015",118,1,"V-35",1,16667.66,35,"NOK",1),
                BuildHelper.Transaction("31.12.2015",118,1,"V-36",1,12492.86,36,"NOK",1),
                BuildHelper.Transaction("29.01.2016",20,1,"V-37",-1,1106500,37,"NOK",1),
                BuildHelper.Transaction("29.01.2016",20,1,"V-38",1,1080340,38,"NOK",1),
                BuildHelper.Transaction("31.01.2016",29,1,"V-39",-1,290666.67,39,"NOK",1),
                BuildHelper.Transaction("31.01.2016",29,1,"V-40",1,853833.33,40,"NOK",1),
                BuildHelper.Transaction("31.01.2016",118,1,"V-41",1,-11719.83,41,"NOK",1),
                BuildHelper.Transaction("31.01.2016",118,1,"V-42",1,18843.65,42,"NOK",1),
                BuildHelper.Transaction("31.01.2016",118,1,"V-43",1,-5085.59,43,"NOK",1),
                BuildHelper.Transaction("31.01.2016",118,1,"V-44",1,7900.84,44,"NOK",1),
                BuildHelper.Transaction("31.01.2016",118,1,"V-45",1,-68773.81,45,"NOK",1),
                BuildHelper.Transaction("31.01.2016",118,1,"V-46",1,88886.9,46,"NOK",1),
                BuildHelper.Transaction("31.01.2016",118,1,"V-47",1,-16667.66,47,"NOK",1),
                BuildHelper.Transaction("31.01.2016",118,1,"V-48",1,21542.16,48,"NOK",1),
                BuildHelper.Transaction("31.01.2016",118,1,"V-49",1,-12492.86,49,"NOK",1),
                BuildHelper.Transaction("31.01.2016",118,1,"V-50",1,16146.43,50,"NOK",1),
                BuildHelper.Transaction("29.02.2016",29,1,"V-51",-1,853833.33,51,"NOK",1),
                BuildHelper.Transaction("29.02.2016",29,1,"V-52",1,1380666.67,52,"NOK",1),
                BuildHelper.Transaction("29.02.2016",20,1,"V-53",-1,1080340,53,"NOK",1),
                BuildHelper.Transaction("29.02.2016",20,1,"V-54",1,1086880,54,"NOK",1),
                BuildHelper.Transaction("29.02.2016",118,1,"V-55",1,-18843.65,55,"NOK",1),
                BuildHelper.Transaction("29.02.2016",118,1,"V-56",1,25507.87,56,"NOK",1),
                BuildHelper.Transaction("29.02.2016",118,1,"V-57",1,-7900.84,57,"NOK",1),
                BuildHelper.Transaction("29.02.2016",118,1,"V-58",1,10534.45,58,"NOK",1),
                BuildHelper.Transaction("29.02.2016",118,1,"V-59",1,-88886.9,59,"NOK",1),
                BuildHelper.Transaction("29.02.2016",118,1,"V-60",1,107702.38,60,"NOK",1),
                BuildHelper.Transaction("29.02.2016",118,1,"V-61",1,-21542.16,61,"NOK",1),
                BuildHelper.Transaction("29.02.2016",118,1,"V-62",1,26102.18,62,"NOK",1),
                BuildHelper.Transaction("29.02.2016",118,1,"V-63",1,-16146.43,63,"NOK",1),
                BuildHelper.Transaction("29.02.2016",118,1,"V-64",1,19564.29,64,"NOK",1),
                BuildHelper.Transaction("09.03.2016",4,1,"V-65",100000000,1.0007,65,"NOK",1),
                BuildHelper.Transaction("09.03.2016",70,1,"V-66",100000000,0.0007,66,"NOK",1),
                BuildHelper.Transaction("09.03.2016",18,1,"V-67",100000000,0.0035833333,67,"NOK",1),
                BuildHelper.Transaction("09.03.2016",29,1,"V-68",100000000,0.0035833333,68,"NOK",1),
                BuildHelper.Transaction("16.03.2016",9,1,"V-69",536000000,0.00308426615671642,69,"NOK",1),
                BuildHelper.Transaction("16.03.2016",29,1,"V-70",-1,1739000,70,"NOK",1),
                BuildHelper.Transaction("31.03.2016",29,1,"V-71",1,338275.56,71,"NOK",1),
                BuildHelper.Transaction("31.03.2016",20,1,"V-72",-1,1086880,72,"NOK",1),
                BuildHelper.Transaction("31.03.2016",20,1,"V-73",1,795420,73,"NOK",1),
                BuildHelper.Transaction("31.03.2016",118,1,"V-74",1,1848.74,74,"NOK",1),
                BuildHelper.Transaction("31.03.2016",118,1,"V-75",1,-25507.87,75,"NOK",1),
                BuildHelper.Transaction("31.03.2016",118,1,"V-76",1,32631.69,76,"NOK",1),
                BuildHelper.Transaction("31.03.2016",118,1,"V-77",1,-10534.45,77,"NOK",1),
                BuildHelper.Transaction("31.03.2016",118,1,"V-78",1,13349.69,78,"NOK",1),
                BuildHelper.Transaction("31.03.2016",118,1,"V-79",1,-107702.38,79,"NOK",1),
                BuildHelper.Transaction("31.03.2016",118,1,"V-80",1,127815.48,80,"NOK",1),
                BuildHelper.Transaction("31.03.2016",118,1,"V-81",1,-26102.18,81,"NOK",1),
                BuildHelper.Transaction("31.03.2016",118,1,"V-82",1,30976.69,82,"NOK",1),
                BuildHelper.Transaction("31.03.2016",118,1,"V-83",1,-19564.29,83,"NOK",1),
                BuildHelper.Transaction("31.03.2016",118,1,"V-84",1,23217.86,84,"NOK",1),
                BuildHelper.Transaction("30.04.2016",29,1,"V-85",-1,338275.56,85,"NOK",1),
                BuildHelper.Transaction("30.04.2016",29,1,"V-86",1,972542.22,86,"NOK",1),
                BuildHelper.Transaction("30.04.2016",20,1,"V-87",-1,795420,87,"NOK",1),
                BuildHelper.Transaction("30.04.2016",20,1,"V-88",1,570300,88,"NOK",1),
                BuildHelper.Transaction("30.04.2016",118,1,"V-89",1,-1848.74,89,"NOK",1),
                BuildHelper.Transaction("30.04.2016",118,1,"V-90",1,4369.75,90,"NOK",1),
                BuildHelper.Transaction("30.04.2016",118,1,"V-91",1,-32631.69,91,"NOK",1),
                BuildHelper.Transaction("30.04.2016",118,1,"V-92",1,39525.71,92,"NOK",1),
                BuildHelper.Transaction("30.04.2016",118,1,"V-93",1,-13349.69,93,"NOK",1),
                BuildHelper.Transaction("30.04.2016",118,1,"V-94",1,16074.11,94,"NOK",1),
                BuildHelper.Transaction("30.04.2016",118,1,"V-95",1,-127815.48,95,"NOK",1),
                BuildHelper.Transaction("30.04.2016",118,1,"V-96",1,147279.76,96,"NOK",1),
                BuildHelper.Transaction("30.04.2016",118,1,"V-97",1,-30976.69,97,"NOK",1),
                BuildHelper.Transaction("30.04.2016",118,1,"V-98",1,35693.95,98,"NOK",1),
                BuildHelper.Transaction("30.04.2016",118,1,"V-99",1,-23217.86,99,"NOK",1),
                BuildHelper.Transaction("30.04.2016",118,1,"V-100",1,26753.57,100,"NOK",1),
                BuildHelper.Transaction("31.05.2016",29,1,"V-101",-1,972542.22,101,"NOK",1),
                BuildHelper.Transaction("31.05.2016",29,1,"V-102",1,1627951.11,102,"NOK",1),
                BuildHelper.Transaction("31.05.2016",20,1,"V-103",-1,570300,103,"NOK",1),
                BuildHelper.Transaction("31.05.2016",20,1,"V-104",1,200460,104,"NOK",1),
                BuildHelper.Transaction("31.05.2016",118,1,"V-105",1,-4369.75,105,"NOK",1),
                BuildHelper.Transaction("31.05.2016",118,1,"V-106",1,6974.79,106,"NOK",1),
                BuildHelper.Transaction("31.05.2016",118,1,"V-107",1,-39525.71,107,"NOK",1),
                BuildHelper.Transaction("31.05.2016",118,1,"V-108",1,46649.53,108,"NOK",1),
                BuildHelper.Transaction("31.05.2016",118,1,"V-109",1,-16074.11,109,"NOK",1),
                BuildHelper.Transaction("31.05.2016",118,1,"V-110",1,18889.35,110,"NOK",1),
                BuildHelper.Transaction("31.05.2016",118,1,"V-111",1,-147279.76,111,"NOK",1),
                BuildHelper.Transaction("31.05.2016",118,1,"V-112",1,167392.86,112,"NOK",1),
                BuildHelper.Transaction("31.05.2016",118,1,"V-113",1,-35693.95,113,"NOK",1),
                BuildHelper.Transaction("31.05.2016",118,1,"V-114",1,40568.45,114,"NOK",1),
                BuildHelper.Transaction("31.05.2016",118,1,"V-115",1,-26753.57,115,"NOK",1),
                BuildHelper.Transaction("31.05.2016",118,1,"V-116",1,30407.14,116,"NOK",1),
                BuildHelper.Transaction("15.06.2016",9,1,"V-117",1,1923942.22,117,"NOK",1),
                BuildHelper.Transaction("15.06.2016",29,1,"V-118",-1,1627951.11,118,"NOK",1),
                BuildHelper.Transaction("30.06.2016",29,1,"V-119",1,331128.89,119,"NOK",1),
                BuildHelper.Transaction("30.06.2016",20,1,"V-120",-1,200460,120,"NOK",1),
                BuildHelper.Transaction("30.06.2016",20,1,"V-121",1,50380,121,"NOK",1),
                BuildHelper.Transaction("30.06.2016",118,1,"V-122",1,-6974.79,122,"NOK",1),
                BuildHelper.Transaction("30.06.2016",118,1,"V-123",1,9495.8,123,"NOK",1),
                BuildHelper.Transaction("30.06.2016",118,1,"V-124",1,-46649.53,124,"NOK",1),
                BuildHelper.Transaction("30.06.2016",118,1,"V-125",1,53543.55,125,"NOK",1),
                BuildHelper.Transaction("30.06.2016",118,1,"V-126",1,-18889.35,126,"NOK",1),
                BuildHelper.Transaction("30.06.2016",118,1,"V-127",1,21613.78,127,"NOK",1),
                BuildHelper.Transaction("30.06.2016",118,1,"V-128",1,-167392.86,128,"NOK",1),
                BuildHelper.Transaction("30.06.2016",118,1,"V-129",1,186857.14,129,"NOK",1),
                BuildHelper.Transaction("30.06.2016",118,1,"V-130",1,-40568.45,130,"NOK",1),
                BuildHelper.Transaction("30.06.2016",118,1,"V-131",1,45285.71,131,"NOK",1),
                BuildHelper.Transaction("30.06.2016",118,1,"V-132",1,-30407.14,132,"NOK",1),
                BuildHelper.Transaction("30.06.2016",118,1,"V-133",1,33942.86,133,"NOK",1),
                BuildHelper.Transaction("31.07.2016",29,1,"V-134",-1,331128.89,134,"NOK",1),
                BuildHelper.Transaction("31.07.2016",29,1,"V-135",1,972691.11,135,"NOK",1),
                BuildHelper.Transaction("31.07.2016",20,1,"V-136",-1,50380,136,"NOK",1),
                BuildHelper.Transaction("31.07.2016",19,1,"V-137",1,142580,137,"NOK",1),
                BuildHelper.Transaction("31.07.2016",118,1,"V-138",1,-9495.8,138,"NOK",1),
                BuildHelper.Transaction("31.07.2016",118,1,"V-139",1,12100.84,139,"NOK",1),
                BuildHelper.Transaction("31.07.2016",118,1,"V-140",1,-53543.55,140,"NOK",1),
                BuildHelper.Transaction("31.07.2016",118,1,"V-141",1,60667.37,141,"NOK",1),
                BuildHelper.Transaction("31.07.2016",118,1,"V-142",1,-21613.78,142,"NOK",1),
                BuildHelper.Transaction("31.07.2016",118,1,"V-143",1,24429.02,143,"NOK",1),
                BuildHelper.Transaction("31.07.2016",118,1,"V-144",1,-186857.14,144,"NOK",1),
                BuildHelper.Transaction("31.07.2016",118,1,"V-145",1,206970.24,145,"NOK",1),
                BuildHelper.Transaction("31.07.2016",118,1,"V-146",1,-45285.71,146,"NOK",1),
                BuildHelper.Transaction("31.07.2016",118,1,"V-147",1,50160.22,147,"NOK",1),
                BuildHelper.Transaction("31.07.2016",118,1,"V-148",1,-33942.86,148,"NOK",1),
                BuildHelper.Transaction("31.07.2016",118,1,"V-149",1,37596.43,149,"NOK",1)};

            return Input;
        }

        [TestMethod]
        public void TestBondSantander()
        {
            AmortizationInput input = SetUpBondsSantander();
            AmortizationOutput output = CommandHelper.GenerateAmortizationOutput(input);
            AssertHelper.VerifyOutputTotalAccumulatedAmortizationEquals("15.06.2016", 325968.8224, output);
        }
    }

}