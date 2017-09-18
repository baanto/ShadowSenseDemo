using System;
using System.Runtime.InteropServices;

internal static class UsbNotification
{
    public const int DbtDeviceTypeDeviceInterface = 5;

    public const int DbtDevicearrival = 0x8000; // system detected a new device        
    public const int DbtDeviceremovecomplete = 0x8004; // device is gone      
    public const int WmDevicechange = 0x0219; // device change event      
    private const int DbtDevtypDeviceinterface = 5;
    private static readonly Guid GuidDevinterfaceUSBDevice = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED"); // USB devices
    private static IntPtr notificationHandle;

    /// <summary>
    /// Registers a window to receive notifications when USB devices are plugged or unplugged.
    /// </summary>
    /// <param name="windowHandle">Handle to the window receiving notifications.</param>
    public static void RegisterUsbDeviceNotification(IntPtr windowHandle)
    {
        DevBroadcastDeviceinterface dbi = new DevBroadcastDeviceinterface
        {
            DeviceType = DbtDevtypDeviceinterface,
            Reserved = 0,
            ClassGuid = GuidDevinterfaceUSBDevice.ToByteArray(),
            Name = new char[255]
        };

        dbi.Size = Marshal.SizeOf(dbi);
        IntPtr buffer = Marshal.AllocHGlobal(dbi.Size);
        Marshal.StructureToPtr(dbi, buffer, true);
        notificationHandle = RegisterDeviceNotification(windowHandle, buffer, 0);
    }

    /// <summary>
    /// Unregisters the window for USB device notifications
    /// </summary>
    public static void UnregisterUsbDeviceNotification()
    {
        UnregisterDeviceNotification(notificationHandle);
    }
    public static String GetNameFromInterface(IntPtr p)
    {
        Int32 stringSize;

        try
        {
            // The LParam parameter of Message is a pointer to a DevBroadcastHeader structure.
            DevBroadcastHeader devBroadcastHeader = new DevBroadcastHeader();
            Marshal.PtrToStructure(p, devBroadcastHeader);

            if (devBroadcastHeader.DeviceType == DbtDeviceTypeDeviceInterface)
            {
                DevBroadcastDeviceinterface devBroadcastDeviceInterface = new DevBroadcastDeviceinterface();
                Marshal.PtrToStructure(p, devBroadcastDeviceInterface);

                // The parameter indicates that the event applies to a device interface.
                // So the structure in LParam is actually a DevBroadcastDeviceinterface structure, 
                // which begins with a DevBroadcastHeader.

                // Obtain the number of characters in Name by subtracting the 32 bytes
                // in the strucutre that are not part of Name and dividing by 2 because there are 
                // 2 bytes per character.

                stringSize = System.Convert.ToInt32((devBroadcastHeader.Size - 32) / 2);

                // The Name parameter of devBroadcastDeviceInterface contains the device name. 
                // Trim Name to match the size of the String.         

                devBroadcastDeviceInterface.Name = new char[stringSize + 1];
                // Marshal data from the unmanaged block pointed to by m.LParam 
                // to the managed object devBroadcastDeviceInterface.

                Marshal.PtrToStructure(p, devBroadcastDeviceInterface);

                return new String(devBroadcastDeviceInterface.Name, 0, stringSize); ;
            }
        }
        catch(Exception e)
        {
            //do something clever here
            return e.Message;
        }

        return @"";
    }


    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, int flags);

    [DllImport("user32.dll")]
    private static extern bool UnregisterDeviceNotification(IntPtr handle);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class DevBroadcastDeviceinterface
    {
        internal int Size;
        internal int DeviceType;
        internal int Reserved;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
        internal byte[] ClassGuid;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
        public char[] Name;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class DevBroadcastHeader
    {
        internal Int32 Size;
        public Int32 DeviceType;
        internal Int32 Reserved;
    }
}