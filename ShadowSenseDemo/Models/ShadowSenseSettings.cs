using Baanto.ShadowSense;
using Baanto.ShadowSense.Models;
using ReactiveUI;
using System.Windows.Media.Media3D;

namespace ShadowSenseDemo.Models
{
    public class ShadowSenseSettings : ReactiveObject
    {
        #region Calibration

        private byte usbBootDelay;
        public byte UsbBootDelay
        {
            get { return this.usbBootDelay; }
            set { this.RaiseAndSetIfChanged(ref this.usbBootDelay, value); }
        }

        private byte newTouchDelay;
        public byte NewTouchDelay
        {
            get { return this.newTouchDelay; }
            set { this.RaiseAndSetIfChanged(ref this.newTouchDelay, value); }
        }

        private int separationThreshold;
        public int SeparationThreshold
        {
            get { return this.separationThreshold; }
            set { this.RaiseAndSetIfChanged(ref this.separationThreshold, value); }
        }

        private byte maximumShadow;
        public byte MaximumShadow
        {
            get { return this.maximumShadow; }
            set { this.RaiseAndSetIfChanged(ref this.maximumShadow, value); }
        }

        private byte minimumShadow;
        public byte MinimumShadow
        {
            get { return this.minimumShadow; }
            set { this.RaiseAndSetIfChanged(ref this.minimumShadow, value); }
        }

        private byte filterSize;
        public byte FilterSize
        {
            get { return this.filterSize; }
            set { this.RaiseAndSetIfChanged(ref this.filterSize, value); }
        }

        private byte calibrationPeriod;
        public byte CalibrationPeriod
        {
            get { return this.calibrationPeriod; }
            set { this.RaiseAndSetIfChanged(ref this.calibrationPeriod, value); }
        }

        private byte recoverySpeed;
        public byte RecoverySpeed
        {
            get { return this.recoverySpeed; }
            set { this.RaiseAndSetIfChanged(ref this.recoverySpeed, value); }
        }

        private byte filterDepth;
        public byte FilterDepth
        {
            get { return this.filterDepth; }
            set { this.RaiseAndSetIfChanged(ref this.filterDepth, value); }
        }

        #endregion

        #region Calibration Ex

        private byte touchRejectEnable;
        public byte TouchRejectEnable
        {
            get { return this.touchRejectEnable; }
            set { this.RaiseAndSetIfChanged(ref this.touchRejectEnable, value); }
        }
        private int touchRejectMaxArea;
        public int TouchRejectMaximumArea
        {
            get { return this.touchRejectMaxArea; }
            set { this.RaiseAndSetIfChanged(ref this.touchRejectMaxArea, value); }
        }
        private int touchRejectMinArea;
        public int TouchRejectMinimumArea
        {
            get { return this.touchRejectMinArea; }
            set { this.RaiseAndSetIfChanged(ref this.touchRejectMinArea, value); }
        }
        private byte activeModeLedBrightness;
        public byte ActiveModeLedBrightness
        {
            get { return this.activeModeLedBrightness; }
            set { this.RaiseAndSetIfChanged(ref this.activeModeLedBrightness, value); }
        }
        private byte activeModeFrameTime;
        public byte ActiveModeFrameTime
        {
            get { return this.activeModeFrameTime; }
            set { this.RaiseAndSetIfChanged(ref this.activeModeFrameTime, value); }
        }
        private byte idleModeLedBrightness;
        public byte IdleModeLedBrightness
        {
            get { return this.idleModeLedBrightness; }
            set { this.RaiseAndSetIfChanged(ref this.idleModeLedBrightness, value); }
        }
        private int idleModeFrameTime;
        public int IdleModeFrameTime
        {
            get { return this.idleModeFrameTime; }
            set { this.RaiseAndSetIfChanged(ref this.idleModeFrameTime, value); }
        }
        private int idleModeTimeUntilIdle;
        public int IdleModeTimeUntilIdle
        {
            get { return this.idleModeTimeUntilIdle; }
            set { this.RaiseAndSetIfChanged(ref this.idleModeTimeUntilIdle, value); }
        }

