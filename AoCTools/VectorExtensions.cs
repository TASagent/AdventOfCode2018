namespace AoCTools;

public static class VectorExtensions
{
    #region Vector2D

    public static Vector3D To3D(in this Vector2D point) => new Vector3D(point.x, point.y, 0);
    public static Vector4D To4D(in this Vector2D point) => new Vector4D(point.x, point.y, 0, 0);

    public static Vector2D Round(in this Vector2D point) =>
        new Vector2D(Math.Round(point.x), Math.Round(point.y));
    public static Vector2D Truncate(in this Vector2D point) =>
        new Vector2D(Math.Truncate(point.x), Math.Truncate(point.y));

    public static Vector2D Normalized(in this Vector2D point) => point / point.Length;

    public static Point2D ToPointTruncate(in this Vector2D point) =>
        new Point2D((int)point.x, (int)point.y);
    public static Point2D ToPointRound(in this Vector2D point) =>
        new Point2D((int)Math.Round(point.x), (int)Math.Round(point.y));

    public static LongPoint2D ToLongPointTruncate(in this Vector2D point) =>
        new LongPoint2D((long)point.x, (long)point.y);
    public static LongPoint2D ToLongPointRound(in this Vector2D point) =>
        new LongPoint2D((long)Math.Round(point.x), (long)Math.Round(point.y));

    public static IEnumerable<Vector2D> GetAdjacent(this Vector2D point)
    {
        yield return point + Vector2D.XAxis;
        yield return point + Vector2D.YAxis;
        yield return point - Vector2D.XAxis;
        yield return point - Vector2D.YAxis;
    }

    public static double Dot(in this Vector2D point, in Vector2D other) =>
        point.x * other.x + point.y * other.y;

    public static Vector2D MinCoordinate(this IEnumerable<Vector2D> points)
    {
        double minX = double.MaxValue;
        double minY = double.MaxValue;

        foreach (Vector2D point in points)
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

        return new Vector2D(minX, minY);
    }

    public static Vector2D MaxCoordinate(this IEnumerable<Vector2D> points)
    {
        double maxX = double.MinValue;
        double maxY = double.MinValue;

        foreach (Vector2D point in points)
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

        return new Vector2D(maxX, maxY);
    }

    public static Vector2D Rotate(in this Vector2D point, bool right)
    {
        if (right)
        {
            return new Vector2D(point.y, -point.x);
        }
        else
        {
            return new Vector2D(-point.y, point.x);
        }
    }

    #endregion Vector2D
    #region Vector3D

    public static Vector2D To2D(in this Vector3D point) => new Vector2D(point.x, point.y);
    public static Vector4D To4D(in this Vector3D point) => new Vector4D(point.x, point.y, point.z, 0);

    public static Vector3D Round(in this Vector3D point) =>
        new Vector3D(Math.Round(point.x), Math.Round(point.y), Math.Round(point.z));
    public static Vector3D Truncate(in this Vector3D point) =>
        new Vector3D(Math.Truncate(point.x), Math.Truncate(point.y), Math.Truncate(point.z));

    public static Vector3D Normalized(in this Vector3D point) => point / point.Length;

    public static Point3D ToPointTruncate(in this Vector3D point) =>
        new Point3D((int)point.x, (int)point.y, (int)point.z);
    public static Point3D ToPointRound(in this Vector3D point) =>
        new Point3D((int)Math.Round(point.x), (int)Math.Round(point.y), (int)Math.Round(point.z));

    public static LongPoint3D ToLongPointTruncate(in this Vector3D point) =>
        new LongPoint3D((long)point.x, (long)point.y, (long)point.z);
    public static LongPoint3D ToLongPointRound(in this Vector3D point) =>
        new LongPoint3D((long)Math.Round(point.x), (long)Math.Round(point.y), (long)Math.Round(point.z));

    public static IEnumerable<Vector3D> Get2DAdjacent(this Vector3D point)
    {
        yield return point + Vector3D.XAxis;
        yield return point + Vector3D.YAxis;
        yield return point - Vector3D.XAxis;
        yield return point - Vector3D.YAxis;
    }

    public static IEnumerable<Vector3D> Get3DAdjacent(this Vector3D point)
    {
        yield return point + Vector3D.XAxis;
        yield return point + Vector3D.YAxis;
        yield return point + Vector3D.ZAxis;
        yield return point - Vector3D.XAxis;
        yield return point - Vector3D.YAxis;
        yield return point - Vector3D.ZAxis;
    }

