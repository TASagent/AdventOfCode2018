
const string inputFile = @"../../../../input22.txt";


Console.WriteLine("Day 22 - Mode Maze");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] lines = File.ReadAllLines(inputFile);
string[] coords = lines[1][8..].Split(',');

int TargetX = int.Parse(coords[0]);
int TargetY = int.Parse(coords[1]);

long Depth = int.Parse(lines[0][7..]);
const long GeologicMod = 20183L;

const long XFactor = 16807L;
const long YFactor = 48271L;

//Note, I originally hadn't made the region wide enough to get the correct answer
int width = TargetX + 40;
int depth = TargetY + 10;


//Rules:
//The region at 0,0(the mouth of the cave) has a geologic index of 0.
//The region at the coordinates of the target has a geologic index of 0.
//If the region's Y coordinate is 0, the geologic index is its X coordinate times 16807.
//If the region's X coordinate is 0, the geologic index is its Y coordinate times 48271.
//Otherwise, the region's geologic index is the result of multiplying the erosion levels
//  of the regions at X-1,Y and X,Y-1.

//A region's erosion level is its geologic index plus the cave system's depth,
//all modulo 20183. Then

//Geologic Index:
//If the erosion level modulo 3 is 0, the region's type is rocky.
//If the erosion level modulo 3 is 1, the region's type is wet.
//If the erosion level modulo 3 is 2, the region's type is narrow.

int[,] erosionLevel = new int[width, depth];

erosionLevel[0, 0] = (int)((0 + Depth) % GeologicMod);

for (int x = 1; x < width; x++)
{
    erosionLevel[x, 0] = (int)((XFactor * x + Depth) % GeologicMod);
}

for (int y = 1; y < depth; y++)
{
    erosionLevel[0, y] = (int)((YFactor * y + Depth) % GeologicMod);
}

for (int y = 1; y < depth; y++)
{
    for (int x = 1; x < width; x++)
    {
        if (x == TargetX && y == TargetY)
        {
            erosionLevel[x, y] = erosionLevel[0, 0];
        }
        else
        {
            erosionLevel[x, y] = (int)(((long)erosionLevel[x - 1, y] * (long)erosionLevel[x, y - 1] + Depth) % GeologicMod);
        }
    }
}


int cumulative = 0;
for (int y = 0; y <= TargetY; y++)
{
    for (int x = 0; x <= TargetX; x++)
    {
        cumulative += erosionLevel[x, y] % 3;
    }
}

Console.WriteLine($"Risk level: {cumulative}");

Console.WriteLine("Star 2");
Console.WriteLine();

Node[,] grid = new Node[width, depth];
Node.grid = grid;
Node.width = width;
Node.depth = depth;

for (int y = 0; y < depth; y++)
{
    for (int x = 0; x < width; x++)
    {
        grid[x, y] = new Node(x, y, erosionLevel[x, y] % 3);
    }
}


//Gear Notes:


//Gear:
//0: Neither
//1: Torch
//2: Climbing Gear

//Require that Gear != new type;

List<Node> pendingNodes = new List<Node>();
grid[0, 0].torchCost = 0;
grid[0, 0].gearCost = Node.SwitchCost;

pendingNodes.Add(grid[0, 0]);

while (pendingNodes.Count > 0)
{
    Node node = pendingNodes[0];
    pendingNodes.RemoveAt(0);

    node.Explore(pendingNodes);

    pendingNodes.Sort();
}

Console.WriteLine($"Time to Target: {grid[TargetX, TargetY].torchCost}");

Console.WriteLine();
Console.ReadKey();

class Node : IComparable<Node>
{
    public const int MoveCost = 1;
    public const int SwitchCost = 7;

    public static Node[,] grid;
    public static int width;
    public static int depth;

    public int neitherCost = int.MaxValue / 2;
    public int torchCost = int.MaxValue / 2;
    public int gearCost = int.MaxValue / 2;

    private int this[int gear]
    {
        get
        {
            switch (gear)
            {
                case 0: return neitherCost;
                case 1: return torchCost;
                case 2: return gearCost;
                default: throw new Exception();
            }
        }

        set
        {
            switch (gear)
            {
                case 0: neitherCost = Math.Min(value, neitherCost); break;
                case 1: torchCost = Math.Min(value, torchCost); break;
                case 2: gearCost = Math.Min(value, gearCost); break;
                default: throw new Exception();
            }
        }
    }

    public int MinCost => Math.Min(Math.Min(neitherCost, torchCost), gearCost);

    public readonly int terrain;
    public readonly int x;
    public readonly int y;

    public readonly int equipA;
    public readonly int equipB;

    public char Icon
    {
        get
        {
            switch (terrain)
            {
                case 0: return '.';
                case 1: return '=';
                case 2: return '|';
                default: throw new Exception();
            }
        }
    }

    public int CostA
    {
        get => this[equipA];
        set
        {
            this[equipA] = value;
            this[equipB] = value + SwitchCost;
        }
    }

    public int CostB
    {
        get => this[equipB];
        set
        {
            this[equipB] = value;
            this[equipA] = value + SwitchCost;
        }
    }

    public Node(int x, int y, int terrain)
    {
        this.x = x;
        this.y = y;
        this.terrain = terrain;
        equipA = (terrain + 1) % 3;
        equipB = (terrain + 2) % 3;
    }

    public void Explore(List<Node> pendingNodes)
    {
        if (y + 1 < depth && TryTravel(grid[x, y + 1]))
        {
            pendingNodes.Add(grid[x, y + 1]);
        }

        if (x + 1 < width && TryTravel(grid[x + 1, y]))
        {
            pendingNodes.Add(grid[x + 1, y]);
        }

        if (x - 1 >= 0 && TryTravel(grid[x - 1, y]))
        {
            pendingNodes.Add(grid[x - 1, y]);
        }

        if (y - 1 >= 0 && TryTravel(grid[x, y - 1]))
        {
            pendingNodes.Add(grid[x, y - 1]);
        }
    }

    public bool TryTravel(Node neighbor)
    {
        //Same Terrain
        if (neighbor.terrain == terrain)
        {
            if (CostA + MoveCost < neighbor.CostA ||
                CostB + MoveCost < neighbor.CostB)
            {
                neighbor[equipA] = this[equipA] + MoveCost;
                neighbor[equipB] = this[equipB] + MoveCost;
                return true;
            }

            return false;
        }

        //Different Terrain
        int moveItem = equipA;
        if (moveItem == neighbor.terrain)
        {
            moveItem = equipB;
        }

        if (this[moveItem] + MoveCost < neighbor[moveItem])
        {
            if (moveItem == neighbor.equipA)
            {
                neighbor.CostA = this[moveItem] + MoveCost;
            }
            else
            {
                neighbor.CostB = this[moveItem] + MoveCost;
            }
            return true;
        }

        return false;
    }

    int IComparable<Node>.CompareTo(Node other) => MinCost.CompareTo(other.MinCost);
}
