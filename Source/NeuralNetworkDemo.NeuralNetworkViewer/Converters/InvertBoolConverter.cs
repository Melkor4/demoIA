using System;
using System.Globalization;
using System.Windows.Data;

namespace NeuralNetworkDemo.NeuralNetworkViewer.Converters
{
    public class InvertBoolConverter : IValueConverter
    {

        #region Public Methods

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || value.GetType() != typeof(bool))
                return null;

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion

    }
}
