﻿using DevelopmentInProgress.TradeView.Interface.Events;
using DevelopmentInProgress.TradeView.Interface.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DevelopmentInProgress.TradeView.Interface.Interfaces
{
    public interface IExchangeApi
    {
        Task<Order> PlaceOrder(User user, ClientOrder clientOrder, long recWindow = 0, CancellationToken cancellationToken = default(CancellationToken));
        Task<string> CancelOrderAsync(User user, string symbol, string orderId, string newClientOrderId = null, long recWindow = 0, CancellationToken cancellationToken = default(CancellationToken));
        Task<AccountInfo> GetAccountInfoAsync(User user, CancellationToken cancellationToken);
        Task<IEnumerable<AccountTrade>> GetAccountTradesAsync(User user, string symbol, DateTime startDate, DateTime endDate, long recWindow = 0, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<Symbol>> GetSymbols24HourStatisticsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Symbol>> GetSymbolsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<SymbolStats>> Get24HourStatisticsAsync(CancellationToken cancellationToken);
        Task<OrderBook> GetOrderBookAsync(string symbol, int limit, CancellationToken cancellationToken);
        Task<IEnumerable<AggregateTrade>> GetAggregateTradesAsync(string symbol, int limit, CancellationToken cancellationToken);
        Task<IEnumerable<Trade>> GetTradesAsync(string symbol, int limit, CancellationToken cancellationToken);
        Task<IEnumerable<Order>> GetOpenOrdersAsync(User user, string symbol = null, long recWindow = 0, Action<Exception> exception = default(Action<Exception>), CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<Candlestick>> GetCandlesticksAsync(string symbol, CandlestickInterval interval, DateTime startTime, DateTime endTime, int limit = default(int), CancellationToken token = default(CancellationToken));
        Task SubscribeCandlesticks(string symbol, CandlestickInterval candlestickInterval, int limit, Action<CandlestickEventArgs> callback, Action<Exception> exception, CancellationToken cancellationToken);
        Task SubscribeStatistics(Action<StatisticsEventArgs> callback, Action<Exception> exception, CancellationToken cancellationToken);
        Task SubscribeStatistics(IEnumerable<string> symbols, Action<StatisticsEventArgs> callback, Action<Exception> exception, CancellationToken cancellationToken);
        Task SubscribeOrderBook(string symbol, int limit, Action<OrderBookEventArgs> callback, Action<Exception> exception, CancellationToken cancellationToken);
        Task SubscribeAggregateTrades(string symbol, int limit, Action<TradeEventArgs> callback, Action<Exception> exception, CancellationToken cancellationToken);
        Task SubscribeTrades(string symbol, int limit, Action<TradeEventArgs> callback, Action<Exception> exception, CancellationToken cancellationToken);
        Task SubscribeAccountInfo(User user, Action<AccountInfoEventArgs> callback, Action<Exception> exception, CancellationToken cancellationToken);
    }
}