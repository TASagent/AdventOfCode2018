const string inputFile = @"../../../../input16.txt";


Console.WriteLine("Day 16 - Chronal Classification");
Console.WriteLine("Star 1");
Console.WriteLine();

string inputFileText = File.ReadAllText(inputFile).Replace("\r", "");
string[] splitInput = inputFileText.Split("\n\n\n\n");


string[] registerModLines = splitInput[0].Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
string[] programLines = splitInput[1].Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);


List <TestCase>[] tests = new List<TestCase>[16];
for (int i = 0; i < 16; i++)
{
    tests[i] = new List<TestCase>();
}

{
    int i = 0;

    while (i < registerModLines.Length)
    {
        TestCase newTestCase = new TestCase(registerModLines[i++], registerModLines[i++], registerModLines[i++]);

        tests[newTestCase.instruction.Op].Add(newTestCase);
    }
}

List<Operation> operations = new List<Operation>()
{
    ADDR,
    ADDI,
    MULR,
    MULI,
    BANR,
    BANI,
    BORR,
    BORI,
    SETR,
    SETI,
    GTIR,
    GTRI,
    GTRR,
    EQIR,
    EQRI,
    EQRR
};


string[] methodNames = operations.Select(x => x.Method.Name).ToArray();

TestCase headerExample = new TestCase(new Registers(0, 0, 0, 0), new Instruction(0, 0, 0, 0), new Registers(0, 0, 0, 0));

int labelOffset = $"{headerExample} Matches: ".Length;
string labelSpacer = new string(' ', labelOffset);
string header =
    $"{labelSpacer}{string.Join("  ", methodNames.Select(x => x[0]))}\n" +
    $"{labelSpacer}{string.Join("  ", methodNames.Select(x => x[1]))}\n" +
    $"{labelSpacer}{string.Join("  ", methodNames.Select(x => x[2]))}\n" +
    $"{labelSpacer}{string.Join("  ", methodNames.Select(x => x[3]))}";