        #endregion

        #region Modes

        private MouseMode mouseMode;
        public MouseMode MouseMode
        {
            get { return this.mouseMode; }
            set { this.RaiseAndSetIfChanged(ref this.mouseMode, value); }
        }
        private bool windowsTouchPipeEnable;
        public bool WindowsTouchPipeEnable
        {
            get { return this.windowsTouchPipeEnable; }
            set { this.RaiseAndSetIfChanged(ref this.windowsTouchPipeEnable, value); }
        }
        private bool windowsTouchDataEnable;
        public bool WindowsTouchDataEnable
        {
            get { return this.windowsTouchDataEnable; }
            set { this.RaiseAndSetIfChanged(ref this.windowsTouchDataEnable, value); }
        }
        private bool stylusPipeEnable;
        public bool StylusPipeEnable
        {
            get { return this.stylusPipeEnable; }
            set { this.RaiseAndSetIfChanged(ref this.stylusPipeEnable, value); }
        }
        private bool stylusDataEnable;
        public bool StylusDataEnable
        {
            get { return this.stylusDataEnable; }
            set { this.RaiseAndSetIfChanged(ref this.stylusDataEnable, value); }
        }
        private bool mousePipeEnable;
        public bool MousePipeEnable
        {
            get { return this.mousePipeEnable; }
            set { this.RaiseAndSetIfChanged(ref this.mousePipeEnable, value); }
        }
        private bool mouseDataEnable;
        public bool MouseDataEnable
        {
            get { return this.mouseDataEnable; }
            set { this.RaiseAndSetIfChanged(ref this.mouseDataEnable, value); }
        }
        private bool mouseRightClickEnable;
        public bool MouseRightClickEnable
        {
            get { return this.mouseRightClickEnable; }
            set { this.RaiseAndSetIfChanged(ref this.mouseRightClickEnable, value); }
        }
        private bool mouseMoveTrackModeEnable;
        public bool MouseMoveTrackModeEnable
        {
            get { return this.mouseMoveTrackModeEnable; }
            set { this.RaiseAndSetIfChanged(ref this.mouseMoveTrackModeEnable, value); }
        }
        private byte mouseRightClickSensitivity;
        public byte MouseRightClickSensitivity
        {
            get { return this.mouseRightClickSensitivity; }
            set { this.RaiseAndSetIfChanged(ref this.mouseRightClickSensitivity, value); }
        }
        private byte mouseDeadband;
        public byte MouseDeadband
        {
            get { return this.mouseDeadband; }
            set { this.RaiseAndSetIfChanged(ref this.mouseDeadband, value); }
        }

        #endregion

        #region Stylus

        private bool stylusEnable;
        public bool StylusEnable
        {
            get { return this.stylusEnable; }
            set { this.RaiseAndSetIfChanged(ref this.stylusEnable, value); }
        }
        private bool eraserEnable;
        public bool EraserEnable
        {
            get { return this.eraserEnable; }
            set { this.RaiseAndSetIfChanged(ref this.eraserEnable, value); }
        }
        private int stylusMaximumArea;
        public int StylusMaximumArea
        {
            get { return this.stylusMaximumArea; }
            set { this.RaiseAndSetIfChanged(ref this.stylusMaximumArea, value); }
        }
        private int stylusMinimumArea;
        public int StylusMinimumArea
        {
            get { return this.stylusMinimumArea; }
            set { this.RaiseAndSetIfChanged(ref this.stylusMinimumArea, value); }
        }
        private int eraserMaximumArea;
        public int EraserMaximumArea
        {
            get { return this.eraserMaximumArea; }
            set { this.RaiseAndSetIfChanged(ref this.eraserMaximumArea, value); }
        }
        private int eraserMinimumArea;
        public int EraserMinimumArea
        {
            get { return this.eraserMinimumArea; }
            set { this.RaiseAndSetIfChanged(ref this.eraserMinimumArea, value); }
        }

