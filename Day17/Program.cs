const string inputFile = @"../../../../input17.txt";

Console.WriteLine("Day 17 - Reservoir Research");
Console.WriteLine("Star 1");
Console.WriteLine();

char[,] field;
int xOffset;
int depth;
Queue<Flow> sourceQueue;

List<Range> ranges = new List<Range>();

foreach (string line in System.IO.File.ReadAllLines(inputFile))
{
    string[] splitLine = line.Split(',');

    if (splitLine[0][0] == 'x')
    {
        ranges.Add(new YRange(
            x: splitLine[0].Substring(2),
            y: splitLine[1].Substring(3)));
    }
    else
    {
        ranges.Add(new XRange(
            x: splitLine[1].Substring(3),
            y: splitLine[0].Substring(2)));
    }
}


depth = 0;

int minX = ranges[0].MinX;
int maxX = ranges[0].MaxX;
int minY = ranges[0].MinY;

foreach (Range range in ranges)
{
    if (range.MinY < minY)
    {
        minY = range.MinY;
    }

    if (range.MaxY > depth)
    {
        depth = range.MaxY;
    }

    if (range.MinX < minX)
    {
        minX = range.MinX;
    }

    if (range.MaxX > maxX)
    {
        maxX = range.MaxX;
    }
}

//Move the boundaries out two more
minX -= 2;
maxX -= 2;

xOffset = minX;

int w = maxX - minX + 3;
int h = depth + 1;

field = new char[w, h];

for (int y = 0; y < h; y++)
{
    for (int x = 0; x < w; x++)
    {
        field[x, y] = Flow.Empty;
    }
}

field[500 - minX, 0] = Flow.Source;

foreach (Range range in ranges)
{
    range.Apply(field, xOffset);
}

//Flow travels downward until it is over an obstacle, and then sideways until it is not.

//When flowing Down:
//If you hit a Flow (|), stop.
//If you hit Clay (#) or Still Water (~), propagate Left and Right

//When flowing Sideways:
//Flow left until termination, then flow right
//If you hit a Flow (|), stop.
//If you hit Clay when going both directions, convert to still water and jump to prior flow as new source to propagate.


//Console.ReadKey();

//Console.WriteLine();
//for (int y = 0; y < h; y++)
//{
//    for (int x = 0; x < w; x++)
//    {
//        Console.Write(field[x, y]);
//    }
//    Console.Write('\n');
//}



sourceQueue = new Queue<Flow>();
sourceQueue.Enqueue(new DownFlow(500 - minX, 1));
field[500 - minX, 1] = Flow.WaterFlow;


while (sourceQueue.Count > 0)
{
    sourceQueue.Dequeue().ExecuteFlow(field, sourceQueue, depth);
}


Console.WriteLine("Press a key to begin rendering.");
Console.ReadKey();
Console.WriteLine();
for (int y = 0; y < h; y++)
{
    for (int x = 0; x < w; x++)
    {
        switch (field[x, y])
        {
            case Flow.Steady:
            case Flow.Source:
            case Flow.WaterFlow:
                Console.BackgroundColor = ConsoleColor.Blue;
                break;
            case Flow.Clay:
                Console.BackgroundColor = ConsoleColor.Gray;
                break;
            case Flow.Empty:
                Console.BackgroundColor = ConsoleColor.Black;
                break;
            default: throw new Exception();
        }

        Console.Write(field[x, y]);
    }

    Console.BackgroundColor = ConsoleColor.Black;
    Console.Write('\n');
}


int flowCount = 0;
int steadyCount = 0;
for (int y = minY; y < h; y++)
{
    for (int x = 0; x < w; x++)
    {
        switch (field[x, y])
        {
            case Flow.Steady:
                steadyCount++;
                break;
            case Flow.WaterFlow:
                flowCount++;
                break;
            default:
                break;
        }
    }
}

Console.WriteLine($"Tiles with water: {steadyCount + flowCount}");
Console.WriteLine();

Console.WriteLine("Star 2");
Console.WriteLine();
Console.WriteLine($"Tiles with steady water: {steadyCount}");

Console.ReadKey();

abstract class Flow
{
    public const char Empty = ' ';
    public const char Source = '+';
    public const char WaterFlow = '|';
    public const char Steady = '~';
    public const char Clay = '#';

