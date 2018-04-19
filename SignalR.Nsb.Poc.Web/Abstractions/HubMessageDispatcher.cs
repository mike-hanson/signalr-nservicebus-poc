using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Nsb.Poc.Web.Abstractions
{
    public abstract class HubMessageDispatcher<T> : IHubMessageDispatcher<T> where T: Hub
    {
        private readonly IHubContext<T> _hubContext;

        protected HubMessageDispatcher(IHubContext<T> hubContext){
            _hubContext = hubContext;
        }

        public async Task SendToGroupAsync(string groupName, string method, object[] args)
        {
            await _hubContext.Clients.Group(groupName).SendAsync(method, args);
        }
        
        public async Task SendToGroupAsync(string groupName, string method, object arg1)
        {
            await _hubContext.Clients.Group(groupName).SendAsync(method, arg1);
        }
        
        public async Task SendToGroupAsync(string groupName, string method, object arg1, object arg2)
        {
            await _hubContext.Clients.Group(groupName).SendAsync(method, arg1, arg2);
        }
        
        public async Task SendToGroupAsync(string groupName, string method, object arg1, object arg2, object arg3)
        {
            await _hubContext.Clients.Group(groupName).SendAsync(method, arg1, arg2, arg3);
        }
    }
}