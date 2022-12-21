using System.Runtime.CompilerServices;
using ConsoleTables;

namespace Day6;

public class Solution
{
    public async static Task Print()
    {
        // sequence of characters that are all different 
        var startOfPacketMarkerCharactersCount = 4;
        
        Console.WriteLine("--- Day 6: Tuning Trouble ---");

        var datastreams = await ReadInput();

        var markerTable = new ConsoleTable("Sequence of characters that are all different");
        markerTable.AddRow(startOfPacketMarkerCharactersCount);

        var messageTable = new ConsoleTable("Message", "Start of Packet");

        foreach (var datastream in datastreams)
        {   
            var marker = await ConsumeDataStream(datastream, startOfPacketMarkerCharactersCount);
            messageTable.AddRow(datastream, marker);
        }

        markerTable.Write();
        messageTable.Write();
        Console.WriteLine();
    }

    private async static Task<IEnumerable<string>> ReadInput()
    {
        var content = await File.ReadAllTextAsync("./Day6/input.txt");

        var datastreams = content.Split("\r\n").ToArray();

        return datastreams;
    }

    private async static IAsyncEnumerable<char> RelayDataStream(string datastream, [EnumeratorCancellation]CancellationToken cancellationToken = default)
    {
        var stream = await Task.Run(() => datastream.ToCharArray(), cancellationToken);

        foreach (var item in stream)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
                
            //await Task.Delay(1000);
            yield return item;
        }
    }

    private async static Task<int> ConsumeDataStream(string datastream, int startOfPacketMarkerCharactersCount)
    {
        var data = string.Empty;
        bool isMarkerFound = false;

        var cancellationTokenSource = new CancellationTokenSource();

        await foreach (var item in RelayDataStream(datastream).WithCancellation(cancellationTokenSource.Token))
        {
            data += item;
            isMarkerFound = DetectMarker(data, startOfPacketMarkerCharactersCount);           

            if (isMarkerFound)
                cancellationTokenSource.Cancel();
        }

        return data.Length;
    }

    private static bool DetectMarker(string data, int markerRule)
    {
        var compactData = data;

        if (data.Length > markerRule)
        {
            compactData = data.Substring(data.Length - markerRule, markerRule);
        }

        if (compactData.Length == markerRule)
        {
            var messageDuplicateCharatersCount = compactData
                .GroupBy(d => d)
                .Where(g => g.Count() > 1)
                .ToList()
                .Count();

            compactData = null;

            if (messageDuplicateCharatersCount == 0)
                return true;
        }

        return false;
    }
}