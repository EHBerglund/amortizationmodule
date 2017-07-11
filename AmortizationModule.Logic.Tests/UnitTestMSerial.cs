﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AmortizationModule.Logic.Language;
using AmortizationModule.Logic.DTO.Internal;
using System.Collections.Generic;
using AmortizationModule.Logic.DTO.External;

namespace AmortizationModule.Logic
{
    [TestClass]
    public class TestBondsSerial
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

        private AmortizationInput SetUpBondsSerial()
        {
            AmortizationInput Input = new AmortizationInput();

            Input.UserInput = BuildHelper.CreateUserInput(PositionSeq: 1, CalculationDate: "30.06.2008");

            Input.AmortizationSecurity = BuildHelper.Security(
            SecuritySeq: 1,
            SecurityType: 2,
            MaturityDate: "31.12.2009",
            Floater: false,
            Currency: "NOK");

            Input.Settings = BuildHelper.CreateSettings(
            Method: 1,
            OutputAggregated: true,
            InterestMethod: 1);

            Input.AmortizationSecurity.Instalments = new Dictionary<DateTime, double>() {
                { new DateTime(2005,12,31), 0.2},
                { new DateTime(2006,12,31), 0.2},
                { new DateTime(2007,12,31), 0.2},
                { new DateTime(2008,12,31), 0.2},
                { new DateTime(2009,12,31), 0.2},
            };

            Input.AmortizationSecurity.InterestTerms = new Dictionary<DateTime, double> {
                { new DateTime(2005,06,30), 0.06},
                { new DateTime(2005,12,31), 0.06},
                { new DateTime(2006,06,30), 0.048},
                { new DateTime(2006,12,31), 0.048},
                { new DateTime(2007,06,30), 0.036},
                { new DateTime(2007,12,31), 0.036},
                { new DateTime(2008,06,30), 0.024},
                { new DateTime(2008,12,31), 0.024},
                { new DateTime(2009,06,30), 0.012},
                { new DateTime(2009,12,31), 0.012},
            };

            Input.InterestRates = BuildHelper.CreateInterestRates(new Dictionary<string, double>(){
                { "01.01.2005",0.12}});

            Input.AmortizationTransactions = new List<AmortizationTransaction>(){
                BuildHelper.Transaction("01.01.2005",4,1,"V-01",100000,0.95409,1,"NOK",1),
                BuildHelper.Transaction("30.06.2005",9,1,"V-02",6000,1,2,"NOK",1),
                BuildHelper.Transaction("31.12.2005",9,1,"V-03",6000,1,3,"NOK",1),
                BuildHelper.Transaction("31.12.2005",68,1,"V-04",20000,1,4,"NOK",1),
                BuildHelper.Transaction("30.06.2006",9,1,"V-05",4800,1,5,"NOK",1),
                BuildHelper.Transaction("31.12.2006",9,1,"V-06",4800,1,6,"NOK",1),
                BuildHelper.Transaction("31.12.2006",68,1,"V-07",20000,1,7,"NOK",1),
                BuildHelper.Transaction("30.06.2007",9,1,"V-08",3600,1,8,"NOK",1),
                BuildHelper.Transaction("31.12.2007",9,1,"V-09",3600,1,9,"NOK",1),
                BuildHelper.Transaction("31.12.2007",68,1,"V-10",20000,1,10,"NOK",1),
                BuildHelper.Transaction("30.06.2008",9,1,"V-11",2400,1,11,"NOK",1),
                BuildHelper.Transaction("31.12.2008",9,1,"V-12",2400,1,12,"NOK",1),
                BuildHelper.Transaction("31.12.2008",68,1,"V-13",20000,1,13,"NOK",1),
                BuildHelper.Transaction("30.06.2009",9,1,"V-14",1200,1,14,"NOK",1),
                BuildHelper.Transaction("31.12.2009",9,1,"V-15",1200,1,15,"NOK",1),
                BuildHelper.Transaction("31.12.2009",68,1,"V-16",20000,1,16,"NOK",1)
            };

            return Input;

        }

        [TestMethod]
        public void TestBondSerial()

        {
            AmortizationInput input = SetUpBondsSerial();
            AmortizationOutput output = CommandHelper.GenerateAmortizationOutput(input);
            AssertHelper.VerifyOutputTotalAccumulatedAmortizationEquals("30.06.2008", 327.243, output);
        }

    }


}
