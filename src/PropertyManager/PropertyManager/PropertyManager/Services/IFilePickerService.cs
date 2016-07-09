using System.Threading.Tasks;
using PropertyManager.Models;

namespace PropertyManager.Services
{
    public interface IFilePickerService
    {
        Task<PickedFileModel> GetFileAsync();
    }
}
