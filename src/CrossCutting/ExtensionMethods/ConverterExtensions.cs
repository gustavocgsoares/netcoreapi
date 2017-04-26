using System;
using System.Globalization;
using System.Reflection;

namespace Template.CrossCutting.ExtensionMethods
{
    public static class ConverterExtensions
    {
        #region Public methods
        public static TTarget To<TTarget>(this object value)
        {
            if (value == null)
            {
                return default(TTarget);
            }

            if (typeof(TTarget).GetTypeInfo().IsEnum)
            {
                if (value.GetType().GetTypeInfo().IsEnum)
                {
                    try
                    {
                        return (TTarget)Enum.ToObject(typeof(TTarget), Convert.ToInt32(value));
                    }
                    catch (InvalidCastException)
                    {
                        return (TTarget)Enum.ToObject(typeof(TTarget), Convert.ToChar(value));
                    }
                }

                return (TTarget)Enum.Parse(typeof(TTarget), value.ToString(), true);
            }

            if (typeof(TTarget) == typeof(Guid))
            {
                return (TTarget)(object)new Guid(value.ToString());
            }

            switch (Type.GetTypeCode(typeof(TTarget)))
            {
                case TypeCode.Boolean:
                    return (TTarget)(object)Convert.ToBoolean(value, CultureInfo.InvariantCulture);
                case TypeCode.Byte:
                    return (TTarget)(object)Convert.ToByte(value, CultureInfo.InvariantCulture);
                case TypeCode.Char:
                    return (TTarget)(object)Convert.ToChar(value, CultureInfo.InvariantCulture);
                case TypeCode.DateTime:
                    return (TTarget)(object)Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                case TypeCode.Decimal:
                    return (TTarget)(object)Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                case TypeCode.Double:
                    return (TTarget)(object)Convert.ToDouble(value, CultureInfo.InvariantCulture);
                case TypeCode.Int16:
                    return (TTarget)(object)Convert.ToInt16(value, CultureInfo.InvariantCulture);
                case TypeCode.Int32:
                    return (TTarget)(object)Convert.ToInt32(value, CultureInfo.InvariantCulture);
                case TypeCode.Int64:
                    return (TTarget)(object)Convert.ToInt64(value, CultureInfo.InvariantCulture);
                case TypeCode.SByte:
                    return (TTarget)(object)Convert.ToSByte(value, CultureInfo.InvariantCulture);
                case TypeCode.Single:
                    return (TTarget)(object)Convert.ToSingle(value, CultureInfo.InvariantCulture);
                case TypeCode.String:
                    return (TTarget)(object)Convert.ToString(value, CultureInfo.InvariantCulture);
                case TypeCode.UInt16:
                    return (TTarget)(object)Convert.ToUInt16(value, CultureInfo.InvariantCulture);
                case TypeCode.UInt32:
                    return (TTarget)(object)Convert.ToUInt32(value, CultureInfo.InvariantCulture);
                case TypeCode.UInt64:
                    return (TTarget)(object)Convert.ToUInt64(value, CultureInfo.InvariantCulture);
                case TypeCode.Object:
                    return (TTarget)value;
                default:
                    var name = value != null ? value.GetType().Name : string.Empty;
                    throw new NotImplementedException($"Generic type '{name}' not suported.");
            }
        }
        #endregion
    }
}
