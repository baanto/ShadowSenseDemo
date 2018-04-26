using Baanto.ShadowSense.Events;
using Baanto.ShadowSense;
using Baanto.ShadowSense.Services;
using ReactiveUI;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Threading;
using ShadowSenseDemo.ViewModels;

namespace ShadowSenseDemo
{
    public class ShellViewModel : ReactiveObject
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private readonly IShadowSenseService shadowSenseService;
        private readonly ObservableCollection<ReactiveObject> tabContents = new ObservableCollection<ReactiveObject>();


        public ShellViewModel(IShadowSenseService ss)
        {

            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(OnConnectTimerTick);
            timer.Start();


            this.tabContents.Add(Locator.Current.GetService<DeviceSelectViewModel>());
            this.tabContents.Add(Locator.Current.GetService<DrawViewModel>());
            this.tabContents.Add(Locator.Current.GetService<ConfigurationViewModel>());
            this.tabContents.Add(Locator.Current.GetService<UpgradeViewModel>());
            this.CurrentTab = Locator.Current.GetService<DeviceSelectViewModel>();

            this.shadowSenseService = ss;

           Observable.FromEventPattern<InsertedEvent>(this.shadowSenseService, "Inserted")
                .Subscribe(x => ShadowSenseDeviceInserted(x.Sender, x.EventArgs));

            Observable.FromEventPattern<RemovedEvent>(this.shadowSenseService, "Removed")
                 .Subscribe(x => ShadowSenseDeviceRemoved(x.Sender, x.EventArgs));

        }

        private string deviceName = "No ShadowSense device detected";
        public string DeviceName
        {
            get { return this.deviceName; }
            set { this.RaiseAndSetIfChanged(ref this.deviceName, value); }
        }

        private string deviceVersion = "";
        public string DeviceVersion
        {
            get { return this.deviceVersion; }
            set { this.RaiseAndSetIfChanged(ref this.deviceVersion, value); }
        }

        private string deviceSerial = "";
        public string DeviceSerial
        {
            get { return this.deviceSerial; }
            set { this.RaiseAndSetIfChanged(ref this.deviceSerial, value); }
        }

        private void OnConnectTimerTick(object o, EventArgs e)
        {
            //Open ShadowSense device
            if (this.shadowSenseService.ShadowSenseDevice != null &&
                this.shadowSenseService.ShadowSenseDevice.IsOpen)
            {
                    this.ShadowSenseDeviceInserted(null, null);
                    this.timer.Stop();
            }
            else
            {
                if (this.shadowSenseService.OpenDevice())
                    this.timer.Stop();
            }
        }

        public ObservableCollection<ReactiveObject> TabContents
        {
            get { return this.tabContents; }
        }
        private ReactiveObject currentTab;
        public ReactiveObject CurrentTab
        {
            get { return this.currentTab; }
            set { this.RaiseAndSetIfChanged(ref this.currentTab, value); }
        }


        private void ShadowSenseDeviceInserted(object sender, InsertedEvent e)
        {
            this.DeviceName = this.shadowSenseService.ShadowSenseDevice.Name;
            this.DeviceVersion = this.shadowSenseService.ShadowSenseDevice.Version != null ? string.Format("{0}.{1}", this.shadowSenseService.ShadowSenseDevice.Version.Major, this.shadowSenseService.ShadowSenseDevice.Version.Minor) : "Not available";
            this.DeviceSerial = this.shadowSenseService.ShadowSenseDevice.Serial;
        }

        private void ShadowSenseDeviceRemoved(object sender, Baanto.ShadowSense.Events.RemovedEvent e)
        {
            this.timer.Start();

            this.DeviceName = "No ShadowSense device detected";
            this.DeviceVersion = "";
            this.DeviceSerial = "";
        }
    }
}
