﻿using DevelopmentInProgress.TradeView.Interface.Enums;
using System;

namespace DevelopmentInProgress.TradeView.Interface.Interfaces
{
    public interface ITrade
    {
        string Symbol { get; set; }
        Exchange Exchange { get; set; }
        long Id { get; set; }
        decimal Price { get; set; }
        decimal Quantity { get; set; }
        DateTime Time { get; set; }
        bool IsBuyerMaker { get; set; }
        bool IsBestPriceMatch { get; set; }
    }
}
