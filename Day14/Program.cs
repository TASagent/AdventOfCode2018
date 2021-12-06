const string inputFile = @"../../../../input14.txt";

Console.WriteLine("Day 14 - Chocolate Charts");
Console.WriteLine("Star 1");
Console.WriteLine();

string inputText = File.ReadAllText(inputFile);
int inputValue = int.Parse(inputText);
int targetValue = inputValue + 10;

List<int> targetSequenceList = new List<int>();

foreach (char c in inputText.Reverse())
{
    targetSequenceList.Add(int.Parse(c.ToString()));
}


//Reversed inputValue
int[] targetSequence = targetSequenceList.ToArray();

//New recipes are made by adding together the current recipe values.
//The recipes are values 0-9
//If the sum is greater than 9, then 2 recipes are created and appended to the list

//Then each elf walks forward 1 + their current recipe value

//The starting configuration is (3)[7]

List<int> recipes = new List<int>(targetValue + 2) { 3, 7 };

//Elf indices
int elfA = 0;
int elfB = 1;

while (recipes.Count < targetValue)
{
    int newVal = recipes[elfA] + recipes[elfB];
    if (newVal > 9)
    {
        recipes.Add(1);
        recipes.Add(newVal % 10);
    }
    else
    {
        recipes.Add(newVal);
    }

    elfA += recipes[elfA] + 1;
    elfB += recipes[elfB] + 1;

    elfA %= recipes.Count;
    elfB %= recipes.Count;
}

Console.WriteLine($"{targetValue} recipes after {inputValue}: {string.Join("", recipes.GetRange(inputValue, 10).Select(x => x.ToString()))}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

recipes.Clear();
recipes.Add(3);
recipes.Add(7);

//Elf indices
elfA = 0;
elfB = 1;

while (true)
{
    int newVal = recipes[elfA] + recipes[elfB];
    if (newVal > 9)
    {
        if (Add(1))
        {
            break;
        }

        if (Add(newVal % 10))
        {
            break;
        }

    }
    else
    {
        if (Add(newVal))
        {
            break;
        }
    }

    elfA += recipes[elfA] + 1;
    elfB += recipes[elfB] + 1;

    elfA %= recipes.Count;
    elfB %= recipes.Count;
}

Console.WriteLine($"Recipe {inputValue} appears after {recipes.Count - targetSequence.Length} others");
Console.WriteLine();

Console.ReadKey();



bool Add(int value)
{
    recipes.Add(value);

    if (recipes.Count >= 6)
    {
        for (int i = 0; i < targetSequence.Length; i++)
        {
            if (recipes[recipes.Count - 1 - i] != targetSequence[i])
            {
                return false;
            }
        }

        return true;
    }

    return false;
}
