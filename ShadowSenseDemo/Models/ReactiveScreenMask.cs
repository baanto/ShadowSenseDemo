using Baanto.ShadowSense;
using ReactiveUI;

namespace ShadowSenseDemo.Models
{
    public static class ScreenMaskExtensions
    {
        public static ScreenMask ConvertToShadowSenseScreenMask(this ReactiveScreenMask rsm)
        {
            ScreenMask shadowSenseScreenMask = new ScreenMask();
            shadowSenseScreenMask.Top = rsm.Top;
            shadowSenseScreenMask.Left = rsm.Left;
            shadowSenseScreenMask.Right = rsm.Right;
            shadowSenseScreenMask.Bottom = rsm.Bottom;

            return shadowSenseScreenMask;
        }
        public static ReactiveScreenMask ConvertToReactiveScreenMask(this ScreenMask sm)
        {
            ReactiveScreenMask rsm = new ReactiveScreenMask();
            rsm.Top = sm.Top;
            rsm.Left = sm.Left;
            rsm.Right = sm.Right;
            rsm.Bottom = sm.Bottom;

            return rsm;
        }

    }
    public class ReactiveScreenMask : ReactiveObject
    {
        private double top;
        public double Top
        {
            get { return this.top; }
            set { this.RaiseAndSetIfChanged(ref this.top, value); }
        }
        private double left;
        public double Left
        {
            get { return this.left; }
            set { this.RaiseAndSetIfChanged(ref this.left, value); }
        }
        private double bottom;
        public double Bottom
        {
            get { return this.bottom; }
            set { this.RaiseAndSetIfChanged(ref this.bottom, value); }
        }
        private double right;
        public double Right
        {
            get { return this.right; }
            set { this.RaiseAndSetIfChanged(ref this.right, value); }
        }


    }
}
