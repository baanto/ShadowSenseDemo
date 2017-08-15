using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

#pragma warning disable 1591

namespace ShadowSenseDemo.Helpers
{
    public static class User32
    {

        [StructLayout(LayoutKind.Sequential)]
        struct OsVersionInfoEx
        {
            public uint OSVersionInfoSize;
            public uint MajorVersion;
            public uint MinorVersion;
            public uint BuildNumber;
            public uint PlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string CSDVersion;
            public ushort ServicePackMajor;
            public ushort ServicePackMinor;
            public ushort SuiteMask;
            public byte ProductType;
            public byte Reserved;
        }

        [DllImport("kernel32.dll")]
        static extern ulong VerSetConditionMask(ulong dwlConditionMask,
           uint dwTypeBitMask, byte dwConditionMask);
        [DllImport("kernel32.dll")]
        static extern bool VerifyVersionInfo(
            [In] ref OsVersionInfoEx lpVersionInfo,
            uint dwTypeMask, ulong dwlConditionMask);

        public static bool IsWindowsVersionOrGreater(
            uint majorVersion, uint minorVersion, ushort servicePackMajor)
        {
            OsVersionInfoEx osvi = new OsVersionInfoEx();
            osvi.OSVersionInfoSize = (uint)Marshal.SizeOf(osvi);
            osvi.MajorVersion = majorVersion;
            osvi.MinorVersion = minorVersion;
            osvi.ServicePackMajor = servicePackMajor;
            // These constants initialized with corresponding definitions in
            // winnt.h (part of Windows SDK)
            const uint VER_MINORVERSION = 0x0000001;
            const uint VER_MAJORVERSION = 0x0000002;
            const uint VER_SERVICEPACKMAJOR = 0x0000020;
            const byte VER_GREATER_EQUAL = 3;
            ulong versionOrGreaterMask = VerSetConditionMask(
                VerSetConditionMask(
                    VerSetConditionMask(
                        0, VER_MAJORVERSION, VER_GREATER_EQUAL),
                    VER_MINORVERSION, VER_GREATER_EQUAL),
                VER_SERVICEPACKMAJOR, VER_GREATER_EQUAL);
            uint versionOrGreaterTypeMask = VER_MAJORVERSION |
                VER_MINORVERSION | VER_SERVICEPACKMAJOR;
            return VerifyVersionInfo(ref osvi, versionOrGreaterTypeMask,
                versionOrGreaterMask);
        }

        //Misc APIs
        [DllImport("user32")]
        public static extern bool SetProcessDPIAware();

        [DllImport("user32")]
        public static extern bool IsWindow(IntPtr hWnd);

        public const int WM_NCDESTROY = 0x0082;

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetMessageExtraInfo();

        //Touch
        [DllImport("user32", EntryPoint = "GetSystemMetrics")]
        public static extern int GetDigitizerCapabilities(DigitizerIndex index);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetWindowFeedbackSetting(IntPtr hWnd, FEEDBACK_TYPE feedback, int dwFlags, int size, ref bool val);

        public enum DigitizerIndex
        {
            SM_DIGITIZER = 94,
            SM_MAXIMUMTOUCHES = 95
        }

        public static bool UseWin7Touch
        {
            get;
            set;
        }

        public static bool IsWin8
        {
            get
            {
                if (UseWin7Touch)
                    return false;
                else
                {
                    OperatingSystem os = Environment.OSVersion;
                    return !(os.Platform != PlatformID.Win32NT || os.Version.CompareTo(new Version(6, 2)) < 0);
                }
            }
        }

        #region POINTER_INFO

        internal const int
        WM_PARENTNOTIFY = 0x0210,
        WM_NCPOINTERUPDATE = 0x0241,
        WM_NCPOINTERDOWN = 0x0242,
        WM_NCPOINTERUP = 0x0243,
        WM_POINTERUPDATE = 0x0245,
        WM_POINTERDOWN = 0x0246,
        WM_POINTERUP = 0x0247,
        WM_POINTERENTER = 0x0249,
        WM_POINTERLEAVE = 0x024A,
        WM_POINTERACTIVATE = 0x024B,
        WM_POINTERCAPTURECHANGED = 0x024C,
        WM_POINTERWHEEL = 0x024E,
        WM_POINTERHWHEEL = 0x024F,

        // WM_POINTERACTIVATE return codes
        PA_ACTIVATE = 1,
        PA_NOACTIVATE = 3,

        MAX_TOUCH_COUNT = 256;

        public enum POINTER_INPUT_TYPE
        {
            POINTER = 0x00000001,
            TOUCH = 0x00000002,
            PEN = 0x00000003,
            MOUSE = 0x00000004
        }

