using AoCTools;

const string inputFile = @"../../../../input23.txt";

Console.WriteLine("Day 23 - Experimental Emergency Teleportation");
Console.WriteLine("Star 1");
Console.WriteLine();

List<Nanobot> nanobots = new List<Nanobot>();

Nanobot biggestRadius = null;

foreach (string line in System.IO.File.ReadAllLines(inputFile))
{
    Nanobot newNanoBot = new Nanobot(line);
    nanobots.Add(newNanoBot);

    if (biggestRadius == null || newNanoBot.r > biggestRadius.r)
    {
        biggestRadius = newNanoBot;
    }
}

int interiorCount = 0;
foreach (Nanobot bot in nanobots)
{
    if (biggestRadius.IsWithinRange(bot))
    {
        interiorCount++;
    }
}


Console.WriteLine($"There are {interiorCount} bots inside radius.");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

//Find the coordinate in range of most


//Let's try a binary search
//Set up bounds in the laziest way
LongPoint3D min = new LongPoint3D(
    x: nanobots.Min(x => x.position.x),
    y: nanobots.Min(x => x.position.y),
    z: nanobots.Min(x => x.position.z));

LongPoint3D max = new LongPoint3D(
    x: nanobots.Max(x => x.position.x) + 1,
    y: nanobots.Max(x => x.position.y) + 1,
    z: nanobots.Max(x => x.position.z) + 1);

List<Box> bestBoxes = new List<Box>();
List<Box> testBoxes = new List<Box>
{
    new Box(min, max)
};

while (true)
{
    //Console.WriteLine("###################################");
    //Console.WriteLine();
    //Console.WriteLine("Staring Iteration");
    //Console.WriteLine();

    int bestBoxCount = -1;
    bestBoxes.Clear();

    foreach (Box testBox in testBoxes)
    {
        foreach (Box box in testBox.SliceBox())
        {
            int count = CountBotsBox(box);
            //Console.WriteLine($"Box {box}, Count {count}");
            if (count > bestBoxCount)
            {
                bestBoxes.Clear();
                bestBoxes.Add(box);
                bestBoxCount = count;
            }
            else if (count == bestBoxCount)
            {
                bestBoxes.Add(box);
            }
        }
    }

    //Console.WriteLine();
    //Console.WriteLine($"Best Box Bot Count: {bestBoxCount}");
    //foreach (Box bestBox in bestBoxes)
    //{
    //    Console.WriteLine($"    {bestBox}");
    //}
    //Console.WriteLine();

    testBoxes.Clear();
    testBoxes.AddRange(bestBoxes);

    Box testingBox = testBoxes[0];
    if ((testingBox.min.x + 1 == testingBox.max.x) && (testingBox.min.y + 1 == testingBox.max.y) && (testingBox.min.z + 1 == testingBox.max.z))
    {
        break;
    }
}

Console.WriteLine($"Found {testBoxes.Count} final boxes");

min = testBoxes.Select(x => x.min).OrderBy(x => x.TaxiCabLength).First();

Console.WriteLine($"Best Position: {min}, Bots: {CountBotsPoint(min)}, Distance from Origin: {min.TaxiCabLength}");

Console.WriteLine();
Console.ReadKey();

int CountBotsPoint(in LongPoint3D position)
{
    int count = 0;

    foreach (Nanobot bot in nanobots)
    {
        if (bot.IsWithinRange(position))
        {
            count++;
        }
    }

    return count;
}

int CountBotsBox(in Box box)
{
    int count = 0;

    foreach (Nanobot bot in nanobots)
    {
        if (bot.IsWithinRange(box))
        {
            count++;
        }
    }

    return count;
}

readonly struct Box
{
    public readonly LongPoint3D min;
    public readonly LongPoint3D max;

    public static readonly LongPoint3D Ones = new LongPoint3D(1, 1, 1);

    public Box(in LongPoint3D min, in LongPoint3D max)
    {
        this.min = min;
        this.max = max;
    }

    public bool Contains(in LongPoint3D point) =>
        (point.x >= min.x && point.x < max.x) &&
        (point.y >= min.y && point.y < max.y) &&
        (point.z >= min.z && point.z < max.z);

    public long DistanceFromPoint(in LongPoint3D point)
    {
        if (Contains(point))
        {
            return 0;
        }

        return (AoCMath.Clamp(point, min, max - Ones) - point).TaxiCabLength;
    }

    public long DistanceFromOrigin => DistanceFromPoint(LongPoint3D.Zero);

    public override string ToString() => $"[{min} {max}]";

    public IEnumerable<Box> SliceBox()
    {
        LongPoint3D deltas = max - min;

        LongPoint3D sliceMin = Ones;
        LongPoint3D sliceMax = Ones * 4;

        LongPoint3D slices = AoCMath.Clamp(deltas, sliceMin, sliceMax);

        LongPoint3D stepSize = ElementWiseDivide(deltas + slices - Ones, slices);

        for (long xSlice = 0; xSlice < slices.x; xSlice++)
        {
            for (long ySlice = 0; ySlice < slices.y; ySlice++)
            {
                for (long zSlice = 0; zSlice < slices.z; zSlice++)
                {
                    LongPoint3D slice = new LongPoint3D(xSlice, ySlice, zSlice);

                    yield return new Box(
                        min: min + slice * stepSize,
                        max: min + (slice + Ones) * stepSize);
                }
            }
        }
    }

    private LongPoint3D ElementWiseDivide(in LongPoint3D numerator, in LongPoint3D denominator) => new LongPoint3D(
        x: numerator.x / denominator.x,
        y: numerator.y / denominator.y,
        z: numerator.z / denominator.z);
}

class Nanobot
{
    public readonly long r;

    public readonly LongPoint3D position;

    public Nanobot(string line)
    {
        int endOfPos = line.IndexOf('>');
        long[] values = line[5..endOfPos].Split(',').Select(long.Parse).ToArray();

        position = new LongPoint3D(
            x: values[0],
            y: values[1],
            z: values[2]);

        int startOfRadius = line.IndexOf('r') + 2;

        r = long.Parse(line[startOfRadius..]);
    }

    public bool IsWithinRange(Nanobot source) => (position - source.position).TaxiCabLength <= r;
    public bool IsWithinRange(in Box box) => box.DistanceFromPoint(position) <= r;
    public bool IsWithinRange(LongPoint3D pos) => (pos - position).TaxiCabLength <= r;
}