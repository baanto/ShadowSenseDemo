using Baanto.ShadowSense.Events;
using ShadowSenseDemo.Helpers;
using ShadowSenseDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;

namespace ShadowSenseDemo.Views
{
    /// <summary>
    /// Interaction logic for DrawView.xaml
    /// </summary>
    public partial class DrawView : UserControl
    {
        private Dictionary<int, Stroke> currentStrokes = new Dictionary<int, Stroke>();
        private Dictionary<int, TouchDot> currentTouches = new Dictionary<int, TouchDot>();

        private DrawViewModel drawViewModel;

        public DrawView()
        {
            InitializeComponent();

            //Disable extra stylus processing so we can subscribe to events
            DisableWPFTouchAndStylus.DisableWPFTabletSupport();

            this.Loaded += DrawViewLoaded;
            this.Unloaded += DrawViewUnloaded;

            this.DataContextChanged += DrawViewDataContextChanged;
        }



        private void DrawViewDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = e.NewValue as DrawViewModel;
            if (vm != null)
            {
                this.drawViewModel = vm;

                Observable.FromEventPattern<InsertedEvent>(this.drawViewModel.ShadowSenseService, "Inserted")
                     .Subscribe(x => ShadowSenseDeviceInserted(x.Sender, x.EventArgs));

                Observable.FromEventPattern<RemovedEvent>(this.drawViewModel.ShadowSenseService, "Removed")
                     .Subscribe(x => ShadowSenseDeviceRemoved(x.Sender, x.EventArgs));

            }

        }
        private void ShadowSenseDeviceRemoved(object sender, Baanto.ShadowSense.Events.RemovedEvent e)
        {
            var device = this.drawViewModel.ShadowSenseService.ShadowSenseDevice;
            if (device != null)
            {
                device.TouchDown -= DeviceTouchDown;
                device.TouchMove -= DeviceTouchMove;
                device.TouchUp -= DeviceTouchUp;
            }
        }
        private void ShadowSenseDeviceInserted(object sender, InsertedEvent e)
        {
            var device = this.drawViewModel.ShadowSenseService.ShadowSenseDevice;
            if(device != null)
            {
                device.TouchDown += DeviceTouchDown;
                device.TouchMove += DeviceTouchMove;
                device.TouchUp += DeviceTouchUp;
            }
        }

        private void DeviceTouchDown(object sender, ShadowSenseTouchEvent e)
        {

        }
        private void DeviceTouchMove(object sender, ShadowSenseTouchEvent e)
        {
        }
        private void DeviceTouchUp(object sender, ShadowSenseTouchEvent e)
        {
        }

        private void DrawViewUnloaded(object sender, RoutedEventArgs e)
        {
            //Remove the WndProc
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.RemoveHook(WndProc);

            ShadowSenseDeviceRemoved(this, null);
        }

        private void DrawViewLoaded(object sender, RoutedEventArgs e)
        {
            //Add hook to receive WndProc messages
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
        }
        protected IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            //Process pointer event messages
            switch (msg)
            {
                case User32.WM_POINTERLEAVE:
                case User32.WM_POINTERUPDATE:
                case User32.WM_POINTERDOWN:
                case User32.WM_POINTERUP:
                    {
                        DecodeTouches(msg, wparam, lparam);
                        break;
                    }
                default:
                    break;
            }

