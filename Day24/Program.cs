using System.Text.RegularExpressions;

const string inputFile = @"../../../../input24.txt";


Console.WriteLine("Day 24 - Immune System Simulator 20XX");
Console.WriteLine("Star 1");
Console.WriteLine();

//Each group also has an effective power:
//the number of units in that group multiplied by their attack damage.

//During the target selection phase, each group attempts to choose one target.
//In decreasing order of effective power, groups choose their targets; 
//in a tie, the group with the higher initiative chooses first.
//The attacking group chooses to target the group in the enemy army to which 
//it would deal the most damage
//(after accounting for weaknesses and immunities, but not accounting for whether the
//defending group has enough units to actually receive all of that damage).

//If an attacking group is considering two defending groups to which it would deal 
//equal damage, it chooses to target the defending group with the 
//largest effective power;
//if there is still a tie, it chooses the defending group with the highest initiative.
//If it cannot deal any defending groups damage, it does not choose a target.
//Defending groups can only be chosen as a target by one attacking group.


//At the end of the target selection phase,
//each group has selected zero or one groups to attack, 
//and each group is being attacked by zero or one groups.

//By default, an attacking group would deal damage equal to its effective power 
//to the defending group. 
//However, if the defending group is immune to the attacking group's attack type, 
//the defending group instead takes no damage;
//if the defending group is weak to the attacking group's attack type, 
//the defending group instead takes double damage.

string[] input = System.IO.File.ReadAllLines(inputFile);

List<UnitGroup> rawUnits = new List<UnitGroup>();

bool immuneSystem = true;

foreach (string line in input)
{
    if (string.IsNullOrEmpty(line))
    {
        continue;
    }
    else if (line == "Immune System:")
    {
        immuneSystem = true;
        continue;
    }
    else if (line == "Infection:")
    {
        immuneSystem = false;
        continue;
    }

    rawUnits.Add(new UnitGroup(line, immuneSystem));
}

List<UnitGroup> units = rawUnits.Clone();

Fight(units);

Console.WriteLine($"{(units[0].ImmuneSystem ? "ImmuneSystem" : "Infection")} wins with: {units.Sum(x => x.UnitCount)} units remaining.");
Console.WriteLine();

Console.WriteLine("Star 2");
Console.WriteLine();

bool growing = true;
bool done = false;

int boost = 2;
int boostLB = 1;
int boostUB = 2;

while (!done)
{
    Console.Write($"Immune Boost: {boost}");

    //Reset
    units = rawUnits.CloneWithBoost(boost);
    Fight(units);

    bool immuneWon = units.Count(x => x.ImmuneSystem) == units.Count;

    Console.WriteLine(immuneWon ? " Won" : " Loss");

    if (growing)
    {
        if (immuneWon)
        {
            growing = false;
            //Move to binary search

            boost = (boostUB + boostLB) / 2;
        }
        else
        {
            boostLB = boostUB;
            boostUB *= 2;
            boost = boostUB;
        }
    }
    else
    {
        if (immuneWon)
        {
            if (boostUB == boostLB)
            {
                break;
            }

            boostUB = boost;
        }
        else
        {
            boostLB = boost + 1;
        }

        boost = (boostUB + boostLB) / 2;
    }
}


Console.WriteLine($"{(units[0].ImmuneSystem ? "ImmuneSystem" : "Infection")} wins with: {units.Sum(x => x.UnitCount)} units remaining.");
Console.WriteLine();
Console.ReadKey();


static void Fight(List<UnitGroup> units)
{
    Dictionary<UnitGroup, UnitGroup> targetDictionary = new Dictionary<UnitGroup, UnitGroup>();
    Comparer<UnitGroup> targetingComparer = new UnitGroupTargetSelectionOrder();
    Comparer<UnitGroup> turnComparer = new UnitGroupTurnSelectionOrder();

    string lastRound = "";

    //Simulate battle
    while (true)
    {
        units.Sort(targetingComparer);

        foreach (UnitGroup unit in units)
        {
            UnitGroup selectedTarget = null;
            int damage = 0;

            foreach (UnitGroup target in units)
            {
                if (target.ImmuneSystem == unit.ImmuneSystem || targetDictionary.ContainsValue(target))
                {
                    //Skip friendlies, self, and already-targeted units
                    continue;
                }

                int potentialDamage = unit.CalculateDamage(target);
                if (potentialDamage > damage)
                {
                    selectedTarget = target;
                    damage = potentialDamage;
                }
                else if (potentialDamage > 0 && potentialDamage == damage)
                {
                    if (target.EffectivePower > selectedTarget.EffectivePower ||
                        (target.EffectivePower == selectedTarget.EffectivePower &&
                        target.Initiative > selectedTarget.Initiative))
                    {
                        selectedTarget = target;
                    }
                }
            }

            if (selectedTarget != null)
            {
                targetDictionary.Add(unit, selectedTarget);
            }
        }

        units.Sort(turnComparer);

        foreach (UnitGroup unit in units)
        {
            if (!unit.Alive || !targetDictionary.ContainsKey(unit))
            {
                continue;
            }

            UnitGroup target = targetDictionary[unit];

            if (!target.Alive)
            {
                continue;
            }

            target.TakeDamage(unit.CalculateDamage(target));
        }
        targetDictionary.Clear();

        units.RemoveAll(x => !x.Alive);

        int immuneSystemCount = units.Count(x => x.ImmuneSystem);
        if (immuneSystemCount == units.Count || immuneSystemCount == 0)
        {
            //End when one side is dead
            break;
        }

        string thisRound = $"Immune:[{string.Join(",", units.Where(x => x.ImmuneSystem).Select(x => x.UnitCount.ToString()).ToArray())}]  Infection:[{string.Join(",", units.Where(x => !x.ImmuneSystem).Select(x => x.UnitCount.ToString()).ToArray())}]";

        if (thisRound == lastRound)
        {
            //End if there is a stalemate
            break;
        }

        lastRound = thisRound;
    }
}

