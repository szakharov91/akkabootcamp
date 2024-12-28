using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaBootcamp.WinTail;

/// <summary>
/// Actor responsible for reading FROM the console. 
/// Also responsible for calling <see cref="ActorSystem.Terminate"/>.
/// </summary>
public class ConsoleReaderActor : UntypedActor
{
    public const string ExitCommand = "exit";
    public const string StartCommand = "start";

    private readonly IActorRef _validationActor;

    public ConsoleReaderActor(IActorRef validatorActor)
    {
        _validationActor = validatorActor;
    }

    protected override void OnReceive(object message)
    {
        if (message.Equals(StartCommand))
        {
            DoPrintInstructions();
        }
        
        GetAndValidateInput();
    }

    #region Internal methods
    private void DoPrintInstructions()
    {
        Console.WriteLine("Please provide the URI of a log file on disk.\n");
    }

    private void GetAndValidateInput()
    {
        var message = Console.ReadLine();
        if (!string.IsNullOrEmpty(message)
            && string.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
        {
            // shut down the entire actor system (allows the process to exit)
            Context.System.Terminate();
            return;
        }

        _validationActor.Tell(message);
    }
    #endregion
}
