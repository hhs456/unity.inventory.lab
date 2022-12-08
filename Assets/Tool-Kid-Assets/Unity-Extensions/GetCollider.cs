using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolKid.ExtensionMethods.UnityEngine;

public class GetCollider : MonoBehaviour
{
    public uint depth = 0;
    public BoxCollider[] Colliders;    

    private void OnValidate() {
        Colliders = this.GetComponentsArrayInRoot<BoxCollider>(depth);
    }
}
