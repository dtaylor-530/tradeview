﻿using DevelopmentInProgress.TradeView.Data;
using DevelopmentInProgress.TradeView.Wpf.Common.Extensions;
using DevelopmentInProgress.TradeView.Wpf.Common.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopmentInProgress.TradeView.Wpf.Common.Services
{
    public class StrategyService : IStrategyService
    {
        private readonly ITradeViewConfigurationStrategy configurationStrategy;

        public StrategyService(ITradeViewConfigurationStrategy configurationStrategy)
        {
            this.configurationStrategy = configurationStrategy;
        }

        public async Task<List<Strategy>> GetStrategies()
        {
            var result = await configurationStrategy.GetStrategiesAsync();
            return result.Select(s => s.ToWpfStrategy()).ToList();
        }

        public async Task<Strategy> GetStrategy(string strategyName)
        {
            var result = await configurationStrategy.GetStrategyAsync(strategyName);
            return result.ToWpfStrategy();
        }

        public Task SaveStrategy(Strategy strategy)
        {
            return configurationStrategy.SaveStrategyAsync(strategy.ToInterfaceStrategyConfig());
        }

        public Task DeleteStrategy(Strategy strategy)
        {
            return configurationStrategy.DeleteStrategyAsync(strategy.ToInterfaceStrategyConfig());
        }
    }
}