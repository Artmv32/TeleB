using System;
using System.Collections.Generic;
using System.Linq;

namespace TeleBot
{
    public enum Term : byte
    {
        Undefined,
        Short,
        Middle,
        Long
    }

    public class TradeSignal
    {
        public string Currency { get; set; }

        public double BuyPriceMin { get; set; }

        public double BuyPriceMax { get; set; }

        public double[] SellPrice { get; set; }

        public double StopLoss { get; set; }

        public Term Term { get; set; }

        public static TradeSignal Empty { get; private set; }
        static TradeSignal()
        {
            Empty = new TradeSignal();
        }

        public TradeSignal()
        {
            StopLoss = double.NaN;
        }
    }

    public class MessageParser
    {
        private const string CoinKeyword = "МОНЕТА";
        private const string BuyKeyword = "ПОКУПКА";
        private const string SellKeyword = "ПРОДАЖА";
        private const string StopLossKeyword = "STOP-LOSS";
        private const string MoneyMark = "💸";
        private const string ShortTermKeyword = "Короткосрок";
        private const string MiddleTermKeyword = "Среднесрок";
        private const string LongTermKeyword = "Долгосрок";

        protected static string RemoveKeyword(string text, string keyword)
        {
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(keyword))
            {
                var result = text;
                result = result.Trim();
                if (result.StartsWith(keyword))
                {
                    result = result.Replace(keyword, string.Empty).TrimStart();
                    if (result.StartsWith(":"))
                    {
                        result = result.Substring(1).Trim();
                        text = result;
                    }
                }
            }
            return text;
        }

        public static TradeSignal ProcessMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return TradeSignal.Empty;
            }

            var lines = message.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            var result = new TradeSignal();
            int i = 0;
            while (i < lines.Length)
            {
                var line = lines[i];
                if (line.StartsWith(CoinKeyword))
                {
                    var coin = RemoveKeyword(line, CoinKeyword);
                    int bracketIndex = coin.IndexOf("(");
                    if (bracketIndex > -1)
                    {
                        coin = coin.Substring(0, bracketIndex);
                    }
                    result.Currency = coin;
                }
                else if (line.StartsWith(BuyKeyword))
                {
                    var buyPrice = RemoveKeyword(line, BuyKeyword);
                    if (buyPrice.Contains("-"))
                    {
                        var range = buyPrice.Split('-');
                        double min, max;
                        if (double.TryParse(range[0], out min) && double.TryParse(range[1], out max))
                        {
                            result.BuyPriceMin = min;
                            result.BuyPriceMax = max;
                        }
                    }
                    else
                    {
                        double price;
                        if (double.TryParse(buyPrice, out price))
                        {
                            result.BuyPriceMin = result.BuyPriceMax = price;
                        }
                    }
                }
                else if (line.StartsWith(SellKeyword))
                {
                    var sellList = new List<double>();
                    int j = i + 1;
                    while (j < lines.Length)
                    {
                        line = lines[j];
                        if (line.StartsWith(MoneyMark))
                        {
                            var clearLine = line.Replace(MoneyMark, string.Empty);
                            var sellPrice = double.Parse(clearLine);
                            sellList.Add(sellPrice);
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    result.SellPrice = sellList.ToArray();
                }
                else if (line.StartsWith(StopLossKeyword))
                {
                    var stopLoss = RemoveKeyword(line, StopLossKeyword);
                    result.StopLoss = double.Parse(stopLoss);
                }
                else
                {
                    if (line.Contains(ShortTermKeyword))
                    {
                        result.Term = Term.Short;
                    }
                    else if (line.Contains(MiddleTermKeyword))
                    {
                        result.Term = Term.Middle;
                    }
                    else if (line.Contains(LongTermKeyword))
                    {
                        result.Term = Term.Short;
                    }
                }
                i++;
            }

            if (IsValidAction(result))
            {
                return result;
            }
            return TradeSignal.Empty;
        }

        private static bool IsValidAction(TradeSignal action)
        {
            if (action != null && !string.IsNullOrWhiteSpace(action.Currency) &&
                action.BuyPriceMax > 0 && action.BuyPriceMin > 0 &&
                action.SellPrice?.Any() == true)
            {
                return true;
            }
            return false;
        }
    }
}
