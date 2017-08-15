using Baanto.ShadowSense.DFU;
using Baanto.ShadowSense.Events;
using Baanto.ShadowSense.Services;
using ReactiveUI;
using MaterialDesignThemes.Wpf;
using Splat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ShadowSenseDemo.Helpers;
using System.Threading.Tasks;

namespace ShadowSenseDemo.ViewModels
{
    public class UpgradeViewModel : ReactiveObject
    {
        private readonly IShadowSenseService shadowSenseService;
        private readonly IWindowsDialogs windowsDialogs;

        public UpgradeViewModel(IShadowSenseService ss, IWindowsDialogs wd)
        {
            this.DisplayName = "UPGRADE";

            this.shadowSenseService = ss;
            this.windowsDialogs = wd;

            Observable.FromEventPattern<FirmwareUpdateProgressEvent>(this.shadowSenseService.ShadowSenseDFUDevice, "FirmwareUpdateProgress")
                 .Subscribe(x => ShadowSenseDFUFirmwareUpdateProgress(x.Sender, x.EventArgs));

            Observable.FromEventPattern<InsertedEvent>(this.shadowSenseService.ShadowSenseDFUDevice, "DFUDeviceArrived")
                 .Subscribe(x => ShadowSenseDeviceArrived(x.Sender, x.EventArgs));

            Observable.FromEventPattern<RemovedEvent>(this.shadowSenseService.ShadowSenseDFUDevice, "DFUDeviceRemoved")
                 .Subscribe(x => ShadowSenseDeviceRemoved(x.Sender, x.EventArgs));

            Observable.FromEventPattern<InsertedEvent>(this.shadowSenseService, "Inserted")
                 .Subscribe(x => ShadowSenseDeviceArrived(x.Sender, x.EventArgs));

            Observable.FromEventPattern<RemovedEvent>(this.shadowSenseService, "Removed")
                 .Subscribe(x => ShadowSenseDeviceRemoved(x.Sender, x.EventArgs));

            this.Upgrade = ReactiveCommand.CreateAsyncTask(x => Task.Run(() =>
            {
                //this.shadowSenseService.ShadowSenseDevice.Reboot();

                //return;
                //var updateFile = Path.Combine(
                //    Path.Combine(
                //        Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Updates"),
                //    @"Firmware-SDW-565W1-M6L-XXX-XX-PRD-Ver-976-RETAIL.zedfu");

                string tn = string.Empty;
                Version ver;
                string error = string.Empty;
                if (this.shadowSenseService.ShadowSenseDFUDevice.ParseDFUFile(this.FileName, out tn, out ver, out error))
                {
                    this.TargetName = tn;
                    this.TargetVersion = string.Format("{0}.{1:D2}", ver.Major, ver.Minor);

                    this.shadowSenseService.UpdateFirmware(this.FileName);
                }
                else
                {
                    this.UpgradeStatus = error;
                    this.IsErrorDialogOpen = true;
                }

                return;
            }));



            this.GetDFUDeviceName = ReactiveCommand.CreateAsyncTask<string>(async x =>
            {
                return await this.shadowSenseService.ShadowSenseDFUDevice.GetDFUDeviceName();
            });

            this.GetDFUDeviceName
                .ExecuteAsync()
                .Subscribe(x => 
                {
                    if(!string.IsNullOrEmpty(x))
                        this.Name = x;
                });

            this.OpenFile = ReactiveCommand.CreateAsyncObservable(x => Observable.Start(() =>
            {
                var result = this.windowsDialogs.ShowOpenFileDialog("Open File", "DFU Files (*.edfu; *.zedfu)|*.edfu; *.zedfu");

                if (result != null)
                    if (!string.IsNullOrEmpty(result[0]))
                    {
                        string tn = string.Empty;
                        Version ver;
                        string error = string.Empty;
                        this.FileName = result[0]; // Path.GetFileName(result[0]);

                        if (this.shadowSenseService.ShadowSenseDFUDevice.ParseDFUFile(this.FileName, out tn, out ver, out error))
                        {
                            this.TargetName = tn;
                            this.TargetVersion = string.Format("{0}.{1:D2}", ver.Major, ver.Minor);
                        }
                        else
                        {
                            this.UpgradeStatus = error;
                            this.IsErrorDialogOpen = true;
                        }
                    }

            }, RxApp.MainThreadScheduler));
        }

        public string DisplayName { get; private set; }
        public ReactiveCommand<Unit> OpenFile { get; private set; }
        public ReactiveCommand<Unit> Upgrade { get; private set; }

        public ReactiveCommand<string> GetDFUDeviceName { get; private set; }


        private Boolean isErrorDialogOpen;
        public Boolean IsErrorDialogOpen
        {
            get { return this.isErrorDialogOpen; }
            set { this.RaiseAndSetIfChanged(ref this.isErrorDialogOpen, value); }
        }

        private byte upgradeProgress = 0;
        public byte UpgradeProgress
        {
            get { return this.upgradeProgress; }
            set { this.RaiseAndSetIfChanged(ref this.upgradeProgress, value); }
        }
        private string upgradeStatus = string.Empty;
        public string UpgradeStatus
        {
            get { return this.upgradeStatus; }
            set { this.RaiseAndSetIfChanged(ref this.upgradeStatus, value); }
        }

        private byte batchProgress = 0;
        public byte BatchProgress
        {
            get { return this.batchProgress; }
            set { this.RaiseAndSetIfChanged(ref this.batchProgress, value); }
        }

        private string batchStatus = string.Empty;
        public string BatchStatus
        {
            get { return this.batchStatus; }
            set { this.RaiseAndSetIfChanged(ref this.batchStatus, value); }
        }

        private string name = "Not Detected";
        public string Name
        {
            get { return this.name; }
            set { this.RaiseAndSetIfChanged(ref this.name, value); }
        }

        private string version = string.Empty;
        public string Version
        {
            get { return this.version; }
            set { this.RaiseAndSetIfChanged(ref this.version, value); }
        }

        private string fileName = string.Empty;
        public string FileName
        {
            get { return this.fileName; }
            set { this.RaiseAndSetIfChanged(ref this.fileName, value); }
        }
        private string targetName = string.Empty;
        public string TargetName
        {
            get { return this.targetName; }
            set { this.RaiseAndSetIfChanged(ref this.targetName, value); }
        }

        private string targetVersion = string.Empty;
        public string TargetVersion
        {
            get { return this.targetVersion; }
            set { this.RaiseAndSetIfChanged(ref this.targetVersion, value); }
        }

        private void ShadowSenseDFUFirmwareUpdateProgress(object sender, FirmwareUpdateProgressEvent e)
        {
            this.UpgradeStatus = e.StatusString;
            this.BatchStatus = e.BatchStatusString;
            this.UpgradeProgress = e.ProgressPercent;
            this.BatchProgress = e.BatchProgressPercent;

            if (e.Failed)
            {
                this.UpgradeStatus = e.Message;
                this.IsErrorDialogOpen = true;
            }
        }

        private void ShadowSenseDeviceRemoved(object sender, Baanto.ShadowSense.Events.RemovedEvent e)
        {
            this.Name = "No Device";
        }

        private void ShadowSenseDeviceArrived(object sender, InsertedEvent e)
        {
            this.Version = e.Version.ToString();
            this.Name = e.Name;

        }
    }
}
