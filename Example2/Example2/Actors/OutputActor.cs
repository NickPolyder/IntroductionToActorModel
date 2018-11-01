using Akka.Actor;
using Example2.Actors.Messages;
using System;
using System.IO;

namespace Example2.Actors
{
    public class OutputActor : ReceiveActor
    {
        private Guid _guid;
        private readonly string _actorName;

        public OutputActor(string actorName)
        {
            _guid = Guid.NewGuid();
            _actorName = actorName;
            Receive<SendMessage>((msg) => HandleMessage(msg));

            Receive<KillActorMessage>(___ => HandleKill());

            ReceiveAny(obj => { WarningMessage($"Cannot handle: {obj.GetType().Name}"); });
        }

        private void HandleKill()
        {
            if (new Random().NextDouble() > 0.5)
            {
                throw new NotImplementedException("Some Exception");
            }
            else
            {
                var invalidEx = new InvalidDataException("Some Other Exception");
                invalidEx.Data["key"] = _actorName;
                throw invalidEx;
            }
        }

        private void HandleMessage(SendMessage obj)
        {
            Console.WriteLine();
            Console.WriteLine($"Handled by: **{_actorName}** Id: **{_guid}**");
            Console.WriteLine();
            Console.Write("Name: ");
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(!string.IsNullOrWhiteSpace(obj.Name) ? obj.Name : "Anonymous");
            Console.ForegroundColor = oldColor;
            Console.WriteLine($", Message: {obj.Message}");
            Console.WriteLine();

            obj.Sender.Tell(new GenericResponseMessage(true, $"Message handled by: {_actorName}"));
        }

        private void WarningMessage(string msg)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(msg);
            Console.ForegroundColor = oldColor;
        }

        public static Props Create(string name) => Props.Create(() => new OutputActor(name));
    }
}