        [Flags]
        public enum POINTER_FLAGS
        {
            NONE = 0x00000000,
            NEW = 0x00000001,
            INRANGE = 0x00000002,
            INCONTACT = 0x00000004,
            FIRSTBUTTON = 0x00000010,
            SECONDBUTTON = 0x00000020,
            THIRDBUTTON = 0x00000040,
            FOURTHBUTTON = 0x00000080,
            FIFTHBUTTON = 0x00000100,
            PRIMARY = 0x00002000,
            CONFIDENCE = 0x00004000,
            CANCELED = 0x00008000,
            DOWN = 0x00010000,
            UPDATE = 0x00020000,
            UP = 0x00040000,
            WHEEL = 0x00080000,
            HWHEEL = 0x00100000,
            CAPTURECHANGED = 0x00200000,
        }

        [Flags]
        internal enum POINTER_TOUCH_FLAGS
        {
            NONE = 0x00000000,
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }
            public POINT(System.Windows.Point pt)
            {
                X = (int)pt.X;
                Y = (int)pt.Y;
            }
            public System.Windows.Point ToPoint()
            {
                return new System.Windows.Point(X, Y);
            }
            public void AssignTo(ref System.Windows.Point destination)
            {
                destination.X = X;
                destination.Y = Y;
            }
            public void CopyFrom(System.Windows.Point source)
            {
                X = (int)source.X;
                Y = (int)source.Y;
            }
            public void CopyFrom(POINT source)
            {
                X = source.X;
                Y = source.Y;
            }
        }

