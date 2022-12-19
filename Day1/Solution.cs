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

        var totalCalories = content
            .Split("\r\n\r\n")
            .Select(elfList => elfList.Split("\r\n"))
            .Select(elfCalories => 
            {
                return elfCalories
                    .Select(calorie => int.Parse(calorie))
                    .Sum();                        
            })
            .Max();


        return totalCalories;
    }
}