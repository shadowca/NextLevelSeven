﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.MessageGeneration;

namespace NextLevelSeven.Test.MessageGeneration
{
    [TestClass]
    public class MessageGeneratorPerformanceTests
    {
        [TestMethod]
        public void MessageGenerator_Timely_GeneratesMessages()
        {
            var type = Randomized.StringLetters(3);
            var trigger = Randomized.StringLetters(3);
            var time = Measure.ExecutionTime(() =>
            {
                var message = MessageGenerator.Generate(type, trigger, Randomized.String());
                Assert.AreEqual(message.Type, type);
                Assert.AreEqual(message.TriggerEvent, trigger);
            }, 100);
            AssertTime.IsWithin(1000, time);
        }
    }
}