    public static double Dot(in this Vector3D point, in Vector3D other) =>
        point.x * other.x + point.y * other.y + point.z * other.z;

    public static Vector3D Cross(in this Vector3D point, in Vector3D other) =>
        new Vector3D(
            x: point.y * other.z - point.z * other.y,
            y: point.z * other.x - point.x * other.z,
            z: point.x * other.y - point.y * other.x);

    public static Vector3D MinCoordinate(this IEnumerable<Vector3D> points)
    {
        double minX = double.MaxValue;
        double minY = double.MaxValue;
        double minZ = double.MaxValue;

        foreach (Vector3D point in points)
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

        return new Vector3D(minX, minY, minZ);
    }

    public static Vector3D MaxCoordinate(this IEnumerable<Vector3D> points)
    {
        double maxX = double.MinValue;
        double maxY = double.MinValue;
        double maxZ = double.MinValue;

        foreach (Vector3D point in points)
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

        return new Vector3D(maxX, maxY, maxZ);
    }

    #endregion Vector3D
    #region Vector4D

    public static Vector2D To2D(in this Vector4D point) => new Vector2D(point.x, point.y);
    public static Vector3D To3D(in this Vector4D point) => new Vector3D(point.x, point.y, point.z);

    public static Vector4D Round(in this Vector4D point) =>
        new Vector4D(Math.Round(point.x), Math.Round(point.y), Math.Round(point.z), Math.Round(point.w));
    public static Vector4D Truncate(in this Vector4D point) =>
        new Vector4D(Math.Truncate(point.x), Math.Truncate(point.y), Math.Truncate(point.z), Math.Truncate(point.w));

    public static Vector4D Normalized(in this Vector4D point) => point / point.Length;

    public static Point4D ToPointTruncate(in this Vector4D point) =>
        new Point4D((int)point.x, (int)point.y, (int)point.z, (int)point.w);
    public static Point4D ToPointRound(in this Vector4D point) =>
        new Point4D((int)Math.Round(point.x), (int)Math.Round(point.y), (int)Math.Round(point.z), (int)Math.Round(point.w));

    public static LongPoint4D ToLongPointTruncate(in this Vector4D point) =>
        new LongPoint4D((long)point.x, (long)point.y, (long)point.z, (long)point.w);
    public static LongPoint4D ToLongPointRound(in this Vector4D point) =>
        new LongPoint4D((long)Math.Round(point.x), (long)Math.Round(point.y), (long)Math.Round(point.z), (long)Math.Round(point.w));

    public static IEnumerable<Vector4D> Get2DAdjacent(this Vector4D point)
    {
        yield return point + Vector4D.XAxis;
        yield return point + Vector4D.YAxis;
        yield return point - Vector4D.XAxis;
        yield return point - Vector4D.YAxis;
    }

    public static IEnumerable<Vector4D> Get3DAdjacent(this Vector4D point)
    {
        yield return point + Vector4D.XAxis;
        yield return point + Vector4D.YAxis;
        yield return point + Vector4D.ZAxis;
        yield return point - Vector4D.XAxis;
        yield return point - Vector4D.YAxis;
        yield return point - Vector4D.ZAxis;
    }

    public static IEnumerable<Vector4D> Get4DAdjacent(this Vector4D point)
    {
        yield return point + Vector4D.XAxis;
        yield return point + Vector4D.YAxis;
        yield return point + Vector4D.ZAxis;
        yield return point + Vector4D.WAxis;
        yield return point - Vector4D.XAxis;
        yield return point - Vector4D.YAxis;
        yield return point - Vector4D.WAxis;
    }

    public static double Dot(in this Vector4D point, in Vector4D other) =>
        point.x * other.x + point.y * other.y + point.z * other.z + point.w * other.w;

    public static Vector4D MinCoordinate(this IEnumerable<Vector4D> points)
    {
        double minX = double.MaxValue;
        double minY = double.MaxValue;
        double minZ = double.MaxValue;
        double minW = double.MaxValue;

        foreach (Vector4D point in points)
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

        return new Vector4D(minX, minY, minZ, minW);
    }

    public static Vector4D MaxCoordinate(this IEnumerable<Vector4D> points)
    {
        double maxX = double.MinValue;
        double maxY = double.MinValue;
        double maxZ = double.MinValue;
        double maxW = double.MinValue;

        foreach (Vector4D point in points)
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

        return new Vector4D(maxX, maxY, maxZ, maxW);
    }

    #endregion Vector4D
}
