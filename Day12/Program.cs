const string inputFile = @"../../../../input12.txt";

Console.WriteLine("Day 12 - Subterranean Sustainability");
Console.WriteLine("Star 1");
Console.WriteLine();

List<bool> initialState = null;
List<bool> currentState = null;
HashSet<string> reproducingPatterns = new HashSet<string>();

foreach (string line in File.ReadAllLines(inputFile))
{
    if (string.IsNullOrWhiteSpace(line))
    {
        //Header Break
        continue;
    }
    else if (line.StartsWith("initial state: "))
    {
        //Header
        initialState = line.Substring(15).Select(x => x == '#').ToList();
        currentState = new List<bool>(initialState);
    }
    else
    {
        //Normal Line
        if (line.EndsWith("#"))
        {
            reproducingPatterns.Add(line.Substring(0, 5));
        }

        //Skip dead - don't care
    }
}

int firstNum = 0;

for (int gen = 0; gen < 20; gen++)
{
    //Prepare
    if (currentState[0] || currentState[1] || currentState[2] || currentState[3])
    {
        currentState.Insert(0, false);
        currentState.Insert(0, false);
        currentState.Insert(0, false);
        currentState.Insert(0, false);
        firstNum -= 4;
    }

    if (currentState[currentState.Count - 1] ||
        currentState[currentState.Count - 2] ||
        currentState[currentState.Count - 3] ||
        currentState[currentState.Count - 4])
    {
        currentState.Add(false);
        currentState.Add(false);
        currentState.Add(false);
        currentState.Add(false);
    }

    bool[] newList = new bool[currentState.Count];

    for (int i = 2; i < newList.Length - 2; i++)
    {
        newList[i] = Transform(currentState, reproducingPatterns, i);
    }

    currentState = new List<bool>(newList);


}
int lastCount = 0;

int totalCount = 0;
for (int i = 0; i < currentState.Count; i++)
{
    if (currentState[i])
    {
        totalCount += i + firstNum;
    }
}

Console.WriteLine($"Sum of plant numbers after 20 gens: {totalCount}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

long benchmark = 20;
long lastBenchmark = 0;
long diffPerGen = 0;
long offset = 0;
firstNum = 0;

currentState = new List<bool>(initialState);

for (long gen = 0; gen < 10000; gen++)
{
    //Cull
    int firstIndex = currentState.IndexOf(true);
    int lastIndex = currentState.LastIndexOf(true);

    //Handle End
    if (currentState.Count - 1 - lastIndex < 4)
    {
        int create = 4 - (currentState.Count - 1 - lastIndex);
        for (int i = 0; i < create; i++)
        {
            currentState.Add(false);
        }
    }
    else if (currentState.Count - 1 - lastIndex > 4)
    {
        currentState.RemoveRange(lastIndex + 1, currentState.Count - 1 - lastIndex - 4);
    }

    if (firstIndex < 4)
    {
        for (int i = 0; i < 4 - firstIndex; i++)
        {
            currentState.Insert(0, false);
            firstNum--;
        }
    }
    else if (firstIndex > 4)
    {
        currentState.RemoveRange(0, firstIndex - 4);
        firstNum += firstIndex - 4;
    }

    if (gen == benchmark)
    {
        int tempCount = 0;
        for (int i = 0; i < currentState.Count; i++)
        {
            if (currentState[i])
            {
                tempCount += i + firstNum;
            }
        }

        diffPerGen = (long)(((double)tempCount - lastCount) / (benchmark - lastBenchmark));
        offset = (long)(tempCount - gen * diffPerGen);
        Console.WriteLine($"{gen,8}: {new string(currentState.Select(x => x ? '#' : '.').ToArray()),60} \t Total: {tempCount,8} \t DiffPerGen: {diffPerGen,8} \t Offset: {offset,8}");

        lastBenchmark = benchmark;
        lastCount = tempCount;

        benchmark *= 2;
    }

    bool[] newList = new bool[currentState.Count];

    for (int i = 2; i < newList.Length - 2; i++)
    {
        newList[i] = Transform(currentState, reproducingPatterns, i);
    }

    currentState = new List<bool>(newList);
}

//After it stabilizes, the whole pattern moves right 1 unit per unit time.
//Lagging offset steps behind the generation.
long bigTotalCount = offset + 50000000000 * diffPerGen;


Console.WriteLine($"Sum of plant numbers after 50000000000 gens: {bigTotalCount}");

Console.ReadKey();

static bool Transform(List<bool> state, HashSet<string> reproducingPatterns, int currentIndex) =>
    reproducingPatterns.Contains(GetPattern(state, currentIndex));

static string GetPattern(List<bool> state, int currentIndex) =>
    new string(state.GetRange(currentIndex - 2, 5).Select(x => x ? '#' : '.').ToArray());