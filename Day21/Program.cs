
const string inputFile = @"../../../../input21.txt";


Console.WriteLine("Day 21 - Chronal Conversion");
Console.WriteLine("Star 1");
Console.WriteLine();


int instrPtr = 0;
Dictionary<string, Operation> opDict = new Dictionary<string, Operation>();


List<Operation> operations = new List<Operation>()
{
    new ADDR(),
    new ADDI(),
    new MULR(),
    new MULI(),
    new BANR(),
    new BANI(),
    new BORR(),
    new BORI(),
    new SETR(),
    new SETI(),
    new GTIR(),
    new GTRI(),
    new GTRR(),
    new EQIR(),
    new EQRI(),
    new EQRR()
};

foreach (Operation operation in operations)
{
    opDict.Add(operation.Op, operation);
}

List<Instruction> instructions = new List<Instruction>();

int boundRegister = -1;
foreach (string line in System.IO.File.ReadAllLines(inputFile))
{
    if (line[0] == '#')
    {
        boundRegister = int.Parse(line.Substring(4));
        continue;
    }

    instructions.Add(new Instruction(line, opDict));
}

Console.WriteLine("Printing out clearer instructions");
for (instrPtr = 0; instrPtr < instructions.Count; instrPtr++)
{
    instructions[instrPtr].Print(instrPtr);
}

Console.WriteLine();
Console.WriteLine();

//Effective Program:

//    F = 0b1111011
//    
//    if (F & 0b111001000 != 0b001001000)
//    {
//        Lock;
//    }
//    
//    F = 0
//    
//    while (true)
//    {
//        E = F | 0b10000000000000000;
//        F = 12670166;
//    
//        while (true)
//        {
//            F += E % 255
//            F %= 16777215;
//            F *= 65899;
//            F %= 16777215;
//    
//            if (E < 256)
//            {
//                if (F == A)
//                {
//                    return;
//                }
//                break;
//            }
//    
//            E = (E / 256) - 1;
//        }
//    }

//So we need to stop on instruction 28 and see what the value of the Biggest Register (F) is:


Registers reg = new Registers(0, 0, 0, 0, 0, 0);

while (true)
{
    instrPtr = reg[boundRegister];

    //Halt when instrPtr exceeds instructions
    if (instrPtr >= instructions.Count)
    {
        break;
    }

    if (instrPtr == 28)
    {
        //Found it
        break;
    }

    //Execute instruction
    reg = instructions[instrPtr].Execute(reg, instrPtr);

    //Increment Instr pointer
    reg = reg.SetReg(boundRegister, reg[boundRegister] + 1);
}

Console.WriteLine($"Value of Regs at halt test: {reg}");

int value = GetMaxRegister(reg);



Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

//To find this, let's explore the pattern to F

HashSet<int> foundInts = new HashSet<int>();
int lastInt = GetMaxRegister(reg);

while (true)
{
    instrPtr = reg[boundRegister];

    //Halt when instrPtr exceeds instructions
    if (instrPtr >= instructions.Count)
    {
        break;
    }

    if (instrPtr == 28)
    {
        int newValue = GetMaxRegister(reg);
        if (!foundInts.Add(newValue))
        {
            break;
        }
        //Console.WriteLine($"{reg.F,-10}:{Convert.ToString(reg.F, 2),31}");
        lastInt = newValue;
    }

    //Execute instruction
    reg = instructions[instrPtr].Execute(reg, instrPtr);

    //Increment Instr pointer
    reg = reg.SetReg(boundRegister, reg[boundRegister] + 1);
}

Console.WriteLine($"Last value of MaxRegister at halt test: {lastInt}");
Console.WriteLine();

Console.ReadKey();


int GetMaxRegister(in Registers reg) => Math.Max(
    Math.Max(reg.A, reg.B),
    Math.Max(
        Math.Max(reg.C, reg.D),
        Math.Max(reg.E, reg.F)));


abstract class Operation
{
    public abstract string Op { get; }

    public abstract Registers Invoke(in Instruction instr, in Registers input);

    public abstract void Print(in Instruction instr, int instrPtr);

    public static string RegName(int index)
    {
        switch (index)
        {
            case 0: return "A";
            case 1: return "B";
            case 2: return "C";
            case 3: return "D";
            case 4: return "E";
            case 5: return "F";
            default: throw new Exception();
        }
    }
}

class ADDR : Operation
{
    public override string Op => "addr";

    public override Registers Invoke(in Instruction instr, in Registers input) =>
        input.SetReg(instr.C, input[instr.A] + input[instr.B]);

