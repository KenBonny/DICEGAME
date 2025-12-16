//Initial Values

int bonus;
int score;
var rollspeed = 1;
var themecolor = ConsoleColor.White;
var shop = false;
var win = false;
var playing = true;
var wins = 0;
var losses = 0;
var coins = 0;
bool Double;
bool Tripple;
string DoubleWin;
string TrippleWin;


//While loop for Game
while (playing)
{
    //Resets each game
    win = false;
    Double = false;
    Tripple = false;
    bonus = 0;
    score = 0;

    //Randomizing each dice 1-6
    Random dice = new();
    var dice1 = dice.Next(1, 7);
    var dice2 = dice.Next(1, 7);
    var dice3 = dice.Next(1, 7);


    //Asks User if they want to play
    Console.WriteLine(
        "\n\nDice Game v 1.0\nDo you want to play? Press any key. \nPress 'n' to Exit\nPress 's' to Visit Shop");
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
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(
            $"\n\n\n\n\nWelcome to The Shop!\nHere you can buy cosmetics with Coins by Entering their Character.\nYou have {coins} Coins!\nPress 'e' To go back!\n");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("'y'  10 Coins - Yellow Dice\t1 1 1");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("\n'b'  20 Coins - Blue Dice:\t1 1 1");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("\n'r'  50 Coins - Red Dice:\t1 1 1");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("\n'x'  30 Coins - 2x Roll Speed - (Can Stack)\n");
        var Choice = Console.ReadKey();

        //Buying Yellow Color and Changing Team
        if (Choice.KeyChar == 'y' && coins >= 10)
        {
            coins -= 10;
            Console.WriteLine($"\nYou have Purchased Yellow! -10 Coins You have {coins} Coins left!\n");
            Choice = Console.ReadKey();
            themecolor = ConsoleColor.Yellow;
            Console.WriteLine();
        }

        else if (Choice.KeyChar == 'y')

        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\nYou Cant Afford that, You have {coins}!\n");
            Choice = Console.ReadKey();
        }

        //Buying Blue Color And changing Theme
        if (Choice.KeyChar == 'b' && coins >= 20)
        {
            coins -= 20;
            Console.WriteLine($"\nYou have Purchased Blue! -20 Coins You have {coins} Coins left!\n");
            Choice = Console.ReadKey();
            themecolor = ConsoleColor.Blue;
        }

        else if (Choice.KeyChar == 'b')
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\nYou Cant Afford that, You have {coins}!\n");
            Choice = Console.ReadKey();
        }

        //Buying Yellow Color And changing Theme
        if (Choice.KeyChar == 'r' && coins >= 50)
        {
            coins -= 50;
            Console.WriteLine($"\nWow! You have Purchased Red! -50?! Coins You have {coins} Coins left!\n");
            Choice = Console.ReadKey();
            themecolor = ConsoleColor.Red;
        }
        else if (Choice.KeyChar == 'r')
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\nYou Cant Afford that, You have {coins}!\n");
            Choice = Console.ReadKey();
        }

        if (Choice.KeyChar == 'x' && coins >= 30)
            //Buy Faster Rollspeed
        {
            rollspeed *= 2;
            coins -= 30;
            Console.WriteLine($"\n-30 Coins Your Roll Speed Multiplier is Now x{rollspeed}, you have {coins} left!");
            Choice = Console.ReadKey();
        }

        else if (Choice.KeyChar == 'x')
        {
            Console.WriteLine($"\nYou Cant Afford that, You have {coins}!\n");
            Choice = Console.ReadKey();
        }


        // When Player Exits with 'e'
        if (Choice.KeyChar == 'e')
        {
            shop = false;
            playing = true;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nLoading Game...\n");
            Thread.Sleep(2000);
        }

        // Option to Exit
        if (Choice.KeyChar == 'n')
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
        Double = true;
    }

    //Tripple Checker
    if (dice1 == dice2 && dice2 == dice3)
    {
        coins += 8;
        bonus += 4;
        Double = false;
        Tripple = true;
    }

    //Double Win Message 
    if (Double)
        DoubleWin = "You Rolled a Double! +2 And +2 Coins";
    else
        DoubleWin = "";
    ;

    //Tripple Win Message
    if (Tripple)
        TrippleWin = "You Rolled a Tripple! +6 And +10 Coins";
    else
        TrippleWin = "";


    //Score tally
    score = dice1 + dice2 + dice3 + bonus;


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
    Bonus: {DoubleWin}{TrippleWin}
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
    Bonus: {DoubleWin}{TrippleWin}
    Score: {score}!
    Wins: {wins} Losses: {losses}
    Coins: {coins}
    You Lost!");
    }
}

; //End While Loop