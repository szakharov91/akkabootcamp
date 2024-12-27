using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaBootcamp.WinTail;

/// <summary>
/// Actor responsible for serializing message writes to the console.
/// (write one message at a time, champ :)
/// </summary>
public class ConsoleWriterActor : UntypedActor
{
    protected override void OnReceive(object message)
    {
        var msg = message as string;

        if (string.IsNullOrEmpty(msg))
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Please provide an input.\n");
            Console.ResetColor();
            return;
        }

        var even = msg.Length % 2 == 0;
        var color = even ? ConsoleColor.Red : ConsoleColor.Green;
        var alert = even
            ? "Your string had an even # of characters.\n"
            : "Your string had an odd  # of characters.\n";

        Console.ForegroundColor = color;
        Console.WriteLine(alert);
        Console.ResetColor();
    }
}
