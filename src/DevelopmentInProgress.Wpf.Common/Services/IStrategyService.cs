﻿using DevelopmentInProgress.Wpf.Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevelopmentInProgress.Wpf.Common.Services
{
    public interface IStrategyService
    {
        Task<List<Strategy>> GetStrategies();
        Task<Strategy> GetStrategy(string strategyName);
        Task SaveStrategy(Strategy strategy);
        Task DeleteStrategy(Strategy strategy);
        Task<MarketView.Interface.Strategy.StrategyPerformance> GetStrategyPerformance(string strategyName);
        Task SaveStrategyPerformance(MarketView.Interface.Strategy.StrategyPerformance strategyPerformance);
    }
}