using PropertyManager.Models;

namespace PropertyManager.Services
{
    public interface IConfigService
    {
        UserModel User { get; set; }

        GroupModel AppGroup { get; set; }

        GroupModel[] Groups { get; set; } 

        DataFileModel DataFile { get; set; }
    }
}
