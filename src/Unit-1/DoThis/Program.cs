using System;
﻿using Akka.Actor;

namespace WinTail
{
    #region Program
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            var writerProps = Props.Create<ConsoleWriterActor>();
            var writerActor = MyActorSystem.ActorOf(writerProps, "writerActor");

            var validationActorProps = Props.Create(() => new ValidationActor(writerActor));
            var validationActor = MyActorSystem.ActorOf(validationActorProps, "validationActor");

            var readerProps = Props.Create<ConsoleReaderActor>(validationActor);
            var readerActor = MyActorSystem.ActorOf(readerProps, "readerActor");


            // tell console reader to begin
            readerActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.AwaitTermination();
        }
    }
    #endregion
}
