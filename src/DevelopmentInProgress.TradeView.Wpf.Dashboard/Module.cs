﻿using DevelopmentInProgress.TradeView.Wpf.Dashboard.View;
using DevelopmentInProgress.TradeView.Wpf.Dashboard.ViewModel;
using DevelopmentInProgress.TradeView.Wpf.Host.Module;
using DevelopmentInProgress.TradeView.Wpf.Host.Navigation;
using Microsoft.Practices.Unity;
using Prism.Logging;
using System;

namespace DevelopmentInProgress.TradeView.Wpf.Dashboard
{
    public class Module : ModuleBase
    {
        public const string ModuleName = "Dashboard";

        public Module(IUnityContainer container, ModuleNavigator moduleNavigator, ILoggerFacade logger)
            : base(container, moduleNavigator, logger)
        {
        }

        public override void Initialize()
        {
            Container.RegisterType<Object, ServerMonitorView>(typeof(ServerMonitorView).Name);
            Container.RegisterType<ServerMonitorViewModel>(typeof(ServerMonitorViewModel).Name);

            var moduleSettings = new ModuleSettings();
            moduleSettings.ModuleName = ModuleName;
            moduleSettings.ModuleImagePath = @"/DevelopmentInProgress.TradeView.Wpf.Dashboard;component/Images/Dashboard.png";

            var moduleGroup = new ModuleGroup();
            moduleGroup.ModuleGroupName = "Dashboard";

            var newDocument = new ModuleGroupItem();
            newDocument.ModuleGroupItemName = "Server Monitor";
            newDocument.TargetView = typeof(ServerMonitorView).Name;
            newDocument.TargetViewTitle = "Server Monitor";
            newDocument.ModuleGroupItemImagePath = @"/DevelopmentInProgress.TradeView.Wpf.Dashboard;component/Images/ServerMonitor.png";

            moduleGroup.ModuleGroupItems.Add(newDocument);
            moduleSettings.ModuleGroups.Add(moduleGroup);
            ModuleNavigator.AddModuleNavigation(moduleSettings);

            Logger.Log("Initialize DevelopmentInProgress.TradeView.Wpf.Dashboard", Category.Info, Priority.None);

        }
    }
}