/*
Console.WriteLine("Example Tests:");
Console.WriteLine();
Console.WriteLine();
Console.WriteLine(header);
Console.WriteLine();

List<TestCase>[] examples = new List<TestCase>[17];
string[] exampleLabels = new string[17];
exampleLabels[0] = "Text Examples";
for (int i = 0; i < 17; i++)
{
    examples[i] = new List<TestCase>();
    if (i > 0)
    {
        exampleLabels[i] = $"{methodNames[i - 1]} Tests";
    }
}

//Text examples
examples[0].Add(new TestCase(
    preTest: (3, 2, 1, 1),
    instruction: (9, 2, 1, 2),
    postTest: (3, 2, 2, 1)));

//My test all go (T, T, T, F) in each category
//ADDR Tests
examples[1].Add(new TestCase(
    preTest: (1, 2, 3, 4),
    instruction: (0, 2, 1, 2),
    postTest: (1, 2, 5, 4)));

examples[1].Add(new TestCase(
    preTest: (0, 1, 2, 3),
    instruction: (0, 0, 3, 1),
    postTest: (0, 3, 2, 3)));

examples[1].Add(new TestCase(
    preTest: (1, 1, 2, 3),
    instruction: (0, 3, 0, 1),
    postTest: (1, 4, 2, 3)));

examples[1].Add(new TestCase(
    preTest: (1, 1, 2, 3),
    instruction: (0, 3, 0, 1),
    postTest: (1, 3, 2, 3)));

//ADDI Tests
examples[2].Add(new TestCase(
    preTest: (1, 2, 3, 4),
    instruction: (0, 2, 1, 2),
    postTest: (1, 2, 4, 4)));

examples[2].Add(new TestCase(
    preTest: (0, 1, 2, 3),
    instruction: (0, 0, 3, 1),
    postTest: (0, 3, 2, 3)));

examples[2].Add(new TestCase(
    preTest: (1, 1, 2, 3),
    instruction: (0, 3, 0, 1),
    postTest: (1, 3, 2, 3)));

examples[2].Add(new TestCase(
    preTest: (1, 1, 2, 3),
    instruction: (0, 3, 0, 1),
    postTest: (1, 2, 2, 3)));

//MULR Tests
examples[3].Add(new TestCase(
    preTest: (1, 2, 3, 4),
    instruction: (0, 2, 1, 2),
    postTest: (1, 2, 6, 4)));

examples[3].Add(new TestCase(
    preTest: (0, 1, 2, 3),
    instruction: (0, 0, 3, 1),
    postTest: (0, 0, 2, 3)));

examples[3].Add(new TestCase(
    preTest: (1, 1, 2, 3),
    instruction: (0, 3, 0, 1),
    postTest: (1, 3, 2, 3)));

examples[3].Add(new TestCase(
    preTest: (1, 1, 2, 3),
    instruction: (0, 3, 0, 1),
    postTest: (1, 1, 2, 3)));

//MULI Tests
examples[4].Add(new TestCase(
    preTest: (1, 2, 3, 4),
    instruction: (0, 2, 1, 2),
    postTest: (1, 2, 3, 4)));

examples[4].Add(new TestCase(
    preTest: (0, 1, 2, 3),
    instruction: (0, 0, 3, 1),
    postTest: (0, 0, 2, 3)));

examples[4].Add(new TestCase(
    preTest: (1, 1, 2, 3),
    instruction: (0, 3, 0, 1),
    postTest: (1, 0, 2, 3)));

examples[4].Add(new TestCase(
    preTest: (1, 1, 2, 3),
    instruction: (0, 3, 0, 1),
    postTest: (1, 3, 2, 3)));

//BANR Tests
examples[5].Add(new TestCase(
    preTest: (0b0101, 0b1101, 0b0111, 0b0011),
    instruction: (0, 2, 1, 2),
    postTest: (0b0101, 0b1101, 0b0101, 0b0011)));

examples[5].Add(new TestCase(
    preTest: (0b0101, 0b1101, 0b0111, 0b0011),
    instruction: (0, 0, 3, 1),
    postTest: (0b0101, 0b0001, 0b0111, 0b0011)));

examples[5].Add(new TestCase(
    preTest: (0b0101, 0b1101, 0b0111, 0b0011),
    instruction: (0, 3, 0, 1),
    postTest: (0b0101, 0b0001, 0b0111, 0b0011)));

examples[5].Add(new TestCase(
    preTest: (0b0101, 0b1101, 0b0111, 0b0011),
    instruction: (0, 3, 0, 1),
    postTest: (0b0101, 0b1111, 0b0111, 0b0011)));

//BANI Tests
examples[6].Add(new TestCase(
    preTest: (0b0101, 0b1101, 0b0111, 0b0011),
    instruction: (0, 2, 0b1111, 3),
    postTest: (0b0101, 0b1101, 0b0111, 0b0111)));

examples[6].Add(new TestCase(
    preTest: (0b0101, 0b1101, 0b0111, 0b0011),
    instruction: (0, 0, 0b0000, 1),
    postTest: (0b0101, 0b0000, 0b0111, 0b0011)));

examples[6].Add(new TestCase(
    preTest: (0b0101, 0b1101, 0b0111, 0b0011),
    instruction: (0, 3, 0b0010, 1),
    postTest: (0b0101, 0b0010, 0b0111, 0b0011)));

examples[6].Add(new TestCase(
    preTest: (0b0101, 0b1101, 0b0111, 0b0011),
    instruction: (0, 1, 0b0010, 1),
    postTest: (0b0101, 0b0010, 0b0111, 0b0011)));

//BORR Tests
examples[7].Add(new TestCase(
    preTest: (0b0101, 0b1101, 0b0111, 0b0011),
    instruction: (0, 2, 1, 2),
    postTest: (0b0101, 0b1101, 0b1111, 0b0011)));

examples[7].Add(new TestCase(
    preTest: (0b0101, 0b1101, 0b0111, 0b0011),
    instruction: (0, 0, 3, 1),
    postTest: (0b0101, 0b0111, 0b0111, 0b0011)));

examples[7].Add(new TestCase(
    preTest: (0b0101, 0b1101, 0b0111, 0b0011),
    instruction: (0, 3, 0, 1),
    postTest: (0b0101, 0b0111, 0b0111, 0b0011)));

examples[7].Add(new TestCase(
    preTest: (0b0101, 0b1101, 0b0111, 0b0011),
    instruction: (0, 3, 0, 1),
    postTest: (0b0101, 0b1111, 0b0111, 0b0011)));

//BORI Tests
examples[8].Add(new TestCase(
    preTest: (0b0101, 0b1101, 0b0111, 0b0011),
    instruction: (0, 2, 0b1111, 3),
    postTest: (0b0101, 0b1101, 0b0111, 0b1111)));

examples[8].Add(new TestCase(
    preTest: (0b0101, 0b1101, 0b0111, 0b0011),
    instruction: (0, 0, 0b0000, 1),
    postTest: (0b0101, 0b0101, 0b0111, 0b0011)));

examples[8].Add(new TestCase(
    preTest: (0b0101, 0b1101, 0b0111, 0b0011),
    instruction: (0, 3, 0b0111, 1),
    postTest: (0b0101, 0b0111, 0b0111, 0b0011)));

examples[8].Add(new TestCase(
    preTest: (0b0101, 0b1101, 0b0111, 0b0011),
    instruction: (0, 1, 0b0010, 1),
    postTest: (0b0101, 0b0010, 0b0111, 0b0011)));

//SETR Tests
examples[9].Add(new TestCase(
    preTest: (1, 2, 3, 4),
    instruction: (0, 2, 1, 2),
    postTest: (1, 2, 3, 4)));

examples[9].Add(new TestCase(
    preTest: (0, 1, 2, 3),
    instruction: (0, 0, 3, 1),
    postTest: (0, 0, 2, 3)));

examples[9].Add(new TestCase(
    preTest: (1, 1, 2, 3),
    instruction: (0, 3, 0, 1),
    postTest: (1, 3, 2, 3)));

examples[9].Add(new TestCase(
    preTest: (1, 1, 2, 3),
    instruction: (0, 3, 0, 1),
    postTest: (1, 1, 2, 3)));

//SETI Tests
examples[10].Add(new TestCase(
    preTest: (1, 2, 3, 4),
    instruction: (0, 2, 1, 2),
    postTest: (1, 2, 2, 4)));

examples[10].Add(new TestCase(
    preTest: (0, 1, 2, 3),
    instruction: (0, 0, 3, 1),
    postTest: (0, 0, 2, 3)));

examples[10].Add(new TestCase(
    preTest: (1, 1, 2, 3),
    instruction: (0, 0, 0, 1),
    postTest: (1, 0, 2, 3)));

examples[10].Add(new TestCase(
    preTest: (1, 1, 2, 3),
    instruction: (0, 3, 0, 1),
    postTest: (1, 1, 2, 3)));

//GTIR Tests

//GTRI Tests

//GTRR Tests

//EQIR Tests

//EQRI Tests

//EQRR Tests

for (int i = 0; i < 17; i++)
{
    Console.WriteLine(exampleLabels[i]);
    foreach (TestCase example in examples[i])
    {
        var exampleMatches = operations.Select(example.TestOperation);

        Console.WriteLine($"{example} Matches: {string.Join(", ", exampleMatches.Select(x => x ? "1" : "0"))}");
    }
}

Console.WriteLine();
Console.WriteLine("Real Test:");
Console.WriteLine();
Console.WriteLine();
*/

