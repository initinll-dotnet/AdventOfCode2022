using System.IO;
using ConsoleTables;

namespace Day1;

public class Solution
{

    public async static Task Print()
    {
        Console.WriteLine();
        Console.WriteLine("--- Day 1: Calorie Counting ---");
        
        var maxCalorie = await GetMaxCalories();

        var caloriesTable = new ConsoleTable("Max Calorie");
        caloriesTable.AddRow(maxCalorie);

        caloriesTable.Write();
        Console.WriteLine();
    }

    private async static Task<int> GetMaxCalories()
    {
        var content = await File.ReadAllTextAsync("./Day1/input.txt");

        var maxCalorie = content
            .Split("\r\n\r\n")
            .Select(c =>
            {
                var elfsCal = c
                    .Split("\r\n")
                    .Select(cal => int.Parse(cal))
                    .Sum();
                return elfsCal;
            }).Max();

        return maxCalorie;
    }
}