﻿using DevelopmentInProgress.TradeView.Interface.Model;
using System.Collections.Generic;

namespace DevelopmentInProgress.TradeView.Interface.Strategy
{
    public class StrategySymbolPerformance
    {
        public StrategySymbolPerformance()
        {
            Marks = new List<Mark>();
            Trades = new List<AccountTrade>();
        }

        public string Symbol { get; set; }
        public List<Mark> Marks { get; set; }
        public List<AccountTrade> Trades { get; set; }
    }
}