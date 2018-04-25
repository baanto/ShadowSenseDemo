using Baanto.ShadowSense;
using Baanto.ShadowSense.HID;
using Baanto.ShadowSense.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ShadowSenseDemo.ViewModels
{
    public class DeviceSelectViewModel : ReactiveObject
    {
        private readonly IShadowSenseService shadowSenseService;
        public DeviceSelectViewModel(IShadowSenseService ss)
        {
            this.DisplayName = "HOME";

            this.shadowSenseService = ss;

            this.localDevices = new ReactiveList<ShadowSenseDeviceInfo>();
      //      this.localDevices.AddRange(this.shadowSenseService.GetDeviceInfo());
            this.currentDevice = this.localDevices.FirstOrDefault();

            this.Refresh = ReactiveCommand.CreateAsyncTask(x => Task.Run(() =>
            {
                var devices = new ReactiveList<ShadowSenseDeviceInfo>();

 //               devices.AddRange(this.shadowSenseService.GetDeviceInfo());

                this.LocalDevices = devices;
                this.CurrentDevice = this.localDevices.FirstOrDefault();
            }));

            this.WhenAnyValue(x => x.CurrentDevice)
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    if (this.shadowSenseService.ShadowSenseDevice != null)
                    {
                        this.shadowSenseService.CloseDevice();
                    }
                    this.shadowSenseService.OpenDevice(x.DevicePath);
                });


        }
        public string DisplayName { get; private set; }
        public ReactiveCommand<Unit> Refresh { get; private set; }

        private ReactiveList<ShadowSenseDeviceInfo> localDevices;
        public ReactiveList<ShadowSenseDeviceInfo> LocalDevices
        {
            get { return this.localDevices; }
            set { this.RaiseAndSetIfChanged(ref localDevices, value); }
        }
        private ShadowSenseDeviceInfo currentDevice;
        public ShadowSenseDeviceInfo CurrentDevice
        {
            get { return this.currentDevice; }
            set { this.RaiseAndSetIfChanged(ref currentDevice, value); }
        }

    }
}
