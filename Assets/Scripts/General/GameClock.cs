using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClock : MonoBehaviour
{
    public Text display;
    public Watch watch;
    private bool isStart;
    private bool isPause;

    void Awake() {
        Timer.Pause += Pause;
        GameBase.Begin += Begin;
        GameBase.End += End;
        GameBase.OnBegin(this, new EventArgs());
    }

    private void Begin(object sender, EventArgs e) {
        isStart = true;
        isPause = false;
        Timer.Reset();
    }

    private void Pause(object sender, bool isPause) {
        this.isPause = isPause;
    }

    private void End(object sender, EventArgs e) {
        isStart = false;
    }

    void Update() {
        if (!isPause && isStart) {
            Timer.Update(this);
            watch = Timer.stageWatch;
            if (display)
                display.text = watch.playTime.ToString("000.00");
        }
    }
}