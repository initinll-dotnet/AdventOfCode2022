namespace Day5;

public class Solution
{
    public async static Task Print()
    {
        Console.WriteLine("--- Day 5: Supply Stacks ---");

        var (rawStacks, rawInstructions) = await ReadInput();

        var stacks = FetchStacks(rawStacks);

        var instructions = FetchInstructions(rawInstructions);

        Console.WriteLine();    
    }

    private async static Task<(string rawStacks, string rawInstructions)> ReadInput()
    {
        var content = await File.ReadAllTextAsync("./Day5/input.txt");

        var stacksAndInstructions = content.Split("\r\n\r\n").ToArray();

        var rawStacks = stacksAndInstructions.ElementAt(0);        // stack input
        var rawInstructions = stacksAndInstructions.ElementAt(1);  // instructions

        if (rawStacks.Contains("\t"))
            throw new Exception("Tabs not allowed in input, use only space");

        if (rawInstructions.Contains("\t"))
            throw new Exception("Tabs not allowed in input, use only space");

        return (rawStacks, rawInstructions);
    }

    private static IEnumerable<Stack<string>> FetchStacks(string rawStacks)
    {
        var stackColumns = GetStackColumns(rawStacks);

        var stackRows = GetStackRows(rawStacks);

        var stackCratesArray = GetStackCratesArray(stackColumns, stackRows);

        var stacks = GetStacks(stackCratesArray, stackColumns);

        return stacks;
    }

    private static IEnumerable<Instruction> FetchInstructions(string rawInstructions)
    {
        var instructions = new List<Instruction>();

        foreach (var instructionLine in rawInstructions.Split("\r\n"))
        {
            var indexOfFrom = instructionLine.ToLower().IndexOf("from");
            var indexOfTo = instructionLine.ToLower().IndexOf("to");

            var moveStartIndex = 0;
            var moveLength = indexOfFrom - 1;

            var move = instructionLine
                .Substring(moveStartIndex, moveLength)
                .Split(" ")
                .Where(instruction => instruction.ToLower() != "move" && instruction != "")
                .Select(instruction => Convert.ToInt32(instruction))
                .FirstOrDefault();

            var fromStartIndex = indexOfFrom;
            var fromLength = indexOfTo - fromStartIndex;

            var from = instructionLine
                .Substring(fromStartIndex, fromLength)
                .Split(" ")
                .Where(instruction => instruction.ToLower() != "from" && instruction != "")
                .Select(instruction => Convert.ToInt32(instruction))
                .FirstOrDefault();

            var to = instructionLine
                .Substring(indexOfTo)
                .Split(" ")
                .Where(instruction => instruction.ToLower() != "to" && instruction != "")
                .Select(instruction => Convert.ToInt32(instruction))
                .FirstOrDefault();

            var instruction = new Instruction
            {
                Move = move,
                From = from,
                To = to
            };

            instructions.Add(instruction);
        }

        return instructions;
    }

    private static int GetStackColumns(string rawStacks)
    {
        // calculate no of stack columns
        var stackColumns = rawStacks
                .Split("\r\n")  // split stack set by newline
                .Last()         // get the last line having stack nos
                .Split(" ")     // split the last line by space
                .Where(s => !string.IsNullOrEmpty(s)) // only get nos
                .ToList()
                .AsEnumerable();

        return stackColumns.Count();
    }

    private static IEnumerable<string[]> GetStackRows(string rawStacks)
    {
        // calculate no of crate rows
        var stackRows = rawStacks
                .Split("\r\n")  // split stack set by newline
                .SkipLast(1)
                .Select(s => s.Split(" "))
                .ToList();

        return stackRows;
    }

    private static string[,] GetStackCratesArray(int stackColumns, IEnumerable<string[]> stackRows)
    {
        var stackRowsCount = stackRows.Count();
        var stackColumnsCount = stackColumns;

        int emptySpaceBetweenEachStack = 4;

        string[,] stackCratesArray = new string[stackRowsCount, stackColumnsCount];

        for (int stackRowIndex = 0; stackRowIndex < stackRowsCount; stackRowIndex++)
        {
            var currentStackRow = stackRows.ElementAt(stackRowIndex);
            var currentCrateIndex = 0;
            var currentStackRowTotalIndex = currentStackRow.Count() - 1;

            for (int stackColumnIndex = 0; stackColumnIndex < stackColumnsCount; stackColumnIndex++)
            {
                if (currentCrateIndex > currentStackRowTotalIndex)
                {
                    break;
                }

                if (currentStackRow[currentCrateIndex] != "")
                {
                    stackCratesArray[stackRowIndex,stackColumnIndex] = currentStackRow[currentCrateIndex];
                    currentCrateIndex++;
                    continue;
                }

                currentCrateIndex += emptySpaceBetweenEachStack;
            } 
        }
        
        return stackCratesArray;
    }

    private static IEnumerable<Stack<string>> GetStacks(string[,] stackCratesArray, int stackColumns)
    {
        var stacks = new List<Stack<string>>();

        var stackRows = stackCratesArray.Length / stackColumns;

        for (int column = 0; column < stackColumns; column++)
        {
            var stack = new Stack<string>();

            for (int row = stackRows - 1; row >= 0; row--)
            {
                var crate = stackCratesArray[row, column];

                if (crate != null)
                    stack.Push(crate);
            }

            stacks.Add(stack);
            stack = null;
        }

        return stacks;
    }
}

public class Instruction
{
    public int Move { get; set; }
    public int From { get; set; }
    public int To { get; set; }
}