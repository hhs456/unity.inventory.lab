using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Expand {
    public static bool TryRepeatFront(this int[] value, int index) {
        for (int i = 0; i < index; i++) {
            if (value[index] == value[i])
                return true;
        }
        return false;
    }
}
