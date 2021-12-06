namespace AoCTools;

public static class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> GetUnorderedSubsets<T>(this IEnumerable<T> elements, int count)
    {
        if (count < 0)
        {
            throw new Exception();
        }

        if (count == 0)
        {
            return new[] { Enumerable.Empty<T>() };
        }

        return elements.SelectMany((x, i) => elements.Skip(i + 1).GetUnorderedSubsets(count - 1).Select(y => y.Prepend(x)));
    }

    public static T[,] ToArrayGrid<T>(
        this IEnumerable<IEnumerable<T>> input,
        T defaultValue = default,
        int widthOffset = 0,
        int widthExtension = 0,
        int lengthOffset = 0,
        int lengthExtension = 0)
    {
        int width = input.Max(x => x.Count()) + widthExtension + widthOffset;
        int length = input.Count() + lengthExtension + lengthOffset;
        T[,] grid = new T[width, length];

        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = defaultValue;
            }
        }

        foreach ((IEnumerable<T> row, int y) in input.Select((r, i) => (r, i)))
        {
            foreach ((T value, int x) in row.Select((v, i) => (v, i)))
            {
                grid[x + widthOffset, y + lengthOffset] = value;
            }
        }

        return grid;
    }


    public static Dictionary<Point2D, T> ToDictionaryGrid<T>(
        this IEnumerable<IEnumerable<T>> input,
        int widthOffset = 0,
        int lengthOffset = 0)
    {
        Dictionary<Point2D, T> grid = new Dictionary<Point2D, T>();

        foreach ((IEnumerable<T> row, int y) in input.Select((r, i) => (r, i)))
        {
            foreach ((T value, int x) in row.Select((v, i) => (v, i)))
            {
                grid[(x + widthOffset, y + lengthOffset)] = value;
            }
        }

        return grid;
    }
}
