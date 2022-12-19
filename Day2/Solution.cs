using ConsoleTables;

namespace Day2;


public enum GameMove
{
    NONE = 0,
    ROCK = 1,
    PAPER = 2,
    SCISSORS = 3
}

public enum RoundResult
{
    LOST = 0,
    DRAW = 3,
    WON = 6
}

public enum PlayerType
{
    OPPONENT,
    YOU
}

public class GameRound 
{
    public PlayerMove Opponent { get; set; }
    public PlayerMove You { get; set; }

    public string Winner 
    { 
        get
        {
            if (this.Opponent.GameMove == this.You.GameMove)
                return "DRAW";
            
            if (this.Opponent.RoundResult == RoundResult.WON)
                return "Opponent";

            if (this.You.RoundResult == RoundResult.WON)
                return "You";

            return "";
        }
    }
}

public class PlayerMove
{
    public GameMove GameMove { get; set; }

    public RoundResult RoundResult { get; set; }

    public int MoveScore => (int) this.GameMove;    

    public int RoundScore => (int) this.RoundResult;    

    public int TotalScore => this.MoveScore + this.RoundScore;
    
}

public class Solution
{
    public async static Task Print()
    {
        Console.WriteLine("--- Day 2: Rock Paper Scissors ---");

        var roundMoves = await GetRoundMoves();
        
        var gameRounds = GetGameResult(roundMoves);

        PrintResult(gameRounds);
    }

    private static void PrintResult(IEnumerable<GameRound> gameRounds)
    {
        var roundsTable = new ConsoleTable("Round","Opponent Move", "Your Move", "Winner", "Opponent Score", "Your Score");

        for (int i = 0; i < gameRounds.Count(); i++)
        {
            var round = gameRounds.ElementAt(i);

            roundsTable.AddRow(i + 1, round.Opponent.GameMove.ToString(), round.You.GameMove.ToString(), round.Winner, round.Opponent.TotalScore, round.You.TotalScore);
        }

        roundsTable.Write();
        Console.WriteLine();

        var totalScoreTable = new ConsoleTable("Opponent Total Score", "Your Total Score");

        var opponentTotalScore = gameRounds
            .Select(g => g.Opponent.TotalScore)
            .Sum();

        var yourTotalScore = gameRounds
            .Select(g => g.You.TotalScore)
            .Sum();

        totalScoreTable.AddRow(opponentTotalScore, yourTotalScore);

        totalScoreTable.Write();
        Console.WriteLine();
    }

    private async static Task<List<(string OpponentMove, string YourMove)>> GetRoundMoves()
    {
        var roundMoves = new List<(string, string)>();

        var content = await File.ReadAllTextAsync("./Day2/input.txt");
        var rounds = content.Split("\r\n").ToArray();

        foreach (var round in rounds)
        {
            var playerMoves = round.Split(" ").ToArray();

            var opponentMove = playerMoves[0]?.Trim();
            var yourMove = playerMoves[1]?.Trim();

            if (!string.IsNullOrEmpty(opponentMove) && !string.IsNullOrEmpty(yourMove))
            {
                roundMoves.Add((opponentMove, yourMove));
            }
        }

        return roundMoves;
    }

    private static List<GameRound> GetGameResult(IEnumerable<(string OpponentMove, string YourMove)> roundMoves)
    {
        List<GameRound> gameRounds = new List<GameRound>();

        foreach (var roundMove in roundMoves)
        {
            var gameRound = GetGameRound(roundMove.OpponentMove, roundMove.YourMove);

            if (gameRound is not null)
            {
                GetRoundResult(gameRound);
                gameRounds.Add(gameRound);
            }
        }

        return gameRounds;
    }

