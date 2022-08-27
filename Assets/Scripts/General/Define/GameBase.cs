using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameBase
{
    public static event EventHandler<EventArgs> Init;
    public static event EventHandler Begin;
    public static event EventHandler End;

    public static void OnInit(object sender, EventArgs e) {
        Init?.Invoke(sender, e);
    }
    public static void OnBegin(object sender, EventArgs e) {
        Begin?.Invoke(sender, e);
    }
    public static void OnEnd(object sender, EventArgs e) {
        End?.Invoke(sender, e);
    }
}
