const string inputFile = @"../../../../input02.txt";


Console.WriteLine("Day 02 - Inventory Management System");
Console.WriteLine("Star 1");
Console.WriteLine();

List<string> values = System.IO.File.ReadAllLines(inputFile).ToList();

int twos = 0;
int threes = 0;

foreach (string id in values)
{
    (bool two, bool three) = Characterize(id);

    if (two)
    {
        twos++;
    }

    if (three)
    {
        threes++;
    }
}


Console.WriteLine($"Box Checksum: {twos * threes}");
Console.WriteLine();

Console.WriteLine("Star 2");
Console.WriteLine();

for (int i = 0; i < values.Count - 1; i++)
{
    for (int j = i + 1; j < values.Count; j++)
    {
        string idA = values[i];
        string idB = values[j];

        if (CompareStrings(idA, idB) != null)
        {
            Console.WriteLine($"Letters shared by closest matches: {new string(CompareStrings(idA, idB))}");
        }
    }
}

Console.WriteLine();
Console.ReadKey();


/// <summary>
/// Returns whether a string has pairs, triplets, or both.
/// </summary>
static (bool two, bool three) Characterize(string id)
{
    Dictionary<char, int> characters = new Dictionary<char, int>();

    foreach (char c in id)
    {
        if (characters.ContainsKey(c))
        {
            characters[c] = characters[c] + 1;
        }
        else
        {
            characters[c] = 1;
        }
    }

    bool two = false;
    bool three = false;
    foreach (int i in characters.Values)
    {
        switch (i)
        {
            case 2:
                two = true;
                break;
            case 3:
                three = true;
                break;
            default:
                break;
        }
    }

    return (two, three);
}

/// <summary>
/// Returns a char[] of the characters shared by the two input strings only if there is
/// 1 or fewer differences between them, otherwise it returns null
/// </summary>
static char[] CompareStrings(string idA, string idB)
{
    int errorCount = 0;
    char[] output = new char[idA.Length - 1];
    for (int i = 0; i < idA.Length; i++)
    {
        if (idA[i] != idB[i])
        {
            if (++errorCount >= 2)
            {
                return null;
            }
        }
        else
        {
            output[i - errorCount] = idA[i];
        }
    }


    return output;
}