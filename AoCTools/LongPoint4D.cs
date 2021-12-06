namespace AoCTools;

public readonly struct LongPoint4D
{
    public readonly long x;
    public readonly long y;
    public readonly long z;
    public readonly long w;

    public static readonly LongPoint4D Zero = new LongPoint4D(0, 0, 0, 0);
    public static readonly LongPoint4D XAxis = new LongPoint4D(1, 0, 0, 0);
    public static readonly LongPoint4D YAxis = new LongPoint4D(0, 1, 0, 0);
    public static readonly LongPoint4D ZAxis = new LongPoint4D(0, 0, 1, 0);
    public static readonly LongPoint4D WAxis = new LongPoint4D(0, 0, 0, 1);

    public long TaxiCabLength => Math.Abs(x) + Math.Abs(y) + Math.Abs(z) + Math.Abs(w);
    public double GeometricLength => Math.Sqrt(x * x + y * y + z * z + w * w);

    public LongPoint4D(long x, long y, long z, long w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static implicit operator LongPoint4D((long x, long y, long z, long w) point) =>
        new LongPoint4D(point.x, point.y, point.z, point.w);

    public static implicit operator (long x, long y, long z, long w)(LongPoint4D point) =>
        (point.x, point.y, point.z, point.w);

    public override bool Equals(object obj)
    {
        if (obj is not LongPoint4D other)
        {
            return false;
        }

        return x == other.x && y == other.y && z == other.z && w == other.w;
    }

    public override int GetHashCode() => HashCode.Combine(x, y, z, w);
    public override string ToString() => $"({x}, {y}, {z}, {w})";

    public static bool operator ==(in LongPoint4D lhs, in LongPoint4D rhs) =>
        lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z && lhs.w == rhs.w;

    public static bool operator !=(in LongPoint4D lhs, in LongPoint4D rhs) =>
        lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z || lhs.w != rhs.w;

    public static LongPoint4D operator +(in LongPoint4D lhs, in LongPoint4D rhs) =>
        new LongPoint4D(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
    public static LongPoint4D operator -(in LongPoint4D lhs, in LongPoint4D rhs) =>
        new LongPoint4D(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
    public static LongPoint4D operator *(in LongPoint4D lhs, in LongPoint4D rhs) =>
        new LongPoint4D(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
    public static LongPoint4D operator *(in LongPoint4D lhs, long rhs) =>
        new LongPoint4D(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs, lhs.w * rhs);
    public static LongPoint4D operator *(long lhs, in LongPoint4D rhs) =>
        new LongPoint4D(rhs.x * lhs, rhs.y * lhs, rhs.z * lhs, rhs.w * lhs);

    public static LongPoint4D operator -(in LongPoint4D value) =>
        new LongPoint4D(-value.x, -value.y, -value.z, -value.w);

    public void Deconstruct(out long x, out long y, out long z, out long w)
    {
        x = this.x;
        y = this.y;
        z = this.z;
        w = this.w;
    }
}
