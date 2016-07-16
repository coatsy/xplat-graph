using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using PropertyManager.Models;

namespace PropertyManager.Droid.Converters
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
                    return Resource.Drawable.ic_image_black_48dp;
                case FileType.Document:
                    return Resource.Drawable.ic_insert_drive_file_black_48dp;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}