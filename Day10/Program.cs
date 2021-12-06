using AoCTools;

const string inputFile = @"../../../../input10.txt";

Console.WriteLine("Day 10 - The Stars Align");

List<Star> stars = new List<Star>();


using (FileStream stream = File.OpenRead(inputFile))
using (StreamReader reader = new StreamReader(stream))
{
    while (!reader.EndOfStream)
    {
        stars.Add(new Star(reader.ReadLine()));
    }
}

bool ready = false;
int time = 10000;

foreach (Star star in stars)
{
    star.ProgressBy(time);
}

Point2D max;
Point2D min;
Star firstStar = stars[0];

while (!ready)
{
    time++;
    ready = true;

    firstStar.Progress();
    max = firstStar.position;
    min = firstStar.position;

    foreach (Star star in stars.Skip(1))
    {
        star.Progress();

        if (ready)
        {
            max = AoCMath.Max(max, star.position);
            min = AoCMath.Min(min, star.position);

            if ((max - min).TaxiCabLength > 300)
            {
                ready = false;
            }
        }
    }

    if (time > 100000)
    {
        throw new Exception("Didn't end. Constraints too tight.");
    }
}

HashSet<Point2D> currentStarPositions = new HashSet<Point2D>();
ready = false;

while (!ready)
{
    time++;
    currentStarPositions.Clear();
    int adjacentCounts = 0;

    foreach (Star c in stars)
    {
        c.Progress();

        currentStarPositions.Add(c.position);
        foreach (Point2D adjacentPosition in c.position.GetAdjacent())
        {
            if (currentStarPositions.Contains(adjacentPosition))
            {
                adjacentCounts++;
            }
        }

        if (adjacentCounts > 300)
        {
            //We may have found it?
            ready = true;
        }
    }

    if (time > 100000)
    {
        throw new Exception("Didn't end. Constraints too tight.");
    }
}

Console.WriteLine($"Time: {time}");

Point2D gridMin = currentStarPositions.MinCoordinate();
Point2D gridMax = currentStarPositions.MaxCoordinate();
if ((gridMax - gridMin).TaxiCabLength > 600)
{
    throw new Exception("Bad end. Constraints too loose.");
}

Point2D charOffset = new Point2D(5, 5);

foreach (Point2D star in currentStarPositions)
{
    Point2D charPosition = charOffset + star - gridMin;
    Console.SetCursorPosition(charPosition.x, charPosition.y);
    Console.Write('#');
}

Console.WriteLine();
Console.ReadKey();

class Star
{
    public Point2D position;
    public readonly Point2D velocity;

    public Star(string line)
    {
        position = new Point2D(
            x: int.Parse(line.Substring(10, 6)),
            y: int.Parse(line.Substring(18, 6)));

        velocity = new Point2D(
            x: int.Parse(line.Substring(36, 2)),
            y: int.Parse(line.Substring(39, 3)));
    }

    public void Progress()
    {
        position += velocity;
    }

    public void ProgressBy(int steps)
    {
        position += steps * velocity;
    }
}