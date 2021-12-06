
const string inputFile = @"../../../../input05.txt";


Console.WriteLine("Day 05 - Alchemical Reduction");
Console.WriteLine("Star 1");
Console.WriteLine();

LinkedList<char> chars = new LinkedList<char>();
Dictionary<char, char> translator = new Dictionary<char, char>();

for (int i = 0; i < 26; i++)
{
    char a = (char)('a' + i);
    char A = (char)('A' + i);
    translator.Add(a, A);
    translator.Add(A, a);
}
using (FileStream stream = File.OpenRead(inputFile))
using (StreamReader reader = new StreamReader(stream))
{
    while (!reader.EndOfStream)
    {
        char newChar = (char)reader.Read();
        if (chars.Count == 0)
        {
            chars.AddLast(newChar);
            continue;
        }

        if (chars.Last.Value == translator[newChar])
        {
            //Annihilate
            chars.RemoveLast();
            continue;
        }

        chars.AddLast(newChar);
    }
}


Console.WriteLine($"Number of remaining Characters: {chars.Count}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

//Find the shortest segment you can get after removing all of one char and reacting it

int length = chars.Count;
for (int i = 0; i < 26; i++)
{
    char removal = (char)('a' + i);
    LinkedList<char> newList = new LinkedList<char>();
    LinkedListNode<char> node = chars.First;
    do
    {
        char newChar = node.Value;
        if (newChar == removal || newChar == translator[removal])
        {
            continue;
        }

        if (newList.Count == 0)
        {
            newList.AddLast(newChar);
            continue;
        }


        if (newList.Last.Value == translator[newChar])
        {
            //Annihilate
            newList.RemoveLast();
            continue;
        }

        newList.AddLast(newChar);
    }
    while ((node = node.Next) != null);

    if (newList.Count < length)
    {
        length = newList.Count;
    }

    Console.WriteLine($"Removing {char.ToUpper(removal)}: {newList.Count}");

}

Console.WriteLine($"Shortest Result: {length}");
Console.WriteLine();

Console.ReadKey();
