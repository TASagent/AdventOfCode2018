const string inputFile = @"../../../../input15.txt";


Console.WriteLine("Day 15 - Beverage Bandits");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] inputFileLines = System.IO.File.ReadAllLines(inputFile);

int xMax = inputFileLines[0].Length;
int yMax = inputFileLines.Length;

char[,] grid = new char[xMax, yMax];
char[,] initialGrid = new char[xMax, yMax];
List<Creature> creatures = new List<Creature>();

Creature.Initialize(grid, creatures);

for (int y = 0; y < yMax; y++)
{
    string line = inputFileLines[y];
    for (int x = 0; x < xMax; x++)
    {
        initialGrid[x, y] = line[x];
        grid[x, y] = line[x];

        switch (grid[x, y])
        {
            case 'E':
            case 'G':
                creatures.Add(new Creature(x, y));
                break;
            default:
                break;
        }
    }
}

int round = 0;
CreatureType winner = CreatureType.MAX;

while (true)
{
    //This will act as the turn order
    creatures.Sort();

    foreach (Creature creature in creatures)
    {
        if (!creature.Alive)
        {
            continue;
        }

        //Check for game end condition at the start of a turn
        if (!creatures.Any(x => x.CreatureType == creature.EnemyType && x.Alive))
        {
            winner = creature.CreatureType;
            goto Done;
        }

        creature.Act();

    }

    round++;
}

Done:
int HP = creatures.Where(x => x.Alive && x.CreatureType == winner).Sum(x => x.HitPoints);

Console.WriteLine($"Team {winner} wins after {round} rounds with {HP}: {round * HP} ");
Console.WriteLine();

Console.WriteLine("Star 2");
Console.WriteLine();

//Find the attackpower that results in no elf casualties
while (true)
{
    Creature.ElfAttackPower++;

    creatures.Clear();

    //Reset board
    for (int y = 0; y < yMax; y++)
    {
        for (int x = 0; x < xMax; x++)
        {
            grid[x, y] = initialGrid[x, y];

            switch (grid[x, y])
            {
                case 'E':
                case 'G':
                    creatures.Add(new Creature(x, y));
                    break;
                default:
                    break;
            }
        }
    }


    round = 0;
    winner = CreatureType.MAX;

    while (true)
    {
        //This will act as the turn order
        creatures.Sort();

        foreach (Creature creature in creatures)
        {
            if (!creature.Alive)
            {
                continue;
            }

            //Check for game end condition at the start of a turn
            if (!creatures.Any(x => x.CreatureType == creature.EnemyType && x.Alive))
            {
                winner = creature.CreatureType;
                goto Check;
            }

            creature.Act();

        }

        round++;
    }

Check:
    if (!creatures.Any(x => x.CreatureType == CreatureType.Elf && !x.Alive))
    {
        //Found it!
        break;
    }
}

HP = creatures.Where(x => x.Alive && x.CreatureType == winner).Sum(x => x.HitPoints);

Console.WriteLine($"Elves win with {Creature.ElfAttackPower} AttackPower after {round} rounds with {HP}: {round * HP} ");
Console.WriteLine();

Console.ReadKey();



class Creature : IComparable<Creature>
{
    public int X { get; private set; }
    public int Y { get; private set; }

    private (int x, int y) Pos => (X, Y);

    public CreatureType CreatureType { get; }
    public CreatureType EnemyType { get; }

    public int HitPoints { get; private set; }

    public bool Alive => HitPoints > 0;

    private static int XMax;
    private static int YMax;

    private const int Range = 1;
    public static int ElfAttackPower = 3;

    private int AttackPower
    {
        get
        {
            switch (CreatureType)
            {
                case CreatureType.Elf: return ElfAttackPower;
                case CreatureType.Goblin: return 3;
                default: throw new Exception();
            }
        }
    }

    //Move Buffers
    private static char[,] grid;
    private static int[,] cost;
    private static bool[,] visitable;
    private static Queue<(int x, int y)> pendingLocations = new Queue<(int x, int y)>();
    private static List<Creature> creatures = null;

    private char Character
    {
        get
        {
            switch (CreatureType)
            {
                case CreatureType.Elf: return 'E';
                case CreatureType.Goblin: return 'G';
                default: throw new Exception();
            }
        }
    }

    public Creature(int x, int y)
    {
        X = x;
        Y = y;

        switch (grid[x, y])
        {
            case 'G':
                CreatureType = CreatureType.Goblin;
                EnemyType = CreatureType.Elf;
                HitPoints = 200;
                break;
            case 'E':
                CreatureType = CreatureType.Elf;
                EnemyType = CreatureType.Goblin;
                HitPoints = 200;
                break;
            default: throw new Exception($"Unexpected Char: {grid[x, y]}");
        }
    }

