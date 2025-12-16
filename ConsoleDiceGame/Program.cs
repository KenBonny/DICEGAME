//Initial Values

var gameEvents = new List<GameEvent>();

DisplayGameIntro();
//While loop for Game
while (true)
{
    var roundEvents = new List<GameEvent>();
    roundEvents.Add(ChooseGameOption());
    switch (roundEvents.Last())
    {
        case global::Shop:
            var game = new GameState(gameEvents).Game;
            var shopChoice = DisplayShopMenu(game);
            var choice = Shop(game, shopChoice);
            roundEvents.Add(choice);
            break;
        case Play:
            roundEvents.Add(
                new DiceRolled(
                [
                    RollDie(),
                    RollDie(),
                    RollDie()
                ]));
            break;
        case Exit:
            roundEvents.Add(new Exit());
            break;
    }
    
    gameEvents.AddRange(roundEvents);
    foreach (var roundEvent in roundEvents)
    {
        switch (roundEvent)
        {
            case ShopChoice choice:
                DisplayShopChoice(choice);
                break;
            case DiceRolled round:
                var game = new GameState(gameEvents).Game;
                DisplayRoundResults(round, game);
                break;
            case Exit:
                Environment.Exit(0);
                break;
        }
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

GameEvent ChooseGameOption()
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
        'n' => new Exit(),
        's' => new Shop(),
        _ => new Play()
    };
}

ShopChoices DisplayShopMenu(Game game)
{
    Console.ResetColor();
    Console.WriteLine(
        $"""


         Welcome to The Shop!
         Here you can buy cosmetics with Coins by Entering their Character.
         You have {game.Coins} Coins!
         Press 'e' To go back!

         """);
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("1.  10 Coins - Yellow Dice\t1 1 1");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("2.  20 Coins - Blue Dice:\t1 1 1");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("3.  50 Coins - Red Dice:\t1 1 1");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("4.  30 Coins - 2x Roll Speed - (Can Stack)");
    
    return Console.ReadKey().KeyChar switch
    {
        '1' => ShopChoices.YellowDice,
        '2' => ShopChoices.BlueDice,
        '3' => ShopChoices.RedDice,
        '4' => ShopChoices.DoubleRollSpeed,
        _ => ShopChoices.DoNotShop
    };
}

ShopChoice Shop(Game game, ShopChoices choice)
{
    return choice switch
    {
        ShopChoices.YellowDice when game.Coins >= Price.YellowDice => new BoughtThemedDice(ConsoleColor.Yellow, Price.YellowDice),
        ShopChoices.BlueDice when game.Coins >= Price.BlueDice => new BoughtThemedDice(ConsoleColor.Blue, Price.BlueDice),
        ShopChoices.RedDice when game.Coins >= Price.RedDice => new BoughtThemedDice(ConsoleColor.Red, Price.RedDice),
        ShopChoices.DoubleRollSpeed when game.Speed == Speed.Triple => game.Speed switch
        {
            Speed.Normal => new BoughtDoubleSpeed(Price.DoubleRollSpeed),
            _ => new BoughtTripleSpeed(Price.DoubleRollSpeed),
        },
        ShopChoices.DoNotShop => new BoughtNothing(),
        _ => new CouldNotBuy(choice)
    };
}

void DisplayShopChoice(ShopChoice choice)
{
    switch (choice)
    {
        case BoughtThemedDice x:
            WriteLine(x.Color, $"You have purchased {x.Color} dice for {x.Price} coins!");
            break;
        case BoughtDoubleSpeed x:
            Console.WriteLine($"You have purchased double roll speed for {x.Price} coins!");
            break;
        case BoughtTripleSpeed x:
            Console.WriteLine($"You have purchased triple roll speed for {x.Price} coins!");
            break;
        case CouldNotBuy x:
            var message = x.Choice switch
            {
                ShopChoices.DoubleRollSpeed => "double roll speed",
                ShopChoices.BlueDice => "blue dice",
                ShopChoices.RedDice => "red dice",
                ShopChoices.YellowDice => "yellow dice",
                _ => x.Choice.ToString()
            };
            Console.WriteLine($"You could not buy {message} because you do not have enough coins!");
            break;
        case MaxSpeedReached:
            Console.WriteLine("You have reached max roll speed!");
            break;
        default:
            Console.WriteLine();
            break;
    }
}

