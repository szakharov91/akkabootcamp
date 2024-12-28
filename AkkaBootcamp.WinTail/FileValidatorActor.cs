using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaBootcamp.WinTail;

/// <summary>
/// Actor that validates user input and signals result to others.
/// </summary>
public class FileValidatorActor : UntypedActor
{
    private readonly IActorRef consoleWriterActor;

    public FileValidatorActor(IActorRef consoleWriterActor)
    {
        this.consoleWriterActor = consoleWriterActor;
    }

    protected override void OnReceive(object message)
    {
        var msg = message as string;

        if (string.IsNullOrEmpty(msg))
        {
            consoleWriterActor.Tell(new NullInputError("Input was blank. Please try again!\n"));
            Sender.Tell(new ContinueProcessing());
        }
        else
        {
            var valid = IsFileUri(msg);
            if (valid)
            {
                consoleWriterActor.Tell(new InputSuccess(string.Format("Starting processing for {0}", msg)));

                Context.ActorSelection("akka://MyActorSystem/user/tailCoordinatorActor").Tell(
                    new TailCoordinatorActor.StartTail(msg, consoleWriterActor));
            }
            else
            {
                consoleWriterActor.Tell(new ValidationError(string.Format("{0} is not an existing URI on disk", msg)));

                Sender.Tell(new ContinueProcessing());
            }
        }
    }

    /// <summary>
    /// Checks if file exists at path provided by user.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static bool IsFileUri(string path)
    {
        return File.Exists(path);
    }
}
