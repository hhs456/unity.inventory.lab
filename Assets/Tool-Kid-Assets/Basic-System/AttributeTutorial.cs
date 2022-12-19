using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AttributeTutorial : MonoBehaviour {
    /// <summary>
    /// The name text of object.
    /// </summary>
    [SerializeField, Multiline(2), Tooltip("The name text of object.")]
    private string objectName;
    [Header("Content")]
    [TextArea(2, 4)]
    public string description;
    [HideInInspector]
    public int count;
    [Range(0f, 1f)]
    public float rate;
}
