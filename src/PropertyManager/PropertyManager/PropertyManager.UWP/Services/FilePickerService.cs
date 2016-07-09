﻿using System;
using System.IO;
using System.Threading.Tasks;
using PropertyManager.Extensions;
using PropertyManager.Models;
using PropertyManager.Services;

namespace PropertyManager.UWP.Services
{
    public class FilePickerService : IFilePickerService
    {
        public async Task<PickedFileModel> GetFileAsync()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.AddRange(Constants.MediaFileExtensions);
            picker.FileTypeFilter.AddRange(Constants.DocumentFileExtensions);

            // Get stream.
            var file = await picker.PickSingleFileAsync();
            if (file == null)
            {
                return null;
            }
            return new PickedFileModel
            {
                Name = file.Name,
                Stream = await file.OpenStreamForReadAsync()
            };
        }
    }
}