using Akka.Actor;
using Example2.Actors.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Example2.Actors
{
    /// <summary>
    /// This is for just demonstration purposes. If you need routing  see <see cref="https://getakka.net/articles/actors/routers.html"/>
    /// </summary>
    public class RouterActor : ReceiveActor
    {
        private Dictionary<string, IActorRef> _routees;
        private Random _random;
        public RouterActor()
        {
            _random = new Random(231);
            Receive<CreateRouteeMessage>((msg) => CreateActor(msg));
            Receive<SendMessage>(msg => HandleSendMessage(msg));
            Receive<KillActorMessage>(message =>
            {
                var key = GetKey(message.ActorName);
                _routees[key].Tell(message);
                Sender.Tell(new GenericResponseMessage(true, $"Killed: {key}"));
            });
        }

        private void HandleSendMessage(SendMessage msg)
        {
            var key = GetKey(msg.RouterName);

            _routees[key].Tell(msg);
        }

        private string GetKey(string routerName)
        {
            string key;
            if (string.IsNullOrWhiteSpace(routerName))
            {
                int index = _random.Next(0, _routees.Count);
                key = _routees.Keys.ToArray()[index];
            }
            else
            {
                key = routerName;
            }

            return key;
        }

        private void CreateActor(CreateRouteeMessage message)
        {
            if (_routees == null)
            {
                _routees = new Dictionary<string, IActorRef>();
            }

            if (_routees.ContainsKey(message.Name))
            {
                Sender.Tell(new GenericResponseMessage(false, $"Already registered: {message.Name}"));
                return;

            }
            _routees.Add(message.Name, Context.ActorOf(OutputActor.Create(message.Name), message.Name));

            Sender.Tell(new GenericResponseMessage(true, $"Registered: {message.Name}"));
        }

        public static Props Create() => Props.Create<RouterActor>();

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(ex =>
            {
                switch (ex)
                {
                    case NotImplementedException notImplemented:
                        return Directive.Restart;
                    case InvalidDataException invalidData:
                        var key = invalidData.Data["key"].ToString();

                        string randomKey = null;
                        while (randomKey == null)
                        {
                            randomKey = GetKey(null);
                            if (key == randomKey && _routees.Count > 1)
                            {
                                randomKey = null;
                            }
                        }

                        _routees[key] = _routees[randomKey];
                        return Directive.Stop;
                    default:
                        return Directive.Resume;
                }
            }, false);
        }
    }
}