    public override void Print(in Instruction instr, int instrPtr) =>
        Console.WriteLine($"[{instrPtr:D2}] {RegName(instr.C)} = {RegName(instr.A)} + {RegName(instr.B)}");
}

class ADDI : Operation
{
    public override string Op => "addi";

    public override Registers Invoke(in Instruction instr, in Registers input) =>
        input.SetReg(instr.C, input[instr.A] + instr.B);

    public override void Print(in Instruction instr, int instrPtr) =>
        Console.WriteLine($"[{instrPtr:D2}] {RegName(instr.C)} = {RegName(instr.A)} + {instr.B}");
}

class MULR : Operation
{
    public override string Op => "mulr";

    public override Registers Invoke(in Instruction instr, in Registers input) =>
        input.SetReg(instr.C, input[instr.A] * input[instr.B]);

    public override void Print(in Instruction instr, int instrPtr) =>
        Console.WriteLine($"[{instrPtr:D2}] {RegName(instr.C)} = {RegName(instr.A)} * {RegName(instr.B)}");
}

class MULI : Operation
{
    public override string Op => "muli";

    public override Registers Invoke(in Instruction instr, in Registers input) =>
        input.SetReg(instr.C, input[instr.A] * instr.B);

    public override void Print(in Instruction instr, int instrPtr) =>
        Console.WriteLine($"[{instrPtr:D2}] {RegName(instr.C)} = {RegName(instr.A)} * {instr.B}");
}

class BANR : Operation
{
    public override string Op => "banr";

    public override Registers Invoke(in Instruction instr, in Registers input) =>
        input.SetReg(instr.C, input[instr.A] & input[instr.B]);

    public override void Print(in Instruction instr, int instrPtr) =>
        Console.WriteLine($"[{instrPtr:D2}] {RegName(instr.C)} = {RegName(instr.A)} & {RegName(instr.B)}");
}

class BANI : Operation
{
    public override string Op => "bani";

    public override Registers Invoke(in Instruction instr, in Registers input) =>
        input.SetReg(instr.C, input[instr.A] & instr.B);

    public override void Print(in Instruction instr, int instrPtr) =>
        Console.WriteLine($"[{instrPtr:D2}] {RegName(instr.C)} = {RegName(instr.A)} & {instr.B}");
}

class BORR : Operation
{
    public override string Op => "borr";

    public override Registers Invoke(in Instruction instr, in Registers input) =>
        input.SetReg(instr.C, input[instr.A] | input[instr.B]);

    public override void Print(in Instruction instr, int instrPtr) =>
        Console.WriteLine($"[{instrPtr:D2}] {RegName(instr.C)} = {RegName(instr.A)} | {RegName(instr.B)}");
}

class BORI : Operation
{
    public override string Op => "bori";

    public override Registers Invoke(in Instruction instr, in Registers input) =>
        input.SetReg(instr.C, input[instr.A] | instr.B);

    public override void Print(in Instruction instr, int instrPtr) =>
        Console.WriteLine($"[{instrPtr:D2}] {RegName(instr.C)} = {RegName(instr.A)} | {instr.B}");
}

class SETR : Operation
{
    public override string Op => "setr";

    public override Registers Invoke(in Instruction instr, in Registers input) =>
    input.SetReg(instr.C, input[instr.A]);

    public override void Print(in Instruction instr, int instrPtr) =>
        Console.WriteLine($"[{instrPtr:D2}] {RegName(instr.C)} = {RegName(instr.A)}");
}

class SETI : Operation
{
    public override string Op => "seti";

    public override Registers Invoke(in Instruction instr, in Registers input) =>
    input.SetReg(instr.C, instr.A);

    public override void Print(in Instruction instr, int instrPtr) =>
        Console.WriteLine($"[{instrPtr:D2}] {RegName(instr.C)} = {instr.A}");
}

class GTIR : Operation
{
    public override string Op => "gtir";

    public override Registers Invoke(in Instruction instr, in Registers input) =>
        input.SetReg(instr.C, instr.A > input[instr.B] ? 1 : 0);

    public override void Print(in Instruction instr, int instrPtr) =>
        Console.WriteLine($"[{instrPtr:D2}] {RegName(instr.C)} = {instr.A} > {RegName(instr.B)}");
}

class GTRI : Operation
{
    public override string Op => "gtri";

    public override Registers Invoke(in Instruction instr, in Registers input) =>
        input.SetReg(instr.C, input[instr.A] > instr.B ? 1 : 0);

    public override void Print(in Instruction instr, int instrPtr) =>
        Console.WriteLine($"[{instrPtr:D2}] {RegName(instr.C)} = {RegName(instr.A)} > {instr.B}");
}

