using AoCTools;
using System.Text.RegularExpressions;
using System.Diagnostics;

const string inputFile = @"../../../../input20.txt";

Console.WriteLine("Day 20 - A Regular Map");
Console.WriteLine("Star 1");
Console.WriteLine();

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();

Room startingRoom = new Room(Point2D.Zero);

string input = FilterOutLoops(File.ReadAllText(inputFile));

//Build map
TravelPaths(startingRoom, input.AsSpan(1, input.Length - 2));

Dictionary<Point2D, int> cost = new Dictionary<Point2D, int>(30000);

cost[startingRoom.location] = 0;
Queue<Room> pendingRooms = new Queue<Room>();
pendingRooms.Enqueue(startingRoom);

while (pendingRooms.Count > 0)
{
    Room nextRoom = pendingRooms.Dequeue();

    int newCost = cost[nextRoom.location] + 1;

    if (nextRoom.N is not null && !cost.ContainsKey(nextRoom.N.location))
    {
        cost[nextRoom.N.location] = newCost;
        pendingRooms.Enqueue(nextRoom.N);
    }

    if (nextRoom.S is not null && !cost.ContainsKey(nextRoom.S.location))
    {
        cost[nextRoom.S.location] = newCost;
        pendingRooms.Enqueue(nextRoom.S);
    }

    if (nextRoom.E is not null && !cost.ContainsKey(nextRoom.E.location))
    {
        cost[nextRoom.E.location] = newCost;
        pendingRooms.Enqueue(nextRoom.E);
    }

    if (nextRoom.W is not null && !cost.ContainsKey(nextRoom.W.location))
    {
        cost[nextRoom.W.location] = newCost;
        pendingRooms.Enqueue(nextRoom.W);
    }
}

int maxCost = cost.Values.Max();
int highCost = cost.Values.Count(x => x >= 1000);

stopwatch.Stop();

Console.WriteLine();
Console.WriteLine($"Execution took {stopwatch.Elapsed.TotalMilliseconds} ms");
Console.WriteLine();

Console.WriteLine($"The farthest room requires passing through {maxCost} doors.");
Console.WriteLine();


Console.WriteLine("Star 2");
Console.WriteLine();
Console.WriteLine($"There are {highCost} rooms father at least 1000 doors away.");

Console.WriteLine();
Console.ReadKey();

static void TravelPaths(Room currentRoom, ReadOnlySpan<char> input)
{
    if (input.Length == 0)
    {
        //Empty - We are done
        return;
    }

    int firstIndex = input.IndexOf('(');

    if (firstIndex == -1)
    {
        //No ( found - Travel the remainder
        foreach (char c in input)
        {
            currentRoom = currentRoom.Travel(c);
        }

        return;
    }

    //Travel up to the next split
    for (int i = 0; i < firstIndex; i++)
    {
        currentRoom = currentRoom.Travel(input[i]);
    }

    int depth = 1;
    int closePosition = -1;
    List<int> pipePosition = new List<int>();


    for (int i = firstIndex + 1; i < input.Length; i++)
    {
        switch (input[i])
        {
            case 'N':
            case 'S':
            case 'W':
            case 'E':
                //Don't care.
                break;

            case '(':
                ++depth;
                break;

            case ')':
                if (--depth == 0)
                {
                    pipePosition.Add(i);
                    closePosition = i;
                    goto Breakout;
                }
                break;

            case '|':
                if (depth == 1)
                {
                    pipePosition.Add(i);
                }
                break;

            default: throw new Exception();
        }
    }

Breakout:

    int currentPos = firstIndex + 1;

    for (int i = 0; i < pipePosition.Count; i++)
    {
        int nextPos = pipePosition[i];

        TravelPaths(
            currentRoom: currentRoom,
            input: $"{input[currentPos..nextPos]}{input[(closePosition + 1)..]}");
        currentPos = nextPos + 1;
    }
}

static string FilterOutLoops(string input)
{
    Regex possibleLoopFinder = new Regex(@"\(((?:[NESW]{2})+)\|\)");
    MatchEvaluator matchEvaluator = new MatchEvaluator(FilterEvaluator);

    string initial;
    do
    {
        initial = input;
        input = possibleLoopFinder.Replace(input, matchEvaluator);
    }
    while (string.Compare(initial, input) != 0);

    return input;
}

static string FilterEvaluator(Match match) =>
    IsCycle(match.Groups[1].Value) ? match.Groups[1].Value : match.Value;


static bool IsCycle(string sequence)
{
    int ns = 0;
    int ew = 0;

    foreach (char c in sequence)
    {
        switch (c)
        {
            case 'N':
                ns++;
                break;

            case 'S':
                ns--;
                break;

            case 'E':
                ew--;
                break;

            case 'W':
                ew++;
                break;

            default: throw new Exception();
        }
    }

    return ns == 0 && ew == 0;
}

class Room
{
    public readonly Point2D location;

    public Room N = null;
    public Room S = null;
    public Room E = null;
    public Room W = null;

    public static Dictionary<Point2D, Room> rooms = new Dictionary<Point2D, Room>(30000);

    public Room(in Point2D location)
    {
        Debug.Assert(!rooms.ContainsKey(location));

        this.location = location;

        rooms.Add(location, this);
    }

    public Room(in Point2D location, char source)
        : this(location)
    {
        switch (source)
        {
            case 'N':
                S = rooms[location - Point2D.YAxis];
                break;

            case 'S':
                N = rooms[location + Point2D.YAxis];
                break;

            case 'E':
                W = rooms[location - Point2D.XAxis];
                break;

            case 'W':
                E = rooms[location + Point2D.XAxis];
                break;

            default: throw new Exception();
        }
    }

    public Room Travel(char dir)
    {
        switch (dir)
        {
            case 'N': return N ??= rooms.GetValueOrDefault(location + Point2D.YAxis, new Room(location + Point2D.YAxis, dir));
            case 'S': return S ??= rooms.GetValueOrDefault(location - Point2D.YAxis, new Room(location - Point2D.YAxis, dir));
            case 'E': return E ??= rooms.GetValueOrDefault(location + Point2D.XAxis, new Room(location + Point2D.XAxis, dir));
            case 'W': return W ??= rooms.GetValueOrDefault(location - Point2D.XAxis, new Room(location - Point2D.XAxis, dir));
            default: throw new Exception();
        }
    }
}