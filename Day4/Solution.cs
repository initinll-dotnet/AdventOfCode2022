using ConsoleTables;

namespace Day4;

public class Solution
{
    public async static Task Print()
    {
        Console.WriteLine("--- Day 4: Camp Cleanup ---");

        var assignmentPairs = await GetAssignmentPairs();

        var pairCount = GetPairCount(assignmentPairs);

        var assignmentTable = new ConsoleTable("Assignment pairs does one range fully contain the other");
        assignmentTable.AddRow(pairCount);

        assignmentTable.Write();        
        Console.WriteLine();    
    }

    private async static Task<IEnumerable<string>> GetAssignmentPairs()
    {
        var pairs = Enumerable.Empty<string>();

        var content = await File.ReadAllTextAsync("./Day4/input.txt");
        pairs = content.Split("\r\n").ToArray();

        return pairs;
    }

    private static int GetPairCount(IEnumerable<string> assignmentPairs)
    {
        int pairCount = 0;

        foreach (var assignmentPair in assignmentPairs)
        {
            var assignments = assignmentPair.Split(",");
            var range1 = assignments.ElementAt(0);
            var range2 = assignments.ElementAt(1);
            var isContainsRange = ContainsRange(range1, range2);

            if (isContainsRange)
                pairCount++;
        }

        return pairCount;
    }

    private static bool ContainsRange(string range1, string range2)
    {
        var range1_limits = range1.Split("-");
        var range1_lowerlimit = Convert.ToInt32(range1_limits.ElementAt(0));
        var range1_upperlimit = Convert.ToInt32(range1_limits.ElementAt(1));

        var range2_limits = range2.Split("-");
        var range2_lowerlimit = Convert.ToInt32(range2_limits.ElementAt(0));
        var range2_upperlimit = Convert.ToInt32(range2_limits.ElementAt(1));

        if ((range1_lowerlimit >= range2_lowerlimit && range1_lowerlimit <= range2_upperlimit) &&
            (range1_upperlimit >= range2_lowerlimit && range1_upperlimit <= range2_upperlimit))
        {
            return true;
        }

        if ((range2_lowerlimit >= range1_lowerlimit && range2_lowerlimit <= range1_upperlimit) &&
            (range2_upperlimit >= range1_lowerlimit && range2_upperlimit <= range1_upperlimit))
        {
            return true;
        }

        return false;
    }
}