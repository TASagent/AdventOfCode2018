
const string inputFile = @"../../../../input04.txt";


Console.WriteLine("Day 04 - Repose Record");
Console.WriteLine("Star 1");
Console.WriteLine();

List<GuardEvent> guardEvents = new List<GuardEvent>();

foreach (string line in System.IO.File.ReadAllLines(inputFile))
{
    guardEvents.Add(new GuardEvent(line));
}

guardEvents.Sort();


for (int i = 0; i < guardEvents.Count; i++)
{
    if (guardEvents[i].id == -1)
    {
        guardEvents[i].id = guardEvents[i - 1].id;
    }
}

//Cull start events from the dataset

guardEvents.RemoveAll(x => x._Event == GuardEvent.Event.Start);

//Step 1: which guard has the most minutes asleep?

Dictionary<int, (int id, int time)> minuteAccumulator = new Dictionary<int, (int id, int time)>();

int startTime = -1;

foreach (GuardEvent thing in guardEvents)
{
    switch (thing._Event)
    {
        case GuardEvent.Event.WakeUp:
            if (startTime == -1)
            {
                throw new Exception("Unexpected startTime not set");
            }
            int newTime = thing.minute - startTime;
            startTime = -1;

            if (minuteAccumulator.ContainsKey(thing.id))
            {
                minuteAccumulator[thing.id] = (thing.id, minuteAccumulator[thing.id].time + newTime);
            }
            else
            {
                minuteAccumulator.Add(thing.id, (thing.id, newTime));
            }


            break;
        case GuardEvent.Event.FallAsleep:
            if (startTime != -1)
            {
                throw new Exception("Unexpected startTime already set");
            }
            startTime = thing.minute;
            break;
        default:
            throw new Exception(thing._Event.ToString());
    }
}

List<(int id, int time)> output = minuteAccumulator.Values.ToList();
output.Sort((x, y) => -1 * x.time.CompareTo(y.time));

(int id, int time) = output[0];
Console.WriteLine($"Guard {id} sleeps for {time} minutes");

List<GuardEvent> targetGuard = guardEvents.Where(x => x.id == id).ToList();

int[] minuteTracker = new int[60];

foreach (GuardEvent thing in targetGuard)
{
    switch (thing._Event)
    {
        case GuardEvent.Event.WakeUp:
            if (startTime == -1)
            {
                throw new Exception("Unexpected startTime not set");
            }

            for (int i = startTime; i < thing.minute; i++)
            {
                minuteTracker[i]++;
            }
            startTime = -1;

            break;
        case GuardEvent.Event.FallAsleep:
            if (startTime != -1)
            {
                throw new Exception("Unexpected startTime already set");
            }
            startTime = thing.minute;
            break;
        default:
            throw new Exception(thing._Event.ToString());
    }
}

int targetMin = 0;
for (int i = 1; i < minuteTracker.Length; i++)
{
    if (minuteTracker[i] > minuteTracker[targetMin])
    {
        targetMin = i;
    }
}

Console.WriteLine();
Console.WriteLine($"Guard {id} sleeps most on minute {targetMin}");
Console.WriteLine($"Problem Answer: {id * targetMin}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

//Which guard spends the most time sleeping on a specific minute?
//Transform each guard into a (id, minute, time).
//just replace if the new guard is better, don't need to accumulate.

int id2 = -1;
int minute2 = -1;
int time2 = -1;

foreach (int uniqueID in minuteAccumulator.Keys)
{
    int[] minuteTracker2 = new int[60];
    foreach (GuardEvent thing in guardEvents.Where(x => x.id == uniqueID))
    {
        switch (thing._Event)
        {
            case GuardEvent.Event.WakeUp:
                if (startTime == -1)
                {
                    throw new Exception("Unexpected startTime not set");
                }

                for (int i = startTime; i < thing.minute; i++)
                {
                    minuteTracker2[i]++;
                }
                startTime = -1;

                break;
            case GuardEvent.Event.FallAsleep:
                if (startTime != -1)
                {
                    throw new Exception("Unexpected startTime already set");
                }
                startTime = thing.minute;
                break;
            default:
                throw new Exception(thing._Event.ToString());
        }
    }

    for (int i = 1; i < minuteTracker2.Length; i++)
    {
        if (minuteTracker2[i] > time2)
        {
            time2 = minuteTracker2[i];
            minute2 = i;
            id2 = uniqueID;
        }
    }

}

Console.WriteLine($"Guard {id2} sleeps most on minute {minute2}");
Console.WriteLine($"Problem Answer: {id2 * minute2}");
Console.WriteLine();


Console.ReadKey();

class GuardEvent : IComparable
{
    public enum Event
    {
        Start = 0,
        WakeUp,
        FallAsleep
    }

    public Event _Event;

    public int id;

    public readonly int year;
    public readonly int month;
    public readonly int day;
    public readonly int hour;
    public readonly int minute;

    private static readonly char[] separators = new char[] { '-', '[', ' ', ']', ':', '#' };
    public GuardEvent(string serialized)
    {
        string[] splitStr =
            serialized.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        year = int.Parse(splitStr[0]);
        month = int.Parse(splitStr[1]);
        day = int.Parse(splitStr[2]);
        hour = int.Parse(splitStr[3]);
        minute = int.Parse(splitStr[4]);

        switch (splitStr[5])
        {
            case "falls":
                _Event = Event.FallAsleep;
                id = -1;
                break;
            case "wakes":
                _Event = Event.WakeUp;
                id = -1;
                break;
            case "Guard":
                _Event = Event.Start;
                id = int.Parse(splitStr[6]);
                break;
            default:
                throw new Exception(splitStr[5]);

        }
    }

    public int CompareTo(GuardEvent other)
    {
        if (other == null)
        {
            return 1;
        }

        if (year != other.year)
        {
            return year.CompareTo(other.year);
        }

        if (month != other.month)
        {
            return month.CompareTo(other.month);
        }

        if (day != other.day)
        {
            return day.CompareTo(other.day);
        }

        if (hour != other.hour)
        {
            return hour.CompareTo(other.hour);
        }

        return minute.CompareTo(other.minute);
    }

    public int CompareTo(object obj)
    {
        return CompareTo((GuardEvent)obj);
    }
}