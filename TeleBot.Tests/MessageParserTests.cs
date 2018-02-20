using NUnit.Framework;
using System.IO;
using System.Reflection;

namespace TeleBot.Tests
{
    [TestFixture]
    public class MessageAnalyzerTests
    {
        public class InternalMessageParser : MessageParser
        {
            public static string RemoveKeyword_(string text, string keyword)
            {
                return MessageParser.RemoveKeyword(text, keyword);
            }
        }

        [TestCase("", "", ExpectedResult = "")]
        [TestCase("", "asd", ExpectedResult = "")]
        [TestCase("", null, ExpectedResult = "")]
        [TestCase(null, null, ExpectedResult = null)]
        [TestCase(null, "", ExpectedResult = null)]
        [TestCase(null, "asd", ExpectedResult = null)]
        [TestCase("asd", "bca", ExpectedResult = "asd")]
        [TestCase("asd :", "bca", ExpectedResult = "asd :")]
        [TestCase(": asd", "bca", ExpectedResult = ": asd")]
        [TestCase(" asd", "bca", ExpectedResult = " asd")]
        [TestCase(" asd", "asd", ExpectedResult = " asd")]
        [TestCase(" asd:", "asd", ExpectedResult = "")]
        [TestCase("asd", "asd", ExpectedResult = "asd")]
        [TestCase("asd:", "asd", ExpectedResult = "")]
        [TestCase("!asd", "asd", ExpectedResult = "!asd")]
        [TestCase("ПОКУПКА: 20", "ПОКУПКА", ExpectedResult = "20")]
        [TestCase("ПОКУПКА: 20-30", "ПОКУПКА", ExpectedResult = "20-30")]
        [TestCase("ПОКУПКА : 20-30", "ПОКУПКА", ExpectedResult = "20-30")]
        [TestCase("ПОКУПКА :20-30", "ПОКУПКА", ExpectedResult = "20-30")]
        [TestCase("ПОКУПКА 20-30", "ПОКУПКА", ExpectedResult = "ПОКУПКА 20-30")]
        public string RemoveKeyword(string input, string keyword)
        {
            return InternalMessageParser.RemoveKeyword_(input, keyword);
        }

        [TestCase]
        public void TestMessage_BuySellSellSell()
        {
            var action = CreateAndParse("SampleMessages/BuySellSellSellSell.txt");
            Assert.AreEqual(Term.Undefined, action.Term);
            Assert.AreEqual("ADA", action.Currency);
            Assert.AreEqual(double.NaN, action.StopLoss);
            Assert.AreEqual(0.000052, action.BuyPriceMin);
            Assert.AreEqual(0.000053, action.BuyPriceMax);
            Assert.AreEqual(4, action.SellPrice.Length);
            Assert.AreEqual(0.0000567, action.SellPrice[0]);
            Assert.AreEqual(0.0000604, action.SellPrice[1]);
            Assert.AreEqual(0.0000641, action.SellPrice[2]);
            Assert.AreEqual(0.0000678, action.SellPrice[3]);
        }

        [TestCase]
        public void TestMessage_NewsWithBuySell()
        {
            var action = CreateAndParse("SampleMessages/NewsWithBuySell.txt");
            Assert.AreSame(TradeSignal.Empty, action);
        }

        [TestCase]
        public void TestMessage_XrpAnnouncement()
        {
            var action = CreateAndParse("SampleMessages/XrpAnnouncement.txt");
            Assert.AreSame(TradeSignal.Empty, action);
        }

        [TestCase]
        public void TestMessage_SimpleBuySell()
        {
            var action = CreateAndParse("SampleMessages/SimpleBuySell.txt");
            Assert.AreEqual("NXT", action.Currency);
            Assert.AreEqual(double.NaN, action.StopLoss);
            Assert.AreEqual(Term.Short, action.Term);
            Assert.AreEqual(0.0000256, action.BuyPriceMin);
            Assert.AreEqual(0.0000256, action.BuyPriceMax);
            Assert.AreEqual(1, action.SellPrice.Length);
            Assert.AreEqual(0.0000294, action.SellPrice[0]);
        }

        [TestCase]
        public void TestMessage_StopLoss()
        {
            var action = CreateAndParse("SampleMessages/StopLoss.txt");
            Assert.AreEqual("VIBE", action.Currency);
            Assert.AreEqual(Term.Middle, action.Term);
            Assert.AreEqual(0.000057, action.StopLoss);
            Assert.AreEqual(0.000080, action.BuyPriceMin);
            Assert.AreEqual(0.000082, action.BuyPriceMax);
            Assert.AreEqual(4, action.SellPrice.Length);
            Assert.AreEqual(0.00010, action.SellPrice[0]);
            Assert.AreEqual(0.00012, action.SellPrice[1]);
            Assert.AreEqual(0.00014, action.SellPrice[2]);
            Assert.AreEqual(0.00016, action.SellPrice[3]);
            Assert.AreEqual(0.000057, action.StopLoss);
        }

        [TestCase]
        public void TestMessage_TermMiddle()
        {
            var action = CreateAndParse("SampleMessages/TermMiddle.txt");
            Assert.AreEqual("VIBE", action.Currency);
            Assert.AreEqual(Term.Middle, action.Term);
            Assert.AreEqual(0.000080, action.BuyPriceMin);
            Assert.AreEqual(0.000082, action.BuyPriceMax);
            Assert.AreEqual(4, action.SellPrice.Length);
            Assert.AreEqual(0.00010, action.SellPrice[0]);
            Assert.AreEqual(0.00012, action.SellPrice[1]);
            Assert.AreEqual(0.00014, action.SellPrice[2]);
            Assert.AreEqual(0.00016, action.SellPrice[3]);
            Assert.AreEqual(0.000057, action.StopLoss);
        }

        [TestCase]
        public void TestMessage_TermShort()
        {
            var action = CreateAndParse("SampleMessages/TermShort.txt");
            Assert.AreEqual("TRST", action.Currency);
            Assert.AreEqual(Term.Short, action.Term);
            Assert.AreEqual(double.NaN, action.StopLoss);
            Assert.AreEqual(0.00005, action.BuyPriceMin);
            Assert.AreEqual(0.00005, action.BuyPriceMax);
            Assert.AreEqual(1, action.SellPrice.Length);
            Assert.AreEqual(0.000575, action.SellPrice[0]);
        }

        private static TradeSignal CreateAndParse(string file)
        {
            string message = null;
            var resource = "TeleBot.Tests.SampleMessages." + file.Replace("SampleMessages/", "");
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
            using (var reader = new StreamReader(stream))
            {
                message = reader.ReadToEnd();
            }
            var target = new MessageParser();
            return target.ProcessMessage(message);
        }
    }
}
