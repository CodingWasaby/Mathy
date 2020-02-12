// Dandelion.EnumValueAttribute
using System;

namespace Mathy.Utils.Dandelion
{
    public class EnumValueAttribute : Attribute
    {
        public object Value
        {
            get;
            private set;
        }

        public string LocalizationID
        {
            get;
            private set;
        }

        public EnumValueAttribute(object value)
        {
            Value = value;
        }

        public EnumValueAttribute(object value, string localizationID)
        {
            Value = value;
            LocalizationID = localizationID;
        }
    }
}
