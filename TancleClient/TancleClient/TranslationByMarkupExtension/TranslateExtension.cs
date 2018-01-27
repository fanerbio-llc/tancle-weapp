using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace TancleClient.TranslationByMarkupExtension
{
    /// <summary>
    /// The Translate Markup extension returns a binding to a TranslationData
    /// that provides a translated resource of the specified key
    /// </summary>
    public class TranslateExtension : MarkupExtension
    {
        #region Private Members

        private string _key;

        private Binding _binding;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateExtension"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public TranslateExtension(string key)
        {
            _key = key;
        }

        public TranslateExtension(Binding binding)
        {
            _binding = binding;
            _binding.Converter = new TranslateConverter();
        }

        #endregion

        [ConstructorArgument("key")]
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// See <see cref="MarkupExtension.ProvideValue" />
        /// </summary>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = _binding ?? new Binding("Value")
            {
                Source = new TranslationData(_key)
            };

            return binding.ProvideValue(serviceProvider);
        }
    }
}
