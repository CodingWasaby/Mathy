// Dandelion.Types
using Mathy.Utils.Dandelion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mathy.Utils.Dandelion
{
    public class Types
    {
        private class EnumMetadata
        {
            public Type EnumType
            {
                get;
                set;
            }

            public EnumMemberMetadata[] Members
            {
                get;
                set;
            }

            public object GetValue(string memberName, string localizationID)
            {
                return Members.First((EnumMemberMetadata i) => i.MemberName == memberName && i.LocalizationID == localizationID).Value;
            }

            public Enum GetEnum(object value, string localizationID)
            {
                return (Enum)Enum.Parse(EnumType, Members.First((EnumMemberMetadata i) => object.Equals(i.Value, value) && i.LocalizationID == localizationID).MemberName);
            }
        }

        private class EnumMemberMetadata
        {
            public string LocalizationID
            {
                get;
                set;
            }

            public string MemberName
            {
                get;
                set;
            }

            public object Value
            {
                get;
                set;
            }
        }

        private static Dictionary<string, EnumMetadata> enums;

        private static List<Type> primitiveTypes;

        public static Type GetType(string s, params string[] importedPackages)
        {
            switch (s)
            {
                case null:
                    return null;
                case "double":
                    return typeof(double);
                case "float":
                    return typeof(float);
                case "decimal":
                    return typeof(decimal);
                case "long":
                    return typeof(long);
                case "int":
                    return typeof(int);
                case "short":
                    return typeof(short);
                case "char":
                    return typeof(char);
                case "byte":
                    return typeof(byte);
                case "bool":
                    return typeof(bool);
                default:
                    {
                        if (s.IndexOf(".") >= 0)
                        {
                            return Type.GetType(s);
                        }
                        if (importedPackages.Length == 0)
                        {
                            return Type.GetType("System" + s);
                        }
                        Type type = null;
                        foreach (string str in importedPackages)
                        {
                            string typeName = str + "." + s;
                            if (Type.GetType(typeName) != null)
                            {
                                return Type.GetType(typeName);
                            }
                        }
                        return null;
                    }
            }
        }

        public static object CoerceValue(object value, int primitiveClassIndex)
        {
            return CoerceValue(value, primitiveTypes[primitiveClassIndex]);
        }

        private static EnumMetadata GetEnumMetadata(Type enumType)
        {
            if (!enums.ContainsKey(enumType.FullName))
            {
                List<EnumMemberMetadata> list = new List<EnumMemberMetadata>();
                Array values = Enum.GetValues(enumType);
                int num = 0;
                string[] names = Enum.GetNames(enumType);
                foreach (string text in names)
                {
                    MemberInfo element = enumType.GetMember(text)[0];
                    list.Add(new EnumMemberMetadata
                    {
                        MemberName = text,
                        Value = values.GetValue(num)
                    });
                    foreach (EnumValueAttribute item in from i in element.GetCustomAttributes()
                                                        where i is EnumValueAttribute
                                                        select i)
                    {
                        list.Add(new EnumMemberMetadata
                        {
                            LocalizationID = item.LocalizationID,
                            MemberName = text,
                            Value = item.Value
                        });
                    }
                    num++;
                }
                EnumMetadata enumMetadata = new EnumMetadata();
                enumMetadata.EnumType = enumType;
                enumMetadata.Members = list.ToArray();
                EnumMetadata value = enumMetadata;
                enums.Add(enumType.FullName, value);
            }
            return enums[enumType.FullName];
        }

        public static bool IsConvertable(object value, Type toType)
        {
            if (value == null || toType == null)
            {
                return true;
            }
            Type type = value.GetType();
            if (toType.IsAssignableFrom(type) || (IsPrimitiveType(type) && IsPrimitiveType(toType)))
            {
                return true;
            }
            if (toType == typeof(string))
            {
                return true;
            }
            if (type == typeof(string) && IsPrimitiveType(toType))
            {
                return true;
            }
            if (type.IsEnum && IsPrimitiveType(toType))
            {
                return true;
            }
            if (toType.IsEnum)
            {
                EnumMetadata enumMetadata = GetEnumMetadata(toType);
                if (IsPrimitiveType(type))
                {
                    int num = CoerceValue<int>(value);
                    return num >= 0 && num <= enumMetadata.Members.Length - 1;
                }
                if (type == typeof(string))
                {
                    EnumMemberMetadata[] members = enumMetadata.Members;
                    foreach (EnumMemberMetadata enumMemberMetadata in members)
                    {
                        if (enumMemberMetadata.MemberName.ToLower().Equals(((string)value).ToLower()))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static object GetValue(Enum instance)
        {
            return GetValue(instance, null);
        }

        public static object GetValue(Enum instance, string localizationID)
        {
            return GetEnumMetadata(instance.GetType()).GetValue(instance.ToString(), localizationID);
        }

        public static Enum GetEnum(object instance, Type enumType)
        {
            return GetEnum(instance, null, enumType);
        }

        public static Enum GetEnum(object instance, string localizationID, Type enumType)
        {
            return GetEnumMetadata(enumType).GetEnum(instance, localizationID);
        }

        public static object ConvertValue(object value, Type toType)
        {
            object obj = null;
            if (value == null)
            {
                return null;
            }
            if (toType == null)
            {
                return value;
            }
            Type type = value.GetType();
            if (toType.IsAssignableFrom(type) || (IsPrimitiveType(type) && IsPrimitiveType(toType)))
            {
                obj = CoerceValue(value, toType);
            }
            else if (toType == typeof(string))
            {
                obj = ((type == typeof(char)) ? new string(new char[1]
                {
                (char)value
                }) : value.ToString());
            }
            else if (type == typeof(string) && IsPrimitiveType(toType))
            {
                string text = (string)value;
                int primitiveTypeIndex = GetPrimitiveTypeIndex(toType);
                if (primitiveTypeIndex == 5 && text.Length != 1)
                {
                    throw new Exception($"Cannot convert from {text} to char.");
                }
                if (primitiveTypeIndex == 7 && text != "true" && text != "false")
                {
                    throw new Exception($"Cannot convert from {text} to boolean.");
                }
                switch (primitiveTypeIndex)
                {
                    case 0:
                        obj = double.Parse(text);
                        break;
                    case 1:
                        obj = float.Parse(text);
                        break;
                    case 2:
                        obj = decimal.Parse(text);
                        break;
                    case 3:
                        obj = long.Parse(text);
                        break;
                    case 4:
                        obj = int.Parse(text);
                        break;
                    case 5:
                        obj = short.Parse(text);
                        break;
                    case 6:
                        obj = text[0];
                        break;
                    case 7:
                        obj = byte.Parse(text);
                        break;
                    case 8:
                        obj = (text == "true");
                        break;
                }
            }
            else if (type.IsEnum && IsPrimitiveType(toType))
            {
                obj = CoerceValue(GetValue((Enum)value), toType);
            }
            else if (toType.IsEnum)
            {
                EnumMetadata enumMetadata = GetEnumMetadata(toType);
                if (IsPrimitiveType(type))
                {
                    int primitiveTypeIndex = CoerceValue<int>(value);
                    if (primitiveTypeIndex >= 0 && primitiveTypeIndex <= enumMetadata.Members.Length - 1)
                    {
                        obj = enumMetadata.Members[primitiveTypeIndex].Value;
                    }
                }
                else if (type == typeof(string))
                {
                    obj = enumMetadata.Members.FirstOrDefault((EnumMemberMetadata i) => i.MemberName == (string)value).Value;
                }
            }
            else if (typeof(IList).IsAssignableFrom(type) && toType.IsArray)
            {
                IList list = value as IList;
                Array array = Array.CreateInstance(toType.GetElementType(), list.Count);
                for (int j = 0; j <= list.Count - 1; j++)
                {
                    array.SetValue(list[j], j);
                }
                obj = array;
            }
            else if (type.IsArray && typeof(IList).IsAssignableFrom(toType))
            {
                IList list = Activator.CreateInstance(toType) as IList;
                Array array = value as Array;
                for (int j = 0; j <= array.Length - 1; j++)
                {
                    list.Add(array.GetValue(j));
                }
                obj = list;
            }
            if (value != null && obj == null)
            {
                throw new Exception($"Cannot convert from {value} to {toType}.");
            }
            return obj;
        }

        public static T ConvertValue<T>(object value)
        {
            return (T)ConvertValue(value, typeof(T));
        }

        public static object CoerceValue(object value, Type toType)
        {
            object obj = null;
            if (value == null)
            {
                return null;
            }
            if (toType == null)
            {
                return value;
            }
            Type type = value.GetType();
            if (toType.IsAssignableFrom(type))
            {
                obj = value;
            }
            else if (IsPrimitiveType(type) && IsPrimitiveType(toType))
            {
                int primitiveTypeIndex = GetPrimitiveTypeIndex(type);
                int primitiveTypeIndex2 = GetPrimitiveTypeIndex(toType);
                switch ((primitiveTypeIndex << 4) + primitiveTypeIndex2)
                {
                    case 0:
                        obj = (double)value;
                        break;
                    case 1:
                        obj = (float)(double)value;
                        break;
                    case 2:
                        obj = (decimal)(double)value;
                        break;
                    case 3:
                        obj = (long)(double)value;
                        break;
                    case 4:
                        obj = (int)(double)value;
                        break;
                    case 5:
                        obj = (short)(double)value;
                        break;
                    case 6:
                        obj = (char)(double)value;
                        break;
                    case 7:
                        obj = (byte)(double)value;
                        break;
                    case 8:
                        obj = ((double)value != 0.0);
                        break;
                    case 16:
                        obj = (double)(float)value;
                        break;
                    case 17:
                        obj = (float)value;
                        break;
                    case 18:
                        obj = (decimal)(float)value;
                        break;
                    case 19:
                        obj = (long)(float)value;
                        break;
                    case 20:
                        obj = (int)(float)value;
                        break;
                    case 21:
                        obj = (short)(float)value;
                        break;
                    case 22:
                        obj = (char)(float)value;
                        break;
                    case 23:
                        obj = (byte)(float)value;
                        break;
                    case 24:
                        obj = ((float)value != 0f);
                        break;
                    case 32:
                        obj = (double)(decimal)value;
                        break;
                    case 33:
                        obj = (float)(decimal)value;
                        break;
                    case 34:
                        obj = (decimal)value;
                        break;
                    case 35:
                        obj = (long)(decimal)value;
                        break;
                    case 36:
                        obj = (int)(decimal)value;
                        break;
                    case 37:
                        obj = (short)(decimal)value;
                        break;
                    case 38:
                        obj = (char)(decimal)value;
                        break;
                    case 39:
                        obj = (byte)(decimal)value;
                        break;
                    case 40:
                        obj = ((decimal)value != 0m);
                        break;
                    case 48:
                        obj = (double)(long)value;
                        break;
                    case 49:
                        obj = (float)(long)value;
                        break;
                    case 50:
                        obj = (decimal)(long)value;
                        break;
                    case 51:
                        obj = (long)value;
                        break;
                    case 52:
                        obj = (int)(long)value;
                        break;
                    case 53:
                        obj = (short)(long)value;
                        break;
                    case 54:
                        obj = (char)(long)value;
                        break;
                    case 55:
                        obj = (byte)(long)value;
                        break;
                    case 56:
                        obj = ((long)value != 0);
                        break;
                    case 64:
                        obj = (double)(int)value;
                        break;
                    case 65:
                        obj = (float)(int)value;
                        break;
                    case 66:
                        obj = (decimal)(int)value;
                        break;
                    case 67:
                        obj = (long)(int)value;
                        break;
                    case 68:
                        obj = (int)value;
                        break;
                    case 69:
                        obj = (short)(int)value;
                        break;
                    case 70:
                        obj = (char)(int)value;
                        break;
                    case 71:
                        obj = (byte)(int)value;
                        break;
                    case 72:
                        obj = ((int)value != 0);
                        break;
                    case 80:
                        obj = (double)(short)value;
                        break;
                    case 81:
                        obj = (float)(short)value;
                        break;
                    case 82:
                        obj = (decimal)(short)value;
                        break;
                    case 83:
                        obj = (long)(short)value;
                        break;
                    case 84:
                        obj = (int)(short)value;
                        break;
                    case 85:
                        obj = (short)value;
                        break;
                    case 86:
                        obj = (char)(short)value;
                        break;
                    case 87:
                        obj = (byte)(short)value;
                        break;
                    case 88:
                        obj = ((short)value != 0);
                        break;
                    case 96:
                        obj = (double)(int)(char)value;
                        break;
                    case 97:
                        obj = (float)(int)(char)value;
                        break;
                    case 98:
                        obj = (decimal)(char)value;
                        break;
                    case 99:
                        obj = (long)(char)value;
                        break;
                    case 100:
                        obj = (int)(char)value;
                        break;
                    case 101:
                        obj = (short)(char)value;
                        break;
                    case 102:
                        obj = (char)value;
                        break;
                    case 103:
                        obj = (byte)(char)value;
                        break;
                    case 104:
                        obj = ((char)value != '\0');
                        break;
                    case 112:
                        obj = (double)(int)(byte)value;
                        break;
                    case 113:
                        obj = (float)(int)(byte)value;
                        break;
                    case 114:
                        obj = (decimal)(byte)value;
                        break;
                    case 115:
                        obj = (long)(byte)value;
                        break;
                    case 116:
                        obj = (int)(byte)value;
                        break;
                    case 117:
                        obj = (short)(byte)value;
                        break;
                    case 118:
                        obj = (char)(byte)value;
                        break;
                    case 119:
                        obj = (byte)value;
                        break;
                    case 120:
                        obj = ((byte)value != 0);
                        break;
                    case 128:
                        obj = (double)(((bool)value) ? 1 : 0);
                        break;
                    case 129:
                        obj = (float)(((bool)value) ? 1 : 0);
                        break;
                    case 130:
                        obj = (decimal)(((bool)value) ? 1 : 0);
                        break;
                    case 131:
                        obj = (long)(((bool)value) ? 1 : 0);
                        break;
                    case 132:
                        obj = (((bool)value) ? 1 : 0);
                        break;
                    case 133:
                        obj = (short)(((bool)value) ? 1 : 0);
                        break;
                    case 134:
                        obj = (char)(((bool)value) ? 1 : 0);
                        break;
                    case 135:
                        obj = (byte)(((bool)value) ? 1 : 0);
                        break;
                    case 136:
                        obj = (bool)value;
                        break;
                    default:
                        obj = value;
                        break;
                }
            }
            if (value != null && obj == null)
            {
                throw new Exception($"Cannot coerce from {value} to {toType}.");
            }
            return obj;
        }

        public static T CoerceValue<T>(object value)
        {
            return (T)CoerceValue(value, typeof(T));
        }

        static Types()
        {
            enums = new Dictionary<string, EnumMetadata>();
            primitiveTypes = new List<Type>();
            primitiveTypes.Add(typeof(double));
            primitiveTypes.Add(typeof(float));
            primitiveTypes.Add(typeof(decimal));
            primitiveTypes.Add(typeof(long));
            primitiveTypes.Add(typeof(int));
            primitiveTypes.Add(typeof(short));
            primitiveTypes.Add(typeof(char));
            primitiveTypes.Add(typeof(byte));
            primitiveTypes.Add(typeof(bool));
        }

        public static bool IsNumberType(Type type)
        {
            int primitiveTypeIndex = GetPrimitiveTypeIndex(type);
            return (primitiveTypeIndex >= 0 && primitiveTypeIndex <= 5) || primitiveTypeIndex == 7;
        }

        public static bool IsPrimitiveType(Type type)
        {
            return primitiveTypes.IndexOf(type) >= 0;
        }

        public static int GetPrimitiveTypeIndex(Type type)
        {
            return primitiveTypes.IndexOf(type);
        }

        public static int ResolvePrimitiveTypeIndex(Type type1, Type type2)
        {
            return Math.Min(GetPrimitiveTypeIndex(type1), GetPrimitiveTypeIndex(type2));
        }

        public static Type GetPrimitiveType(int index)
        {
            return primitiveTypes[index];
        }

        public static Type SubstituteComponentTypes(Type type, Type toComponentType)
        {
            if (!type.IsArray)
            {
                return toComponentType;
            }
            return Array.CreateInstance(SubstituteComponentTypes(type.GetElementType(), toComponentType), 0).GetType();
        }
    }
}
