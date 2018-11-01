using Akka.Actor;

namespace Example2.Actors.Messages
{
    public class SendMessage
    {
        public IActorRef Sender { get; }
        public string RouterName { get; }
        public string Name { get; }
        public string Message { get; }

        public SendMessage(string routerName, string name, string message,IActorRef sender)
        {
            Sender = sender;
            RouterName = routerName;
            Name = name;
            Message = message;
        }
    }
}