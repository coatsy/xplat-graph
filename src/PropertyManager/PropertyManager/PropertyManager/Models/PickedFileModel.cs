using System;
using System.IO;

namespace PropertyManager.Models
{
    public class PickedFileModel : IDisposable
    {
        public Stream Stream { get; set; }

        public string Name { get; set; }

        public void Dispose()
        {
            Stream.Dispose();
        }
    }
}
