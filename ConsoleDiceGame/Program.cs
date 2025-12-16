//Initial Values

var game = new Game(0, 0, 0, ConsoleColor.White, Speed.Normal);

//While loop for Game
while (true)
{
    DisplayGameIntro();
    switch (ChooseGameOption())
    {
        case GameChoices.Shop:
            var shopChoice = DisplayShopMenu(game);
            var (gameState, result) = Shop(game, shopChoice);
            game = gameState;
            Write(game, result);
            break;
        case GameChoices.Play:
            int[] diceRolls =
            [
                RollDie(),
                RollDie(),
                RollDie()
            ];
            var round = PlayRound(diceRolls);
            game = round.Win
                ? game with { Wins = game.Wins + 1, Coins = game.Coins + round.Coins + 1 }
                : game with { Losses = game.Losses + 1, Coins = game.Coins + round.Coins };

            DisplayRoundResults(round, game);

            break;
        case GameChoices.Exit:
            Environment.Exit(0);
            break;
    }

}

int RollDie() => Random.Shared.Next(1, 7);

void DisplayGameIntro()
{
    Console.WriteLine(
        """


        Dice Game v 1.0
        """);
}

GameChoices ChooseGameOption()
{
    Console.ResetColor();
    Console.WriteLine(
        """
        Do you want to play?
        Press any key. 
        Press 'n' to Exit
        Press 's' to Visit Shop
        """);
    return Console.ReadKey().KeyChar switch
    {
        'n' => GameChoices.Exit,
        's' => GameChoices.Shop,
        _ => GameChoices.Play
    };
}

ShopChoices DisplayShopMenu(Game game)
{
    Console.ResetColor();
    Console.WriteLine(
        $@"

Welcome to The Shop!
Here you can buy cosmetics with Coins by Entering their Character.
You have {game.Coins} Coins!
Press 'e' To go back!
");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("1.  10 Coins - Yellow Dice\t1 1 1");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("2.  20 Coins - Blue Dice:\t1 1 1");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("3.  50 Coins - Red Dice:\t1 1 1");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("4.  30 Coins - 2x Roll Speed - (Can Stack)");
    
    return Console.ReadKey().KeyChar switch
    {
        '1' => ShopChoices.YellowDice,
        '2' => ShopChoices.BlueDice,
        '3' => ShopChoices.RedDice,
        '4' => ShopChoices.DoubleRollSpeed,
        _ => ShopChoices.DoNotShop
    };
}

(Game, string) Shop(Game game, ShopChoices choice)
{
    return choice switch
    {
        ShopChoices.YellowDice when game.Coins >= Price.YellowDice => (
            game with { Coins = game.Coins - Price.YellowDice, ThemeColor = ConsoleColor.Yellow },
            "You have purchased yellow dice for 10 coins."),
        ShopChoices.BlueDice when game.Coins >= Price.BlueDice => (
            game with { Coins = game.Coins - Price.BlueDice, ThemeColor = ConsoleColor.Blue },
            "You have purchased blue dice for 20 coins"),
        ShopChoices.RedDice when game.Coins >= Price.RedDice => (
            game with { Coins = game.Coins - Price.RedDice, ThemeColor = ConsoleColor.Red },
            "You have purchased red dice for 50 coins"),
        ShopChoices.DoubleRollSpeed when game.Speed == Speed.Triple => (game, "You have reached max roll speed"),
        ShopChoices.DoubleRollSpeed when game.Coins >= Price.DoubleRollSpeed => (game with
        {
            Coins = game.Coins - Price.DoubleRollSpeed,
            Speed = game.Speed switch
            {
                Speed.Normal => Speed.Double,
                _ => Speed.Triple
            }
        }, "You have purchased double roll speed for 30 coins"),
        ShopChoices.DoNotShop => (game, "You did not buy anything"),
        _ => (game, "You do not have enough coins to buy that!")
    };
}

Round PlayRound(int[] diceRolls)
{
    var bonus = diceRolls.Pair().Select(pair => pair.Item1 == pair.Item2).Count(x => x) switch
    {
        2 => Bonus.Double,
        3 => Bonus.Triple,
        _ => Bonus.Normal
    };
    return new Round(diceRolls, bonus);
}

void Write(Game game, string message)
{
    Console.ForegroundColor = game.ThemeColor;
    Console.WriteLine(message);
    Console.ResetColor();
}

void DisplayRoundResults(Round round, Game game)
{
    Console.WriteLine("Loading round...");
    Thread.Sleep(TimeSpan.FromSeconds(2));
    Console.WriteLine("Rolling the dice...");
    foreach (var roll in round.DiceRolls)
    {
        var speed = game.Speed switch
        {
            Speed.Normal => 1,
            Speed.Double => 2,
            Speed.Triple => 4
        };
        Thread.Sleep(Random.Shared.Next(300, 1000) / speed);
        Write(game, $"\t - {roll}");
    }

    Console.WriteLine();
    if (round.Win)
        Write(game with { ThemeColor = ConsoleColor.Green }, "You won!");
    else
        Write(game with { ThemeColor = ConsoleColor.Magenta }, "You lost!");
    Console.WriteLine();
    Console.WriteLine(
        round.Bonus switch
        {
            Bonus.Normal => "You Rolled with normal dice!",
            Bonus.Double => "You Rolled a Double! +2 Points And +2 Coins",
            Bonus.Triple => "You Rolled a Tripple! +6 Points And +10 Coins"
        });
    Console.WriteLine($"Points: {round.Points}");
    Console.WriteLine($" Coins: {round.Coins}");
    Console.WriteLine($"  Wins: {game.Wins} Losses: {game.Losses}");
    Console.WriteLine();
}

enum GameChoices
{
    Play,
    Shop,
    Exit
}

enum ShopChoices
{
    YellowDice,
    BlueDice,
    RedDice,
    DoubleRollSpeed,
    DoNotShop
}

enum Speed
{
    Normal,
    Double,
    Triple
}

enum Bonus
{
    Normal,
    Double,
    Triple
}

internal static class Price
{
    public const int YellowDice = 10;
    public const int BlueDice = 20;
    public const int RedDice = 50;
    public const int DoubleRollSpeed = 30;
}

record Game(int Wins, int Losses, int Coins, ConsoleColor ThemeColor, Speed Speed);

record Round(int[] DiceRolls, Bonus Bonus)
{
    public int Points =>
        DiceRolls.Sum() +
        (Bonus switch
        {
            Bonus.Double => 2,
            Bonus.Triple => 6,
            _ => 0
        });

    public int Coins =>
        Bonus switch
        {
            Bonus.Double => 2,
            Bonus.Triple => 10,
            _ => 0
        };
    public bool Win => Points >= 15;
}

internal static class Extensions
{
    public static IEnumerable<(T, T)> Pair<T>(this IEnumerable<T> source)
    {
        var prev = source.FirstOrDefault();
        foreach (var item in source.Skip(1))
        {
            yield return (prev, item)!;
            prev = item;
        }
    }
}