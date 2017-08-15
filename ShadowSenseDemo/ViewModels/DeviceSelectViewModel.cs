using Baanto.ShadowSense;
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

            this.localDevices = new ReactiveList<IShadowSenseDevice>();
            this.localDevices.AddRange(this.shadowSenseService.GetDevices());
            this.currentDevice = this.localDevices.FirstOrDefault();

            this.Refresh = ReactiveCommand.CreateAsyncTask(x => Task.Run(() =>
            {
                var devices = new ReactiveList<IShadowSenseDevice>();

                devices.AddRange(this.shadowSenseService.GetDevices());

                this.LocalDevices = devices;
            }));

            this.WhenAnyValue(x => x.CurrentDevice)
            .Subscribe(x =>
            {
                if(this.shadowSenseService.ShadowSenseDevice != null)
                {
                    this.shadowSenseService.CloseDevice();
                }
                this.shadowSenseService.OpenDevice(x);
            });


        }
        public string DisplayName { get; private set; }
        public ReactiveCommand<Unit> Refresh { get; private set; }

        private ReactiveList<IShadowSenseDevice> localDevices;
        public ReactiveList<IShadowSenseDevice> LocalDevices
        {
            get { return this.localDevices; }
            set { this.RaiseAndSetIfChanged(ref localDevices, value); }
        }
        private IShadowSenseDevice currentDevice;
        public IShadowSenseDevice CurrentDevice
        {
            get { return this.currentDevice; }
            set { this.RaiseAndSetIfChanged(ref currentDevice, value); }
        }

    }
}
