﻿using System.Threading.Tasks;
using Foundation;
using PropertyManager.Models;
using PropertyManager.Services;
using UIKit;

namespace PropertyManager.iOS
{
	public class FilePickerService : IFilePickerService
	{
		public Task<PickedFileModel> GetFileAsync()
		{
			// Find the root view controller.
			var viewController = UIApplication.SharedApplication.KeyWindow.RootViewController;

			// Create the task completion source.
			var taskCompletionSource = new TaskCompletionSource<PickedFileModel>();

			// Create the image picker.
			var imagePicker = new UIImagePickerController();
			imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
			imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);

			// Register event handlers.
			imagePicker.FinishedPickingMedia += (sender, e) =>
			{
				// Extract the file name.
				var referenceUrl = e.Info.Values[1] as NSUrl;
				var assetsLibrary = new AssetsLibrary.ALAssetsLibrary();
				assetsLibrary.AssetForUrl(referenceUrl, (obj) =>
				{
					// Get the file stream.
					var stream = (e.EditedImage ?? e.OriginalImage).AsPNG().AsStream();

					// Complete the task.
					taskCompletionSource.SetResult(new PickedFileModel
					{
						Name = obj.DefaultRepresentation.Filename,
						Stream = stream
					});

					// Dismiss the image picker.
					imagePicker.DismissViewController(true, null);
				}, (obj) =>
				{
					taskCompletionSource.SetResult(null);

					// Dismiss the image picker.
					imagePicker.DismissViewController(true, null);
				});
			};
			imagePicker.Canceled += (sender, e) =>
			{
				taskCompletionSource.SetResult(null);

				// Dismiss the image picker.
				imagePicker.DismissViewController(true, null);
			};

			// Show the image picker.
			viewController.PresentModalViewController(imagePicker, true);

			return taskCompletionSource.Task;
		}
	}
}