void Write(Game game, string message) => WriteLine(game.ThemeColor, message);

void WriteLine(ConsoleColor color, string message) 
{
    Console.ForegroundColor = color;
    Console.WriteLine(message);
    Console.ResetColor();
}

void DisplayRoundResults(DiceRolled diceRolled, Game game)
{
    Console.WriteLine("Loading round...");
    Thread.Sleep(TimeSpan.FromSeconds(2));
    Console.WriteLine("Rolling the dice...");
    foreach (var roll in diceRolled.DiceRolls)
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
    if (diceRolled.Win)
        Write(game with { ThemeColor = ConsoleColor.Green }, "You won!");
    else
        Write(game with { ThemeColor = ConsoleColor.Magenta }, "You lost!");
    Console.WriteLine();
    Console.WriteLine(
        diceRolled.Bonus switch
        {
            Bonus.Normal => "You Rolled with normal dice!",
            Bonus.Double => "You Rolled a Double! +2 Points And +2 Coins",
            Bonus.Triple => "You Rolled a Tripple! +6 Points And +10 Coins"
        });
    Console.WriteLine($"Points: {diceRolled.Points}");
    Console.WriteLine($" Coins: {game.Coins} (+{diceRolled.Coins} this round)");
    Console.WriteLine($"  Wins: {game.Wins} Losses: {game.Losses}");
    Console.WriteLine();
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

internal class GameState
{
    public GameState(List<GameEvent> gameEvents)
    {
        var game = new Game(0, 0, 0, ConsoleColor.White, Speed.Normal);
        game = gameEvents.Aggregate(
            game,
            (current, @event) => @event switch
            {
                DiceRolled dicedRolled => dicedRolled.Win
                    ? current with { Wins = current.Wins + 1, Coins = current.Coins + dicedRolled.Coins }
                    : current with { Losses = current.Losses + 1, Coins = current.Coins + dicedRolled.Coins },
                BoughtThemedDice boughtThemedDice => current with
                {
                    ThemeColor = boughtThemedDice.Color, Coins = current.Coins - boughtThemedDice.Price
                },
                _ => current
            });

        Game = game;
    }

    public Game Game { get; }
}

record DiceRolled : GameEvent
{
    public DiceRolled(int[] diceRolls)
    {
        DiceRolls = diceRolls;
        Bonus = DiceRolls.Pair().Select(pair => pair.Item1 == pair.Item2).Count(x => x) switch
        {
            1 => Bonus.Double,
            3 => Bonus.Triple,
            _ => Bonus.Normal
        };
        Points = DiceRolls.Sum() +
            Bonus switch
            {
                Bonus.Double => 2,
                Bonus.Triple => 6,
                _ => 0
            };
        Win = Points >= 15;
        var bonusCoins = Bonus switch
        {
            Bonus.Double => 2,
            Bonus.Triple => 10,
            _ => 0
        };
        Coins = (Win ? 1 : 0) + bonusCoins;
    }

    public int[] DiceRolls { get; }
    public Bonus Bonus { get; }
    public int Points { get; }
    public bool Win { get; }
    public int Coins { get; }
}

internal static class Extensions
{
    public static IEnumerable<(T, T)> Pair<T>(this IEnumerable<T> source)
    {
        foreach (var item in source.Select((value, i) => new { value, i}))
        {
            foreach (var item2 in source.Skip(item.i + 1))
                yield return (item.value, item2);
        }
    }
}

internal abstract record GameEvent;

internal record Exit : GameEvent;
internal record Play : GameEvent;
internal record Shop : GameEvent;

internal abstract record ShopChoice : GameEvent;
internal record BoughtThemedDice(ConsoleColor Color, int Price) : ShopChoice;
internal record BoughtDoubleSpeed(int Price) : ShopChoice;
internal record BoughtTripleSpeed(int Price) : ShopChoice;

internal record CouldNotBuy(ShopChoices Choice) : ShopChoice;
internal record BoughtNothing : ShopChoice;
internal record MaxSpeedReached : ShopChoice;