        #endregion

        #region Stylus Shadows

        private int stylusShadowMaximumArea;
        public int StylusShadowMaximumArea
        {
            get { return this.stylusShadowMaximumArea; }
            set { this.RaiseAndSetIfChanged(ref this.stylusShadowMaximumArea, value); }
        }
        private int stylusShadowMinimumArea;
        public int StylusShadowMinimumArea
        {
            get { return this.stylusShadowMinimumArea; }
            set { this.RaiseAndSetIfChanged(ref this.stylusShadowMinimumArea, value); }
        }
        private int eraserShadowMaximumArea;
        public int EraserShadowMaximumArea
        {
            get { return this.eraserShadowMaximumArea; }
            set { this.RaiseAndSetIfChanged(ref this.eraserShadowMaximumArea, value); }
        }
        private int eraserShadowMinimumArea;
        public int EraserShadowMinimumArea
        {
            get { return this.eraserShadowMinimumArea; }
            set { this.RaiseAndSetIfChanged(ref this.eraserShadowMinimumArea, value); }
        }
        private int stylusPalmRejectionRadius;
        public int StylusPalmRejectionRadius
        {
            get { return this.stylusPalmRejectionRadius; }
            set { this.RaiseAndSetIfChanged(ref this.stylusPalmRejectionRadius, value); }
        }

        private byte stylusUpThreshold;
        public byte StylusUpThreshold
        {
            get { return this.stylusUpThreshold; }
            set { this.RaiseAndSetIfChanged(ref this.stylusUpThreshold, value); }
        }

        private byte irFrequency;
        public byte IrFrequency
        {
            get { return this.irFrequency; }
            set { this.RaiseAndSetIfChanged(ref this.irFrequency, value); }
        }

        #endregion

        #region Water

        private WaterRejectMode waterRejectMode;
        public WaterRejectMode WaterRejectMode
        {
            get { return this.waterRejectMode; }
            set { this.RaiseAndSetIfChanged(ref this.waterRejectMode, value); }
        }
        private byte waterRejectAmount;
        public byte WaterRejectAmount
        {
            get { return this.waterRejectAmount; }
            set { this.RaiseAndSetIfChanged(ref this.waterRejectAmount, value); }
        }
        private int waterShadowMaximumArea;
        public int WaterShadowMaximumArea
        {
            get { return this.waterShadowMaximumArea; }
            set { this.RaiseAndSetIfChanged(ref this.waterShadowMaximumArea, value); }
        }
        private int waterShadowMinimumArea;
        public int WaterShadowMinimumArea
        {
            get { return this.waterShadowMinimumArea; }
            set { this.RaiseAndSetIfChanged(ref this.waterShadowMinimumArea, value); }
        }

        #endregion

        #region Screen Mask
        private ReactiveScreenMask screenMask = new ReactiveScreenMask();
        public ReactiveScreenMask ScreenMask
        {
            get { return this.screenMask; }
            set { this.RaiseAndSetIfChanged(ref this.screenMask, value); }
        }
        private byte touchConfidence;
        public byte TouchConfidence
        {
            get { return this.touchConfidence; }
            set { this.RaiseAndSetIfChanged(ref this.touchConfidence, value); }
        }


        #endregion

        #region Transforms

        private ReactiveMatrix transform = new ReactiveMatrix();
        public ReactiveMatrix Transform
        {
            get { return this.transform; }
            set { this.RaiseAndSetIfChanged(ref this.transform, value); }
        }

        private ReactiveAltTransform altTransform = new ReactiveAltTransform();
        public ReactiveAltTransform AltTransform
        {
            get { return this.altTransform; }
            set { this.RaiseAndSetIfChanged(ref this.altTransform, value); }
        }

        private bool transformMode;
        public bool TransformMode
        {
            get { return this.transformMode; }
            set { this.RaiseAndSetIfChanged(ref this.transformMode, value); }
        }

