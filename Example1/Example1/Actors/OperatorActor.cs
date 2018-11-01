using Akka.Actor;
using Example1.Actors.Messages;

namespace Example1.Actors
{
    public class OperatorActor : ReceiveActor
    {
        private bool _authorised;
        private string _loginName;
        private string _accountNumber;

        public OperatorActor()
        {
            Receive<LoginMessage>((msg)=>HandleLogin(msg));
            Receive<LogoutMessage>((___)=>HandleLogout());
            Receive<LoadAccountDetailsMessage>((___) => HandleLoadAccountDetailsRequest());
        }

        private void HandleLogin(LoginMessage message)
        {
            if (_authorised)
            {
                Sender.Tell(new GenericResponseMessage(false, $"You are already logged in as: {_loginName}"));
                return;
            }
            _loginName = message.LoginName;
            _accountNumber = message.AccountNumber;
            _authorised = true;
            Sender.Tell(new GenericResponseMessage(true,$"You have logged in as: {_loginName}"));
        }

        private void HandleLogout()
        {
            if (!_authorised)
            {
                Sender.Tell(new GenericResponseMessage(false, $"You are not logged in!"));
                return;
            }
            _loginName = null;
            _accountNumber = null;
            _authorised = false;
            Sender.Tell(new GenericResponseMessage(true, "You have been logged out"));

        }

        private void HandleLoadAccountDetailsRequest()
        {
            if (!_authorised)
            {
                Sender.Tell(new GenericResponseMessage(false,"The Operator is not logged in!"));
                return;
            }

            Sender.Tell(new LoadAccountDetailsMessageResponse(_loginName, _accountNumber));
        }

        public static Props CreateProps() => Props.Create<OperatorActor>();
    }
}