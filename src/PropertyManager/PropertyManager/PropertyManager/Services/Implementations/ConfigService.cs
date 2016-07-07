using PropertyManager.Models;

namespace PropertyManager.Services
{
    public class ConfigService : IConfigService
    {
        public GroupModel AppGroup { get; set; }

        public GroupModel[] Groups { get; set; }

        public DataFileModel DataFile { get; set; }
    }
}
