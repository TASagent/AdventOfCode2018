using System.Numerics;

namespace AoCTools;

public static class AoCMath
{
    #region Point2D

    public static Point2D Min(in Point2D a, in Point2D b) => new Point2D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y));

    public static Point2D Max(in Point2D a, in Point2D b) => new Point2D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y));

    public static Point2D Clamp(in Point2D point, in Point2D min, in Point2D max) => new Point2D(
        x: Math.Clamp(point.x, min.x, max.x),
        y: Math.Clamp(point.y, min.y, max.y));

    #endregion Point2D
    #region Point3D

    public static Point3D Min(in Point3D a, in Point3D b) => new Point3D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y),
        z: Math.Min(a.z, b.z));

    public static Point3D Max(in Point3D a, in Point3D b) => new Point3D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y),
        z: Math.Max(a.z, b.z));

    public static Point3D Clamp(in Point3D point, in Point3D min, in Point3D max) => new Point3D(
        x: Math.Clamp(point.x, min.x, max.x),
        y: Math.Clamp(point.y, min.y, max.y),
        z: Math.Clamp(point.z, min.z, max.z));

    #endregion Point3D
    #region Point4D

    public static Point4D Min(in Point4D a, in Point4D b) => new Point4D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y),
        z: Math.Min(a.z, b.z),
        w: Math.Min(a.w, b.w));

    public static Point4D Max(in Point4D a, in Point4D b) => new Point4D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y),
        z: Math.Max(a.z, b.z),
        w: Math.Max(a.w, b.w));

    public static Point4D Clamp(in Point4D point, in Point4D min, in Point4D max) => new Point4D(
        x: Math.Clamp(point.x, min.x, max.x),
        y: Math.Clamp(point.y, min.y, max.y),
        z: Math.Clamp(point.z, min.z, max.z),
        w: Math.Clamp(point.w, min.w, max.w));

    #endregion Point4D
    #region LongPoint2D

    public static LongPoint2D Min(in LongPoint2D a, in LongPoint2D b) => new LongPoint2D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y));

    public static LongPoint2D Max(in LongPoint2D a, in LongPoint2D b) => new LongPoint2D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y));

    public static LongPoint2D Clamp(in LongPoint2D point, in LongPoint2D min, in LongPoint2D max) => new LongPoint2D(
        x: Math.Clamp(point.x, min.x, max.x),
        y: Math.Clamp(point.y, min.y, max.y));

    #endregion LongPoint2D
    #region LongPoint3D

    public static LongPoint3D Min(in LongPoint3D a, in LongPoint3D b) => new LongPoint3D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y),
        z: Math.Min(a.z, b.z));

    public static LongPoint3D Max(in LongPoint3D a, in LongPoint3D b) => new LongPoint3D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y),
        z: Math.Max(a.z, b.z));

    public static LongPoint3D Clamp(in LongPoint3D point, in LongPoint3D min, in LongPoint3D max) => new LongPoint3D(
        x: Math.Clamp(point.x, min.x, max.x),
        y: Math.Clamp(point.y, min.y, max.y),
        z: Math.Clamp(point.z, min.z, max.z));

    #endregion LongPoint3D
    #region LongPoint4D

    public static LongPoint4D Min(in LongPoint4D a, in LongPoint4D b) => new LongPoint4D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y),
        z: Math.Min(a.z, b.z),
        w: Math.Min(a.w, b.w));

    public static LongPoint4D Max(in LongPoint4D a, in LongPoint4D b) => new LongPoint4D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y),
        z: Math.Max(a.z, b.z),
        w: Math.Max(a.w, b.w));

    public static LongPoint4D Clamp(in LongPoint4D point, in LongPoint4D min, in LongPoint4D max) => new LongPoint4D(
        x: Math.Clamp(point.x, min.x, max.x),
        y: Math.Clamp(point.y, min.y, max.y),
        z: Math.Clamp(point.z, min.z, max.z),
        w: Math.Clamp(point.w, min.w, max.w));

    #endregion LongPoint4D
    #region Vector2D

    public static Vector2D Min(in Vector2D a, in Vector2D b) => new Vector2D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y));

    public static Vector2D Max(in Vector2D a, in Vector2D b) => new Vector2D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y));

    public static Vector2D Clamp(in Vector2D point, in Vector2D min, in Vector2D max) => new Vector2D(
        x: Math.Clamp(point.x, min.x, max.x),
        y: Math.Clamp(point.y, min.y, max.y));

    #endregion Vector2D
    #region Vector3D

    public static Vector3D Min(in Vector3D a, in Vector3D b) => new Vector3D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y),
        z: Math.Min(a.z, b.z));

    public static Vector3D Max(in Vector3D a, in Vector3D b) => new Vector3D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y),
        z: Math.Max(a.z, b.z));

    public static Vector3D Clamp(in Vector3D point, in Vector3D min, in Vector3D max) => new Vector3D(
        x: Math.Clamp(point.x, min.x, max.x),
        y: Math.Clamp(point.y, min.y, max.y),
        z: Math.Clamp(point.z, min.z, max.z));

    #endregion Vector3D
    #region Vector4D

    public static Vector4D Min(in Vector4D a, in Vector4D b) => new Vector4D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y),
        z: Math.Min(a.z, b.z),
        w: Math.Min(a.w, b.w));

    public static Vector4D Max(in Vector4D a, in Vector4D b) => new Vector4D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y),
        z: Math.Max(a.z, b.z),
        w: Math.Max(a.w, b.w));

    public static Vector4D Clamp(in Vector4D point, in Vector4D min, in Vector4D max) => new Vector4D(
        x: Math.Clamp(point.x, min.x, max.x),
        y: Math.Clamp(point.y, min.y, max.y),
        z: Math.Clamp(point.z, min.z, max.z),
        w: Math.Clamp(point.w, min.w, max.w));

    #endregion Vector4D


    //Euclid's Algorithm
    public static int GCD(int a, int b)
    {
        //Short cut to handling 0 case
        if (a == 0 || b == 0)
        {
            return a == 0 ? b : a;
        }

        if (b > a)
        {
            (a, b) = (b, a);
        }

        while (b != 0)
        {
            (a, b) = (b, a % b);
        }

        return a;
    }

    //Euclid's Algorithm
    public static long GCD(long a, long b)
    {
        //Short cut to handling 0 case
        if (a == 0 || b == 0)
        {
            return a == 0 ? b : a;
        }

        if (b > a)
        {
            (a, b) = (b, a);
        }

        while (b != 0)
        {
            (a, b) = (b, a % b);
        }

        return a;
    }

    #region BigInteger

    public static BigInteger PositiveMod(in BigInteger value, in BigInteger baseValue) =>
        (value % baseValue + baseValue) % baseValue;

    #endregion BigInteger
}
