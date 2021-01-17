using ReportsApp.Models;
using System.Threading.Tasks;

namespace ReportsApp.Persistence
{
    public interface IAuthorizationStore
    {
        Task<string> GetAuthorizationTokenAsync();
        void RemoveAuthorizationInfo();
        Task SetAuthorizationInfo(string token);
        Task SetAuthorizationInfo(UserInfo activeUser);
    }
}