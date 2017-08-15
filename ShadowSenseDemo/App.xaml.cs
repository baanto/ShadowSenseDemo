using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ShadowSenseDemo.Views;
using Splat;

namespace ShadowSenseDemo
{
    public partial class App : Application
    {
        public static AppBootstrapper Bootstrapper;
        public static ShellView ShellView;

        public App()
        {
            Bootstrapper = new AppBootstrapper();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShellView = new ShellView(Locator.Current.GetService<ShellViewModel>());
            ShellView.Show();
        }
    }
}
