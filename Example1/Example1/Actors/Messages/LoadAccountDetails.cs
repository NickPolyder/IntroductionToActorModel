using System;

namespace Example1.Actors.Messages
{
    public class LoadAccountDetailsMessage
    { }

    public class LoadAccountDetailsMessageResponse
    {
        public string LoginName { get; }
        public string AccountName { get; }

        public LoadAccountDetailsMessageResponse(string loginName, string accountName)
        {
            LoginName = loginName ?? throw new ArgumentNullException(nameof(loginName));
            AccountName = accountName ?? throw new ArgumentNullException(nameof(accountName));
        }
    }
}