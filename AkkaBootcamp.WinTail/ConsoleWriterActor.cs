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
        if(message is InputError)
        {
            var msg = message as InputError;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg.Reason);
        }
        else if(message is InputSuccess)
        {
            var msg = message as InputSuccess;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg.Reason);
        }
        else
        {
            Console.WriteLine(message);
        }

        Console.ResetColor();
    }
}
