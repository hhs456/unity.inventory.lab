using System.Collections;
using UnityEngine;

public static class Vibration {
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;

    public static bool IsAndroid() {
        try {
#if UNITY_ANDROID
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                    vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
                }
            }
            return true;
#else
        return false;
#endif
        }
        catch {
            Debug.LogWarning("It is simulator, not Android.");
            return false;
        }
    }

    /// 

    /// 震動
    /// 
    public static void Vibrate() {
        if (IsAndroid())
            vibrator.Call("vibrate");
        else
            Handheld.Vibrate();
    }

    /// 

    /// 震動(毫秒)
    /// 
    /// 
    public static void Vibrate(long milliseconds) {
        if (IsAndroid())
            vibrator.Call("vibrate", milliseconds);
        else
            Handheld.Vibrate();
    }

    /// 

    /// 模式震動
    /// 重覆
    /// 
    /// 
    /// 
    public static void Vibrate(long[] pattern, int repeat) {
        if (IsAndroid())
            vibrator.Call("vibrate", pattern, repeat);
        else
            Handheld.Vibrate();
    }

    /// 

    /// 判斷是否震動器
    /// 
    /// 
    public static bool HasVibrator() {
        return IsAndroid();
    }

    /// 

    /// 取消
    /// 
    public static void Cancel() {
        if (IsAndroid())
            vibrator.Call("cancel");
    }
}