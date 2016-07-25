using System;
using PropertyManager.Services;

namespace PropertyManager.iOS
{
	public class DialogService : IDialogService
	{
		public IDialogHandle ShowProgress(string title, string message)
		{
			return new ProgressDialogHandle(title, message);
		}
	}
}

