using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace VzalxndrSpace.WpfClient.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int status)
            {
                return status switch
                {
                    0 => Brushes.DarkOrange, // in progress
                    1 => Brushes.MediumSeaGreen, // completed
                    2 => Brushes.DarkRed, // cancelled
                    _ => Brushes.Gray, // undefined
                };
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { 
            throw new NotImplementedException();
        }
    }
}
