﻿using DevelopmentInProgress.TradeView.Wpf.Host.Context;
using DevelopmentInProgress.TradeView.Wpf.Host.View;
using DevelopmentInProgress.TradeView.Wpf.Configuration.ViewModel;

namespace DevelopmentInProgress.TradeView.Wpf.Configuration.View
{
    /// <summary>
    /// Interaction logic for StrategyManagerView.xaml
    /// </summary>
    public partial class StrategyManagerView : DocumentViewBase
    {
        public StrategyManagerView(IViewContext viewContext, StrategyManagerViewModel strategyManagerViewModel)
            : base(viewContext, strategyManagerViewModel, Module.ModuleName)
        {
            InitializeComponent();

            DataContext = strategyManagerViewModel;
        }
    }
}
