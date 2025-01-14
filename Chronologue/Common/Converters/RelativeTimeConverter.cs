using Avalonia.Data.Converters;
using Humanizer;
using System;
using System.Globalization;

namespace Chronologue.Common.Converters;

public class RelativeTimeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return "";
        }

        if (value is not DateTime date)
        {
            return value.ToString();
        }

        return date.Humanize();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