    public abstract void ExecuteFlow(char[,] field, Queue<Flow> sourceQueue, int maxDepth);
}

class DownFlow : Flow
{
    private readonly int x;
    private int y;

    public DownFlow(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override void ExecuteFlow(char[,] field, Queue<Flow> sourceQueue, int maxDepth)
    {
        if (field[x, y] == Steady)
        {
            //Watch out for starting a dead flow in steady water
            return;
        }

        while (y < maxDepth && field[x, y + 1] == Empty)
        {
            ++y;

            field[x, y] = WaterFlow;
        }

        if (y == maxDepth)
        {
            //Don't propagate below bottom - duh!
            return;
        }

        switch (field[x, y + 1])
        {
            case WaterFlow: return;

            case Clay:
            case Steady:
                //Propagate Both directions
                sourceQueue.Enqueue(new AcrossFlow(x, y));

                break;
            default: throw new Exception();
        }
    }
}

class AcrossFlow : Flow
{
    private readonly int x;
    private readonly int y;

    public AcrossFlow(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override void ExecuteFlow(char[,] field, Queue<Flow> sourceQueue, int maxDepth)
    {
        int leftX = x;
        int rightX = x;

        if (field[x, y] == Steady)
        {
            //Watch out for starting a dead flow in steady water
            return;
        }

        while (field[leftX - 1, y] == Empty || field[leftX - 1, y] == WaterFlow)
        {
            --leftX;

            field[leftX, y] = WaterFlow;
            if (field[leftX, y + 1] == Empty)
            {
                sourceQueue.Enqueue(new DownFlow(leftX, y));
                break;
            }
            else if (field[leftX, y + 1] == WaterFlow)
            {
                break;
            }
        }

        while (field[rightX + 1, y] == Empty || field[rightX + 1, y] == WaterFlow)
        {
            ++rightX;

            field[rightX, y] = WaterFlow;
            if (field[rightX, y + 1] == Empty)
            {
                sourceQueue.Enqueue(new DownFlow(rightX, y));
                break;
            }
            else if (field[rightX, y + 1] == WaterFlow)
            {
                break;
            }
        }

        if (field[leftX - 1, y] == Clay && field[rightX + 1, y] == Clay)
        {
            //Test bottoms
            bool solidBottom = true;
            for (int x2 = leftX; x2 <= rightX; x2++)
            {
                solidBottom &= (field[x2, y + 1] == Clay || field[x2, y + 1] == Steady);
            }

            if (solidBottom)
            {
                for (int x2 = leftX; x2 <= rightX; x2++)
                {
                    field[x2, y] = Steady;
                    if (field[x2, y - 1] == WaterFlow)
                    {
                        sourceQueue.Enqueue(new AcrossFlow(x2, y - 1));
                    }
                }
            }
        }
    }
}

abstract class Range
{
    public abstract int MinX { get; }
    public abstract int MaxX { get; }
    public abstract int MinY { get; }
    public abstract int MaxY { get; }

    public abstract void Apply(char[,] field, int xOffset);
}

class XRange : Range
{
    public readonly int x0;
    public readonly int x1;
    public readonly int y;

    public override int MinX => x0;
    public override int MaxX => x1;
    public override int MinY => y;
    public override int MaxY => y;

    public XRange(string x, string y)
    {
        x0 = int.Parse(x.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0]);
        x1 = int.Parse(x.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1]);

        this.y = int.Parse(y);
    }

    public override void Apply(char[,] field, int xOffset)
    {
        for (int x = x0; x <= x1; x++)
        {
            field[x - xOffset, y] = Flow.Clay;
        }
    }
}

class YRange : Range
{
    public readonly int y0;
    public readonly int y1;
    public readonly int x;

    public override int MinX => x;
    public override int MaxX => x;
    public override int MinY => y0;
    public override int MaxY => y1;

    public YRange(string x, string y)
    {
        y0 = int.Parse(y.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0]);
        y1 = int.Parse(y.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1]);

        this.x = int.Parse(x);
    }

    public override void Apply(char[,] field, int xOffset)
    {
        for (int y = y0; y <= y1; y++)
        {
            field[x - xOffset, y] = Flow.Clay;
        }
    }
}