class GTRR : Operation
{
    public override string Op => "gtrr";

    public override Registers Invoke(in Instruction instr, in Registers input) =>
        input.SetReg(instr.C, input[instr.A] > input[instr.B] ? 1 : 0);

    public override void Print(in Instruction instr, int instrPtr) =>
        Console.WriteLine($"[{instrPtr:D2}] {RegName(instr.C)} = {RegName(instr.A)} > {RegName(instr.B)}");
}

class EQIR : Operation
{
    public override string Op => "eqir";

    public override Registers Invoke(in Instruction instr, in Registers input) =>
        input.SetReg(instr.C, instr.A == input[instr.B] ? 1 : 0);

    public override void Print(in Instruction instr, int instrPtr) =>
        Console.WriteLine($"[{instrPtr:D2}] {RegName(instr.C)} = {instr.A} == {RegName(instr.B)}");
}

class EQRI : Operation
{
    public override string Op => "eqri";

    public override Registers Invoke(in Instruction instr, in Registers input) =>
        input.SetReg(instr.C, input[instr.A] == instr.B ? 1 : 0);

    public override void Print(in Instruction instr, int instrPtr) =>
        Console.WriteLine($"[{instrPtr:D2}] {RegName(instr.C)} = {RegName(instr.A)} == {instr.B}");
}

class EQRR : Operation
{
    public override string Op => "eqrr";

    public override Registers Invoke(in Instruction instr, in Registers input) =>
        input.SetReg(instr.C, input[instr.A] == input[instr.B] ? 1 : 0);

    public override void Print(in Instruction instr, int instrPtr) =>
        Console.WriteLine($"[{instrPtr:D2}] {RegName(instr.C)} = {RegName(instr.A)} == {RegName(instr.B)}");
}



readonly struct Instruction
{
    public readonly Operation Op;
    public readonly int A;
    public readonly int B;
    public readonly int C;

    public Instruction(string op, int a, int b, int c, Dictionary<string, Operation> opDict)
    {
        Op = opDict[op];
        A = a;
        B = b;
        C = c;
    }
    public Instruction(Operation op, int a, int b, int c)
    {
        Op = op;
        A = a;
        B = b;
        C = c;
    }

    public Instruction(string line, Dictionary<string, Operation> opDict)
    {
        string[] splitLine = line.Split(' ');
        int[] parsed = splitLine.Skip(1).Select(int.Parse).ToArray();

        Op = opDict[splitLine[0]];
        A = parsed[0];
        B = parsed[1];
        C = parsed[2];
    }

    public void Print(int instrPtr) => Op.Print(this, instrPtr);

    public Registers Execute(Registers input, int instrPtr, bool verbose = false)
    {
        if (verbose)
        {
            Op.Print(this, instrPtr);
        }


        return Op.Invoke(this, input);
    }

    public override string ToString() => $"{Op.Op} {A:X1}{B:X1}{C:X1}";
}

readonly struct Registers
{
    public readonly int A;
    public readonly int B;
    public readonly int C;
    public readonly int D;
    public readonly int E;
    public readonly int F;

    public int this[int i]
    {
        get
        {
            switch (i)
            {
                case 0: return A;
                case 1: return B;
                case 2: return C;
                case 3: return D;
                case 4: return E;
                case 5: return F;
                default: return 0;
            }
        }
    }

    public Registers(int a, int b, int c, int d, int e, int f)
    {
        A = a;
        B = b;
        C = c;
        D = d;
        E = e;
        F = f;
    }

    public static bool operator ==(Registers reg1, Registers reg2)
    {
        return reg1.A == reg2.A && reg1.B == reg2.B && reg1.C == reg2.C &&
            reg1.D == reg2.D && reg1.E == reg2.E && reg1.F == reg2.F;
    }

    public static bool operator !=(Registers reg1, Registers reg2)
    {
        return reg1.A != reg2.A || reg1.B != reg2.B || reg1.C != reg2.C ||
            reg1.D != reg2.D || reg1.E != reg2.E || reg1.F != reg2.F;
    }

    public Registers SetReg(int reg, int val) =>
        new Registers(
            a: reg == 0 ? val : A,
            b: reg == 1 ? val : B,
            c: reg == 2 ? val : C,
            d: reg == 3 ? val : D,
            e: reg == 4 ? val : E,
            f: reg == 5 ? val : F);

    public override string ToString() => $"[{A}, {B}, {C}, {D}, {E}, {F}]";

    public override bool Equals(object obj)
    {
        if (obj is not Registers other)
        {
            return false;
        }

        return this == other;
    }

    public override int GetHashCode() => HashCode.Combine(A, B, C, D, E, F);
}