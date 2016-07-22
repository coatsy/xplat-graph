using System;
using System.Threading.Tasks;
using PropertyManager.Models;
using PropertyManager.Services;

namespace PropertyManager.iOS
{
	public class FilePickerService : IFilePickerService
	{
		public Task<PickedFileModel> GetFileAsync()
		{
			throw new NotImplementedException();
		}
	}
}

