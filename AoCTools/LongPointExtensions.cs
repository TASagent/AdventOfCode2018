namespace AoCTools;

public static class LongPointExtensions
{
    #region LongPoint2D

    public static LongPoint3D To3D(in this LongPoint2D point) => new LongPoint3D(point.x, point.y, 0);
    public static LongPoint4D To4D(in this LongPoint2D point) => new LongPoint4D(point.x, point.y, 0, 0);

    public static Vector2D ToVector(in this LongPoint2D point) => new Vector2D(point.x, point.y);
    public static Point2D ToPoint(in this LongPoint2D point) => new Point2D((int)point.x, (int)point.y);


    public static IEnumerable<LongPoint2D> GetAdjacent(this LongPoint2D point)
    {
        yield return point + LongPoint2D.XAxis;
        yield return point + LongPoint2D.YAxis;
        yield return point - LongPoint2D.XAxis;
        yield return point - LongPoint2D.YAxis;
    }

    public static long Dot(in this LongPoint2D point, in LongPoint2D other) =>
        point.x * other.x + point.y * other.y;

    public static LongPoint2D MinCoordinate(this IEnumerable<LongPoint2D> points)
    {
        long minX = long.MaxValue;
        long minY = long.MaxValue;

        foreach (LongPoint2D point in points)
        {
            if (point.x < minX)
            {
                minX = point.x;
            }

            if (point.y < minY)
            {
                minY = point.y;
            }
        }

        return new LongPoint2D(minX, minY);
    }

    public static LongPoint2D MaxCoordinate(this IEnumerable<LongPoint2D> points)
    {
        long maxX = long.MinValue;
        long maxY = long.MinValue;

        foreach (LongPoint2D point in points)
        {
            if (point.x > maxX)
            {
                maxX = point.x;
            }

            if (point.y > maxY)
            {
                maxY = point.y;
            }
        }

        return new LongPoint2D(maxX, maxY);
    }

    public static LongPoint2D Rotate(in this LongPoint2D point, bool right)
    {
        if (right)
        {
            return new LongPoint2D(point.y, -point.x);
        }
        else
        {
            return new LongPoint2D(-point.y, point.x);
        }
    }

    #endregion LongPoint2D
    #region LongPoint3D

    public static LongPoint2D To2D(in this LongPoint3D point) => new LongPoint2D(point.x, point.y);
    public static LongPoint4D To4D(in this LongPoint3D point) => new LongPoint4D(point.x, point.y, point.z, 0);

    public static Vector3D ToVector(in this LongPoint3D point) => new Vector3D(point.x, point.y, point.z);
    public static Point3D ToPoint(in this LongPoint3D point) => new Point3D((int)point.x, (int)point.y, (int)point.z);

    public static IEnumerable<LongPoint3D> Get2DAdjacent(this LongPoint3D point)
    {
        yield return point + LongPoint3D.XAxis;
        yield return point + LongPoint3D.YAxis;
        yield return point - LongPoint3D.XAxis;
        yield return point - LongPoint3D.YAxis;
    }

    public static IEnumerable<LongPoint3D> Get3DAdjacent(this LongPoint3D point)
    {
        yield return point + LongPoint3D.XAxis;
        yield return point + LongPoint3D.YAxis;
        yield return point + LongPoint3D.ZAxis;
        yield return point - LongPoint3D.XAxis;
        yield return point - LongPoint3D.YAxis;
        yield return point - LongPoint3D.ZAxis;
    }

    public static long Dot(in this LongPoint3D point, in LongPoint3D other) =>
        point.x * other.x + point.y * other.y + point.z * other.z;

    public static LongPoint3D MinCoordinate(this IEnumerable<LongPoint3D> points)
    {
        long minX = long.MaxValue;
        long minY = long.MaxValue;
        long minZ = long.MaxValue;

        foreach (LongPoint3D point in points)
        {
            if (point.x < minX)
            {
                minX = point.x;
            }

            if (point.y < minY)
            {
                minY = point.y;
            }

            if (point.z < minZ)
            {
                minZ = point.z;
            }
        }

        return new LongPoint3D(minX, minY, minZ);
    }

