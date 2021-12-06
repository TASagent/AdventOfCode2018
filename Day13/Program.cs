const string inputFile = @"../../../../input13.txt";

const char Vert = '║';
const char Horiz = '═';
const char LL = '╚';
const char UL = '╔';
const char LR = '╝';
const char UR = '╗';
const char Int = '╬';


Console.WriteLine("Day 13 - Mine Cart Madness");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] file = System.IO.File.ReadAllLines(inputFile);
char[,] grid = new char[file[0].Length, file.Length];

List<Cart> carts = new List<Cart>();

HashSet<(int x, int y)> collisionHash = new HashSet<(int x, int y)>();

for (int y = 0; y < file.Length; y++)
{
    string line = file[y];
    for (int x = 0; x < line.Length; x++)
    {
        switch (line[x])
        {
            case ' ':
                grid[x, y] = ' ';
                break;
            case '/':
                if (x == 0 || (grid[x - 1, y] == ' ' || grid[x - 1, y] == Vert || grid[x - 1, y] == LR))
                {
                    grid[x, y] = UL;
                }
                else
                {
                    grid[x, y] = LR;
                }
                break;
            case '\\':
                if (x == 0 || (grid[x - 1, y] == ' ' || grid[x - 1, y] == Vert || grid[x - 1, y] == UR))
                {
                    grid[x, y] = LL;
                }
                else
                {
                    grid[x, y] = UR;
                }
                break;
            case '|':
                grid[x, y] = Vert;
                break;
            case '-':
                grid[x, y] = Horiz;
                break;
            case '+':
                grid[x, y] = Int;
                break;
            case '^':
            case 'v':
                carts.Add(new Cart(x, y, grid, line[x]));
                grid[x, y] = Vert;
                break;
            case '<':
            case '>':
                carts.Add(new Cart(x, y, grid, line[x]));
                grid[x, y] = Horiz;
                break;
            default:
                throw new Exception($"Unimplemented character {line[x]}");
        }
    }
}


for (int y = 0; y < grid.GetLength(1); y++)
{
    for (int x = 0; x < grid.GetLength(0); x++)
    {
        Console.Write(grid[x, y]);
    }
    Console.Write('\n');
}

long firstCollisionTick = -1;
long tick = -1;
(int x, int y) firstCollisionLocation = (0, 0);
HashSet<Cart> destroyedCarts = new HashSet<Cart>();

while (true)
{
    tick++;
    carts.Sort();

    foreach (Cart cart in carts)
    {
        if (destroyedCarts.Contains(cart))
        {
            continue;
        }

        collisionHash.Remove((cart.x, cart.y));

        cart.Progress();

        bool collision = !collisionHash.Add((cart.x, cart.y));

        if (collision)
        {
            if (firstCollisionTick == -1)
            {
                //First collision
                firstCollisionTick = tick;
                //Collision!
                firstCollisionLocation = (cart.x, cart.y);
            }

            //Remove collision hash - can't collide anymore
            collisionHash.Remove((cart.x, cart.y));

            //Find the two colliding carts
            for (int i = 0; i < carts.Count; i++)
            {
                if (carts[i].x == cart.x && carts[i].y == cart.y)
                {
                    destroyedCarts.Add(carts[i]);
                }
            }
        }
    }

    foreach (Cart deadCart in destroyedCarts)
    {
        carts.Remove(deadCart);
    }
    destroyedCarts.Clear();

    if (carts.Count == 1)
    {
        break;
    }
}


Console.WriteLine($"Collision detected at ({firstCollisionLocation.x},{firstCollisionLocation.y}) on tick {firstCollisionTick}");
Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();
Console.WriteLine($"Final cart detected at ({carts[0].x},{carts[0].y}) on tick {tick}");

Console.ReadKey();



class Cart : IComparable<Cart>
{
    const char Vert = '║';
    const char Horiz = '═';
    const char LL = '╚';
    const char UL = '╔';
    const char LR = '╝';
    const char UR = '╗';
    const char Int = '╬';

    public Direction direction;
    public int x;
    public int y;

