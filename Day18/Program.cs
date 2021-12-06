const string inputFile = @"../../../../input18.txt";

Console.WriteLine("Day 18 - Settlers of The North Pole");
Console.WriteLine("Star 1");
Console.WriteLine();

const char Open = '.';
const char Trees = '|';
const char Lumberyard = '#';

string[] inputFileLines = System.IO.File.ReadAllLines(inputFile);

int size = inputFileLines.Length;

char[,] initialCondition = new char[size, size];
char[,] areaA = new char[size, size];
char[,] areaB = new char[size, size];

char[,] tempAreaC = new char[size, size];

for (int j = 0; j < size; j++)
{
    for (int i = 0; i < size; i++)
    {
        areaA[i, j] = inputFileLines[j][i];
        initialCondition[i, j] = areaA[i, j];
    }
}

for (int gen = 0; gen < 10; gen++)
{
    AdvanceMap();
}


Console.WriteLine();
Console.WriteLine($"Resource Value: {CalculateResourceValue()}");
Console.WriteLine();

Console.WriteLine("Star 2");
Console.WriteLine();

//Reset to initial condition
for (int y = 0; y < size; y++)
{
    for (int x = 0; x < size; x++)
    {
        areaA[x, y] = initialCondition[x, y];
    }
}

HashSet<int> seenStates = new HashSet<int>();
int firstClearIndex = -1;
int cyclePeriod = -1;
int cycleStartHash = -1;

for (int gen = 0; gen < 1_000_000_000; gen++)
{
    AdvanceMap();

    if (!seenStates.Add(EncodeMap()))
    {
        if (firstClearIndex == -1)
        {
            firstClearIndex = gen;
            cycleStartHash = EncodeMap();
            Console.WriteLine($"First cycle detected at {gen}. Clearing.");

            seenStates.Clear();
            seenStates.Add(cycleStartHash);
        }
        else
        {
            cyclePeriod = gen - firstClearIndex;
            Console.WriteLine($"Second cycle completed. Took from {firstClearIndex} to {gen}.  Cycle Period: {cyclePeriod}");

            break;
        }
    }
}


int targetMinute = 1_000_000_000;

int targetPhase = (targetMinute - firstClearIndex - 1) % cyclePeriod;
//We are currently at the start of the cycle, so we only need to advance by the Phase

for (int gen = 0; gen < targetPhase; gen++)
{
    AdvanceMap();
}

Console.WriteLine($"Resource Value: {CalculateResourceValue()}");
Console.WriteLine();

Console.ReadKey();


int EncodeMap()
{
    const int SamplesPerBatch = 5;
    int samplesInBatch = 0;
    int accumulatedSamples = 0;

    int hash = (size * size) / SamplesPerBatch;

    for (int y = 0; y < size; y++)
    {
        for (int x = 0; x < size; x++)
        {
            accumulatedSamples = 3 * accumulatedSamples + GetEncodedValue(x, y);

            if (++samplesInBatch == SamplesPerBatch)
            {
                hash = unchecked(hash * 314159 + accumulatedSamples);
                samplesInBatch = 0;
                accumulatedSamples = 0;
            }
        }
    }

    if (samplesInBatch > 0)
    {
        hash = unchecked(hash * 314159 + accumulatedSamples);
    }

    return hash;
}

int GetEncodedValue(int x, int y)
{
    switch (areaA[x, y])
    {
        case Open: return 0;
        case Trees: return 1;
        case Lumberyard: return 2;
        default: throw new Exception();
    }
}

void CopyMap(char[,] destination)
{
    for (int y = 0; y < size; y++)
    {
        for (int x = 0; x < size; x++)
        {
            destination[x, y] = areaA[x, y];
        }
    }
}

bool CompareMaps(char[,] a, char[,] b)
{
    for (int y = 0; y < size; y++)
    {
        for (int x = 0; x < size; x++)
        {
            if (a[x, y] != b[x, y])
            {
                return false;
            }
        }
    }

    return true;
}

void PrintMap()
{
    Console.Write("\n\n");

    for (int y = 0; y < size; y++)
    {
        for (int x = 0; x < size; x++)
        {
            Console.Write(areaA[x, y]);
        }

        Console.Write('\n');
    }
}

void AdvanceMap()
{
    for (int x = 0; x < size; x++)
    {
        for (int y = 0; y < size; y++)
        {
            areaB[x, y] = areaA[x, y];

            switch (areaA[x, y])
            {
                case Open:
                    if (CountNeighbors(x, y, Trees) >= 3)
                    {
                        areaB[x, y] = Trees;
                    }
                    break;
                case Trees:
                    if (CountNeighbors(x, y, Lumberyard) >= 3)
                    {
                        areaB[x, y] = Lumberyard;
                    }
                    break;
                case Lumberyard:
                    if (CountNeighbors(x, y, Trees) == 0 || CountNeighbors(x, y, Lumberyard) == 0)
                    {
                        areaB[x, y] = Open;
                    }
                    break;
                default: throw new Exception();
            }
        }
    }

    (areaA, areaB) = (areaB, areaA);
}

int CountNeighbors(int x0, int y0, char field)
{
    int hits = 0;

    for (int x = -1; x <= 1; x++)
    {
        for (int y = -1; y <= 1; y++)
        {
            if (x == 0 && y == 0)
            {
                //Skip the center tile
                continue;
            }

            if (x0 + x < 0 || x0 + x >= size)
            {
                continue;
            }

            if (y0 + y < 0 || y0 + y >= size)
            {
                continue;
            }

            if (areaA[x0 + x, y0 + y] == field)
            {
                hits++;
            }
        }
    }

    return hits;
}

int CalculateResourceValue()
{
    int lumberyards = 0;
    int woodedAreas = 0;

    for (int y = 0; y < size; y++)
    {
        for (int x = 0; x < size; x++)
        {
            switch (areaA[x, y])
            {
                case Lumberyard:
                    lumberyards++;
                    break;
                case Open:
                    break;
                case Trees:
                    woodedAreas++;
                    break;
            }
        }
    }

    return lumberyards * woodedAreas;
}
