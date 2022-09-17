using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ToolKid.InventorySystem {
    public class EventAction {

        public event SlotAction ActionTrigger;        

        public void OnActionTrigger(object sender, Slot e) {
            ActionTrigger?.Invoke(sender, e);
        }
    }
}
