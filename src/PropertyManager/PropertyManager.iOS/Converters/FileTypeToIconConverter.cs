﻿using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using PropertyManager.Models;
using UIKit;

namespace PropertyManager.iOS
{
	public class FileTypeToIconConverter : IMvxValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is FileType))
			{
				return value;
			}

			switch ((FileType)value)
			{
				case FileType.Media:
					return UIImage.FromBundle("MediaFileIcon");
				case FileType.Document:
					return UIImage.FromBundle("DocumentFileIcon");
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