        private ScreenOrientation screenOrientation;
        public ScreenOrientation ScreenOrientation
        {
            get { return this.screenOrientation; }
            set { this.RaiseAndSetIfChanged(ref this.screenOrientation, value); }
        }

        private bool macTransformEnable;
        public bool MacTransformEnable
        {
            get { return this.macTransformEnable; }
            set { this.RaiseAndSetIfChanged(ref this.macTransformEnable, value); }
        }

        #endregion

        #region Regions

        private bool region1Enable;
        public bool Region1Enable
        {
            get { return this.region1Enable; }
            set { this.RaiseAndSetIfChanged(ref this.region1Enable, value); }
        }
        private bool region2Enable;
        public bool Region2Enable
        {
            get { return this.region2Enable; }
            set { this.RaiseAndSetIfChanged(ref this.region2Enable, value); }
        }
        private bool region3Enable;
        public bool Region3Enable
        {
            get { return this.region3Enable; }
            set { this.RaiseAndSetIfChanged(ref this.region3Enable, value); }
        }
        private bool region4Enable;
        public bool Region4Enable
        {
            get { return this.region4Enable; }
            set { this.RaiseAndSetIfChanged(ref this.region4Enable, value); }
        }

        private ReactiveScreenMask region1Mask = new ReactiveScreenMask();
        public ReactiveScreenMask Region1Mask
        {
            get { return this.region1Mask; }
            set { this.RaiseAndSetIfChanged(ref this.region1Mask, value); }
        }
        private ReactiveScreenMask region2Mask = new ReactiveScreenMask();
        public ReactiveScreenMask Region2Mask
        {
            get { return this.region2Mask; }
            set { this.RaiseAndSetIfChanged(ref this.region2Mask, value); }
        }
        private ReactiveScreenMask region3Mask = new ReactiveScreenMask();
        public ReactiveScreenMask Region3Mask
        {
            get { return this.region3Mask; }
            set { this.RaiseAndSetIfChanged(ref this.region3Mask, value); }
        }
        private ReactiveScreenMask region4Mask = new ReactiveScreenMask();
        public ReactiveScreenMask Region4Mask
        {
            get { return this.region4Mask; }
            set { this.RaiseAndSetIfChanged(ref this.region4Mask, value); }
        }

        #endregion

