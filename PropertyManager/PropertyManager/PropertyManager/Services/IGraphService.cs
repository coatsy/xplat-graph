using System.Threading.Tasks;
using PropertyManager.Models;

namespace PropertyManager.Services
{
    public interface IGraphService
    {
        Task<GroupModel[]> GetGroupsAsync();

        Task<GroupModel[]> GetUserGroupsAsync();
    }
}
