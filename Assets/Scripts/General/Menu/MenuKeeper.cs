using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuKeeper : MonoBehaviour
{
    public AudioClip ensureEffect;
    public AudioClip switchEffect;
    public AudioClip rebackEffect;

    void Awake() {
        Storage.CurrentPage = null;
        Storage.LastPage = new List<GameObject>();
        Timer.CentiSecond += Timer_CentiSecond;
        Storage.ButtonSoundEffect = new AudioClip[3] { ensureEffect, switchEffect, rebackEffect };        
    }

    private void Timer_CentiSecond(object sender, Watch e) {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Storage.CurrentPage.SetActive(false);
            Storage.CurrentPage = Storage.LastPage[Storage.LastPage.Count - 1];
            Storage.LastPage.RemoveAt(Storage.LastPage.Count - 1);
        }
    }
}