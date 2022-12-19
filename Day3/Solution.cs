using ConsoleTables;

namespace Day3;

public class Solution 
{
    public async static Task Print()
    {
        Console.WriteLine("--- Day 3: Rucksack Reorganization ---");

        var supplies = await GetRucksacksSupplies();
        var prioritiesSum = GetScore(supplies);

        var priorityTable = new ConsoleTable("Priority Sum");
        priorityTable.AddRow(prioritiesSum);

        priorityTable.Write();        
        Console.WriteLine();        
    }

    private async static Task<IEnumerable<string>> GetRucksacksSupplies()
    {
        var rucksacksSupplies = Enumerable.Empty<string>();

        var content = await File.ReadAllTextAsync("./Day3/input.txt");
        rucksacksSupplies = content.Split("\r\n").ToArray();

        return rucksacksSupplies;
    }

    private static IEnumerable<char> GetDuplicateItems(string supplyItems)
    {
        var duplicateItems = Enumerable.Empty<char>();

        if (string.IsNullOrEmpty(supplyItems))
            return duplicateItems;

        var totalItems = supplyItems.ToCharArray().Count();

        var compartment1_EndIndex = (totalItems / 2) - 1;
        var compartment2_StartIndex = (totalItems / 2);

        var compartment1_Items = supplyItems.Substring(0, compartment1_EndIndex);
        var compartment2_Items = supplyItems.Substring(compartment2_StartIndex);

        duplicateItems = compartment1_Items.Intersect(compartment2_Items).ToList();

        return duplicateItems;        
    }

    private static int GetScore(IEnumerable<string> supplies)
    {
        var prioritiesSum = supplies
                .Select(supply => GetDuplicateItems(supply))
                .SelectMany(items => items)
                .Select(duplicateItem => GetItemPriority(duplicateItem))
                .Sum();

        return prioritiesSum;
    }

    private static int GetItemPriority(char item)
    {
        if (item >= 'a' && item <= 'z')
            return (int)item - 96;
        
        if (item >= 'A' && item <= 'Z')
            return (int)item - 38;

        return 0;
    }
}