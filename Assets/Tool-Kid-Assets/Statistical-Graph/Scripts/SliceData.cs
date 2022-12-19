using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SliceData {
    [SerializeField]
    protected int index;
    [SerializeField]
    protected float rate;
    [SerializeField]
    protected float count;
    [SerializeField]
    protected Image image;

    public int Index { get => index; }

    public float Rate { get => rate; private set => rate = value; }
    public float Count { get => count; }
    public Image Image { get => image;}

    public event Action Updated;

    public SliceData(int index, float count, Image image) {
        this.index = index;
        this.count = count;
        this.image = image;
    }

    public void OnTotalChanged(object sender, float e) {
        Rate = Count / e;
        Image.fillAmount = Rate;
        Updated?.Invoke();
    }
}
