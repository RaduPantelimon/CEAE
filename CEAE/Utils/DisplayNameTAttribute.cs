using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace CEAE.Utils
{
    public class DisplayNameTAttribute : DisplayNameAttribute
    {
        public DisplayNameTAttribute([CallerMemberName] string propertyName = null) : base(TranslationFor(propertyName))
        {
            
        }

        private static string TranslationFor(string translationName)
        {
            return Translations.ResourceManager.GetString(translationName);
        }
    }
}