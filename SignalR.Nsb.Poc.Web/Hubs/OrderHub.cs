using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalR.Nsb.Poc.Web.Abstractions;

namespace SignalR.Nsb.Poc.Web.Hubs
{ 
    public class OrderHub: Hub
    {
        private readonly IUserIdentityProvider _userIdentityProvider;

        public OrderHub(IUserIdentityProvider userIdentityProvider)
        {
            _userIdentityProvider = userIdentityProvider;
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddAsync(Context.ConnectionId, _userIdentityProvider.CurrentUserId());
        }
    }
}
