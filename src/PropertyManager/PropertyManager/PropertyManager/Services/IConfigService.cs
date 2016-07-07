using PropertyManager.Models;

namespace PropertyManager.Services
{
    public interface IConfigService
    {
        GroupModel AppGroup { get; set; }

        GroupModel[] Groups { get; set; } 

        DataFileModel DataFile { get; set; }
    }
}
