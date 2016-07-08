using System;
using Windows.UI.Xaml.Data;
using PropertyManager.Models;

namespace PropertyManager.UWP.Converters
{
    public class FileTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is FileType))
            {
                return value;
            }

            switch ((FileType) value)
            {
                case FileType.Media:
                    return "\uEB9F";
                case FileType.Document:
                    return "\uE8E5";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
