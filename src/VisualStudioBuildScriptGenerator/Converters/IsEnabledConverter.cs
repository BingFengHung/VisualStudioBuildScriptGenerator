using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace VisualStudioBuildScriptGenerator
{
    abstract class BaseMultipleValueConverter<T> : MarkupExtension, IMultiValueConverter where T : class, new()
    {
        private static T Converter = null;

        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Converter ?? (Converter = new T());
        }

    }
    class IsEnabledConverter : BaseMultipleValueConverter<IsEnabledConverter>
    {
        public static IsEnabledConverter Instance;

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] != null && values[1] != null)
            {
                if (string.IsNullOrEmpty(values[0].ToString()) || string.IsNullOrEmpty(values[1].ToString()))
                    return false;
                return true;
            }

            return false;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance ?? new IsEnabledConverter();
        }
    }
}
