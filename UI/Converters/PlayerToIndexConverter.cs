using System;
using System.Globalization;
using Avalonia.Data.Converters;
using DraughtsGame.DataModels;

namespace DraughtsGame.UI.Converters;

public class PlayerToIndexConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Player player)
        {
            return player == Player.Red ? 0 : 1;
        }
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int index)
        {
            return index == 0 ? Player.Red : Player.White;
        }
        return Player.Red;
    }
} 