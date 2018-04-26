using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using ReactiveUI;
using Baanto.ShadowSense.Services;
using ShadowSenseDemo.Models;
using System.Windows.Threading;
using Baanto.ShadowSense;
using Baanto.ShadowSense.Models;
using System.Windows.Media.Media3D;

namespace ShadowSenseDemo.ViewModels
{
    public class ConfigurationViewModel : ReactiveObject
    {
        private readonly IShadowSenseService shadowSenseService;
        private ReactiveList<string> functionList = new ReactiveList<string> {
            "USB Boot Delay",
            "New Touch Delay",
            "Separation Threshold",
            "Maximum Shadow",
            "Minimum Shadow",
            "Filter Size",
            "Calibration Period",
            "Recovery Speed",
            "Filter Depth",
            "IR Frequency",

            "Touch Reject Enable",
            "Touch Reject Maximum Area",
            "Touch Reject Minimum Area",
            "Active Mode Led Brightness",
            "Active Mode Frame Time",
            "Idle Mode Led Brightness",
            "Idle Mode Frame Time",
            "Idle Mode Time Until Idle",

            "Mouse Mode",
            "Windows Touch Pipe Enable",
            "Windows Touch Data Enable",
            "Stylus Pipe Enable",
            "Stylus Data Enable",
            "Mouse Pipe Enable",
            "Mouse Data Enable",
            "Mouse Right Click Enable",
            "Mouse Move Track Mode Enable",
            "Mouse Right Click Sensitivity",
            "Mouse Deadband",

            "Stylus Enable",
            "Eraser Enable",
            "Stylus Maximum Area",
            "Stylus Minimum Area",
            "Eraser Maximum Area",
            "Eraser Minimum Area",

            "Water Reject Mode",
            "Water Reject Amount",
            "Water Shadow Maximum Area",
            "Water Shadow Minimum Area",

            "Stylus Shadow Maximum Area",
            "Stylus Shadow Minimum Area",
            "Eraser Shadow Maximum Area",
            "Eraser Shadow Minimum Area",
            "Stylus Palm Rejection Radius",
            "Stylus Up Threshold",

            "Touch Confidence",
            "Screen Mask",

            "Transform",
            "Transform Mode",
            "Screen Orientation",
            "Mac Transform Enable",

            "Regions Enable",
            "Region 1 Mask",
            "Region 2 Mask",
            "Region 3 Mask",
            "Region 4 Mask"
        };
        public ConfigurationViewModel(IShadowSenseService ss)
        {

            this.shadowSenseService = ss;
            this.DisplayName = "CONFIGURATION";

 //           this.shadowSenseService.ShadowSenseDevice.SettingsProgressChanged += ShadowSenseDeviceSettingsProgressChanged;


            this.SaveSettings = ReactiveCommand.CreateAsyncTask(x => Task.Run(() =>
            {
                if (!this.shadowSenseService.ConnectedToDriver)
                    return;

                this.IsBusy = true;
                this.LastAction = "Save Settings";

                var result = this.shadowSenseService.ShadowSenseDevice.SaveSettings(ShadowSenseSettings.ShadowSenseSettingsToDeviceSettings(this.shadowSenseSettings));

                if (!result)
                    this.Status = "Failed";

                this.IsBusy = false;
            }));


            this.LoadSettings = ReactiveCommand.CreateAsyncTask(x => Task.Run(() =>
            {
                if (!this.shadowSenseService.ConnectedToDriver)
                    return;

                this.IsBusy = true;
                this.LastAction = "Load Settings";

                DeviceSettings settings = new DeviceSettings();
                var result = this.shadowSenseService.ShadowSenseDevice.LoadSettings(ref settings);

                if (result)
                    this.ShadowSenseSettings = ShadowSenseSettings.DeviceSettingsToShadowSenseSettings(settings);
                else
                    this.Status = "Failed";

                this.IsBusy = false;
            }));


            this.ComboSetting = ReactiveCommand.CreateAsyncTask(x => Task.Run(() =>
            {
                byte data = 0;
                int bigData = 0;
                bool bData = true;
                bool result = false;
                ScreenMask sm = new ScreenMask();
                Matrix3D matrix = new Matrix3D();
                AltTransform trans = new AltTransform();
                ScreenOrientation orientation = ScreenOrientation.Landscape;

                string direction = x as string;

                if (!this.shadowSenseService.ConnectedToDriver)
                    return;
                string setting = direction + " " + this.selectedSetting;

                this.Status = "Working";
                this.LastAction = setting;
                this.IsBusy = true;

                switch (setting)
                {
                    #region Calibration
                    //Calibration
                    //Getters
                    case "Get USB Boot Delay":
                        result = this.shadowSenseService.ShadowSenseDevice.GetUsbBootDelay(ref data);
                        if (result)
                            this.ShadowSenseSettings.UsbBootDelay = data;
                        break;
                    case "Get New Touch Delay":
                        result = this.shadowSenseService.ShadowSenseDevice.GetNewTouchDelay(ref data);
                        if (result)
                            this.ShadowSenseSettings.NewTouchDelay = data;
                        break;
                    case "Get Separation Threshold":
                        result = this.shadowSenseService.ShadowSenseDevice.GetSeparationThreshold(ref bigData);
                        if (result)
                            this.ShadowSenseSettings.SeparationThreshold = bigData;
                        break;
                    case "Get Maximum Shadow":
                        result = this.shadowSenseService.ShadowSenseDevice.GetMaxShadow(ref data);
                        if (result)
                            this.ShadowSenseSettings.MaximumShadow = data;
                        break;
                    case "Get Minimum Shadow":
                        result = this.shadowSenseService.ShadowSenseDevice.GetMinShadow(ref data);
                        if (result)
                            this.ShadowSenseSettings.MinimumShadow = data;
                        break;
                    case "Get Filter Size":
                        result = this.shadowSenseService.ShadowSenseDevice.GetFilterSize(ref data);
                        if (result)
                            this.ShadowSenseSettings.FilterSize = data;
                        break;
                    case "Get Calibration Period":
                        result = this.shadowSenseService.ShadowSenseDevice.GetCalibrationPeriod(ref data);
                        if (result)
                            this.ShadowSenseSettings.CalibrationPeriod = data;
                        break;
                    case "Get Filter Depth":
                        result = this.shadowSenseService.ShadowSenseDevice.GetFilterDepth(ref data);
                        if (result)
                            this.ShadowSenseSettings.FilterDepth = data;
                        break;
                    case "Get Recovery Speed":
                        result = this.shadowSenseService.ShadowSenseDevice.GetRecoverySpeed(ref data);
                        if (result)
                            this.ShadowSenseSettings.RecoverySpeed = data;
                        break;
                    case "Get IR Frequency":
                        result = this.shadowSenseService.ShadowSenseDevice.GetIrFrequency(ref data);
                        if (result)
                            this.ShadowSenseSettings.IrFrequency = data;
                        break;
                    //Setters
                    case "Set USB Boot Delay":
                        result = this.shadowSenseService.ShadowSenseDevice.SetUsbBootDelay(this.shadowSenseSettings.UsbBootDelay);
                        break;
                    case "Set New Touch Delay":
                        result = this.shadowSenseService.ShadowSenseDevice.SetNewTouchDelay(this.shadowSenseSettings.NewTouchDelay);
                        break;
                    case "Set Separation Threshold":
                        result = this.shadowSenseService.ShadowSenseDevice.SetSeparationThreshold(this.shadowSenseSettings.SeparationThreshold);
                        break;
                    case "Set Maximum Shadow":
                        result = this.shadowSenseService.ShadowSenseDevice.SetMaxShadow(this.shadowSenseSettings.MaximumShadow);
                        break;
                    case "Set Minimum Shadow":
                        result = this.shadowSenseService.ShadowSenseDevice.SetMinShadow(this.shadowSenseSettings.MinimumShadow);
                        break;
                    case "Set Filter Size":
                        result = this.shadowSenseService.ShadowSenseDevice.SetFilterSize(this.shadowSenseSettings.FilterSize);
                        break;
                    case "Set Calibration Period":
                        result = this.shadowSenseService.ShadowSenseDevice.SetCalibrationPeriod(this.shadowSenseSettings.CalibrationPeriod);
                        break;
                    case "Set Filter Depth":
                        result = this.shadowSenseService.ShadowSenseDevice.SetFilterDepth(this.shadowSenseSettings.FilterDepth);
                        break;
                    case "Set Recovery Speed":
                        result = this.shadowSenseService.ShadowSenseDevice.SetRecoverySpeed(this.shadowSenseSettings.RecoverySpeed);
                        break;
                    case "Set IR Frequency":
                        result = this.shadowSenseService.ShadowSenseDevice.SetIrFrequency(this.shadowSenseSettings.IrFrequency);
                        break;
                    #endregion

                    #region Calibration Extended
                    //Getters
                    case "Get Touch Reject Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.GetTouchRejectEnable(ref data);
                        if (result)
                            this.ShadowSenseSettings.TouchRejectEnable = data;
                        break;
                    case "Get Touch Reject Maximum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.GetTouchRejectMaximumArea(ref bigData);
                        if (result)
                            this.ShadowSenseSettings.TouchRejectMaximumArea = bigData;
                        break;
                    case "Get Touch Reject Minimum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.GetTouchRejectMinimumArea(ref bigData);
                        if (result)
                            this.ShadowSenseSettings.TouchRejectMinimumArea = bigData;
                        break;

                    case "Get Active Mode Led Brightness":
                        result = this.shadowSenseService.ShadowSenseDevice.GetActiveModeLedBrightness(ref data);
                        if (result)
                            this.ShadowSenseSettings.ActiveModeLedBrightness = data;
                        break;
                    case "Get Active Mode Frame Time":
                        result = this.shadowSenseService.ShadowSenseDevice.GetActiveModeFrameTime(ref data);
                        if (result)
                            this.ShadowSenseSettings.ActiveModeFrameTime = data;
                        break;
                    case "Get Idle Mode Led Brightness":
                        result = this.shadowSenseService.ShadowSenseDevice.GetIdleModeLedBrightness(ref data);
                        if (result)
                            this.ShadowSenseSettings.IdleModeLedBrightness = data;
                        break;
                    case "Get Idle Mode Frame Time":
                        result = this.shadowSenseService.ShadowSenseDevice.GetIdleModeFrameTime(ref bigData);
                        if (result)
                            this.ShadowSenseSettings.IdleModeFrameTime = bigData;
                        break;
                    case "Get Idle Mode Time Until Idle":
                        result = this.shadowSenseService.ShadowSenseDevice.GetIdleModeTimeUntilIdle(ref bigData);
                        if (result)
                            this.ShadowSenseSettings.IdleModeTimeUntilIdle = bigData;
                        break;

                    //Setters
                    case "Set Touch Reject Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.SetTouchRejectEnable(this.shadowSenseSettings.TouchRejectEnable);
                        break;
                    case "Set Touch Reject Maximum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.SetTouchRejectMaximumArea(this.shadowSenseSettings.TouchRejectMaximumArea);
                        break;
                    case "Set Touch Reject Minimum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.SetTouchRejectMinimumArea(this.shadowSenseSettings.TouchRejectMinimumArea);
                        break;
                    case "Set Active Mode Led Brightness":
                        result = this.shadowSenseService.ShadowSenseDevice.SetActiveModeLedBrightness(this.shadowSenseSettings.ActiveModeLedBrightness);
                        break;
                    case "Set Active Mode Frame Time":
                        result = this.shadowSenseService.ShadowSenseDevice.SetActiveModeFrameTime(this.shadowSenseSettings.ActiveModeFrameTime);
                        break;
                    case "Set Idle Mode Led Brightness":
                        result = this.shadowSenseService.ShadowSenseDevice.SetIdleModeLedBrightness(this.shadowSenseSettings.IdleModeLedBrightness);
                        break;
                    case "Set Idle Mode Frame Time":
                        result = this.shadowSenseService.ShadowSenseDevice.SetIdleModeFrameTime(this.shadowSenseSettings.IdleModeFrameTime);
                        break;
                    case "Set Idle Mode Time Until Idle":
                        result = this.shadowSenseService.ShadowSenseDevice.SetIdleModeTimeUntilIdle(this.shadowSenseSettings.IdleModeTimeUntilIdle);
                        break;
                    #endregion

                    #region Modes
                    //Getters
                    case "Get Mouse Mode":
                        result = this.shadowSenseService.ShadowSenseDevice.GetMouseMode(ref data);
                        if (result)
                            this.ShadowSenseSettings.MouseMode = (MouseMode)data;
                        break;
                    case "Get Windows Touch Pipe Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.GetWindowsTouchPipeEnable(ref bData);
                        if (result)
                            this.ShadowSenseSettings.WindowsTouchPipeEnable = bData;
                        break;
                    case "Get Windows Touch Data Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.GetWindowsTouchDataEnable(ref bData);
                        if (result)
                            this.ShadowSenseSettings.WindowsTouchDataEnable = bData;
                        break;
                    case "Get Stylus Pipe Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.GetStylusPipeEnable(ref bData);
                        if (result)
                            this.ShadowSenseSettings.StylusPipeEnable = bData;
                        break;
                    case "Get Stylus Data Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.GetStylusDataEnable(ref bData);
                        if (result)
                            this.ShadowSenseSettings.StylusDataEnable = bData;
                        break;
                    case "Get Mouse Pipe Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.GetMousePipeEnable(ref bData);
                        if (result)
                            this.ShadowSenseSettings.MousePipeEnable = bData;
                        break;
                    case "Get Mouse Data Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.GetMouseDataEnable(ref bData);
                        if (result)
                            this.ShadowSenseSettings.MouseDataEnable = bData;
                        break;
                    case "Get Mouse Right Click Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.GetMouseRightClickEnable(ref bData);
                        if (result)
                            this.ShadowSenseSettings.MouseRightClickEnable = bData;
                        break;
                    case "Get Mouse Move Track Mode Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.GetMouseMoveTrackModeEnable(ref bData);
                        if (result)
                            this.ShadowSenseSettings.MouseMoveTrackModeEnable = bData;
                        break;
                    case "Get Mouse Right Click Sensitivity":
                        result = this.shadowSenseService.ShadowSenseDevice.GetMouseRightClickSensitivity(ref data);
                        if (result)
                            this.ShadowSenseSettings.MouseRightClickSensitivity = data;
                        break;
                    case "Get Mouse Deadband":
                        result = this.shadowSenseService.ShadowSenseDevice.GetMouseDeadband(ref data);
                        if (result)
                            this.ShadowSenseSettings.MouseDeadband = data;
                        break;


                    //Setters
                    case "Set Mouse Mode":
                        result = this.shadowSenseService.ShadowSenseDevice.SetMouseMode(this.shadowSenseSettings.MouseMode);
                        break;
                    case "Set Windows Touch Pipe Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.SetWindowsTouchPipeEnable(this.shadowSenseSettings.WindowsTouchPipeEnable);
                        break;
                    case "Set Windows Touch Data Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.SetWindowsTouchDataEnable(this.shadowSenseSettings.WindowsTouchDataEnable);
                        break;
                    case "Set Stylus Pipe Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.SetStylusPipeEnable(this.shadowSenseSettings.StylusPipeEnable);
                        break;
                    case "Set Stylus Data Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.SetStylusDataEnable(this.shadowSenseSettings.StylusDataEnable);
                        break;
                    case "Set Mouse Pipe Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.SetMousePipeEnable(this.shadowSenseSettings.MousePipeEnable);
                        break;
                    case "Set Mouse Data Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.SetMouseDataEnable(this.shadowSenseSettings.MouseDataEnable);
                        break;
                    case "Set Mouse Right Click Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.SetMouseRightClickEnable(this.shadowSenseSettings.MouseRightClickEnable);
                        break;
                    case "Set Mouse Move Track Mode Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.SetMouseMoveTrackModeEnable(this.shadowSenseSettings.MouseMoveTrackModeEnable);
                        break;
                    case "Set Mouse Right Click Sensitivity":
                        result = this.shadowSenseService.ShadowSenseDevice.SetMouseRightClickSensitivity(this.shadowSenseSettings.MouseRightClickSensitivity);
                        break;
                    case "Set Mouse Deadband":
                        result = this.shadowSenseService.ShadowSenseDevice.SetMouseDeadband(this.shadowSenseSettings.MouseDeadband);
                        break;

                    #endregion

                    #region Stylus
                    //Getters
                    case "Get Stylus Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.GetStylusEnable(ref bData);
                        if (result)
                            this.ShadowSenseSettings.StylusEnable = bData;
                        break;
                    case "Get Eraser Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.GetEraserEnable(ref bData);
                        if (result)
                            this.ShadowSenseSettings.EraserEnable = bData;
                        break;
                    case "Get Stylus Maximum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.GetStylusMaximumArea(ref bigData);
                        if (result)
                            this.ShadowSenseSettings.StylusMaximumArea = bigData;
                        break;
                    case "Get Stylus Minimum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.GetStylusMinimumArea(ref bigData);
                        if (result)
                            this.ShadowSenseSettings.StylusMinimumArea = bigData;
                        break;
                    case "Get Eraser Maximum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.GetEraserMaximumArea(ref bigData);
                        if (result)
                            this.ShadowSenseSettings.EraserMaximumArea = bigData;
                        break;
                    case "Get Eraser Minimum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.GetEraserMinimumArea(ref bigData);
                        if (result)
                            this.ShadowSenseSettings.EraserMinimumArea = bigData;
                        break;
                    case "Get Stylus Up Threshold":
                        result = this.shadowSenseService.ShadowSenseDevice.GetStylusUpThreshold(ref data);
                        if (result)
                            this.ShadowSenseSettings.StylusUpThreshold = data;
                        break;

                    //Setters
                    case "Set Stylus Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.SetStylusEnable(this.shadowSenseSettings.StylusEnable);
                        break;
                    case "Set Eraser Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.SetEraserEnable(this.shadowSenseSettings.EraserEnable);
                        break;
                    case "Set Stylus Maximum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.SetStylusMaximumArea(this.shadowSenseSettings.StylusMaximumArea);
                        break;
                    case "Set Stylus Minimum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.SetStylusMinimumArea(this.shadowSenseSettings.StylusMinimumArea);
                        break;
                    case "Set Eraser Maximum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.SetEraserMaximumArea(this.shadowSenseSettings.EraserMaximumArea);
                        break;
                    case "Set Eraser Minimum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.SetEraserMinimumArea(this.shadowSenseSettings.EraserMinimumArea);
                        break;
                    case "Set Stylus Up Threshold":
                        result = this.shadowSenseService.ShadowSenseDevice.SetStylusUpThreshold(this.shadowSenseSettings.StylusUpThreshold);
                        break;

                    #endregion

                    #region Stylus Shadows
                    //Getters
                    case "Get Stylus Shadow Maximum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.GetStylusShadowMaximumArea(ref bigData);
                        if (result)
                            this.ShadowSenseSettings.StylusShadowMaximumArea = bigData;
                        break;
                    case "Get Stylus Shadow Minimum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.GetStylusShadowMinimumArea(ref bigData);
                        if (result)
                            this.ShadowSenseSettings.StylusShadowMinimumArea = bigData;
                        break;
                    case "Get Eraser Shadow Maximum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.GetEraserShadowMaximumArea(ref bigData);
                        if (result)
                            this.ShadowSenseSettings.EraserShadowMaximumArea = bigData;
                        break;
                    case "Get Eraser Shadow Minimum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.GetEraserShadowMinimumArea(ref bigData);
                        if (result)
                            this.ShadowSenseSettings.EraserShadowMinimumArea = bigData;
                        break;
                    case "Get Stylus Palm Rejection Radius":
                        result = this.shadowSenseService.ShadowSenseDevice.GetStylusPalmRejectionRadius(ref bigData);
                        if (result)
                            this.ShadowSenseSettings.StylusPalmRejectionRadius = bigData;
                        break;

                    //Setters
                    case "Set Stylus Shadow Maximum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.SetStylusShadowMaximumArea(this.shadowSenseSettings.StylusShadowMaximumArea);
                        break;
                    case "Set Stylus Shadow Minimum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.SetStylusShadowMinimumArea(this.shadowSenseSettings.StylusShadowMinimumArea);
                        break;
                    case "Set Eraser Shadow Maximum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.SetEraserShadowMaximumArea(this.shadowSenseSettings.EraserShadowMaximumArea);
                        break;
                    case "Set Eraser Shadow Minimum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.SetEraserShadowMinimumArea(this.shadowSenseSettings.EraserShadowMinimumArea);
                        break;
                    case "Set Stylus Palm Rejection Radius":
                        result = this.shadowSenseService.ShadowSenseDevice.SetStylusPalmRejectionRadius(this.shadowSenseSettings.StylusPalmRejectionRadius);
                        break;

                    #endregion

                    #region Water
                    //Getters
                    case "Get Water Shadow Maximum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.GetWaterShadowMaximumArea(ref bigData);
                        if (result)
                            this.ShadowSenseSettings.WaterShadowMaximumArea = bigData;
                        break;
                    case "Get Water Shadow Minimum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.GetWaterShadowMinimumArea(ref bigData);
                        if (result)
                            this.ShadowSenseSettings.WaterShadowMinimumArea = bigData;
                        break;
                    case "Get Water Reject Amount":
                        result = this.shadowSenseService.ShadowSenseDevice.GetWaterRejectAmount(ref data);
                        if (result)
                            this.ShadowSenseSettings.WaterRejectAmount = data;
                        break;
                    case "Get Water Reject Mode":
                        result = this.shadowSenseService.ShadowSenseDevice.GetWaterRejectMode(ref data);
                        if (result)
                            this.ShadowSenseSettings.WaterRejectMode = (WaterRejectMode)data;
                        break;

                    //Setters

                    case "Set Water Shadow Maximum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.SetWaterShadowMaximumArea(this.shadowSenseSettings.WaterShadowMaximumArea);
                        break;
                    case "Set Water Shadow Minimum Area":
                        result = this.shadowSenseService.ShadowSenseDevice.SetWaterShadowMinimumArea(this.shadowSenseSettings.WaterShadowMinimumArea);
                        break;
                    case "Set Water Reject Amount":
                        result = this.shadowSenseService.ShadowSenseDevice.SetWaterRejectAmount(this.shadowSenseSettings.WaterRejectAmount);
                        break;
                    case "Set Water Reject Mode":
                        result = this.shadowSenseService.ShadowSenseDevice.SetWaterRejectMode(this.shadowSenseSettings.WaterRejectMode);
                        break;

                    #endregion

                    #region Screen Mask
                    //Getters
                    case "Get Touch Confidence":
                        result = this.shadowSenseService.ShadowSenseDevice.GetTouchConfidence(ref data);
                        if (result)
                            this.ShadowSenseSettings.TouchConfidence = data;
                        break;
                    case "Get Screen Mask":
                        result = this.shadowSenseService.ShadowSenseDevice.GetScreenMask(ref sm);
                        if (result)
                            this.ShadowSenseSettings.ScreenMask = sm.ConvertToReactiveScreenMask();
                        break;

                    //Setters
                    case "Set Touch Confidence":
                        result = this.shadowSenseService.ShadowSenseDevice.SetTouchConfidence(this.shadowSenseSettings.TouchConfidence);
                        break;
                    case "Set Screen Mask":
                        result = this.shadowSenseService.ShadowSenseDevice.SetScreenMask(this.shadowSenseSettings.ScreenMask.ConvertToShadowSenseScreenMask());
                        break;

                    #endregion

                    #region Transform
                    //Getters
                    case "Get Transform":
                        object mat = null;
                        result = this.shadowSenseService.ShadowSenseDevice.GetTransform(ref mat, ref bData);
                        if (result)
                        {
                            this.ShadowSenseSettings.TransformMode = bData;

                            if (!bData)
                            {
                                matrix = (Matrix3D)mat;
                                this.ShadowSenseSettings.Transform = matrix.ConvertToReactiveMatrix();
                            }
                            else
                            {
                                trans = (AltTransform)mat;
                                this.ShadowSenseSettings.AltTransform = trans.ConvertToReactiveAltTransform();
                            }
                        }
                        break;
                    case "Get Transform Mode":
                        result = this.shadowSenseService.ShadowSenseDevice.GetTransformMode(ref bData);
                        if (result)
                            this.ShadowSenseSettings.TransformMode = bData;
                        break;
                    case "Get Screen Orientation":
                        result = this.shadowSenseService.ShadowSenseDevice.GetScreenOrientation(ref orientation);
                        if (result)
                            this.ShadowSenseSettings.ScreenOrientation = orientation;
                        break;
                    case "Get Mac Transform Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.GetMacTransformEnable(ref bData);
                        if (result)
                            this.ShadowSenseSettings.MacTransformEnable = bData;
                        break;

                    //Setters
                    case "Set Transform Mode":
                    case "Set Transform":
                        if (!this.shadowSenseSettings.TransformMode)
                            result = this.shadowSenseService.ShadowSenseDevice.SetTransform(this.shadowSenseSettings.Transform.ConvertToMatrix3D());
                        else
                            result = this.shadowSenseService.ShadowSenseDevice.SetAltTransform(this.shadowSenseSettings.AltTransform.ConvertToShadowSenseAltTransform());
                        break;
                    case "Set Mac Transform Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.SetMacTransformEnable(this.shadowSenseSettings.MacTransformEnable);
                        break;

                    #endregion

                    #region Regions
                    //Getters
                    case "Get Regions Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.GetRegionsEnable(ref data);
                        if (result)
                        {
                            this.ShadowSenseSettings.Region1Enable = (data & (byte)Regions.Region1) != 0; ;
                            this.ShadowSenseSettings.Region2Enable = (data & (byte)Regions.Region2) != 0; ;
                            this.ShadowSenseSettings.Region3Enable = (data & (byte)Regions.Region3) != 0; ;
                            this.ShadowSenseSettings.Region4Enable = (data & (byte)Regions.Region4) != 0; ;
                        }
                        break;
                    case "Get Region 1 Mask":
                        result = this.shadowSenseService.ShadowSenseDevice.GetRegionMask(ref sm, Regions.Region1);
                        if (result)
                            this.ShadowSenseSettings.Region1Mask = sm.ConvertToReactiveScreenMask();
                        break;
                    case "Get Region 2 Mask":
                        result = this.shadowSenseService.ShadowSenseDevice.GetRegionMask(ref sm, Regions.Region2);
                        if (result)
                            this.ShadowSenseSettings.Region2Mask = sm.ConvertToReactiveScreenMask();
                        break;
                    case "Get Region 3 Mask":
                        result = this.shadowSenseService.ShadowSenseDevice.GetRegionMask(ref sm, Regions.Region3);
                        if (result)
                            this.ShadowSenseSettings.Region3Mask = sm.ConvertToReactiveScreenMask();
                        break;
                    case "Get Region 4 Mask":
                        result = this.shadowSenseService.ShadowSenseDevice.GetRegionMask(ref sm, Regions.Region4);
                        if (result)
                            this.ShadowSenseSettings.Region4Mask = sm.ConvertToReactiveScreenMask();
                        break;

                    //Setters
                    case "Set Regions Enable":
                        result = this.shadowSenseService.ShadowSenseDevice.SetRegionsEnable(
                            Utility.RegionMaskEnabledToData(
                                this.shadowSenseSettings.Region1Enable,
                                this.shadowSenseSettings.Region2Enable,
                                this.shadowSenseSettings.Region3Enable,
                                this.shadowSenseSettings.Region4Enable));
                        break;
                    case "Set Region 1 Mask":
                        result = this.shadowSenseService.ShadowSenseDevice.SetRegionMask(this.shadowSenseSettings.Region1Mask.ConvertToShadowSenseScreenMask(), Regions.Region1);
                        break;
                    case "Set Region 2 Mask":
                        result = this.shadowSenseService.ShadowSenseDevice.SetRegionMask(this.shadowSenseSettings.Region2Mask.ConvertToShadowSenseScreenMask(), Regions.Region2);
                        break;
                    case "Set Region 3 Mask":
                        result = this.shadowSenseService.ShadowSenseDevice.SetRegionMask(this.shadowSenseSettings.Region3Mask.ConvertToShadowSenseScreenMask(), Regions.Region3);
                        break;
                    case "Set Region 4 Mask":
                        result = this.shadowSenseService.ShadowSenseDevice.SetRegionMask(this.shadowSenseSettings.Region4Mask.ConvertToShadowSenseScreenMask(), Regions.Region4);
                        break;

                    #endregion

                    default:
                        this.LastAction += " ---";
                        this.Status = "Setting not found";
                        this.IsBusy = false;
                        return;
                }

                this.Status = result ? "Success" : "Fail";
                this.IsBusy = false;
            }));

        }
        public string DisplayName { get; private set; }

