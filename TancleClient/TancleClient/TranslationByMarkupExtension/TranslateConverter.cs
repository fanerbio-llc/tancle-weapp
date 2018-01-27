using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TancleClient.Service;
using Microsoft.Practices.ServiceLocation;

namespace TancleClient.TranslationByMarkupExtension
{
    public class TranslateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = value as string ?? parameter as string;

            if (key != null)
            {
                // Do translation based on the key
                return ServiceLocator.Current.GetInstance<ITranslationService>().Translate(key);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
