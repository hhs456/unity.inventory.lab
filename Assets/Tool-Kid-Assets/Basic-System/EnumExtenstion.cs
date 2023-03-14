using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ToolKid.SystemExtension {
    public static class EnumExtension {
        public static string GetInspectorName(this Enum e) {
            Type t = e.GetType();
            string name = e.ToString();
            string[] names = name.Split(',');
            name = "";
            for (int i = 0; i < names.Length; i++) {
                var fieldInfo = t.GetField(names[i]);
                var attributes = (InspectorNameAttribute[])fieldInfo.GetCustomAttributes(typeof(InspectorNameAttribute), false);
                if (i > 0) {
                    name += ", ";
                }
                name += attributes.FirstOrDefault()?.displayName ?? string.Empty;
            }
            return name;
        }
        public static string GetDescription(this Enum value) {
            return value.GetType()
                .GetRuntimeField(value.ToString())
                .GetCustomAttributes<System.ComponentModel.DescriptionAttribute>()
                .FirstOrDefault()?.Description ?? string.Empty;
        }
        public static string[] GetDescriptions(this Enum value) {
            string[] originName = Enum.GetNames(value.GetType());
            int i_size = Enum.GetValues(value.GetType()).Length;
            string[] names = new string[i_size];
            for (int i = 0; i < i_size; i++) {
                names[i] = value.GetType()
                .GetRuntimeField(originName[i])
                .GetCustomAttributes<System.ComponentModel.DescriptionAttribute>()
                .FirstOrDefault()?.Description ?? string.Empty;
            }
            return names;
        }
    }
}