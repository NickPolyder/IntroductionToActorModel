namespace Example2.Actors.Messages
{
    public class CreateRouteeMessage
    {
        public string Name { get; }

        public CreateRouteeMessage(string name)
        {
            Name = name;
        }
    }
}