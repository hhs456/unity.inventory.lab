using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimerType {
    Menu, Game
}

public static class Timer {
    public static event EventHandler<EventArgs> Begin;
    public static event EventHandler<Watch> CentiSecond;    
    public static event EventHandler<bool> Pause;
    public static Watch gameWatch = new Watch();
    public static Watch stageWatch = new Watch();

    public static void Update(GameClock executor) {
        gameWatch.playTime = AudioSettings.dspTime;
        stageWatch.playTime = AudioSettings.dspTime - stageWatch.startTime - stageWatch.pauesTime;
        stageWatch.minute = (int)(stageWatch.playTime / 60f);
        stageWatch.second = (int) stageWatch.playTime - stageWatch.minute * 60;
        stageWatch.centiSecond = (int)(stageWatch.playTime * 100f % 100f);

        CentiSecond?.Invoke(executor, stageWatch);        
    }

    public static void OnPause(object sender, bool isPause) {        
        if (isPause) {
            stageWatch.pauseBeginTime = AudioSettings.dspTime;
        }
        else {
            stageWatch.pauseEndTime = AudioSettings.dspTime;
            stageWatch.GetPauseTime();
        }
        Pause?.Invoke(sender, isPause);
    }

    public static void Start() {        
        stageWatch.startTime = AudioSettings.dspTime;        
    }

    public static void Fix(float comp) {        
        stageWatch.playTime += comp;        
    }

    public static void Reset() {        
        stageWatch = new Watch();
        Start();
    }
}
[Serializable]
public struct Watch {
    public double startTime;
    public double playTime;
    public double pauseBeginTime;
    public double pauseEndTime;
    public double pauesTime;
    public int minute;
    public int second;
    public int centiSecond;

    public int Minute { get => minute; set => minute = value; }
    public int Second { get => second; set => second = value; }
    public int CentiSecond { get => centiSecond; set => centiSecond = value; }

    public double GetPauseTime() {
        pauesTime += pauseEndTime - pauseBeginTime;
        return pauesTime;
    }
}

public interface IWatch {

    int Minute { get; set; }
    int Second { get; set; }
    int CentiSecond { get; set; }
}
