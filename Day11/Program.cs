const string inputFile = @"../../../../input11.txt";

Console.WriteLine("Day 11 - Chronal Charge");
Console.WriteLine("Star 1");
Console.WriteLine();

int inputValue = int.Parse(File.ReadAllText(inputFile));

int[,] fuelCells = new int[300, 300];

for (int x = 0; x < 300; x++)
{
    for (int y = 0; y < 300; y++)
    {
        fuelCells[x, y] = GetValue(x, y);
    }
}

int maxValue = -9999;
int maxX = -1;
int maxY = -1;

for (int x = 0; x < 300 - 2; x++)
{
    for (int y = 0; y < 300 - 2; y++)
    {
        int tempVal = 0;
        for (int dx = 0; dx < 3; dx++)
        {
            for (int dy = 0; dy < 3; dy++)
            {
                tempVal += fuelCells[x + dx, y + dy];
            }
        }

        if (tempVal > maxValue)
        {
            maxValue = tempVal;
            maxX = x;
            maxY = y;
        }
    }
}

Console.WriteLine($"Answer: ({maxX + 1},{maxY + 1}): {maxValue}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

maxValue = -9999;
maxX = -1;
maxY = -1;
int maxSize = -1;

Task<(int, int, int, int)>[] searchTasks = new Task<(int, int, int, int)>[299];

for (int size = 2; size <= 300; size++)
{
    int tempSize = size;
    searchTasks[size - 2] = Task.Run(() => Opt1FindBest(tempSize));
}

Task.WaitAll(searchTasks);

(int, int, int, int)[] results = searchTasks.Select(x => x.Result).ToArray();

(maxValue, maxSize, maxX, maxY) = results.OrderByDescending(x => x.Item1).First();

Console.WriteLine($"Answer: ({maxX + 1},{maxY + 1},{maxSize}): {maxValue}");
Console.WriteLine();
Console.ReadKey();




int GetValue(int x, int y)
{
    //Fixing zero-indexing
    long RackID = x + 11;
    return (int)(((RackID * RackID * (y + 1) + RackID * inputValue) / 100) % 10) - 5;
}

//34 seconds
//(int, int, int, int) UnoptimizedFindBest(int size)
//{
//    int maxValue = int.MinValue;
//    int maxX = 0;
//    int maxY = 0;

//    for (int x = 0; x < 301 - size; x++)
//    {
//        for (int y = 0; y < 301 - size; y++)
//        {
//            int tempVal = 0;

//            for (int dx = 0; dx < size; dx++)
//            {
//                for (int dy = 0; dy < size; dy++)
//                {
//                    tempVal += fuelCells[x + dx, y + dy];
//                }
//            }

//            if (tempVal > maxValue)
//            {
//                maxValue = tempVal;
//                maxX = x;
//                maxY = y;
//            }
//        }
//    }

//    return (maxValue, size, maxX, maxY);
//}

(int, int, int, int) Opt1FindBest(int size)
{
    int maxValue = int.MinValue;
    int maxX = 0;
    int maxY = 0;

    for (int x = 0; x < 301 - size; x++)
    {
        //First Square of Column
        int tempVal = 0;
        for (int dx = 0; dx < size; dx++)
        {
            for (int dy = 0; dy < size; dy++)
            {
                tempVal += fuelCells[x + dx, dy];
            }
        }

        //Compare
        if (tempVal > maxValue)
        {
            maxValue = tempVal;
            maxX = x;
            maxY = 0;
        }

        //Acquire later squares by adding new row and subtracting old row
        for (int y = 0; y < 300 - size; y++)
        {
            for (int dx = 0; dx < size; dx++)
            {
                tempVal -= fuelCells[x + dx, y];
                tempVal += fuelCells[x + dx, y + size];
            }

            if (tempVal > maxValue)
            {
                maxValue = tempVal;
                maxX = x;
                maxY = y + 1;
            }
        }
    }

    return (maxValue, size, maxX, maxY);
}