using Akka;
using Akka.Actor;

namespace AkkaBootcamp.WinTail;

internal class Program
{
    public static ActorSystem MyActorSystem;

    static void Main(string[] args)
    {
        // initialize MyActorSystem
        MyActorSystem = ActorSystem.Create("MyActorSystem");

        PrintInstructions();

        Props consoleWriteProps = Props.Create(typeof(ConsoleWriterActor));
        IActorRef consoleWriterActor = MyActorSystem.ActorOf(consoleWriteProps, "consoleWriterActor");

        Props validationActorProps = Props.Create(() => new ValidationActor(consoleWriterActor));
        IActorRef validationActor = MyActorSystem.ActorOf(validationActorProps, "validationActor");

        Props consoleReadProps = Props.Create<ConsoleReaderActor>(validationActor);
        IActorRef consoleReaderActor = MyActorSystem.ActorOf(consoleReadProps, "consoleReaderActor");

        // tell console reader to begin
        consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

        // blocks the main thread from exiting until the actor system is shut down
        MyActorSystem.WhenTerminated.Wait();
    }

    private static void PrintInstructions()
    {
        Console.WriteLine("Write whatever you want into the console!");
        Console.Write("Some lines will appear as");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write(" red ");
        Console.ResetColor();
        Console.Write(" and others will appear as");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(" green! ");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Type 'exit' to quit this application at any time.\n");
    }
}
