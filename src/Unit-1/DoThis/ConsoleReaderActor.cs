using System;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for reading FROM the console. 
    /// Also responsible for calling <see cref="ActorSystem.Shutdown"/>.
    /// </summary>
    class ConsoleReaderActor : UntypedActor
    {
        public const string StartCommand = "start";
        public const string ExitCommand = "exit";
        private IActorRef _consoleWriterActor;

        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
            {
                DoPrintInstructions();
            }
            else if (message is Messages.InputError)
            {
                _consoleWriterActor.Tell(message as Messages.InputError);
            }

            GetAndValidateInput();
        }
        private void DoPrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.WriteLine("Some entries will pass validation, and some won't...\n\n");
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }

        private void GetAndValidateInput()
        {
            var message = Console.ReadLine();
            if(string.IsNullOrEmpty(message))
            {
                Self.Tell(new Messages.NullInputError("No input recieved."));
            }
            else if (string.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                Context.System.Shutdown();
            }
            else
            {
                var valid = IsValid(message);
                if (valid)
                {
                    _consoleWriterActor.Tell(new Messages.InputSuccess("Thank you! Message was valid"));

                    Self.Tell(new Messages.ContinueProcessing());
                }
                else
                {
                    Self.Tell(new Messages.ValidationError("Invalid: input had odd number of characters"));
                }
            }
        }

        private bool IsValid(string message)
        {
            var valid = message.Length % 2 == 0;
            return valid;
        }
    }
}