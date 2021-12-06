namespace AoCTools;

public static class PointExtensions
{
    #region Point2D

    public static Point3D To3D(in this Point2D point) => new Point3D(point.x, point.y, 0);
    public static Point4D To4D(in this Point2D point) => new Point4D(point.x, point.y, 0, 0);

    public static Vector2D ToVector(in this Point2D point) => new Vector2D(point.x, point.y);
    public static LongPoint2D ToLongPoint(in this Point2D point) => new LongPoint2D(point.x, point.y);

    public static Vector3D ToVector3D(in this Point2D point) => new Vector3D(point.x, point.y, 0.0);

    public static IEnumerable<Point2D> GetAdjacent(this Point2D point)
    {
        yield return point + Point2D.XAxis;
        yield return point + Point2D.YAxis;
        yield return point - Point2D.XAxis;
        yield return point - Point2D.YAxis;
    }

    public static int Dot(in this Point2D point, in Point2D other) =>
        point.x * other.x + point.y * other.y;

    public static Point2D MinCoordinate(this IEnumerable<Point2D> points)
    {
        int minX = int.MaxValue;
        int minY = int.MaxValue;

        foreach (Point2D point in points)
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

        return new Point2D(minX, minY);
    }

    public static Point2D MaxCoordinate(this IEnumerable<Point2D> points)
    {
        int maxX = int.MinValue;
        int maxY = int.MinValue;

        foreach (Point2D point in points)
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

        return new Point2D(maxX, maxY);
    }

    public static Point2D AverageRound(this IEnumerable<Point2D> points)
    {
        double xSum = 0.0;
        double ySum = 0.0;
        int count = 0;

        foreach (Point2D point in points)
        {
            count++;
            xSum += point.x;
            ySum += point.y;
        }

        return new Point2D(
            x: (int)Math.Round(xSum / count),
            y: (int)Math.Round(ySum / count));
    }

    public static Point2D Rotate(in this Point2D point, bool right)
    {
        if (right)
        {
            return new Point2D(point.y, -point.x);
        }
        else
        {
            return new Point2D(-point.y, point.x);
        }
    }

    public static bool IsWithin(in this Point2D point, Point2D corner1, Point2D corner2)
    {
        Point2D lowerBound = AoCMath.Min(corner1, corner2);
        Point2D upperBound = AoCMath.Max(corner1, corner2);

        return point.x >= lowerBound.x &&
            point.y >= lowerBound.y &&
            point.x <= upperBound.x &&
            point.y <= upperBound.y;
    }

    public static bool IsWithinBoundsInc(in this Point2D point, Point2D lowerBound, Point2D upperBound)
    {
        return point.x >= lowerBound.x &&
            point.y >= lowerBound.y &&
            point.x <= upperBound.x &&
            point.y <= upperBound.y;
    }

    #endregion Point2D
    #region Point3D

    public static Point2D To2D(in this Point3D point) => new Point2D(point.x, point.y);
    public static Point4D To4D(in this Point3D point) => new Point4D(point.x, point.y, point.z, 0);

    public static Vector3D ToVector(in this Point3D point) => new Vector3D(point.x, point.y, point.z);
    public static LongPoint3D ToLongPoint(in this Point3D point) => new LongPoint3D(point.x, point.y, point.z);

    public static IEnumerable<Point3D> Get2DAdjacent(this Point3D point)
    {
        yield return point + Point3D.XAxis;
        yield return point + Point3D.YAxis;
        yield return point - Point3D.XAxis;
        yield return point - Point3D.YAxis;
    }

    public static IEnumerable<Point3D> Get3DAdjacent(this Point3D point)
    {
        yield return point + Point3D.XAxis;
        yield return point + Point3D.YAxis;
        yield return point + Point3D.ZAxis;
        yield return point - Point3D.XAxis;
        yield return point - Point3D.YAxis;
        yield return point - Point3D.ZAxis;
    }

    public static int Dot(in this Point3D point, in Point3D other) =>
        point.x * other.x + point.y * other.y + point.z * other.z;

    public static Point3D MinCoordinate(this IEnumerable<Point3D> points)
    {
        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int minZ = int.MaxValue;

        foreach (Point3D point in points)
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

        return new Point3D(minX, minY, minZ);
    }

    public static Point3D MaxCoordinate(this IEnumerable<Point3D> points)
    {
        int maxX = int.MinValue;
        int maxY = int.MinValue;
        int maxZ = int.MinValue;

        foreach (Point3D point in points)
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

        return new Point3D(maxX, maxY, maxZ);
    }

    #endregion Point3D
    #region Point4D

    public static Point2D To2D(in this Point4D point) => new Point2D(point.x, point.y);
    public static Point3D To3D(in this Point4D point) => new Point3D(point.x, point.y, point.z);

    public static Vector4D ToVector(in this Point4D point) => new Vector4D(point.x, point.y, point.z, point.w);
    public static LongPoint4D ToLongPoint(in this Point4D point) => new LongPoint4D(point.x, point.y, point.z, point.w);

    public static IEnumerable<Point4D> Get2DAdjacent(this Point4D point)
    {
        yield return point + Point4D.XAxis;
        yield return point + Point4D.YAxis;
        yield return point - Point4D.XAxis;
        yield return point - Point4D.YAxis;
    }

    public static IEnumerable<Point4D> Get3DAdjacent(this Point4D point)
    {
        yield return point + Point4D.XAxis;
        yield return point + Point4D.YAxis;
        yield return point + Point4D.ZAxis;
        yield return point - Point4D.XAxis;
        yield return point - Point4D.YAxis;
        yield return point - Point4D.ZAxis;
    }

    public static IEnumerable<Point4D> Get4DAdjacent(this Point4D point)
    {
        yield return point + Point4D.XAxis;
        yield return point + Point4D.YAxis;
        yield return point + Point4D.ZAxis;
        yield return point + Point4D.WAxis;
        yield return point - Point4D.XAxis;
        yield return point - Point4D.YAxis;
        yield return point - Point4D.ZAxis;
        yield return point - Point4D.WAxis;
    }

    public static int Dot(in this Point4D point, in Point4D other) =>
        point.x * other.x + point.y * other.y + point.z * other.z + point.w * other.w;

    public static Point4D MinCoordinate(this IEnumerable<Point4D> points)
    {
        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int minZ = int.MaxValue;
        int minW = int.MaxValue;

        foreach (Point4D point in points)
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

        return new Point4D(minX, minY, minZ, minW);
    }

    public static Point4D MaxCoordinate(this IEnumerable<Point4D> points)
    {
        int maxX = int.MinValue;
        int maxY = int.MinValue;
        int maxZ = int.MinValue;
        int maxW = int.MinValue;

        foreach (Point4D point in points)
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

        return new Point4D(maxX, maxY, maxZ, maxW);
    }

    #endregion Point4D
}