    private IEnumerable<(int x, int y)> GetAttackPositions()
    {
        if (X > 0 && grid[X - 1, Y] == '.')
        {
            yield return (X - 1, Y);
        }

        if (X < XMax - 1 && grid[X + 1, Y] == '.')
        {
            yield return (X + 1, Y);
        }

        if (Y > 0 && grid[X, Y - 1] == '.')
        {
            yield return (X, Y - 1);
        }

        if (Y < YMax - 1 && grid[X, Y + 1] == '.')
        {
            yield return (X, Y + 1);
        }
    }

    private bool IsLivingEnemy(Creature other) =>
        other.Alive && (other.CreatureType == EnemyType);

    private bool IsLivingEnemyInRange(Creature other) =>
        other.Alive &&
        (other.CreatureType == EnemyType) &&
        (Math.Abs(Y - other.Y) + Math.Abs(X - other.X) <= Range);

    public void Act()
    {
        var enemiesInRange = creatures.Where(IsLivingEnemyInRange);

        if (enemiesInRange.Count() == 0)
        {
            //Move
            CalculateDistances(X, Y);

            var attackPositions = creatures
                .Where(IsLivingEnemy)
                .SelectMany(x => x.GetAttackPositions())
                .Distinct()
                .Where(x => cost[x.x, x.y] != int.MaxValue);

            if (attackPositions.Count() == 0)
            {
                //No one to attack!
                return;
            }

            int minDistance = attackPositions.Min(x => cost[x.x, x.y]);

            var pos = attackPositions
                .Where(x => cost[x.x, x.y] == minDistance)
                .OrderBy(x => x.y).ThenBy(x => x.x).First();

            //Figure out path to pos
            CalculateDistances(pos.x, pos.y);

            int minCost = GetAttackPositions().Min(x => cost[x.x, x.y]);

            var newPosition = GetAttackPositions()
                .Where(x => cost[x.x, x.y] == minCost)
                .OrderBy(x => x.y).ThenBy(x => x.x).First();

            grid[X, Y] = '.';

            X = newPosition.x;
            Y = newPosition.y;

            grid[X, Y] = Character;

            //Recalculate enemies in range
            enemiesInRange = creatures.Where(IsLivingEnemyInRange);
        }

        if (enemiesInRange.Count() > 0)
        {
            //Attack
            int minHP = enemiesInRange.Min(x => x.HitPoints);
            //Just the minHP enemies
            Creature target = enemiesInRange.Where(x => x.HitPoints == minHP).OrderBy(x => x).First();

            target.TakeDamage(AttackPower);
        }
    }

    private void TakeDamage(int damage)
    {
        if (!Alive)
        {
            throw new Exception("Creature should already be dead.");
        }

        HitPoints -= damage;
        if (!Alive)
        {
            grid[X, Y] = '.';
        }
    }

    public static void Initialize(char[,] grid, List<Creature> creatures)
    {
        XMax = grid.GetLength(0);
        YMax = grid.GetLength(1);

        Creature.creatures = creatures;
        Creature.grid = grid;

        //Initialize
        cost = new int[XMax, YMax];
        visitable = new bool[XMax, YMax];
    }

    private void CalculateDistances(int x0, int y0)
    {
        CleanBuffers();

        cost[x0, y0] = 0;
        pendingLocations.Enqueue((x0, y0));
        visitable[x0, y0] = false;

        while (pendingLocations.Count > 0)
        {
            (int x, int y) = pendingLocations.Dequeue();

            int newCost = cost[x, y] + 1;

            if (x > 0 && visitable[x - 1, y])
            {
                cost[x - 1, y] = newCost;
                pendingLocations.Enqueue((x - 1, y));
                visitable[x - 1, y] = false;
            }

            if (x < XMax - 1 && visitable[x + 1, y])
            {
                cost[x + 1, y] = newCost;
                pendingLocations.Enqueue((x + 1, y));
                visitable[x + 1, y] = false;
            }

            if (y > 0 && visitable[x, y - 1])
            {
                cost[x, y - 1] = newCost;
                pendingLocations.Enqueue((x, y - 1));
                visitable[x, y - 1] = false;
            }

            if (y < YMax - 1 && visitable[x, y + 1])
            {
                cost[x, y + 1] = newCost;
                pendingLocations.Enqueue((x, y + 1));
                visitable[x, y + 1] = false;
            }
        }
    }

    private void CleanBuffers()
    {
        pendingLocations.Clear();

        for (int y = 0; y < YMax; y++)
        {
            for (int x = 0; x < XMax; x++)
            {
                cost[x, y] = int.MaxValue;
                visitable[x, y] = (grid[x, y] == '.');
            }
        }
    }

    #region IComparable

    int IComparable<Creature>.CompareTo(Creature other)
    {
        if (other.Y == Y)
        {
            return X.CompareTo(other.X);
        }

        return Y.CompareTo(other.Y);
    }

    #endregion IComparable
}

enum CreatureType
{
    Elf = 0,
    Goblin,
    MAX
}