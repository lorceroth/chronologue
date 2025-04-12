using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Chronologue.Common.Converters;

public class GuidConvertersIsNullOrEmpty : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is Guid guid && guid == Guid.Empty;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class GuidConvertersIsNotNullOrEmpty : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is Guid guid && guid != Guid.Empty;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
