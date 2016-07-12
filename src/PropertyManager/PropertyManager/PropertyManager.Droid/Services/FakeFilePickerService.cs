using System.Threading.Tasks;
using PropertyManager.Models;
using PropertyManager.Services;

namespace PropertyManager.Droid.Services
{
    class FakeFilePickerService : IFilePickerService
    {
        public Task<PickedFileModel> GetFileAsync()
        {
            return null;
        }
    }
}