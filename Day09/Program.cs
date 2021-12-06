const long players = 411;
const long lastMarble = 72059;

Console.WriteLine("Day 09 - Marble Mania");
Console.WriteLine("Star 1");
Console.WriteLine();

long[] scores = GetScores(players, lastMarble);

Console.WriteLine($"High Score: {scores.Max()}");
Console.WriteLine();

Console.WriteLine("Star 2");
Console.WriteLine();

long[] finalScores = GetScores(players, 100 * lastMarble);

Console.WriteLine($"High Score: {finalScores.Max()}");
Console.WriteLine();

Console.ReadKey();


static long[] GetScores(long players, long lastMarble)
{
    LinkedList<long> marbles = new LinkedList<long>();
    marbles.AddFirst(0);
    LinkedListNode<long> currentMarble = marbles.First;
    long[] scores = new long[players];

    long nextValue = 1;
    bool foundTarget = false;

    while (!foundTarget)
    {
        if (nextValue % 23 == 0)
        {
            //Trigger special Action

            //Marble 6 spaces counter-clockwise is the new current marble
            currentMarble = currentMarble.PrevCir().PrevCir().PrevCir().PrevCir().PrevCir().PrevCir();
            //Prior marble is removed and its value is added to the score
            scores[(nextValue - 1) % players] += nextValue + currentMarble.PrevCir().Value;
            marbles.Remove(currentMarble.PrevCir());

        }
        else
        {
            //Just add marble after the Next marble, make this the new current.
            currentMarble = marbles.AddAfter(currentMarble.NextCir(), nextValue);
        }

        //Console.WriteLine($"[{(nextValue - 1) % players}]  {string.Join(" ", marbles.Select(x => x.ToString()).ToArray())}");

        //Check for end condition
        if (nextValue == lastMarble)
        {
            foundTarget = true;
        }

        nextValue++;
    }

    return scores;
}

static class LinkedListExtensions
{
    public static LinkedListNode<long> NextCir(this LinkedListNode<long> node) =>
        node.Next ?? node.List.First;
    public static LinkedListNode<long> PrevCir(this LinkedListNode<long> node) =>
        node.Previous ?? node.List.Last;
}