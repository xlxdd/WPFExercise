using System;
using System.Globalization;
using System.Windows.Data;

namespace student.Common.Converts;
public class StringToNullableDecimalConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is decimal decValue)
        {
            return decValue.ToString(culture);
        }

        // 在转换失败时返回空字符串
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string strValue)
        {
            if (decimal.TryParse(strValue, out decimal decValue))
            {
                return decValue;
            }
        }

        // 在转换失败时返回null
        return null;

    }
}

