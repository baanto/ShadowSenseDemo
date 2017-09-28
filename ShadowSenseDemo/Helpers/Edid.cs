using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Win32;

namespace ShadowSenseDemo.Helpers
{
    static class NativeMethods
    {
        [Flags]
        public enum DisplayDeviceStateFlags
        {
            AttachedToDesktop = 0x1,
            PrimaryDevice = 0x4,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DisplayDevice
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        [DllImport("user32.dll", CharSet = CharSet.Ansi, BestFitMapping = false)]
        public static extern bool EnumDisplayDevices(
            [MarshalAs(UnmanagedType.LPStr)]
            string lpDevice,
            uint iDevNum,
            ref DisplayDevice lpDisplayDevice,
            uint dwFlags);

    }
    public class Edid
    {
        public static bool HasSettings = false;
        public static string ErrorMessage = string.Empty;
        public static string PlugAndPlayID = string.Empty;
        public static string VendorID = string.Empty;
        public static string SerialNumber = string.Empty;
        public static string Model = string.Empty;
        public static double PhysicalWidth;
        public static double PhysicalHeight;
        public static double HorizontalPixels;
        public static double VerticalPixels;
        public static double HorizontalDPI;
        public static double VerticalDPI;
        public static double EffectiveDPI;

        static Edid()
        {
            var displayDevice = new NativeMethods.DisplayDevice();
            displayDevice.cb = Marshal.SizeOf(displayDevice);

            var primaryDevice = new NativeMethods.DisplayDevice();
            primaryDevice.cb = Marshal.SizeOf(displayDevice);

            try
            {
                // Step through devices, only reporting on the Primary device that is attached to the desktop.
                for (uint id = 0; NativeMethods.EnumDisplayDevices(null, id, ref displayDevice, 0); id++)
                {
                    if (displayDevice.StateFlags.HasFlag(NativeMethods.DisplayDeviceStateFlags.AttachedToDesktop) &&
                        displayDevice.StateFlags.HasFlag(NativeMethods.DisplayDeviceStateFlags.PrimaryDevice))
                    {
                        break;
                    }

                    displayDevice.cb = Marshal.SizeOf(displayDevice);
                }

                if (string.IsNullOrEmpty(displayDevice.DeviceID))
                {
                    ErrorMessage = "Could not get device information.";
                    return;
                }

                NativeMethods.EnumDisplayDevices(displayDevice.DeviceName, 0, ref primaryDevice, 1);

                string deviceinfo = primaryDevice.DeviceID.Substring(primaryDevice.DeviceID.IndexOf("#") + 1);
                string deviceid = deviceinfo.Substring(0, deviceinfo.IndexOf('#'));
                string devicepnpid = primaryDevice.DeviceID.Substring(primaryDevice.DeviceID.IndexOf(deviceid + "#") + deviceid.Length + 1);
                devicepnpid = devicepnpid.Substring(0, devicepnpid.IndexOf('#'));

                RegistryKey Display = Registry.LocalMachine;

                Display = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Enum\DISPLAY");

                // Get all MonitorIDs
                foreach (string sMonitorID in Display.GetSubKeyNames())
                {
                    // Check if this is the active primary monitor.  Skip if not.
                    if (sMonitorID != deviceid)
                        continue;

                    RegistryKey MonitorID = Display.OpenSubKey(sMonitorID);

                    if (MonitorID != null)
                    {
                        //Get all Plug&Play ID's
                        foreach (string sPNPID in MonitorID.GetSubKeyNames())
                        {
                            // Check if this is the PNP id for the primary monitor.  Skip if not.
                            if (sPNPID != devicepnpid)
                                continue;

                            RegistryKey PnPID = MonitorID.OpenSubKey(sPNPID);

                            if (PnPID != null)
                            {
                                string[] sSubkeys = PnPID.GetSubKeyNames();

                                if (sSubkeys.Contains("Device Parameters"))
                                {
                                    RegistryKey DevParam = PnPID.OpenSubKey("Device Parameters");

                                    //Get the EDID code
                                    byte[] edid = DevParam.GetValue("EDID", null) as byte[];

                                    if (edid != null)
                                    {
                                        PlugAndPlayID = sMonitorID;
                                        GetMonitorSizeFromEdid(edid);
                                        HasSettings = true;
                                    }
                                }
                            }
                        }
                    }
                }

                if (!HasSettings)
                {
                    ErrorMessage = string.Format("Could not get device information for {0}.", deviceid);
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = string.Format("{0}", ex);
            }
        }

        static void GetMonitorSizeFromEdid(byte[] edid)
        {
            HorizontalPixels = System.Windows.SystemParameters.PrimaryScreenWidth;
            VerticalPixels = System.Windows.SystemParameters.PrimaryScreenHeight;
            PhysicalWidth = ((edid[68] & 0xF0) << 4) + edid[66];
            PhysicalHeight = ((edid[68] & 0x0F) << 8) + edid[67];

            // Since pixels are always square on Windows XP and beyond we emply a simple calculation for DPI.
            // Horizontal and Vertical DPI should be identical or close to it.
            HorizontalDPI = HorizontalPixels / (PhysicalWidth / 25.4);
            VerticalDPI = VerticalPixels / (PhysicalHeight / 25.4);
            EffectiveDPI = Math.Round((HorizontalDPI + VerticalDPI) / 2);

            // Resolve the three letter vendor identification.
            char[] v = {(char)(((edid[8] & 0x7C) >> 2) + '@'),
                        (char)(((edid[8] & 0x03) << 3) + ((edid[9] & 0xE0) >> 5) + '@'),
                        (char)((edid[9] & 0x1F) + '@')};
            VendorID = new string(v);

            // Search strings for serial number and model
            string findSerial = new string(new char[] { (char)0x00, (char)0x00, (char)0x00, (char)0xff });
            string findModel = new string(new char[] { (char)0x00, (char)0x00, (char)0x00, (char)0xfc });

            string[] searchLocations = new string[4];
            searchLocations[0] = Encoding.Default.GetString(edid, 0x36, 18);
            searchLocations[1] = Encoding.Default.GetString(edid, 0x48, 18);
            searchLocations[2] = Encoding.Default.GetString(edid, 0x5A, 18);
            searchLocations[3] = Encoding.Default.GetString(edid, 0x6C, 18);

            //Search for searial number and model.
            foreach (string sDesc in searchLocations)
            {
                if (sDesc.Contains(findSerial))
                    SerialNumber = sDesc.Substring(4).Trim().Replace("\0", string.Empty).Replace("?", string.Empty);
                if (sDesc.Contains(findModel))
                    Model = sDesc.Substring(4).Trim().Replace("\0", string.Empty).Replace("?", string.Empty);
            }
        }
    }

}
