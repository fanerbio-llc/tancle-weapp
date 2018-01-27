using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TancleClient.TranslationByMarkupExtension
{
    public class TranslationManager
    {
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static TranslationManager _translationManager;

        public event EventHandler LanguageChanged;

        public CultureInfo CurrentLanguage
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
            set
            {
                if (value != Thread.CurrentThread.CurrentUICulture)
                {
                    Thread.CurrentThread.CurrentUICulture = value;
                    CultureInfo.DefaultThreadCurrentUICulture = value;
                    OnLanguageChanged();
                }
            }
        }

        public IEnumerable<CultureInfo> Languages
        {
            get
            {
                if (TranslationProvider != null)
                {
                    return TranslationProvider.Languages;
                }
                return Enumerable.Empty<CultureInfo>();
            }
        }

        public static TranslationManager Instance
        {
            get
            {
                if (_translationManager == null)
                    _translationManager = new TranslationManager();
                return _translationManager;
            }
        }

        public ITranslationProvider TranslationProvider { get; set; }

        private void OnLanguageChanged()
        {
            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }

        public object Translate(string key)
        {
            if (TranslationProvider != null)
            {
                object translatedValue = TranslationProvider.Translate(key);
                if (translatedValue != null)
                {
                    return translatedValue;
                }
            }
            try
            {
                return string.Format("!{0}!", key);
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return null;
            }
        }
    }
}
