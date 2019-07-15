using Etutor.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Etutor.Core.Extensions
{
    public static class EnumExtensions
    {
        public static T ToInt32<T>(this int s) where T : struct
        {
            return (T)Enum.Parse(typeof(T), s.ToString(), true);
        }

        public static IEnumerable<T> ToArray<T>(this int[] s) where T : struct
        {
            for (int i = 0; i < s.Length; i++)
                yield return (T)Enum.Parse(typeof(T), s[i].ToString(), true);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T GetEnum<T>(string value)
        {
            var names = Enum.GetNames(typeof(T));
            return (T)Enum.Parse(typeof(T), value);
        }


        public static IEnumerable<SelectItemOption> ToListOptions<TEnum>(this TEnum enumeracion)
        {
            var enumType = typeof(TEnum);
            var list = from enumerator in Enum.GetNames(enumType)
                       let itemOption = enumerator.GetEnumDescription<TEnum>()
                       where itemOption != null
                       select itemOption;

            return list;
        }

        public static SelectItemOption GetEnumDescription<T>(this string name)
        {
            //
            //get the member info of the enum
            MemberInfo[] memberInfos = typeof(T).GetMembers();
            if (memberInfos.Length > 0)
            {

                var fieldCharacteristics = from memberInfo in memberInfos
                                           let attributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                           where attributes.Length > 0 && memberInfo.Name == name
                                           select new { attributes, fieldInfo = ((FieldInfo)memberInfo) };


                //loop through the member info classes
                foreach (var characteristic in fieldCharacteristics)
                {
                    var descriptionAttribute = (DescriptionAttribute)characteristic.attributes.FirstOrDefault();
                    if (descriptionAttribute != null)
                        return new SelectItemOption
                        {
                            Key = (int)characteristic.fieldInfo.GetValue(new object()),
                            KeyText = characteristic.fieldInfo.Name,
                            Value = descriptionAttribute.Description.ToString(CultureInfo.InvariantCulture)
                        };
                }
            }

            //this means the enum was not found from the description, so return the default
            return null;
        }

    }
}