class UnitGroupTargetSelectionOrder : Comparer<UnitGroup>
{
    public override int Compare(UnitGroup x, UnitGroup y)
    {
        if (x.EffectivePower.CompareTo(y.EffectivePower) != 0)
        {
            return -1 * x.EffectivePower.CompareTo(y.EffectivePower);
        }
        else if (x.Initiative.CompareTo(y.Initiative) != 0)
        {
            //Higher initiative goes first if power is equal
            return -1 * x.Initiative.CompareTo(y.Initiative);
        }

        return 0;
    }
}

class UnitGroupTurnSelectionOrder : Comparer<UnitGroup>
{
    public override int Compare(UnitGroup x, UnitGroup y)
    {
        if (x.Initiative.CompareTo(y.Initiative) != 0)
        {
            //Higher initiative goes first
            return -1 * x.Initiative.CompareTo(y.Initiative);
        }

        return 0;
    }
}

class UnitGroup : IComparable<UnitGroup>
{
    public int UnitCount { get; set; }
    public int HP { get; }
    public int Initiative { get; }
    public int Damage { get; }
    public string DamageType { get; }

    public Dictionary<string, int> DamageMultipliers { get; } = new Dictionary<string, int>();

    public bool ImmuneSystem { get; }

    public int EffectivePower => UnitCount * Damage;

    public bool Alive => UnitCount > 0;

    private static readonly Regex parser = new Regex(
        @"(?<Units>\d+) units each with (?<HP>\d+) hit points (?:\((?:weak to (?<Weakness>(?:\w+|, )+)|immune to (?<Immunity>(?:\w+|, )+)|; )+\) )?with an attack that does (?<Dmg>\d+) (?<DmgType>\w+) damage at initiative (?<Init>\d+)");

    //Psuedo copy-constructor
    public UnitGroup(UnitGroup source, int boost = 0)
    {
        UnitCount = source.UnitCount;
        HP = source.HP;
        Initiative = source.Initiative;

        DamageType = source.DamageType;

        DamageMultipliers = source.DamageMultipliers;
        ImmuneSystem = source.ImmuneSystem;

        if (ImmuneSystem)
        {
            Damage = source.Damage + boost;
        }
        else
        {
            Damage = source.Damage;
        }
    }

    public UnitGroup(string line, bool immuneSystem)
    {
        ImmuneSystem = immuneSystem;

        Match match = parser.Match(line);

        UnitCount = int.Parse(match.Groups["Units"].Value);
        HP = int.Parse(match.Groups["HP"].Value);
        Initiative = int.Parse(match.Groups["Init"].Value);
        Damage = int.Parse(match.Groups["Dmg"].Value);
        DamageType = match.Groups["DmgType"].Value;

        Group weaknessGroup = match.Groups["Weakness"];

        if (weaknessGroup.Success)
        {
            foreach (string weakness in weaknessGroup.Value.Split(", "))
            {
                DamageMultipliers.Add(weakness, 2);
            }
        }

        Group immunityGroup = match.Groups["Immunity"];

        if (immunityGroup.Success)
        {
            foreach (string immunity in immunityGroup.Value.Split(", "))
            {
                DamageMultipliers.Add(immunity, 0);
            }
        }
    }

    public int CalculateDamage(UnitGroup attacked) =>
        EffectivePower * attacked.GetDamageMultiplier(DamageType);

    public int GetDamageMultiplier(string damageType) =>
        DamageMultipliers.TryGetValue(damageType, out int multiplier) ? multiplier : 1;


    public void TakeDamage(int damage) =>
        UnitCount -= damage / HP;

    int IComparable<UnitGroup>.CompareTo(UnitGroup other) =>
        EffectivePower.CompareTo(other.EffectivePower);
}

static class ListExtensions
{
    public static List<UnitGroup> Clone(this List<UnitGroup> units) =>
        units.Select(x => new UnitGroup(x, 0)).ToList();

    public static List<UnitGroup> CloneWithBoost(this List<UnitGroup> units, int boost) =>
        units.Select(x => new UnitGroup(x, boost)).ToList();
}
