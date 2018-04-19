using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Nsb.Poc.Web.Abstractions
{
    public interface IHubMessageDispatcher<T> where T : Hub
    {
        Task SendToGroupAsync(string groupName, string method, object[] args);
        Task SendToGroupAsync(string groupName, string method, object arg1);
        Task SendToGroupAsync(string groupName, string method, object arg1, object arg2);
        Task SendToGroupAsync(string groupName, string method, object arg1, object arg2, object arg3);
    }
}