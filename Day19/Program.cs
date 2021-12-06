const string inputFile = @"../../../../input19.txt";


Console.WriteLine("Day 19 - Go With The Flow");
Console.WriteLine("Star 1");
Console.WriteLine();


Dictionary<string, Operation> opDict = new Dictionary<string, Operation>();
List<Instruction> instructions;

int instrPtr = 0;

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

instructions = new List<Instruction>();

int boundRegister = -1;
foreach (string line in System.IO.File.ReadAllLines(inputFile))
{
    if (line[0] == '#')
    {
        boundRegister = int.Parse(line[4].ToString());
        continue;
    }

    instructions.Add(new Instruction(line, opDict));
}

Registers reg = new Registers(0, 0, 0, 0, 0, 0);

while (true)
{
    instrPtr = reg[boundRegister];

    //Halt when instrPtr exceeds instructions
    if (instrPtr >= instructions.Count)
    {
        break;
    }

    //Execute instruction
    reg = instructions[instrPtr].Execute(reg, instrPtr);

    //Increment Instr pointer
    reg = reg.SetReg(boundRegister, reg[boundRegister] + 1);
}

Console.WriteLine($"Register 0 At End: [{reg.A}]");
Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();
Console.WriteLine();

//Console.WriteLine("Printing out clearer instructions");
//for (instrPtr = 0; instrPtr < instructions.Count; instrPtr++)
//{
//    instructions[instrPtr].Print(instrPtr);
//}

Console.WriteLine();
Console.WriteLine();

instrPtr = 0;
reg = new Registers(1, 0, 0, 0, 0, 0);

for (int i = 0; i < 1000; i++)
{
    instrPtr = reg[boundRegister];

    //Halt when instrPtr exceeds instructions
    if (instrPtr >= instructions.Count)
    {
        break;
    }

    Console.Write(reg);
    //Execute instruction
    reg = instructions[instrPtr].Execute(reg, instrPtr, true);

    //Increment Instr pointer
    reg = reg.SetReg(boundRegister, reg[boundRegister] + 1);
}

int target = Math.Max(
        Math.Max(reg.B, reg.C),
        Math.Max(
            Math.Max(reg.D, reg.E),
            reg.F));

Console.WriteLine($"Target value: {target}");
Console.WriteLine();

//This program sums the divisors of the number that ends up in one of the registers
int answer = 0;
for (int i = 1; i <= target; i++)
{
    if (target % i == 0)
    {
        Console.WriteLine(i);
        answer += i;
    }
}
Console.WriteLine();


Console.WriteLine($"Register 0 at the end: [{answer}]");
Console.WriteLine();
Console.ReadKey();



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

    public Registers Execute(in Registers input, int instrPtr, bool verbose = false)
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

    public static bool operator ==(in Registers reg1, in Registers reg2)
    {
        return reg1.A == reg2.A && reg1.B == reg2.B && reg1.C == reg2.C &&
            reg1.D == reg2.D && reg1.E == reg2.E && reg1.F == reg2.F;
    }

    public static bool operator !=(in Registers reg1, in Registers reg2)
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