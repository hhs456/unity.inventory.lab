using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace ToolKid.InventorySystem {

    public class AbandonBehaviour : MonoBehaviour {

        public bool enableLog = false;
        public SlotBase lastRelatedSlot;
        public Slot lastAbandonedTarget;
        private InventoryBase Base;
        public SlotEventAction action;

        void Awake() {
            Base = GetComponent<InventoryBase>();
            action.Action = Action;
            Base.AbandonAction.Trigger += action.Invoke;            
        }

        void OnDestroy() {
            Base.DescribeAction.Trigger -= action.Invoke;            
        }

        public void Action(Slot e) {
            lastRelatedSlot = Base.transform.GetChild(e.SlotIndex).GetComponent<SlotBase>();
            lastAbandonedTarget = new Slot(e, new ItemProps(e), e.SlotIndex);
            e.Clear();
            TKLog.Log("Abandon " + e.Item.Index + " From " + this, this, enableLog);                     
        }
    }
}
