using System;
using System.ComponentModel;
using System.Globalization;

namespace Etutor.Core.Utilities
{
    public static class Converter
    {

        /// <summary>
        ///     Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }


        /// <summary>
        ///     Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value != null)
            {
                var sourceType = value.GetType();

                var destinationConverter = GetNopCustomTypeConverter(destinationType);

                var sourceConverter = GetNopCustomTypeConverter(sourceType);

                if ((destinationConverter != null) && destinationConverter.CanConvertFrom(value.GetType()))

                    return destinationConverter.ConvertFrom(null, culture, value);

                if ((sourceConverter != null) && sourceConverter.CanConvertTo(destinationType))

                    return sourceConverter.ConvertTo(null, culture, value, destinationType);

                if (destinationType.IsEnum && value is int)

                    return Enum.ToObject(destinationType, (int)value);

                if (!destinationType.IsInstanceOfType(value))

                    return Convert.ChangeType(value, destinationType, culture);
            }

            return value;
        }


        /// <summary>
        ///     Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <returns>The converted value.</returns>
        public static T To<T>(object value)
        {
            return (T)To(value, typeof(T));
        }

        public static TypeConverter GetNopCustomTypeConverter(Type type)
        {
            return TypeDescriptor.GetConverter(type);
        }
    }
}
