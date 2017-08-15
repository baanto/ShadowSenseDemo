using Baanto.ShadowSense.DFU;
using Baanto.ShadowSense.Services;
using ReactiveUI;
using ShadowSenseDemo.Helpers;
using ShadowSenseDemo.ViewModels;
using ShadowSenseDemo.Views;
using Splat;

namespace ShadowSenseDemo
{
    public class AppBootstrapper : ReactiveObject
    {
        public AppBootstrapper()
        {
            Locator.CurrentMutable.RegisterLazySingleton(() => new ShadowSenseService(), typeof(IShadowSenseService));
            Locator.CurrentMutable.RegisterLazySingleton(() => (IWindowsDialogs)new WindowsDialogs(), typeof(IWindowsDialogs));


            // ViewModels
            Locator.CurrentMutable.RegisterLazySingleton(() => new ShellViewModel(Locator.Current.GetService<IShadowSenseService>()), typeof(ShellViewModel));
            Locator.CurrentMutable.RegisterLazySingleton(() => new ConfigurationViewModel(Locator.Current.GetService<IShadowSenseService>()), typeof(ConfigurationViewModel));
            Locator.CurrentMutable.RegisterLazySingleton(() => 
                new UpgradeViewModel(
                    Locator.Current.GetService<IShadowSenseService>(),
                    Locator.Current.GetService<IWindowsDialogs>()), 
                    typeof(UpgradeViewModel));
            Locator.CurrentMutable.RegisterLazySingleton(() => new DrawViewModel(Locator.Current.GetService<IShadowSenseService>()), typeof(DrawViewModel));
            Locator.CurrentMutable.RegisterLazySingleton(() => new DeviceSelectViewModel(Locator.Current.GetService<IShadowSenseService>()), typeof(DeviceSelectViewModel));


            // Views



        }

    }
}
