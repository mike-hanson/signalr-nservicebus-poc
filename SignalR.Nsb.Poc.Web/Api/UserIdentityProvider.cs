using SignalR.Nsb.Poc.Web.Abstractions;

namespace SignalR.Nsb.Poc.Web.Api
{
    public class UserIdentityProvider: IUserIdentityProvider
    {
        public string CurrentUserId() => "mike.hanson";
    }
}