        [Flags]
        internal enum VIRTUAL_KEY_STATES
        {
            NONE = 0x0000,
            LBUTTON = 0x0001,
            RBUTTON = 0x0002,
            SHIFT = 0x0004,
            CTRL = 0x0008,
            MBUTTON = 0x0010,
            XBUTTON1 = 0x0020,
            XBUTTON2 = 0x0040
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINTER_INFO
        {
            public POINTER_INPUT_TYPE pointerType;
            public int PointerID;
            public int FrameID;
            public POINTER_FLAGS PointerFlags;
            public IntPtr SourceDevice;
            public IntPtr WindowTarget;
            [MarshalAs(UnmanagedType.Struct)]
            public POINT PtPixelLocation;
            [MarshalAs(UnmanagedType.Struct)]
            public POINT PtPixelLocationRaw;
            [MarshalAs(UnmanagedType.Struct)]
            public POINT PtHimetricLocation;
            [MarshalAs(UnmanagedType.Struct)]
            public POINT PtHimetricLocationRaw;
            public uint Time;
            public uint HistoryCount;
            public uint InputData;
            public VIRTUAL_KEY_STATES KeyStates;
            public long PerformanceCount;
            public int ButtonChangeType;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINTER_TOUCH_INFO
        {
            public POINTER_INFO pointerinfo;
            public uint touchFlags;
            public uint touchMask;
            public RECT rcContact;
            public RECT rcContactRaw;
            public uint orientation;
            public uint pressure;
        }

        [Flags]
        public enum PEN_FLAGS : uint
        {
            NONE = 0x00000000,
            BARREL = 0x00000001,
            INVERTED = 0x00000002,
            ERASER = 0x00000004,
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINTER_PEN_INFO
        {
            public POINTER_INFO pointerinfo;
            public PEN_FLAGS penFlags;
            public uint penMask;
            public uint pressure;
            public uint rotation;
            public int tiltX;
            public int tiltY;
        }

        internal static int GetPointerId(IntPtr wParam)
        {
            return LoWord(wParam);
        }

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerInfo(int pointerID, ref POINTER_INFO pointerInfo);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerType(int pointerID, out POINTER_INPUT_TYPE pointerType);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerTouchInfo(int pointerID, ref POINTER_TOUCH_INFO pointerInfo);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerPenInfo(int pointerID, ref POINTER_PEN_INFO pointerInfo);
        #endregion

        #region TOUCH_INFO

        [Flags]
        internal enum FEEDBACK_TYPE : uint
        {
            FEEDBACK_TOUCH_CONTACTVISUALIZATION = 1,
            FEEDBACK_PEN_BARRELVISUALIZATION = 2,
            FEEDBACK_PEN_TAP = 3,
            FEEDBACK_PEN_DOUBLETAP = 4,
            FEEDBACK_PEN_PRESSANDHOLD = 5,
            FEEDBACK_PEN_RIGHTTAP = 6,
            FEEDBACK_TOUCH_TAP = 7,
            FEEDBACK_TOUCH_DOUBLETAP = 8,
            FEEDBACK_TOUCH_PRESSANDHOLD = 9,
            FEEDBACK_TOUCH_RIGHTTAP = 10,
            FEEDBACK_GESTURE_PRESSANDTAP = 11,
            FEEDBACK_MAX = 0xFFFFFFFF,
        }

        public const string TPS = "MicrosoftTabletPenServiceProperty";
        public const int TABLET_ENABLE_MULTITOUCHDATA = 0x01000000;

        // Touch event window message constants [winuser.h]
        public const int WM_TOUCH = 0x0240;
        public const int WM_MOUSEFIRST = 0x0200;
        public const int WM_MOUSELAST = 0x020A;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_MOUSEWHEEL = 0x020A;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_RBUTTONUP = 0x0205;

        public const int MK_LBUTTON = 0x0001;

        public enum TouchWindowFlag : uint
        {
            FineTouch = 0x1,
            WantPalm = 0x2
        }

        public const uint PENTOUCHMASK = 0xFFFFFF00;
        public const uint PENTOUCHSIGNATURE = 0xFF515700;

        // Touch event flags ((TOUCHINPUT.dwFlags) [winuser.h]
        public const int TOUCHEVENTF_MOVE = 0x0001;
        public const int TOUCHEVENTF_DOWN = 0x0002;
        public const int TOUCHEVENTF_UP = 0x0004;
        public const int TOUCHEVENTF_INRANGE = 0x0008;
        public const int TOUCHEVENTF_PRIMARY = 0x0010;
        public const int TOUCHEVENTF_NOCOALESCE = 0x0020;
        public const int TOUCHEVENTF_PEN = 0x0040;
        public const int TOUCHEVENTF_PALM = 0x0080;

        // Touch input mask values (TOUCHINPUT.dwMask) [winuser.h]
        public const int TOUCHINPUTMASKF_TIMEFROMSYSTEM = 0x0001; // the dwTime field contains a system generated value
        public const int TOUCHINPUTMASKF_EXTRAINFO = 0x0002; // the dwExtraInfo field is valid
        public const int TOUCHINPUTMASKF_CONTACTAREA = 0x0004; // the cxContact and cyContact fields are valid

        public static bool IsPenOrTouchMessage()
        {
            IntPtr extraInfo = User32.GetMessageExtraInfo();

            if (Environment.Is64BitProcess)
            {
                if (((uint)extraInfo.ToInt64() & PENTOUCHMASK) == PENTOUCHSIGNATURE)
                    return true;
            }
            else
            {
                if (((uint)extraInfo.ToInt32() & PENTOUCHMASK) == PENTOUCHSIGNATURE)
                    return true;
            }
            return false;
        }

        #endregion

        [DllImport("user32")]
        public static extern IntPtr EnableMouseInPointer(bool value);

        [DllImport("user32")]
        public static extern bool RegisterTouchWindow(System.IntPtr hWnd, TouchWindowFlag flags);

        [DllImport("user32")]
        public static extern bool UnregisterTouchWindow(System.IntPtr hWnd);

        [DllImport("user32")]
        public static extern bool IsTouchWindow(System.IntPtr hWnd, out uint ulFlags);

        [DllImport("user32")]
        public static extern bool GetTouchInputInfo(System.IntPtr hTouchInput, int cInputs, [In, Out] TOUCHINPUT[] pInputs, int cbSize);

        [DllImport("user32")]
        public static extern void CloseTouchInputHandle(System.IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int SetProp(IntPtr hWnd, string atom, IntPtr handle);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr GetProp(IntPtr hWnd, string lpString);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern uint GetMessageTime();

        internal static void CheckLastError()
        {
            int errCode = Marshal.GetLastWin32Error();
            if (errCode != 0)
            {
                throw new Win32Exception(errCode);
            }
        }

        // Extracts lower 16-bit word from an 64-bit IntPtr.
        // in:
        //      number      int
        // returns:
        //      lower word
        internal static int LoWord(IntPtr wParam)
        {
            return (int)(wParam.ToInt64() & 0xffff);
        }

        // Extracts lower 16-bit word from an 32-bit int.
        // in:
        //      number      int
        // returns:
        //      lower word
        public static ushort LoWord(uint number)
        {
            return (ushort)(number & 0xffff);
        }

        // Extracts higher 16-bit word from an 32-bit int.
        // in:
        //      number      uint
        // returns:
        //      lower word
        public static ushort HiWord(uint number)
        {
            return (ushort)((number >> 16) & 0xffff);
        }

        // Extracts lower 32-bit word from an 64-bit int.
        // in:
        //      number      ulong
        // returns:
        //      lower word
        public static uint LoDWord(ulong number)
        {
            return (uint)(number & 0xffffffff);
        }

        // Extracts higher 32-bit word from an 64-bit int.
        // in:
        //      number      ulong
        // returns:
        //      lower word
        public static uint HiDWord(ulong number)
        {
            return (uint)((number >> 32) & 0xffffffff);
        }


        // Extracts lower 16-bit word from an 32-bit int.
        // in:
        //      number      int
        // returns:
        //      lower word
        public static short LoWord(int number)
        {
            return (short)number;
        }

        // Extracts higher 16-bit word from an 32-bit int.
        // in:
        //      number      int
        // returns:
        //      lower word
        public static short HiWord(int number)
        {
            return (short)(number >> 16);
        }

        // Extracts lower 32-bit word from an 64-bit int.
        // in:
        //      number      long
        // returns:
        //      lower word
        public static int LoDWord(long number)
        {
            return (int)(number);
        }

        // Extracts higher 32-bit word from an 64-bit int.
        // in:
        //      number      long
        // returns:
        //      lower word
        public static int HiDWord(long number)
        {
            return (int)((number >> 32));
        }

        //Gesture

        public const uint GC_ALLGESTURES = 0x00000001;

        public const uint WM_GESTURE = 0x0119;

        public const uint WM_GESTURENOTIFY = 0x011A;

        // Gesture notification structure
        //  - The WM_GESTURENOTIFY message lParam contains a pointer to this structure.
        //  - The WM_GESTURENOTIFY message notifies a window that gesture recognition is
        //    in progress and a gesture will be generated if one is recognized under the
        //    current gesture settings.
        public struct GESTURENOTIFYSTRUCT
        {
            public uint cbSize;                   // size, in bytes, of this structure
            public uint dwFlags;                  // unused
            public IntPtr hwndTarget;               // handle to window targeted by the gesture
            public POINTS ptsLocation;              // starting location
            public uint dwInstanceID;             // internally used
        };


        [DllImport("user32")]
        public static extern bool SetGestureConfig(
                                    IntPtr hwnd,                        // window for which configuration is specified
                                    uint dwReserved,                    // reserved, must be 0
                                    uint cIDs,                          // count of GESTURECONFIG structures
                                    GESTURECONFIG[] pGestureConfig,    // array of GESTURECONFIG structures, dwIDs will be processed in the
                                                                       // order specified and repeated occurances will overwrite previous ones
                                    uint cbSize);                       // sizeof(GESTURECONFIG)





        [DllImport("user32")]
        public static extern bool GetGestureInfo(IntPtr hGestureInfo, ref GESTUREINFO pGestureInfo);

        // Gesture argument helpers
        //   - Angle should be a double in the range of -2pi to +2pi
        //   - Argument should be an unsigned 16-bit value
        //
        public static ushort GID_ROTATE_ANGLE_TO_ARGUMENT(ushort arg) { return ((ushort)(((arg + 2.0 * 3.14159265) / (4.0 * 3.14159265)) * 65535.0)); }
        public static double GID_ROTATE_ANGLE_FROM_ARGUMENT(ushort arg) { return ((((double)arg / 65535.0) * 4.0 * 3.14159265) - 2.0 * 3.14159265); }


        [DllImport("user32")]
        public static extern bool CloseGestureInfoHandle(IntPtr hGestureInfo);

        //Gesture flags - GESTUREINFO.dwFlags
        public const uint GF_BEGIN = 0x00000001;
        public const uint GF_INERTIA = 0x00000002;
        public const uint GF_END = 0x00000004;

        //Gesture IDs
        public const uint GID_BEGIN = 1;
        public const uint GID_END = 2;
        public const uint GID_ZOOM = 3;
        public const uint GID_PAN = 4;
        public const uint GID_ROTATE = 5;
        public const uint GID_TWOFINGERTAP = 6;
        public const uint GID_PRESSANDTAP = 7;



    }


    //Touch

    /// <summary>
    /// Touch API defined structures [winuser.h]
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TOUCHINPUT
    {
        public int x;
        public int y;
        public System.IntPtr hSource;
        public int dwID;
        public int dwFlags;
        public int dwMask;
        public int dwTime;
        public System.IntPtr dwExtraInfo;
        public int cxContact;
        public int cyContact;
    }

    /// <summary>
    /// A Simple POINTS Interop structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct POINTS
    {
        public short x;
        public short y;
    }

    /// <summary>
    /// A Simple POINT Interop structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }


    //Gesture

    /// <summary>
    /// Gesture Config Interop Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct GESTURECONFIG
    {
        public uint dwID;                     // gesture ID
        public uint dwWant;                   // settings related to gesture ID that are to be turned on
        public uint dwBlock;                  // settings related to gesture ID that are to be turned off
    }

    /// <summary>
    /// Gesture Info Interop Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct GESTUREINFO
    {
        public uint cbSize;                // size, in bytes, of this structure (including variable length Args field)
        public uint dwFlags;               // see GF_* flags
        public uint dwID;                  // gesture ID, see GID_* defines
        public IntPtr hwndTarget;          // handle to window targeted by this gesture
        public POINTS ptsLocation;          // current location of this gesture
        public uint dwInstanceID;          // internally used
        public uint dwSequenceID;          // internally used
        public ulong ullArguments;         // arguments for gestures whose arguments fit in 8 BYTES
        public uint cbExtraArgs;           // size, in bytes, of extra arguments, if any, that accompany this gesture
    }

    class Kernel32
    {
        /// <summary>
        /// Get the native thread id
        /// </summary>
        /// <returns>Thread ID</returns>
        [DllImport("Kernel32")]
        public static extern uint GetCurrentThreadId();
    }
    public static class DisableWPFTouchAndStylus
    {

        public static void DisableWPFTabletSupport()
        {
            // Get a collection of the tablet devices for this window.  
            var devices = Tablet.TabletDevices;

            if (devices.Count > 0)
            {
                // Get the Type of InputManager.
                var inputManagerType = typeof(InputManager);

                // Call the StylusLogic method on the InputManager.Current instance.
                var stylusLogic = inputManagerType.InvokeMember("StylusLogic",
                            BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                            null, InputManager.Current, null);

                if (stylusLogic != null)
                {
                    //  Get the type of the stylusLogic returned from the call to StylusLogic.
                    var stylusLogicType = stylusLogic.GetType();

                    // Loop until there are no more devices to remove.
                    while (devices.Count > 0)
                    {
                        // Remove the first tablet device in the devices collection.
                        stylusLogicType.InvokeMember("OnTabletRemoved",
                                BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic,
                                null, stylusLogic, new object[] { (uint)0 });
                    }
                }
            }

            // END OF ORIGINAL CODE

            // hook into internal class SystemResources to keep it from updating the TabletDevices on system events

            object hwndWrapper = GetSystemResourcesHwnd();
            if (hwndWrapper != null)
            {
                // invoke hwndWrapper.AddHook( .. our method ..)
                var internalHwndWrapperType = hwndWrapper.GetType();

                // if the delegate is already set, we have already added the hook.
                if (_handleAndHideMessageDelegate == null)
                {
                    // create the internal delegate that will hook into the window messages
                    // need to hold a reference to that one, because internally the delegate is stored through a WeakReference object

                    var internalHwndWrapperHookDelegate = internalHwndWrapperType.Assembly.GetType("MS.Win32.HwndWrapperHook");
                    var handleAndHideMessagesHandle = typeof(DisableWPFTouchAndStylus).GetMethod(nameof(HandleAndHideMessages), BindingFlags.Static | BindingFlags.NonPublic);
                    _handleAndHideMessageDelegate = Delegate.CreateDelegate(internalHwndWrapperHookDelegate, handleAndHideMessagesHandle);


                    // add a delegate that handles WM_TABLET_ADD
                    internalHwndWrapperType.InvokeMember("AddHook",
                        BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                        null, hwndWrapper, new object[] { _handleAndHideMessageDelegate });
                }
            }
        }

        private static Delegate _handleAndHideMessageDelegate = null;

        private static object GetSystemResourcesHwnd()
        {
            var internalSystemResourcesType = typeof(Application).Assembly.GetType("System.Windows.SystemResources");

            // get HwndWrapper from internal property SystemRessources.Hwnd;
            var hwndWrapper = internalSystemResourcesType.InvokeMember("Hwnd",
                        BindingFlags.GetProperty | BindingFlags.Static | BindingFlags.NonPublic,
                        null, null, null);
            return hwndWrapper;
        }

        private static IntPtr HandleAndHideMessages(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == (int)WindowMessage.WM_TABLET_ADDED ||
                msg == (int)WindowMessage.WM_TABLET_DELETED ||
                msg == (int)WindowMessage.WM_DEVICECHANGE)
            {
                handled = true;
            }
            return IntPtr.Zero;
        }

        enum WindowMessage : int
        {
            WM_TABLET_DEFBASE = 0x02C0,
            WM_TABLET_ADDED = WM_TABLET_DEFBASE + 8,
            WM_TABLET_DELETED = WM_TABLET_DEFBASE + 9,
            WM_DEVICECHANGE = 0x0219
        }

    }

}