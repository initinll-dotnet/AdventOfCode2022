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

        var data = GetStackCrates(stacks);                
    }

    private static string[,] GetStackCrates(string stacks)
    {
        // calculate no of stack columns
        var stackColumnsCount = stacks
                .Split("\r\n")  // split stack set by newline
                .Last()         // get the last line having stack nos
                .Split(" ")     // split the last line by space
                .Where(s => !string.IsNullOrEmpty(s)) // only get nos
                .Count(); // get total stack count

        // calculate no of crate rows
        var crateRows = stacks
                .Split("\r\n")  // split stack set by newline
                .SkipLast(1)
                .Select(s => s.Split(" "))
                .ToList();

        var crateRowsCount = crateRows.Count;

        int emptySpaceBetweenEachCrateRow = 4;

        string[,] stackCrates = new string[crateRowsCount, stackColumnsCount];

        for (int crateRowIndex = 0; crateRowIndex < crateRowsCount; crateRowIndex++)
        {
            var currentCrateRow = crateRows[crateRowIndex];
            var currentCrateIndex = 0;
            var currentCrateRowTotalIndex = currentCrateRow.Count() - 1;

            for (int stackColumnIndex = 0; stackColumnIndex < stackColumnsCount; stackColumnIndex++)
            {
                if (currentCrateIndex > currentCrateRowTotalIndex)
                {
                    break;
                }

                if (currentCrateRow[currentCrateIndex] != "")
                {
                    stackCrates[crateRowIndex,stackColumnIndex] = currentCrateRow[currentCrateIndex];
                    currentCrateIndex++;
                    continue;
                }

                currentCrateIndex += emptySpaceBetweenEachCrateRow;
            } 
        }
        
        return stackCrates;
    }
}