    public static LongPoint3D MaxCoordinate(this IEnumerable<LongPoint3D> points)
    {
        long maxX = long.MinValue;
        long maxY = long.MinValue;
        long maxZ = long.MinValue;

        foreach (LongPoint3D point in points)
        {
            if (point.x > maxX)
            {
                maxX = point.x;
            }

            if (point.y > maxY)
            {
                maxY = point.y;
            }

            if (point.z > maxZ)
            {
                maxZ = point.z;
            }
        }

        return new LongPoint3D(maxX, maxY, maxZ);
    }

    #endregion LongPoint3D
    #region LongPoint4D

    public static LongPoint2D To2D(in this LongPoint4D point) => new LongPoint2D(point.x, point.y);
    public static LongPoint3D To3D(in this LongPoint4D point) => new LongPoint3D(point.x, point.y, point.z);

    public static Vector4D ToVector(in this LongPoint4D point) => new Vector4D(point.x, point.y, point.z, point.w);
    public static Point4D ToPoint(in this LongPoint4D point) => new Point4D((int)point.x, (int)point.y, (int)point.z, (int)point.w);

    public static IEnumerable<LongPoint4D> Get2DAdjacent(this LongPoint4D point)
    {
        yield return point + LongPoint4D.XAxis;
        yield return point + LongPoint4D.YAxis;
        yield return point - LongPoint4D.XAxis;
        yield return point - LongPoint4D.YAxis;
    }

    public static IEnumerable<LongPoint4D> Get3DAdjacent(this LongPoint4D point)
    {
        yield return point + LongPoint4D.XAxis;
        yield return point + LongPoint4D.YAxis;
        yield return point + LongPoint4D.ZAxis;
        yield return point - LongPoint4D.XAxis;
        yield return point - LongPoint4D.YAxis;
        yield return point - LongPoint4D.ZAxis;
    }

    public static IEnumerable<LongPoint4D> Get4DAdjacent(this LongPoint4D point)
    {
        yield return point + LongPoint4D.XAxis;
        yield return point + LongPoint4D.YAxis;
        yield return point + LongPoint4D.ZAxis;
        yield return point + LongPoint4D.WAxis;
        yield return point - LongPoint4D.XAxis;
        yield return point - LongPoint4D.YAxis;
        yield return point - LongPoint4D.ZAxis;
        yield return point - LongPoint4D.WAxis;
    }

    public static long Dot(in this LongPoint4D point, in LongPoint4D other) =>
        point.x * other.x + point.y * other.y + point.z * other.z + point.w * other.w;

    public static LongPoint4D MinCoordinate(this IEnumerable<LongPoint4D> points)
    {
        long minX = long.MaxValue;
        long minY = long.MaxValue;
        long minZ = long.MaxValue;
        long minW = long.MaxValue;

        foreach (LongPoint4D point in points)
        {
            if (point.x < minX)
            {
                minX = point.x;
            }

            if (point.y < minY)
            {
                minY = point.y;
            }

            if (point.z < minZ)
            {
                minZ = point.z;
            }

            if (point.w < minW)
            {
                minW = point.w;
            }
        }

        return new LongPoint4D(minX, minY, minZ, minW);
    }

    public static LongPoint4D MaxCoordinate(this IEnumerable<LongPoint4D> points)
    {
        long maxX = long.MinValue;
        long maxY = long.MinValue;
        long maxZ = long.MinValue;
        long maxW = long.MinValue;

        foreach (LongPoint4D point in points)
        {
            if (point.x > maxX)
            {
                maxX = point.x;
            }

            if (point.y > maxY)
            {
                maxY = point.y;
            }

            if (point.z > maxZ)
            {
                maxZ = point.z;
            }

            if (point.w > maxW)
            {
                maxW = point.w;
            }
        }

        return new LongPoint4D(maxX, maxY, maxZ, maxW);
    }

    #endregion LongPoint4D
}
