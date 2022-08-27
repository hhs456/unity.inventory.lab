using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {
    public AudioSource Source {
        get {
            if (!Storage.AudioSource) Storage.AudioSource = GetComponent<AudioSource>();
            return Storage.AudioSource;
        }
        set {
            Storage.AudioSource = value;
        }
    }    

    void OnValidate() {
        Source = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Awake() {
        Source = GetComponent<AudioSource>();
    }
}
