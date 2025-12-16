//Initial Values

var rollspeed = 1;
var themecolor = ConsoleColor.White;
var shop = false;
var playing = true;
var wins = 0;
var losses = 0;
var coins = 0;

var game = new Game(0, 0, 0, ConsoleColor.White, Score.Normal);

//While loop for Game
while (playing)
{
    //Resets each game
    var win = false;
    var doubleScore = false;
    var tripleScore = false;
    var bonus = 0;

    var dice1 = RollDie();
    var dice2 = RollDie();
    var dice3 = RollDie();


    //Asks User if they want to play
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
            break;
        case GameChoices.Exit:
            Environment.Exit(0);
            break;
    }
    var input = Console.ReadKey();
    if (input.KeyChar == 'y')
        playing = true;
    if (input.KeyChar == 's')
        shop = true;

    // While Loop for Shop
    // I zoned out making this and i dont know how it works
    while (shop)
    {
        //Shop "UI"
        // DisplayShopMenu(coins);
        var choice = Console.ReadKey();

        //Buying Yellow Color and Changing Team
        if (choice.KeyChar == 'y' && coins >= 10)
        {
            coins -= 10;
            Console.WriteLine($"\nYou have Purchased Yellow! -10 Coins You have {coins} Coins left!\n");
            choice = Console.ReadKey();
            themecolor = ConsoleColor.Yellow;
            Console.WriteLine();
        }

        else if (choice.KeyChar == 'y')

        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\nYou Cant Afford that, You have {coins}!\n");
            choice = Console.ReadKey();
        }

        //Buying Blue Color And changing Theme
        if (choice.KeyChar == 'b' && coins >= 20)
        {
            coins -= 20;
            Console.WriteLine($"\nYou have Purchased Blue! -20 Coins You have {coins} Coins left!\n");
            choice = Console.ReadKey();
            themecolor = ConsoleColor.Blue;
        }

        else if (choice.KeyChar == 'b')
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\nYou Cant Afford that, You have {coins}!\n");
            choice = Console.ReadKey();
        }

        //Buying Yellow Color And changing Theme
        if (choice.KeyChar == 'r' && coins >= 50)
        {
            coins -= 50;
            Console.WriteLine($"\nWow! You have Purchased Red! -50?! Coins You have {coins} Coins left!\n");
            choice = Console.ReadKey();
            themecolor = ConsoleColor.Red;
        }
        else if (choice.KeyChar == 'r')
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\nYou Cant Afford that, You have {coins}!\n");
            choice = Console.ReadKey();
        }

        if (choice.KeyChar == 'x' && coins >= 30)
            //Buy Faster Rollspeed
        {
            rollspeed *= 2;
            coins -= 30;
            Console.WriteLine($"\n-30 Coins Your Roll Speed Multiplier is Now x{rollspeed}, you have {coins} left!");
            choice = Console.ReadKey();
        }

        else if (choice.KeyChar == 'x')
        {
            Console.WriteLine($"\nYou Cant Afford that, You have {coins}!\n");
            choice = Console.ReadKey();
        }


        // When Player Exits with 'e'
        if (choice.KeyChar == 'e')
        {
            shop = false;
            playing = true;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nLoading Game...\n");
            Thread.Sleep(2000);
        }

        // Option to Exit
        if (choice.KeyChar == 'n')
            Environment.Exit(0);
    }

    // Option to Exit
    if (input.KeyChar == 'n')
        Environment.Exit(0);


    //Bonus System 
    //Can give 6 Bonus Points Max
    //And 2-10 Coins
    //Double and Tripple System

    //Double Checker
    if (dice1 == dice2 || dice1 == dice3 || dice2 == dice3)
    {
        coins += 2;
        bonus += 2;
        doubleScore = true;
    }

    //Tripple Checker
    if (dice1 == dice2 && dice2 == dice3)
    {
        coins += 8;
        bonus += 4;
        doubleScore = false;
        tripleScore = true;
    }

    //Double Win Message 
    var doubleScoreMessage = doubleScore ? "You Rolled a Double! +2 And +2 Coins" : "";

    //Tripple Win Message
    var trippleScoreMessage = tripleScore ? "You Rolled a Tripple! +6 And +10 Coins" : "";


    //Score tally
    var score = dice1 + dice2 + dice3 + bonus;


    //win Detector
    if (score >= 15)
        win = true;


    //Random Roll Time
    //Roll speed Multiplier
    //"Animation"
    Random time = new();
    var rolltime = time.Next(500, 1000) / rollspeed;
    Console.WriteLine("\n\n\n\n\n\n\n......Rolling the Dice");
    rolltime = time.Next(300, 1000) / rollspeed;
    Thread.Sleep(rolltime);
    Console.ForegroundColor = themecolor;
    Console.Write($"-{dice1}-");
    rolltime = time.Next(300, 1000) / rollspeed;
    Thread.Sleep(rolltime);
    Console.Write($"\t-{dice2}-");
    rolltime = time.Next(300, 1000) / rollspeed;
    Thread.Sleep(rolltime);
    Console.Write($"\t-{dice3}-");
    rolltime = time.Next(500, 1200) / rollspeed;
    Thread.Sleep(rolltime);
    Console.ForegroundColor = ConsoleColor.White;


    //Win Statement
    if (win)
    {
        wins++;
        coins++;
        Console.WriteLine(
            @$"
    Bonus: {doubleScoreMessage}{trippleScoreMessage}
    Score: {score}
     Wins: {wins} Losses: {losses}
    + 1 Coins: {coins}
    You Won!");
    }

    //Lose Statement
    else if (!win)
    {
        losses++;
        Console.WriteLine(
            @$"
    Bonus: {doubleScoreMessage}{trippleScoreMessage}
    Score: {score}!
    Wins: {wins} Losses: {losses}
    Coins: {coins}
    You Lost!");
    }
}

return;

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
        ShopChoices.DoubleRollSpeed when game.Coins >= Price.DoubleRollSpeed => (
            game with { Coins = game.Coins - Price.DoubleRollSpeed },
            "You have purchased double roll speed for 30 coins"),
        ShopChoices.DoNotShop => (game, "You did not buy anything"),
        _ => (game, "You do not have enough coins to buy that!")
    };
}

void Write(Game game, string message)
{
    Console.ForegroundColor = game.ThemeColor;
    Console.WriteLine(message);
    Console.ResetColor();
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

enum Score
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

record Game(int Wins, int Losses, int Coins, ConsoleColor ThemeColor, Score Score);