    private Turn _nextTurn = Turn.Left;
    private Turn NextTurn
    {
        get
        {
            Turn currentTurn = _nextTurn;
            _nextTurn = _nextTurn.NextTurn();
            return currentTurn;
        }
    }

    private readonly char[,] grid;

    public Cart(int x, int y, char[,] grid, char cart)
    {
        this.x = x;
        this.y = y;

        this.grid = grid;

        switch (cart)
        {
            case '^':
                direction = Direction.Up;
                break;

            case 'v':
                direction = Direction.Down;
                break;

            case '<':
                direction = Direction.Left;
                break;

            case '>':
                direction = Direction.Right;
                break;

            default:
                throw new Exception($"Unimplemented character {cart}");
        }
    }

    public void Progress()
    {
        switch (direction)
        {
            case Direction.Up:
                y--;
                break;
            case Direction.Down:
                y++;
                break;
            case Direction.Left:
                x--;
                break;
            case Direction.Right:
                x++;
                break;

            default:
                throw new Exception($"Unimplemented direction {direction}");
        }

        direction = GetNewDirection();
    }

    private Direction GetNewDirection()
    {
        switch (grid[x, y])
        {
            //Nothing special to do with new location
            case Horiz:
                switch (direction)
                {
                    case Direction.Left:
                    case Direction.Right: return direction;
                    default: throw new Exception($"Unexpected Direction: {direction}");
                }
            case Vert:
                switch (direction)
                {
                    case Direction.Down:
                    case Direction.Up: return direction;
                    default: throw new Exception($"Unexpected Direction: {direction}");
                }
            case LL:
                switch (direction)
                {
                    case Direction.Left: return Direction.Up;
                    case Direction.Down: return Direction.Right;
                    default: throw new Exception($"Unexpected Direction: {direction}");
                }
            case LR:
                switch (direction)
                {
                    case Direction.Right: return Direction.Up;
                    case Direction.Down: return Direction.Left;
                    default: throw new Exception($"Unexpected Direction: {direction}");
                }
            case UL:
                switch (direction)
                {
                    case Direction.Up: return Direction.Right;
                    case Direction.Left: return Direction.Down;
                    default: throw new Exception($"Unexpected Direction: {direction}");
                }
            case UR:
                switch (direction)
                {
                    case Direction.Up: return Direction.Left;
                    case Direction.Right: return Direction.Down;
                    default: throw new Exception($"Unexpected Direction: {direction}");
                }
            case Int: return direction.Rotate(NextTurn);
            default: throw new Exception($"Unexpected character: {grid[x, y]}");
        }

    }

    int IComparable<Cart>.CompareTo(Cart other)
    {
        if (y == other.y)
        {
            return x.CompareTo(other.x);
        }

        return y.CompareTo(other.y);
    }
}



enum Direction
{
    Up,
    Right,
    Down,
    Left,
    MAX
}

enum Turn
{
    Left,
    Straight,
    Right,
    MAX
}

static class EnumExts
{
    public static Turn NextTurn(this Turn turn)
    {
        switch (turn)
        {
            case Turn.Left: return Turn.Straight;
            case Turn.Straight: return Turn.Right;
            case Turn.Right: return Turn.Left;
            default: throw new Exception($"Unexpected Turn: {turn}");
        }
    }

    public static Direction Rotate(this Direction direction, Turn turn)
    {
        switch (turn)
        {
            case Turn.Left: return direction.RotateCCW();
            case Turn.Straight: return direction;
            case Turn.Right: return direction.RotateCW();
            default: throw new Exception($"Unexpected Turn: {turn}");
        }
    }

    public static Direction RotateCW(this Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return Direction.Right;
            case Direction.Right: return Direction.Down;
            case Direction.Down: return Direction.Left;
            case Direction.Left: return Direction.Up;
            default: throw new Exception($"Unexpected Direction: {direction}");
        }
    }
    public static Direction RotateCCW(this Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return Direction.Left;
            case Direction.Right: return Direction.Up;
            case Direction.Down: return Direction.Right;
            case Direction.Left: return Direction.Down;
            default: throw new Exception($"Unexpected Direction: {direction}");
        }
    }
}