        public static ShadowSenseSettings DeviceSettingsToShadowSenseSettings(DeviceSettings settings)
        {
            ShadowSenseSettings sss = new ShadowSenseSettings();

            sss.UsbBootDelay = settings.UsbBootDelay;
            sss.NewTouchDelay = settings.NewTouchDelay;
            sss.SeparationThreshold = settings.SeparationThreshold;
            sss.MaximumShadow = settings.MaximumShadow;
            sss.MinimumShadow = settings.MinimumShadow;
            sss.FilterSize = settings.FilterSize;
            sss.CalibrationPeriod = settings.CalibrationPeriod;
            sss.RecoverySpeed = settings.RecoverySpeed;
            sss.FilterDepth = settings.FilterDepth;

            sss.TouchRejectEnable = settings.TouchRejectEnable;
            sss.TouchRejectMaximumArea = settings.TouchRejectMaxArea;
            sss.TouchRejectMinimumArea = settings.TouchRejectMinArea;
            sss.ActiveModeLedBrightness = settings.ActiveModeLedBrightness;
            sss.ActiveModeFrameTime = settings.ActiveModeFrameTime;
            sss.IdleModeLedBrightness = settings.IdleModeLedBrightness;
            sss.IdleModeFrameTime = settings.IdleModeFrameTime;
            sss.IdleModeTimeUntilIdle = settings.IdleModeTimeUntilIdle;

            sss.MouseMode = settings.MouseMode;
            sss.WindowsTouchPipeEnable = settings.WindowsTouchPipeEnable;
            sss.WindowsTouchDataEnable = settings.WindowsTouchDataEnable;
            sss.StylusPipeEnable = settings.StylusPipeEnable;
            sss.StylusDataEnable = settings.StylusDataEnable;
            sss.MousePipeEnable = settings.MousePipeEnable;
            sss.MouseDataEnable = settings.MouseDataEnable;
            sss.MouseRightClickEnable = settings.MouseRightClickEnable;
            sss.MouseMoveTrackModeEnable = settings.MouseMoveTrackModeEnable;
            sss.MouseRightClickSensitivity = settings.MouseRightClickSensitivity;
            sss.MouseDeadband = settings.MouseDeadband;

            sss.StylusEnable = settings.StylusEnable;
            sss.EraserEnable = settings.EraserEnable;
            sss.StylusMaximumArea = settings.StylusMaximumArea;
            sss.StylusMinimumArea = settings.StylusMinimumArea;
            sss.EraserMaximumArea = settings.EraserMaximumArea;
            sss.EraserMinimumArea = settings.EraserMinimumArea;

            sss.StylusShadowMaximumArea = settings.StylusShadowMaximumArea;
            sss.StylusShadowMinimumArea = settings.StylusShadowMinimumArea;
            sss.EraserShadowMaximumArea = settings.EraserShadowMaximumArea;
            sss.EraserShadowMinimumArea = settings.EraserShadowMinimumArea;
            sss.StylusPalmRejectionRadius = settings.StylusPalmRejectionRadius;
            sss.StylusUpThreshold = settings.StylusUpThreshold;
            sss.IrFrequency = settings.IrFrequency;

            sss.WaterRejectMode = settings.WaterRejectMode;
            sss.WaterRejectAmount = settings.WaterRejectAmount;
            sss.WaterShadowMaximumArea = settings.WaterShadowMaximumArea;
            sss.WaterShadowMinimumArea = settings.WaterShadowMinimumArea;

            sss.Region1Enable = settings.Region1Enable;
            sss.Region2Enable = settings.Region2Enable;
            sss.Region3Enable = settings.Region3Enable;
            sss.Region4Enable = settings.Region4Enable;

            sss.Region1Mask = settings.Region1Mask.ConvertToReactiveScreenMask();
            sss.Region2Mask = settings.Region2Mask.ConvertToReactiveScreenMask();
            sss.Region3Mask = settings.Region3Mask.ConvertToReactiveScreenMask();
            sss.Region4Mask = settings.Region4Mask.ConvertToReactiveScreenMask();

            sss.Transform = settings.Transform.ConvertToReactiveMatrix();
            sss.AltTransform = settings.AltTransform.ConvertToReactiveAltTransform();
            sss.TransformMode = settings.TransformMode;
            sss.ScreenOrientation = settings.ScreenOrientation;
            sss.MacTransformEnable = settings.MacTransformEnable;

            sss.TouchConfidence = settings.TouchConfidence;
            sss.ScreenMask = settings.ScreenMask.ConvertToReactiveScreenMask();

            return sss;
        }

