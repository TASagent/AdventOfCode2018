namespace AoCTools;

public readonly struct Point4D
{
    public readonly int x;
    public readonly int y;
    public readonly int z;
    public readonly int w;

    public static readonly Point4D Zero = new Point4D(0, 0, 0, 0);
    public static readonly Point4D XAxis = new Point4D(1, 0, 0, 0);
    public static readonly Point4D YAxis = new Point4D(0, 1, 0, 0);
    public static readonly Point4D ZAxis = new Point4D(0, 0, 1, 0);
    public static readonly Point4D WAxis = new Point4D(0, 0, 0, 1);

    public int TaxiCabLength => Math.Abs(x) + Math.Abs(y) + Math.Abs(z) + Math.Abs(w);
    public double GeometricLength => Math.Sqrt(x * x + y * y + z * z + w * w);

    public Point4D(int x, int y, int z, int w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static implicit operator Point4D((int x, int y, int z, int w) point) =>
        new Point4D(point.x, point.y, point.z, point.w);

    public static implicit operator (int x, int y, int z, int w)(Point4D point) =>
        (point.x, point.y, point.z, point.w);

    public override bool Equals(object obj)
    {
        if (obj is not Point4D other)
        {
            return false;
        }

        return x == other.x && y == other.y && z == other.z && w == other.w;
    }

    public override int GetHashCode() => HashCode.Combine(x, y, z, w);
    public override string ToString() => $"({x}, {y}, {z}, {w})";

    public static bool operator ==(in Point4D lhs, in Point4D rhs) =>
        lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z && lhs.w == rhs.w;

    public static bool operator !=(in Point4D lhs, in Point4D rhs) =>
        lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z || lhs.w != rhs.w;

    public static Point4D operator +(in Point4D lhs, in Point4D rhs) =>
        new Point4D(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
    public static Point4D operator -(in Point4D lhs, in Point4D rhs) =>
        new Point4D(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
    public static Point4D operator *(in Point4D lhs, in Point4D rhs) =>
        new Point4D(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
    public static Point4D operator *(in Point4D lhs, int rhs) =>
        new Point4D(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs, lhs.w * rhs);
    public static Point4D operator *(int lhs, in Point4D rhs) =>
        new Point4D(rhs.x * lhs, rhs.y * lhs, rhs.z * lhs, rhs.w * lhs);

    public static Point4D operator -(in Point4D value) =>
        new Point4D(-value.x, -value.y, -value.z, -value.w);

    public void Deconstruct(out int x, out int y, out int z, out int w)
    {
        x = this.x;
        y = this.y;
        z = this.z;
        w = this.w;
    }
}
