using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public struct PageInfo {    
    public string name;    
    public int pageId;
}

public class GUIListArgs {
    public int index;
    public bool enable;
    public float compensation;
}

public static class Extension {
    public static void Display(this Text display, string value) {
        display.text = value;
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