        public static DeviceSettings ShadowSenseSettingsToDeviceSettings(ShadowSenseSettings settings)
        {
            DeviceSettings deviceSettings = new DeviceSettings();

            deviceSettings.UsbBootDelay = settings.UsbBootDelay;
            deviceSettings.NewTouchDelay = settings.NewTouchDelay;
            deviceSettings.SeparationThreshold = (byte)settings.SeparationThreshold;
            deviceSettings.MaximumShadow = settings.MaximumShadow;
            deviceSettings.MinimumShadow = settings.MinimumShadow;
            deviceSettings.FilterSize = settings.FilterSize;
            deviceSettings.CalibrationPeriod = settings.CalibrationPeriod;
            deviceSettings.RecoverySpeed = settings.RecoverySpeed;
            deviceSettings.FilterDepth = settings.FilterDepth;

            deviceSettings.TouchRejectEnable = settings.TouchRejectEnable;
            deviceSettings.TouchRejectMaxArea = settings.TouchRejectMaximumArea;
            deviceSettings.TouchRejectMinArea = settings.TouchRejectMinimumArea;
            deviceSettings.ActiveModeLedBrightness = settings.ActiveModeLedBrightness;
            deviceSettings.ActiveModeFrameTime = settings.ActiveModeFrameTime;
            deviceSettings.IdleModeLedBrightness = settings.IdleModeLedBrightness;
            deviceSettings.IdleModeFrameTime = settings.IdleModeFrameTime;
            deviceSettings.IdleModeTimeUntilIdle = settings.IdleModeTimeUntilIdle;

            deviceSettings.MouseMode = settings.MouseMode;
            deviceSettings.WindowsTouchPipeEnable = settings.WindowsTouchPipeEnable;
            deviceSettings.WindowsTouchDataEnable = settings.WindowsTouchDataEnable;
            deviceSettings.StylusPipeEnable = settings.StylusPipeEnable;
            deviceSettings.StylusDataEnable = settings.StylusDataEnable;
            deviceSettings.MousePipeEnable = settings.MousePipeEnable;
            deviceSettings.MouseDataEnable = settings.MouseDataEnable;
            deviceSettings.MouseRightClickEnable = settings.MouseRightClickEnable;
            deviceSettings.MouseMoveTrackModeEnable = settings.MouseMoveTrackModeEnable;
            deviceSettings.MouseRightClickSensitivity = settings.MouseRightClickSensitivity;
            deviceSettings.MouseDeadband = settings.MouseDeadband;

            deviceSettings.StylusEnable = settings.StylusEnable;
            deviceSettings.EraserEnable = settings.EraserEnable;
            deviceSettings.StylusMaximumArea = settings.StylusMaximumArea;
            deviceSettings.StylusMinimumArea = settings.StylusMinimumArea;
            deviceSettings.EraserMaximumArea = settings.EraserMaximumArea;
            deviceSettings.EraserMinimumArea = settings.EraserMinimumArea;

            deviceSettings.StylusShadowMaximumArea = settings.StylusShadowMaximumArea;
            deviceSettings.StylusShadowMinimumArea = settings.StylusShadowMinimumArea;
            deviceSettings.EraserShadowMaximumArea = settings.EraserShadowMaximumArea;
            deviceSettings.EraserShadowMinimumArea = settings.EraserShadowMinimumArea;
            deviceSettings.StylusPalmRejectionRadius = settings.StylusPalmRejectionRadius;
            deviceSettings.StylusUpThreshold = settings.StylusUpThreshold;
            deviceSettings.IrFrequency = settings.IrFrequency;

            deviceSettings.WaterRejectMode = settings.WaterRejectMode;
            deviceSettings.WaterRejectAmount = settings.WaterRejectAmount;
            deviceSettings.WaterShadowMaximumArea = settings.WaterShadowMaximumArea;
            deviceSettings.WaterShadowMinimumArea = settings.WaterShadowMinimumArea;

            deviceSettings.Region1Enable = settings.Region1Enable;
            deviceSettings.Region2Enable = settings.Region2Enable;
            deviceSettings.Region3Enable = settings.Region3Enable;
            deviceSettings.Region4Enable = settings.Region4Enable;

            deviceSettings.Region1Mask = settings.Region1Mask.ConvertToShadowSenseScreenMask();
            deviceSettings.Region2Mask = settings.Region2Mask.ConvertToShadowSenseScreenMask();
            deviceSettings.Region3Mask = settings.Region3Mask.ConvertToShadowSenseScreenMask();
            deviceSettings.Region4Mask = settings.Region4Mask.ConvertToShadowSenseScreenMask();

            deviceSettings.Transform = settings.Transform.ConvertToMatrix3D();
            deviceSettings.AltTransform = settings.AltTransform.ConvertToShadowSenseAltTransform();
            deviceSettings.TransformMode = settings.TransformMode;
            deviceSettings.ScreenOrientation = settings.ScreenOrientation;
            deviceSettings.MacTransformEnable = settings.MacTransformEnable;

            deviceSettings.TouchConfidence = settings.TouchConfidence;
            deviceSettings.ScreenMask = settings.ScreenMask.ConvertToShadowSenseScreenMask();

            return deviceSettings;
        }

    }
}
