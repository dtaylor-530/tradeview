﻿using DevelopmentInProgress.TradeView.Interface.Server;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevelopmentInProgress.TradeView.Data
{
    public interface ITradeViewConfigurationServer
    {
        Task<List<Server>> GetServersAsync();
        Task<Server> GetServerAsync(string serverName);
        Task SaveServerAsync(Server server);
        Task DeleteServerAsync(Server server);
    }
}