        public ReactiveCommand<Unit> ComboSetting { get; private set; }
        public ReactiveCommand<Unit> SaveSettings { get; private set; }
        public ReactiveCommand<Unit> LoadSettings { get; private set; }

        private bool isBusy = false;
        public bool IsBusy
        {
            get { return this.isBusy; }
            set { this.RaiseAndSetIfChanged(ref this.isBusy, value); }
        }

        public ReactiveList<string> FunctionList
        {
            get { return this.functionList; }
            set { this.RaiseAndSetIfChanged(ref this.functionList, value); }
        }

        private ShadowSenseSettings shadowSenseSettings = new ShadowSenseSettings();
        public ShadowSenseSettings ShadowSenseSettings
        {
            get { return this.shadowSenseSettings; }
            set { this.RaiseAndSetIfChanged(ref this.shadowSenseSettings, value); }
        }


        private string selectedSetting;
        public string SelectedSetting
        {
            get { return this.selectedSetting; }
            set { this.RaiseAndSetIfChanged(ref this.selectedSetting, value); }
        }

        private string status = "";
        public string Status
        {
            get { return this.status; }
            set { this.RaiseAndSetIfChanged(ref this.status, value); }
        }
        private string lastAction = "";
        public string LastAction
        {
            get { return this.lastAction; }
            set { this.RaiseAndSetIfChanged(ref this.lastAction, value); }
        }

        private void ShadowSenseDeviceSettingsProgressChanged(object sender, Baanto.ShadowSense.Events.SettingsProgressEvent e)
        {
            this.Status = e.Message;
        }




    }
}
