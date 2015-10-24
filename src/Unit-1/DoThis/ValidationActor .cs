using System;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for validating user input and signaling other actors.
    /// Also responsible for calling <see cref="ActorSystem.Shutdown"/>.
    /// </summary>
    class ValidationActor : UntypedActor
    {
        private IActorRef _consoleWriterActor;

        public ValidationActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object objectMessage)
        {
            var message = objectMessage as string;
            if (string.IsNullOrEmpty(message))
            {
                Self.Tell(new Messages.NullInputError("No input recieved."));
            }
            else
            {
                var valid = IsValid(message);
                if (valid)
                {
                    _consoleWriterActor.Tell(new Messages.InputSuccess("Thank you! Message was valid"));
                }
                else
                {
                    _consoleWriterActor.Tell(new Messages.ValidationError("Invalid: input had odd number of characters"));
                }
            }

            Sender.Tell(new Messages.ContinueProcessing());
        }

        private bool IsValid(string message)
        {
            var valid = message.Length % 2 == 0;
            return valid;
        }
    }
}