Console.WriteLine();
Console.WriteLine(header);
Console.WriteLine();


int sampleMatch = 0;

for (int op = 0; op < 16; op++)
{
    foreach (TestCase testCase in tests[op])
    {
        var matches = operations.Select(testCase.TestOperation);

        Console.WriteLine($"{testCase} Matches: {string.Join(", ", matches.Select(x => x ? "1" : "0"))}");

        if (matches.Count(x => x) >= 3)
        {
            sampleMatch++;
        }
    }
}



Console.WriteLine($"{sampleMatch} samples in the puzzle input behave like 3 or more opcodes");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

//Find opcode mapping

bool[,] opCodeMatrix = new bool[16, 16];

for (int i = 0; i < 16; i++)
{
    for (int j = 0; j < 16; j++)
    {
        opCodeMatrix[i, j] = true;
    }
}


for (int op = 0; op < 16; op++)
{
    foreach (TestCase testCase in tests[op])
    {
        bool[] matches = operations.Select(testCase.TestOperation).ToArray();

        for (int i = 0; i < 16; i++)
        {
            opCodeMatrix[op, i] &= matches[i];
        }
    }
}

Dictionary<int, Operation> opTable = new Dictionary<int, Operation>();

while (opTable.Count < 16)
{
    int nextOpCode = -1;
    int nextOperation = -1;
    for (int opCode = 0; opCode < 16; opCode++)
    {
        if (opTable.ContainsKey(opCode))
        {
            continue;
        }

        int lastFoundOperation = -1;

        int matches = 0;

        for (int operation = 0; operation < 16; operation++)
        {
            if (opCodeMatrix[opCode, operation])
            {
                lastFoundOperation = operation;
                matches++;
            }
        }

        if (matches == 1)
        {
            nextOpCode = opCode;
            nextOperation = lastFoundOperation;

            for (int innerOpCode = 0; innerOpCode < 16; innerOpCode++)
            {
                if (innerOpCode == nextOpCode)
                {
                    continue;
                }

                opCodeMatrix[innerOpCode, nextOperation] = false;
            }

            opTable.Add(nextOpCode, operations[nextOperation]);

            break;
        }
    }

    if (nextOpCode == -1)
    {
        for (int operation = 0; operation < 16; operation++)
        {
            if (opTable.ContainsValue(operations[operation]))
            {
                continue;
            }

            int lastFoundOpCode = -1;

            int matches = 0;

            for (int opCode = 0; opCode < 16; opCode++)
            {
                if (opCodeMatrix[opCode, operation])
                {
                    lastFoundOpCode = opCode;
                    matches++;
                }
            }

            if (matches == 1)
            {
                nextOpCode = lastFoundOpCode;
                nextOperation = operation;

                for (int innerOperation = 0; innerOperation < 16; innerOperation++)
                {
                    if (innerOperation == nextOperation)
                    {
                        continue;
                    }

                    opCodeMatrix[nextOpCode, innerOperation] = false;
                }

                opTable.Add(nextOpCode, operations[nextOperation]);

                break;
            }
        }
    }

    if (nextOpCode == -1)
    {
        throw new Exception();
    }
}

