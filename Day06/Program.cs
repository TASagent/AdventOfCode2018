using AoCTools;

const string inputFile = @"../../../../input06.txt";

Console.WriteLine("Day 06 - Chronal Coordinates");
Console.WriteLine("Star 1");
Console.WriteLine();

List<Point2D> coordinates = File.ReadAllLines(inputFile)
    .Select(Parse)
    .ToList();

Point2D min = coordinates.MinCoordinate();
Point2D max = coordinates.MaxCoordinate();

int width = max.x - min.x + 1;
int height = max.y - min.y + 1;

int[,] gridAssignment = new int[width, height];
int[,] gridDistance = new int[width, height];
long[,] totalGridDistance = new long[width, height];

for (int x = 0; x < width; x++)
{
    for (int y = 0; y < height; y++)
    {
        gridAssignment[x, y] = -1;
        gridDistance[x, y] = int.MaxValue;
    }
}

HashSet<int> validTargets = new HashSet<int>();

for (int c = 0; c < coordinates.Count; c++)
{
    //Just accumulate the valid targets here
    validTargets.Add(c);
    Point2D coord = coordinates[c];

    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            int distance = (coord - min - (x, y)).TaxiCabLength;

            totalGridDistance[x, y] += distance;
            if (distance < gridDistance[x, y])
            {
                gridDistance[x, y] = distance;
                gridAssignment[x, y] = c;
            }
            else if (distance == gridDistance[x, y])
            {
                gridAssignment[x, y] = -1;
            }
        }
    }
}

for (int x = 0; x < width; x++)
{
    validTargets.Remove(gridAssignment[x, 0]);
    validTargets.Remove(gridAssignment[x, height - 1]);
}

for (int y = 0; y < height; y++)
{
    validTargets.Remove(gridAssignment[0, y]);
    validTargets.Remove(gridAssignment[width - 1, y]);
}


Console.WriteLine($"{validTargets.Count} different non-infinite zones");

int maxArea = 0;
int maxTarget = -1;

foreach (int target in validTargets)
{
    int area = 0;
    for (int x = 1; x < width - 1; x++)
    {
        for (int y = 1; y < height - 1; y++)
        {
            if (gridAssignment[x, y] == target)
            {
                area++;
            }
        }
    }

    if (area > maxArea)
    {
        maxArea = area;
        maxTarget = target;
    }
}


Console.WriteLine();
Console.WriteLine($"Zone {maxTarget} has an area of {maxArea}");
Console.WriteLine();


int zone2Count = 0;
for (int x = 0; x < width; x++)
{
    for (int y = 0; y < height; y++)
    {
        if (totalGridDistance[x, y] < 10_000)
        {
            zone2Count++;
        }
    }
}

Console.WriteLine("Star 2");
Console.WriteLine();
Console.WriteLine($"{zone2Count} spaces with distance less than 10,000.");
Console.WriteLine();

Console.ReadKey();

static Point2D Parse(string serialized)
{
    int[] parsed = serialized.Split(',').Select(int.Parse).ToArray();

    return new Point2D(parsed[0], parsed[1]);
}