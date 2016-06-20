using System.IO;
using System.Threading.Tasks;
using PropertyManager.Models;

namespace PropertyManager.Services
{
    public interface IGraphService
    {
        Task<GroupModel[]> GetGroupsAsync();

        Task<GroupModel[]> GetUserGroupsAsync();

        Task<DriveItemModel[]> GetGroupDriveItemsAsync(GroupModel group);

        Task<ConversationModel[]> GetGroupConversationsAsync(GroupModel group);

        Task<DriveItemModel> CreateGroupDriveItemAsync(GroupModel group, string name, Stream stream, 
            string contentType);

        Task<DriveItemModel[]> GetDriveItemsAsync();

        Task<DriveItemModel> CreateDriveItemAsync(string name, Stream stream, string contentType);
    }
}
