using System;

namespace Example1.Actors.Messages
{
    public class LoginMessage
    {
        public string LoginName { get; }
        public string AccountNumber { get; }

        public LoginMessage(string loginName, string accountNumber)
        {
            LoginName = loginName ?? throw new ArgumentNullException(nameof(loginName));
            AccountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber));
        }
    }
    
}