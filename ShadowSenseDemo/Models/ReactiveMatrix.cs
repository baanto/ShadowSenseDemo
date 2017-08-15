using ReactiveUI;
using System.Windows.Media.Media3D;

namespace ShadowSenseDemo.Models
{
    public static class MatrixExtensions
    {
        public static Matrix3D ConvertToMatrix3D(this ReactiveMatrix rm)
        {
            Matrix3D mat = new Matrix3D();
            mat.M11 = rm.M11;
            mat.M12 = rm.M12;
            mat.M13 = rm.M13;
            mat.M21 = rm.M21;
            mat.M22 = rm.M22;
            mat.M23 = rm.M23;
            mat.M31 = rm.M31;
            mat.M32 = rm.M32;
            mat.M33 = rm.M33;
            return mat;
        }
        public static ReactiveMatrix ConvertToReactiveMatrix(this Matrix3D mat)
        {
            ReactiveMatrix rm = new ReactiveMatrix();
            rm.M11 = mat.M11;
            rm.M12 = mat.M12;
            rm.M13 = mat.M13;
            rm.M21 = mat.M21;
            rm.M22 = mat.M22;
            rm.M23 = mat.M23;
            rm.M31 = mat.M31;
            rm.M32 = mat.M32;
            rm.M33 = mat.M33;
            return rm;
        }

    }
    public class ReactiveMatrix : ReactiveObject
    {

        private double m11;
        public double M11
        {
            get { return this.m11; }
            set { this.RaiseAndSetIfChanged(ref this.m11, value); }
        }
        private double m12;
        public double M12
        {
            get { return this.m12; }
            set { this.RaiseAndSetIfChanged(ref this.m12, value); }
        }
        private double m13;
        public double M13
        {
            get { return this.m13; }
            set { this.RaiseAndSetIfChanged(ref this.m13, value); }
        }

        private double m21;
        public double M21
        {
            get { return this.m21; }
            set { this.RaiseAndSetIfChanged(ref this.m21, value); }
        }
        private double m22;
        public double M22
        {
            get { return this.m22; }
            set { this.RaiseAndSetIfChanged(ref this.m22, value); }
        }
        private double m23;
        public double M23
        {
            get { return this.m23; }
            set { this.RaiseAndSetIfChanged(ref this.m23, value); }
        }

        private double m31;
        public double M31
        {
            get { return this.m31; }
            set { this.RaiseAndSetIfChanged(ref this.m31, value); }
        }
        private double m32;
        public double M32
        {
            get { return this.m32; }
            set { this.RaiseAndSetIfChanged(ref this.m32, value); }
        }
        private double m33;
        public double M33
        {
            get { return this.m33; }
            set { this.RaiseAndSetIfChanged(ref this.m33, value); }
        }




    }
}
