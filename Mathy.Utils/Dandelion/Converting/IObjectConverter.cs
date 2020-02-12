// Dandelion.Converting.IObjectConverter
using System;
using System.Collections;

namespace Mathy.Utils.Dandelion.Converting
{
    internal interface IObjectConverter
    {
        IDictionary ConvertToDictionary(object obj, Func<object, object> valueConverter);

        object ConvertFromDictionary(IDictionary dict, Func<object, object> valueConverter);
    }

}