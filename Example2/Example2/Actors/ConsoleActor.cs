using Akka.Actor;
using Example2.Actors.Messages;
using System;

namespace Example2.Actors
{
    public class ConsoleActor : ReceiveActor
    {
        public const string Exit = "-Q";
        public const string Start = "-S";
        public const string Help = "-H";
        private IActorRef _routerActor;
        public ConsoleActor()
        {
            Receive<string>((msg) => HandleConsoleInput(msg));
            Receive<GenericResponseMessage>((msg) =>
            {
                ChangeColor(msg.Successful ? ConsoleColor.Green : ConsoleColor.Red);
                WriteLine(msg.Message);
                ResetColor();
                AskForNextCommand();
            });
        }

        private void HandleConsoleInput(string msg)
        {
            string msgToUpper = msg.ToUpper();
            switch (msgToUpper)
            {
                case Start:
                    _routerActor = Context.ActorOf(RouterActor.Create());
                    Self.Tell(Help);
                    return;
                case Help:
                    HandleHelp();
                    break;
                case Exit:
                    HandleExit();
                    break;
                case "1":
                    HandleCreate();
                    break;
                case "2":
                    HandleSendMessage();
                    break;
                case "3":
                    HandleKillActor();
                    break;
                default:
                    Self.Tell(Help);
                    break;
            }
        }

        private void HandleCreate()
        {
            string name = AskForInput("Name");
            _routerActor?.Tell(new CreateRouteeMessage(name));
        }

        private void HandleSendMessage()
        {
            string routerName = AskForInput("Send To(empty to sent random)");
            string name = AskForInput("Name");
            string message = AskForInput("Message");
            _routerActor.Tell(new SendMessage(routerName,name,message,Self));
        }
        private void HandleKillActor()
        {
            string name = AskForInput("Name(empty to kill random)");
            _routerActor?.Tell(new KillActorMessage(name));
        }

        private void HandleHelp()
        {
            WriteLine("Example 2");
            WriteLine($"To exit type: {Exit}");
            WriteLine($"To show the help message type: {Help}");
            WriteLine("To create more actors type: 1");
            WriteLine("To send a message type: 2");
            WriteLine("to kill an actor type: 3");
            AskForNextCommand();
        }

        private void HandleExit()
        {
            _routerActor?.Tell(PoisonPill.Instance);
            Context.System.Terminate();
        }
        
        #region private helpers

        private void AskForNextCommand()
        {
            Self.Tell(AskForInput("next command"));
        }

        private string AskForInput(string message)
        {
            Write(message + ": ");
            return ReadLine();
        }

        private void Write<T>(T message)
        {
            System.Console.Write(" ");
            System.Console.Write(message);
        }

        private void WriteLine<T>(T message)
        {
            System.Console.Write(" ");
            System.Console.WriteLine(message);
        }

        private string ReadLine()
        {
            return System.Console.ReadLine();
        }

        private void ChangeColor(ConsoleColor color)
        {
            System.Console.ForegroundColor = color;
        }

        private void ResetColor()
        {
            System.Console.ForegroundColor = ConsoleColor.White;
        }

        #endregion
        public static Props Create() => Props.Create<ConsoleActor>();
    }
}