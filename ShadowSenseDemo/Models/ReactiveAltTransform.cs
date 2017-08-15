using Baanto.ShadowSense;
using ReactiveUI;

namespace ShadowSenseDemo.Models
{
    public static class ReactiveAltTransformExtensions
    {
        public static AltTransform ConvertToShadowSenseAltTransform(this ReactiveAltTransform rsm)
        {
            AltTransform shadowSenseAltTransform = new AltTransform();
            shadowSenseAltTransform.OffsetX = rsm.OffsetX;
            shadowSenseAltTransform.OffsetY = rsm.OffsetY;
            shadowSenseAltTransform.GainX = rsm.GainX;
            shadowSenseAltTransform.GainY = rsm.GainY;

            return shadowSenseAltTransform;
        }
        public static ReactiveAltTransform ConvertToReactiveAltTransform(this AltTransform sm)
        {
            ReactiveAltTransform rsm = new ReactiveAltTransform();
            rsm.OffsetX = sm.OffsetX;
            rsm.OffsetY = sm.OffsetY;
            rsm.GainX = sm.GainX;
            rsm.GainY = sm.GainY;

            return rsm;
        }
    }
    public class ReactiveAltTransform : ReactiveObject
    {
        private float offsetX;
        public float OffsetX
        {
            get { return this.offsetX; }
            set { this.RaiseAndSetIfChanged(ref this.offsetX, value); }
        }
        private float offsetY;
        public float OffsetY
        {
            get { return this.offsetY; }
            set { this.RaiseAndSetIfChanged(ref this.offsetY, value); }
        }
        private float gainX;
        public float GainX
        {
            get { return this.gainX; }
            set { this.RaiseAndSetIfChanged(ref this.gainX, value); }
        }
        private float gainY;
        public float GainY
        {
            get { return this.gainY; }
            set { this.RaiseAndSetIfChanged(ref this.gainY, value); }
        }


    }

}
