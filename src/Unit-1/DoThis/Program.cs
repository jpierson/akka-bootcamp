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

            var writerActor = MyActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()));
            var readerActor = MyActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(writerActor)));


            // tell console reader to begin
            readerActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.AwaitTermination();
        }
    }
    #endregion
}
