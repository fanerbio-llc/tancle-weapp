using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TancleClient.TranslationByMarkupExtension;

namespace TancleClient.Service
{
    public class TranslationService : ITranslationService
    {
        public object Translate(string key)
        {
            return TranslationManager.Instance.TranslationProvider.Translate(key);
        }

        public CultureInfo GetCurrentLanguage()
        {
            return TranslationManager.Instance.CurrentLanguage;
        }
    }
}
