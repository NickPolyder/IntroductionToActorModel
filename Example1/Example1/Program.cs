using System;
using Akka.Actor;
using Example1.Actors;

namespace Example1
{
    class Program
    {
        private static ActorSystem _actorSystem;
        static void Main(string[] args)
        {
            // Start the System
            _actorSystem = ActorSystem.Create("Example1");

            //Create first Actor
            var consoleActor = _actorSystem.ActorOf(ConsoleActor.Create());

            //Initialise Message
            consoleActor.Tell(ConsoleActor.Start);

            //Wait for actorSystem to receive termination signal
            _actorSystem.WhenTerminated.Wait();

        }
    }
}
