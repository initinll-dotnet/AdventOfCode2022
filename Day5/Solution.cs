namespace Day5;

public class Solution
{
    public async static Task Print()
    {
        Console.WriteLine("--- Day 5: Supply Stacks ---");

        await ReadCrates();

        Console.WriteLine();    
    }

    private async static Task ReadCrates()
    {
        var content = await File.ReadAllTextAsync("./Day5/input.txt");

        var stacksAndInstructions = content.Split("\r\n\r\n").ToArray();

        var stacks = stacksAndInstructions.ElementAt(0);        // stack input
        var instructions = stacksAndInstructions.ElementAt(1);  // instructions

        if (stacks.Contains("\t"))
            throw new Exception("Tabs not allowed in input, use only space");

        if (instructions.Contains("\t"))
            throw new Exception("Tabs not allowed in input, use only space");

        var stackColumns = GetStackColumns(stacks);

        var stackRows = GetStackRows(stacks);

        var data = GetStackCrates(stackColumns, stackRows);                
    }

    private static int GetStackColumns(string stacks)
    {
        // calculate no of stack columns
        var stackColumns = stacks
                .Split("\r\n")  // split stack set by newline
                .Last()         // get the last line having stack nos
                .Split(" ")     // split the last line by space
                .Where(s => !string.IsNullOrEmpty(s)) // only get nos
                .ToList()
                .AsEnumerable();

        return stackColumns.Count();
    }

    private static IEnumerable<string[]> GetStackRows(string stacks)
    {
        // calculate no of crate rows
        var stackRows = stacks
                .Split("\r\n")  // split stack set by newline
                .SkipLast(1)
                .Select(s => s.Split(" "))
                .ToList();

        return stackRows;
    }

    private static string[,] GetStackCrates(int stackColumns, IEnumerable<string[]> stackRows)
    {
        var stackRowsCount = stackRows.Count();
        var stackColumnsCount = stackColumns;

        int emptySpaceBetweenEachStack = 4;

        string[,] stackCrates = new string[stackRowsCount, stackColumnsCount];

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
                    stackCrates[stackRowIndex,stackColumnIndex] = currentStackRow[currentCrateIndex];
                    currentCrateIndex++;
                    continue;
                }

                currentCrateIndex += emptySpaceBetweenEachStack;
            } 
        }
        
        return stackCrates;
    }
}