Registers reg = new Registers(0, 0, 0, 0);

List<Instruction> instructions = programLines.Select(x => new Instruction(x)).ToList();

foreach (Instruction instr in instructions)
{
    reg = opTable[instr.Op](instr, reg);
}



Console.WriteLine($"Registers A At End: [{reg.A}]");

Console.ReadKey();

static Registers ADDR(in Instruction instr, in Registers input) => input.SetReg(instr.C, input[instr.A] + input[instr.B]);
static Registers ADDI(in Instruction instr, in Registers input) => input.SetReg(instr.C, input[instr.A] + instr.B);
static Registers MULR(in Instruction instr, in Registers input) => input.SetReg(instr.C, input[instr.A] * input[instr.B]);
static Registers MULI(in Instruction instr, in Registers input) => input.SetReg(instr.C, input[instr.A] * instr.B);
static Registers BANR(in Instruction instr, in Registers input) => input.SetReg(instr.C, input[instr.A] & input[instr.B]);
static Registers BANI(in Instruction instr, in Registers input) => input.SetReg(instr.C, input[instr.A] & instr.B);
static Registers BORR(in Instruction instr, in Registers input) => input.SetReg(instr.C, input[instr.A] | input[instr.B]);
static Registers BORI(in Instruction instr, in Registers input) => input.SetReg(instr.C, input[instr.A] | instr.B);
static Registers SETR(in Instruction instr, in Registers input) => input.SetReg(instr.C, input[instr.A]);
static Registers SETI(in Instruction instr, in Registers input) => input.SetReg(instr.C, instr.A);
static Registers GTIR(in Instruction instr, in Registers input) => input.SetReg(instr.C, instr.A > input[instr.B] ? 1 : 0);
static Registers GTRI(in Instruction instr, in Registers input) => input.SetReg(instr.C, input[instr.A] > instr.B ? 1 : 0);
static Registers GTRR(in Instruction instr, in Registers input) => input.SetReg(instr.C, input[instr.A] > input[instr.B] ? 1 : 0);
static Registers EQIR(in Instruction instr, in Registers input) => input.SetReg(instr.C, instr.A == input[instr.B] ? 1 : 0);
static Registers EQRI(in Instruction instr, in Registers input) => input.SetReg(instr.C, input[instr.A] == instr.B ? 1 : 0);
static Registers EQRR(in Instruction instr, in Registers input) => input.SetReg(instr.C, input[instr.A] == input[instr.B] ? 1 : 0);

