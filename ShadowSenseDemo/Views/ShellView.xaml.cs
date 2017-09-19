using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Ink;
using ShadowSenseDemo.Helpers;

namespace ShadowSenseDemo.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        private HwndSource source;
        private HwndSourceHook sourceHook;

        public ShellView(ShellViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.Loaded += ShellViewLoaded;
            this.Unloaded += ShellViewUnloaded;
        }

        private void ShellViewUnloaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= ShellViewLoaded;
            this.Unloaded -= ShellViewUnloaded;

            UsbNotification.UnregisterUsbDeviceNotification();

            source.RemoveHook(sourceHook);
            sourceHook = null;

            source.Dispose();

        }

        private void ShellViewLoaded(object sender, RoutedEventArgs e)
        {
            // Adds the windows message processing hook and registers USB device add/removal notification.

            //            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            source = new HwndSource(0, 0, 0, 0, 0, "fake", IntPtr.Zero);

            if (source != null)
            {
                sourceHook = new HwndSourceHook(HwndHandler);
                source.AddHook(sourceHook);
                UsbNotification.RegisterUsbDeviceNotification(source.Handle);
            }
        }

        /// <summary>
        /// Method that receives window messages.
        /// </summary>
        private IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == UsbNotification.WmDevicechange)
            {
                switch ((int)wparam)
                {
                    case UsbNotification.DbtDeviceremovecomplete:
                        UsbDeviceRemoved(lparam);
                        break;
                    case UsbNotification.DbtDevicearrival:
                        UsbDeviceAdded(lparam);
                        break;
                }
            }

            handled = false;
            return IntPtr.Zero;
        }

        private void UsbDeviceRemoved(IntPtr arg)
        {
            //do something clever here
        }
        private void UsbDeviceAdded(IntPtr arg)
        {
            //got a device arrival so check if it's the one we want

            if (UsbNotification.GetNameFromInterface(arg).Contains("VID_2453&PID_0100",StringComparison.OrdinalIgnoreCase))
            {
                //it's ours do something with it

            }

        }
    }
}
