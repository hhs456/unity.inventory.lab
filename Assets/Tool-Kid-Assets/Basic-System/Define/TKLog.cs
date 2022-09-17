using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolKid {
    public static class TKLog {
        public static void Log(object message, Object context, bool enable) {
            if (!enable) {
                return;
            }
            Debug.Log(message, context);
        }
    }
}