    private static GameRound GetGameRound(string opponentMove, string yourMove)
    {
        PlayerMove opponentPlayerMove = new PlayerMove
        {
            GameMove = GetPlayerMove(opponentMove)
        };

        PlayerMove yourPlayerMove = new PlayerMove
        {
            GameMove = GetPlayerMove(yourMove)
        };

        GameRound gameRound = new GameRound
        {
            Opponent = opponentPlayerMove,
            You = yourPlayerMove
        };

        if (gameRound.Opponent.GameMove == GameMove.NONE || gameRound.You.GameMove == GameMove.NONE)
            return null;

        return gameRound;
    }

    private static void GetRoundResult(GameRound gameRound)
    {
        Scenario_Draw(gameRound);

        Scenario_RockScissors(gameRound);

        Scenario_ScissorsPaper(gameRound);

        Scenario_RockPaper(gameRound);        
    }

    private static void Scenario_Draw(GameRound gameRound)
    {
        if (gameRound.Opponent.GameMove == gameRound.You.GameMove)
        {
            // Draw
            gameRound.Opponent.RoundResult = RoundResult.DRAW;
            gameRound.You.RoundResult = RoundResult.DRAW;
        }
    }

    private static void Scenario_RockScissors(GameRound gameRound)
    {
        if (gameRound.Opponent.GameMove == GameMove.ROCK && gameRound.You.GameMove == GameMove.SCISSORS)
        {
            // Rock > Scissors
            gameRound.Opponent.RoundResult = RoundResult.WON;
            gameRound.You.RoundResult = RoundResult.LOST;
        }

        if (gameRound.Opponent.GameMove == GameMove.SCISSORS && gameRound.You.GameMove == GameMove.ROCK)
        {
            // Rock > Scissors
            gameRound.Opponent.RoundResult = RoundResult.LOST;
            gameRound.You.RoundResult = RoundResult.WON;
        }
    }

    private static void Scenario_ScissorsPaper(GameRound gameRound)
    {
        if (gameRound.Opponent.GameMove == GameMove.SCISSORS && gameRound.You.GameMove == GameMove.PAPER)
        {
            // Scissors > Paper
            gameRound.Opponent.RoundResult = RoundResult.WON;
            gameRound.You.RoundResult = RoundResult.LOST;
        }

        if (gameRound.Opponent.GameMove == GameMove.PAPER && gameRound.You.GameMove == GameMove.SCISSORS)
        {
            // Scissors > Paper
            gameRound.Opponent.RoundResult = RoundResult.LOST;
            gameRound.You.RoundResult = RoundResult.WON;
        }
    }

    private static void Scenario_RockPaper(GameRound gameRound)
    {
        if (gameRound.Opponent.GameMove == GameMove.PAPER && gameRound.You.GameMove == GameMove.ROCK)
        {
            // Paper > Rock
            gameRound.Opponent.RoundResult = RoundResult.WON;
            gameRound.You.RoundResult = RoundResult.LOST;
        }

        if (gameRound.Opponent.GameMove == GameMove.ROCK && gameRound.You.GameMove == GameMove.PAPER)
        {
            // Paper > Rock
            gameRound.Opponent.RoundResult = RoundResult.LOST;
            gameRound.You.RoundResult = RoundResult.WON;
        }
    }

    private static GameMove GetPlayerMove(string playerMove)
    {
        if (string.IsNullOrEmpty(playerMove))
            return GameMove.NONE;

        if (String.Equals(playerMove, "A", StringComparison.OrdinalIgnoreCase) || 
            String.Equals(playerMove, "X", StringComparison.OrdinalIgnoreCase))
            return GameMove.ROCK;

        if (String.Equals(playerMove, "B", StringComparison.OrdinalIgnoreCase) || 
            String.Equals(playerMove, "Y", StringComparison.OrdinalIgnoreCase))
            return GameMove.PAPER;

        if (String.Equals(playerMove, "C", StringComparison.OrdinalIgnoreCase) || 
            String.Equals(playerMove, "Z", StringComparison.OrdinalIgnoreCase))
            return GameMove.SCISSORS;

        return GameMove.NONE;
    }
}


