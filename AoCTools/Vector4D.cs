namespace AoCTools;

public readonly struct Vector4D
{
    public readonly double x;
    public readonly double y;
    public readonly double z;
    public readonly double w;

    public static readonly Vector4D Zero = new Vector4D(0.0, 0.0, 0.0, 0.0);
    public static readonly Vector4D XAxis = new Vector4D(1.0, 0.0, 0.0, 0.0);
    public static readonly Vector4D YAxis = new Vector4D(0.0, 1.0, 0.0, 0.0);
    public static readonly Vector4D ZAxis = new Vector4D(0.0, 0.0, 1.0, 0.0);
    public static readonly Vector4D WAxis = new Vector4D(0.0, 0.0, 0.0, 1.0);

    public double Length => Math.Sqrt(x * x + y * y + z * z + w * w);

    public Vector4D(double x, double y, double z, double w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static implicit operator Vector4D((double x, double y, double z, double w) point) =>
        new Vector4D(point.x, point.y, point.z, point.w);

    public static implicit operator (double x, double y, double z, double w)(Vector4D point) =>
        (point.x, point.y, point.z, point.w);

    public override bool Equals(object obj)
    {
        if (obj is not Vector4D other)
        {
            return false;
        }

        return x == other.x && y == other.y && z == other.z && w == other.w;
    }

    public override int GetHashCode() => HashCode.Combine(x, y, z, w);
    public override string ToString() => $"({x}, {y}, {z}, {w})";

    public static bool operator ==(in Vector4D lhs, in Vector4D rhs) =>
        lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z && lhs.w == rhs.w;

    public static bool operator !=(in Vector4D lhs, in Vector4D rhs) =>
        lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z || lhs.w != rhs.w;

    public static Vector4D operator +(in Vector4D lhs, in Vector4D rhs) =>
        new Vector4D(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
    public static Vector4D operator -(in Vector4D lhs, in Vector4D rhs) =>
        new Vector4D(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
    public static Vector4D operator *(in Vector4D lhs, double rhs) =>
        new Vector4D(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs, lhs.w * rhs);
    public static Vector4D operator *(double lhs, in Vector4D rhs) =>
        new Vector4D(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z, lhs * rhs.w);
    public static Vector4D operator *(in Vector4D lhs, in Vector4D rhs) =>
        new Vector4D(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
    public static Vector4D operator /(in Vector4D lhs, double rhs) =>
        new Vector4D(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs, lhs.w / rhs);

    public static Vector4D operator -(in Vector4D value) =>
        new Vector4D(-value.x, -value.y, -value.z, -value.w);

    public void Deconstruct(out double x, out double y, out double z, out double w)
    {
        x = this.x;
        y = this.y;
        z = this.z;
        w = this.w;
    }
}