delegate Registers Operation(in Instruction instr, in Registers input);

class TestCase
{
    public readonly Registers preTest;
    public readonly Registers postTest;
    public readonly Instruction instruction;

    public TestCase(string preTest, string instruction, string postTest)
    {
        this.preTest = new Registers(preTest);
        this.instruction = new Instruction(instruction);
        this.postTest = new Registers(postTest);
    }

    public TestCase(in Registers preTest, in Instruction instruction, in Registers postTest)
    {
        this.preTest = preTest;
        this.instruction = instruction;
        this.postTest = postTest;
    }

    public TestCase((int a, int b, int c, int d) preTest, (int op, int a, int b, int c) instruction, (int a, int b, int c, int d) postTest)
    {
        this.preTest = new Registers(preTest.a, preTest.b, preTest.c, preTest.d);
        this.instruction = new Instruction(instruction.op, instruction.a, instruction.b, instruction.c);
        this.postTest = new Registers(postTest.a, postTest.b, postTest.c, postTest.d);
    }


    public bool TestOperation(Operation operation)
    {
        return operation(instruction, preTest) == postTest;
    }

    public override string ToString()
    {
        return $"[{preTest}]{instruction}[{postTest}]";
    }

}

readonly struct Instruction
{
    public readonly int Op;
    public readonly int A;
    public readonly int B;
    public readonly int C;

    public Instruction(int op, int a, int b, int c)
    {
        Op = op;
        A = a;
        B = b;
        C = c;
    }

    public Instruction(string line)
    {
        int[] parsed = line.Split(' ').Select(int.Parse).ToArray();

        Op = parsed[0];
        A = parsed[1];
        B = parsed[2];
        C = parsed[3];
    }

    public override string ToString()
    {
        return $"{Op:X1}{A:X1}{B:X1}{C:X1}";
    }

}

readonly struct Registers
{
    public readonly int A;
    public readonly int B;
    public readonly int C;
    public readonly int D;

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
                default: return 0;
            }
        }
    }

    public Registers(int a, int b, int c, int d)
    {
        A = a;
        B = b;
        C = c;
        D = d;
    }

    public Registers(string line)
    {
        int[] parsed = line.Substring(9, 10).Split(',').Select(int.Parse).ToArray();

        A = parsed[0];
        B = parsed[1];
        C = parsed[2];
        D = parsed[3];
    }

    public static bool operator ==(in Registers reg1, in Registers reg2) =>
        reg1.A == reg2.A && reg1.B == reg2.B && reg1.C == reg2.C && reg1.D == reg2.D;

    public static bool operator !=(in Registers reg1, in Registers reg2) =>
        reg1.A != reg2.A || reg1.B != reg2.B || reg1.C != reg2.C || reg1.D != reg2.D;

    public Registers SetReg(int reg, int val) =>
        new Registers(
            a: reg == 0 ? val : A,
            b: reg == 1 ? val : B,
            c: reg == 2 ? val : C,
            d: reg == 3 ? val : D);

    public override string ToString()
    {
        return $"{A:X1}{B:X1}{C:X1}{D:X1}";
    }

    public override bool Equals(object obj)
    {
        if (obj is not Registers regs)
        {
            return false;
        }
        return regs == this;
    }

    public override int GetHashCode() => HashCode.Combine(A, B, C, D);
}