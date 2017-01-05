using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Drawing
{
    public class Matrix
    {
    }

    public class MatrixBuilder
    {
        public Matrix ToMatrix() {
            return default(Matrix);
        }
    }

    public interface IReadOnlyMatrix : IReadOnlyMatrix2D, IReadOnlyMatrix3D
    {
        bool Is2D { get; }
        bool IsIdentity { get; }

        Matrix Translate(double tx = 0, double ty = 0, double tz = 0);
        Matrix Scale(double scaleX = 1, double scaleY = 1, double scaleZ = 1, double originX = 0, double originY = 0, double originZ = 0);
        Matrix Scale3d(double scale = 1, double originX = 0, double originY = 0, double originZ = 0);
        Matrix Rotate(double rotX = 0, double rotY = 0, double rotZ = 0);
        Matrix RotateFromVector(double x = 0, double y = 0);
        Matrix RotateAxisAngle(double x = 0, double y = 0, double z = 0, double angle = 0);
        Matrix SkewX(double sx = 0);
        Matrix SkewY(double sy = 0);
        Matrix Multiply(MatrixInit other);
        Matrix FlipX();
        Matrix FlipY();
        Matrix Inverse();
    }

    public class MatrixInit
    {
        double a;
        double b;
        double c;
        double d;
        double e;
        double f;
        double m11;
        double m12;
        double m13 = 0d;
        double m14 = 0d;
        double m21;
        double m22;
        double m23 = 0d;
        double m24 = 0d;
        double m31 = 0d;
        double m32 = 0d;
        double m33 = 1d;
        double m34 = 0d;
        double m41;
        double m42;
        double m43 = 0d;
        double m44 = 1d;
        bool is2D;
    };

    public interface IReadOnlyMatrix2D
    {
        double a { get; }
        double b { get; }
        double c { get; }
        double d { get; }
        double e { get; }
        double f { get; }
    }

    public interface IMatrix2D : IReadOnlyMatrix2D
    {
        new double a { get; set; }
        new double b { get; set; }
        new double c { get; set; }
        new double d { get; set; }
        new double e { get; set; }
        new double f { get; set; }
    }

    public interface IReadOnlyMatrix3D
    {
        double m11 { get; }
        double m12 { get; }
        double m13 { get; }
        double m14 { get; }
        double m21 { get; }
        double m22 { get; }
        double m23 { get; }
        double m24 { get; }
        double m31 { get; }
        double m32 { get; }
        double m33 { get; }
        double m34 { get; }
        double m41 { get; }
        double m42 { get; }
        double m43 { get; }
        double m44 { get; }
    }

    public interface IMatrix3D : IReadOnlyMatrix3D
    {
        new double m11 { get; set; }
        new double m12 { get; set; }
        new double m13 { get; set; }
        new double m14 { get; set; }
        new double m21 { get; set; }
        new double m22 { get; set; }
        new double m23 { get; set; }
        new double m24 { get; set; }
        new double m31 { get; set; }
        new double m32 { get; set; }
        new double m33 { get; set; }
        new double m34 { get; set; }
        new double m41 { get; set; }
        new double m42 { get; set; }
        new double m43 { get; set; }
        new double m44 { get; set; }
    }
}
