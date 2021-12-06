const string inputFile = @"../../../../input01.txt";


Console.WriteLine("Day 01 - Chronal Calibration");
Console.WriteLine("Star 1");
Console.WriteLine();

List<long> values = File.ReadAllLines(inputFile).Select(long.Parse).ToList();

long cumulativeValue = values.Sum();

Console.WriteLine($"The cumulative value: {cumulativeValue}");


Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

long cyclicalValue = 0;
HashSet<long> visited = new HashSet<long>();


bool done = false;

while (!done)
{
    foreach (long adj in values)
    {
        cyclicalValue += adj;
        if (!visited.Add(cyclicalValue))
        {
            done = true;
            break;
        }
    }
}

Console.WriteLine($"The first repeated value: {cyclicalValue}");
Console.WriteLine();
Console.ReadKey();