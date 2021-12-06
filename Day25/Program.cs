using AoCTools;

const string inputFile = @"../../../../input25.txt";


Console.WriteLine("Day 25 - Four-Dimensional Adventure");
Console.WriteLine("Star 1");
Console.WriteLine();

Stack<LongPoint4D> coords = new Stack<LongPoint4D>(File.ReadAllLines(inputFile).Select(Parse));

List<Constellation> constellations = new List<Constellation>();

constellations.Add(new Constellation(coords.Pop()));
List<Constellation> matches = new List<Constellation>();

while (coords.Count > 0)
{
    LongPoint4D coord = coords.Pop();
    matches.Clear();

    foreach (Constellation constellation in constellations)
    {
        if (constellation.TestStar(coord))
        {
            matches.Add(constellation);
        }
    }

    if (matches.Count >= 1)
    {
        matches[0].Add(coord);
        for (int i = 1; i < matches.Count; i++)
        {
            matches[0].Merge(matches[i]);
            constellations.Remove(matches[i]);
        }
    }
    else
    {
        constellations.Add(new Constellation(coord));
    }
}

Console.WriteLine($"Number of constellations: {constellations.Count}");
Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();
Console.ReadKey();

LongPoint4D Parse(string input)
{
    string[] split = input.Split(',');
    return new LongPoint4D(
        x: int.Parse(split[0]),
        y: int.Parse(split[1]),
        z: int.Parse(split[2]),
        w: int.Parse(split[3]));
}

class Constellation
{
    public List<LongPoint4D> stars = new List<LongPoint4D>();

    public Constellation(LongPoint4D star)
    {
        stars.Add(star);
    }

    public void Add(LongPoint4D star)
    {
        stars.Add(star);
    }

    public void Merge(Constellation other)
    {
        stars.AddRange(other.stars);
    }

    public bool TestStar(LongPoint4D testStar)
    {
        foreach (LongPoint4D star in stars)
        {
            if ((star - testStar).TaxiCabLength <= 3)
            {
                return true;
            }
        }
        return false;
    }
}

