﻿using DevelopmentInProgress.TradeView.Wpf.Common.Model;
using DevelopmentInProgress.TradeView.Wpf.Common.Services;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;
using Prism.Logging;
using DevelopmentInProgress.TradeView.Wpf.Common.Cache;
using DevelopmentInProgress.TradeView.Wpf.Common.Events;

namespace DevelopmentInProgress.TradeView.Wpf.Common.ViewModel
{
    public class AccountViewModel : ExchangeViewModel
    {
        private CancellationTokenSource accountCancellationTokenSource;
        private DispatcherTimer dispatcherTimer;
        private ISymbolsCacheFactory symbolsCacheFactory;
        private ISymbolsCache symbolsCache;
        private Account account;
        private AccountBalance selectedAsset;
        private bool isLoggingIn;
        private bool disposed;
        private object balancesLock = new object();

        public AccountViewModel(IWpfExchangeService exchangeService, ISymbolsCacheFactory symbolsCacheFactory, ILoggerFacade logger)
            : base(exchangeService, logger)
        {
            accountCancellationTokenSource = new CancellationTokenSource();

            this.symbolsCacheFactory = symbolsCacheFactory;
        }

        public event EventHandler<AccountEventArgs> OnAccountNotification;

        public Account Account
        {
            get { return account; }
            set
            {
                if (account != value)
                {
                    account = value;
                    OnPropertyChanged("Account");
                }
            }
        }

        public AccountBalance SelectedAsset
        {
            get { return selectedAsset; }
            set
            {
                if (selectedAsset != value)
                {
                    selectedAsset = value;
                    OnSelectedAsset(selectedAsset);
                    OnPropertyChanged("SelectedAsset");
                }
            }
        }

        public bool IsLoggingIn
        {
            get { return isLoggingIn; }
            set
            {
                if (isLoggingIn != value)
                {
                    isLoggingIn = value;
                    OnPropertyChanged("IsLoggingIn");
                }
            }
        }

        public override void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                if (dispatcherTimer != null)
                {
                    dispatcherTimer.Tick -= new EventHandler(DispatcherTimerTick);
                }

                if (accountCancellationTokenSource != null
                    && !accountCancellationTokenSource.IsCancellationRequested)
                {
                    accountCancellationTokenSource.Cancel();
                }
            }

            disposed = true;
        }

        public async Task Login(Account account)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(account.AccountInfo.User.ApiKey)
                    || string.IsNullOrWhiteSpace(account.AccountInfo.User.ApiSecret))
                {
                    return;
                }

                IsLoggingIn = true;

                Account = await ExchangeService.GetAccountInfoAsync(account.AccountInfo.User.Exchange, account.AccountInfo.User, accountCancellationTokenSource.Token);

                OnAccountLoggedIn();

                await ExchangeService.SubscribeAccountInfo(Account.AccountInfo.User.Exchange, Account.AccountInfo.User, e => AccountInfoUpdate(e.AccountInfo), SubscribeAccountInfoException, accountCancellationTokenSource.Token);

                symbolsCache = symbolsCacheFactory.GetSymbolsCache(Account.AccountInfo.User.Exchange);

                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(DispatcherTimerTick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
                dispatcherTimer.Start();

                DispatcherTimerTick(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                OnException("AccountViewModel.Login", ex);
            }
            finally
            {
                IsLoggingIn = false;
            }
        }

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            try
            {
                lock (balancesLock)
                {
                    symbolsCache.ValueAccount(Account);
                }
            }
            catch (Exception ex)
            {
                OnException("AccountViewModel.DispatcherTimerTick", ex);
            }
        }

        private void SubscribeAccountInfoException(Exception exception)
        {
            OnException("AccountViewModel.Login - ExchangeService.SubscribeAccountInfo", exception);
        }

        private void OnException(string message, Exception exception)
        {
            var onAccountNotification = OnAccountNotification;
            onAccountNotification?.Invoke(this, new AccountEventArgs { Message = message, Exception = exception });
        }

        private void OnAccountLoggedIn()
        {
            AccountNotification(new AccountEventArgs { Value = Account, AccountEventType = AccountEventType.LoggedIn });
        }

        private void OnSelectedAsset(AccountBalance selectedAsset)
        {
            AccountNotification(new AccountEventArgs { Value = Account, SelectedAsset = selectedAsset, AccountEventType = AccountEventType.SelectedAsset });
        }

        private void AccountNotification(AccountEventArgs args)
        {
            var onAccountNotification = OnAccountNotification;
            onAccountNotification?.Invoke(this, args);
        }

        private void AccountInfoUpdate(Interface.Model.AccountInfo e)
        {
            Action<Interface.Model.AccountInfo> action = aie =>
            {
                lock (balancesLock)
                {
                    if (aie.Balances == null
                        || !aie.Balances.Any())
                    {
                        Account.Balances.Clear();
                        return;
                    }

                    Func<AccountBalance, Interface.Model.AccountBalance, AccountBalance> f = ((ab, nb) =>
                    {
                        ab.Free = nb.Free;
                        ab.Locked = nb.Locked;
                        return ab;
                    });

                    var balances = (from ab in Account.Balances
                                    join nb in aie.Balances on ab.Asset equals nb.Asset
                                    select f(ab, nb)).ToList();

                    var remove = Account.Balances.Where(ab => !aie.Balances.Any(nb => nb.Asset.Equals(ab.Asset))).ToList();
                    foreach (var ob in remove)
                    {
                        Account.Balances.Remove(ob);
                    }

                    var add = aie.Balances.Where(nb => !Account.Balances.Any(ab => ab.Asset.Equals(nb.Asset))).ToList();
                    foreach (var nb in add)
                    {
                        Account.Balances.Add(new AccountBalance { Asset = nb.Asset, Free = nb.Free, Locked = nb.Locked });
                    }
                }
            };

            if (Dispatcher == null)
            {
                action(e);
            }
            else
            {
                Dispatcher.Invoke(action, e);
            }

            AccountNotification(new AccountEventArgs { Value = Account, AccountEventType = AccountEventType.UpdateOrders });
        }
    }
}
