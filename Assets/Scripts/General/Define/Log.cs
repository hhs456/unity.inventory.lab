using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Log {

    public static void Event(string content, bool isDebugging) {
        if (isDebugging)
            Debug.Log("event: " + content);
    }
    public static void Event(Object sender, string content, bool isDebugging) {
        if (isDebugging)
            Debug.Log("event: " + content, sender);
    }
    public static void Warning(Object sender, string content, bool isDebugging) {
        if (isDebugging)
            Debug.LogWarning("warning: " + content, sender);
    }
    public static void Error(Object sender, string content) {
        Debug.LogError("error: " + content, sender);
    }
    public static void Error(Object sender, bool correct, string content) {
        Debug.Assert(correct, "error: " + content, sender);
    }
}
