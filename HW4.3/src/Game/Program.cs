using Game.Helpers;

AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

Console.WriteLine("To start the game. Input game field size. width x height min(5x5) max(10x10)");
var width = ConsoleHelpers.GetIntFromConsole("width");
var height = ConsoleHelpers.GetIntFromConsole("height");

var game = new Game.Core.Game(width, height);

game.Run();

static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
{
    Console.WriteLine("Error " + (e.ExceptionObject as Exception)?.Message);
    Console.WriteLine("Application will be terminated. Press any key...");
    Console.ReadKey();
}

