using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonSoundEffect {
    Ensure, Switch, Reback
}

public static class Storage {
    public static AudioSource AudioSource { get; set; }
    public static GameObject CurrentPage { get; set; }
    public static List<GameObject> LastPage { get; set; }
    public static AudioClip[] ButtonSoundEffect { get; set; }
    public static int CurrentStageId { get; set; }
}

public interface IStage {
    public string Name { get; set; }
    public string Description { get; set; }
    public int Index { get; set; }
    public int SubIndex { get; set; }
    public float Schedule { get; set; }
    public bool Pass { get; set; }    
}
