namespace Example2.Actors.Messages
{
    public class KillActorMessage
    {
        public string ActorName { get; }

        public KillActorMessage(string actorName)
        {
            ActorName = actorName;
        }
    }
}