            return IntPtr.Zero;
        }
        private void DecodeTouches(int msg, IntPtr wParam, IntPtr lParam)
        {
            //wParam contains the id of the pointer
            int pointerId = User32.GetPointerId(wParam);

            User32.POINTER_INPUT_TYPE pointerType;
            User32.POINTER_TOUCH_INFO pt;
            User32.POINTER_PEN_INFO pp;

            User32.POINTER_INFO pointerInfo = new User32.POINTER_INFO();

            if (!User32.GetPointerInfo(pointerId, ref pointerInfo))
                return;

            if (!User32.GetPointerType(pointerId, out pointerType))
                return;

            //select the type of input we want to process
            switch (pointerInfo.pointerType)
            {
                case User32.POINTER_INPUT_TYPE.TOUCH:
                    pt = new User32.POINTER_TOUCH_INFO();

                    if (!User32.GetPointerTouchInfo(pointerId, ref pt) )
                        break;

                    DecodeTouchMessage(msg, pt);

                    break;
                case User32.POINTER_INPUT_TYPE.PEN:
                    pp = new User32.POINTER_PEN_INFO();
                    
                    if (!User32.GetPointerPenInfo(pointerId, ref pp))
                        break;

                    DecodePenMessage(msg, pp);

                    break;
                case User32.POINTER_INPUT_TYPE.MOUSE:
                    break;
            }

            return;
        }

        private void DecodeTouchMessage(int msg, User32.POINTER_TOUCH_INFO pointerInfo)
        {
            switch (msg)
            {
                case User32.WM_POINTERDOWN:
                    HandleTouchDown(pointerInfo);
                    break;
                case User32.WM_POINTERUP:
                    HandleTouchUp(pointerInfo);
                    break;
                case User32.WM_POINTERUPDATE:
                    HandleTouchMove(pointerInfo);
                    break;
            }
        }

        private void DecodePenMessage(int msg, User32.POINTER_PEN_INFO pointerInfo)
        {
            switch (msg)
            {
                case User32.WM_POINTERDOWN:
                    HandlePenDown(pointerInfo);
                    break;
                case User32.WM_POINTERUP:
                    HandlePenUp(pointerInfo);
                    break;
                case User32.WM_POINTERUPDATE:
                    HandlePenMove(pointerInfo);
                    break;
            }
        }

        private void HandleTouchDown(User32.POINTER_TOUCH_INFO pointer)
        {
            //point in local co-ordinates
            var sp = PointFromScreen(new Point(pointer.pointerinfo.PtPixelLocation.X, pointer.pointerinfo.PtPixelLocation.Y));

            //remove stroke if it exists because it shouldn't
            if (this.currentTouches.ContainsKey(pointer.pointerinfo.PointerID))
                this.currentTouches.Remove(pointer.pointerinfo.PointerID);

            var width = pointer.rcContact.Right - pointer.rcContact.Left;
            var height = pointer.rcContact.Bottom - pointer.rcContact.Top;

            //create a new touch visualizer dot
            var dot = new TouchDot();
            dot.Width = width;
            dot.Height = height;

            //Add touch dot to dictionary
            this.currentTouches.Add(pointer.pointerinfo.PointerID, dot);

            //add touch to canvas
            this.LayoutCanvas.Children.Add(dot);

            //move dot to correct location
            Canvas.SetLeft(dot, sp.X - (width / 2));
            Canvas.SetTop(dot, sp.Y - (height / 2));

        }
        private void HandleTouchMove(User32.POINTER_TOUCH_INFO pointer)
        {
            //point in local co-ordinates
            var sp = PointFromScreen(new Point(pointer.pointerinfo.PtPixelLocation.X, pointer.pointerinfo.PtPixelLocation.Y));

            if (this.currentTouches.ContainsKey(pointer.pointerinfo.PointerID))
            {
                var width = pointer.rcContact.Right - pointer.rcContact.Left;
                var height = pointer.rcContact.Bottom - pointer.rcContact.Top;

                var dot = this.currentTouches[pointer.pointerinfo.PointerID];
                dot.Width = width;
                dot.Height = height;

                //move existing dot 
                Canvas.SetLeft(dot, sp.X - (width / 2));
                Canvas.SetTop(dot, sp.Y - (height / 2));
            }
        }

        private void HandleTouchUp(User32.POINTER_TOUCH_INFO pointer)
        {
            //remove dot from dictionary and screen
            if (this.currentTouches.ContainsKey(pointer.pointerinfo.PointerID))
            {
                this.LayoutCanvas.Children.Remove(this.currentTouches[pointer.pointerinfo.PointerID]);
                this.currentTouches.Remove(pointer.pointerinfo.PointerID);
            }

        }

        private void HandlePenDown(User32.POINTER_PEN_INFO pointer)
        {
            var sp = PointFromScreen(new Point(pointer.pointerinfo.PtPixelLocation.X, pointer.pointerinfo.PtPixelLocation.Y));
            StylusPointCollection spc = new StylusPointCollection{
                                    new StylusPoint(sp.X, sp.Y, (pointer.pressure/1024))};

            //remove stroke if it exists because it shouldn't
            if (this.currentStrokes.ContainsKey(pointer.pointerinfo.PointerID))
                this.currentStrokes.Remove(pointer.pointerinfo.PointerID);

            //create a new stroke
            var stroke = new Stroke(spc);
            stroke.DrawingAttributes.FitToCurve = false;

            //add new stroke to presenter
            this.InkPresenter.Strokes.Add(stroke);

            //Add Stroke to dictionary so we can modify it later
            this.currentStrokes.Add(pointer.pointerinfo.PointerID, stroke);

        }
        private void HandlePenUp(User32.POINTER_PEN_INFO pointer)
        {
            //remove stroke form list and screen
            if (this.currentStrokes.ContainsKey(pointer.pointerinfo.PointerID))
            {
                this.currentStrokes[pointer.pointerinfo.PointerID].DrawingAttributes.FitToCurve = true;
                this.currentStrokes[pointer.pointerinfo.PointerID] = null;
                this.currentStrokes.Remove(pointer.pointerinfo.PointerID);
            }
        }
        private void HandlePenMove(User32.POINTER_PEN_INFO pointer)
        {
            var sp = PointFromScreen(new Point(pointer.pointerinfo.PtPixelLocation.X, pointer.pointerinfo.PtPixelLocation.Y));

            //update stroke
            if (this.currentStrokes.ContainsKey(pointer.pointerinfo.PointerID))
            {
                this.currentStrokes[pointer.pointerinfo.PointerID].StylusPoints
                    .Add(
                        new StylusPoint(
                            sp.X,
                            sp.Y, 
                            (pointer.pressure / 1024)));
            }
        }

    }
}
