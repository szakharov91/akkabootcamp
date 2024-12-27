﻿using Akka.Actor;
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
    private readonly IActorRef _consoleWriterActor;

    public ConsoleReaderActor(IActorRef consoleWriterActor)
    {
        _consoleWriterActor = consoleWriterActor;
    }

    protected override void OnReceive(object message)
    {
        var read = Console.ReadLine();
        if(!string.IsNullOrEmpty(read) 
            && string.Equals(read, ExitCommand, StringComparison.OrdinalIgnoreCase))
        {
            // shut down the system (acquire handle to system via
            // this actors context)
            Context.System.Terminate();
            return;
        }

        // send input to the console writer to process and print
        // YOU NEED TO FILL IN HERE

        // continue reading messages from the console
        // YOU NEED TO FILL IN HERE
    }
}
