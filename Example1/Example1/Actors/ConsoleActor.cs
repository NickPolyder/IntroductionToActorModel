using Akka.Actor;
using Example1.Actors.Messages;
using System;

namespace Example1.Actors
{
    public class ConsoleActor : ReceiveActor
    {
        public const string Exit = "-Q";
        public const string Start = "-S";
        public const string Help = "-H";
        private IActorRef _operatorActor;
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
            Receive<LoadAccountDetailsMessageResponse>((msg) =>
            {
                string consoleMsg = $"| Login Name: {msg.LoginName}, Account Number: {msg.AccountName} |";
                ChangeColor(ConsoleColor.DarkGray);
                WriteLine(new string('-', consoleMsg.Length));
                WriteLine(consoleMsg);
                WriteLine(new string('-', consoleMsg.Length));
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
                    HandleInitialise();
                    return;
                case Help:
                    HandleHelp();
                    break;
                case Exit:
                    HandleExit();
                    break;
                case "1":
                    HandleLogin();
                    break;
                case "2":
                    _operatorActor?.Tell(new LoadAccountDetailsMessage());
                    break;
                case "3":
                    _operatorActor?.Tell(new LogoutMessage());
                    break;
                default:
                    Self.Tell(Help);
                    break;
            }
        }

        private void HandleInitialise()
        {
            if (_operatorActor == null)
            {
                _operatorActor = Context.ActorOf(OperatorActor.CreateProps());
            }

            Self.Tell(Help);
        }

        private void HandleHelp()
        {
            WriteLine("Example 1");
            WriteLine($"To exit type: {Exit}");
            WriteLine($"To show the help message type: {Help}");
            WriteLine("To Login type: 1");
            WriteLine("To Load the Account Details type: 2");
            WriteLine("To Logout type: 3");
            AskForNextCommand();
        }

        private void HandleExit()
        {
            _operatorActor?.Tell(PoisonPill.Instance);
            Context.System.Terminate();
        }

        private void HandleLogin()
        {
            string loginName = AskForInput("Login Name");
            string accountNumber = AskForInput("Account Number");
            _operatorActor?.Tell(new LoginMessage(loginName, accountNumber));
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