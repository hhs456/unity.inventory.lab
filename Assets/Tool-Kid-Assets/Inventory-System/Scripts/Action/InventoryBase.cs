using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolKid.InventorySystem {
    public class InventoryBase : MonoBehaviour {

        public bool enableLog = false;
        [SerializeField]
        private Inventory props;
        public Inventory Props {
            get {
                return props;
            }
            set {
                props = value;
            }
        }

        private SlotBase[] slotBases;
        public SlotBase[] SlotBases { get => slotBases; }
        public bool defaultEnable = false;
        private bool enable = false;
        [SerializeField]
        private Vector2 lastPosition;
        [SerializeField]
        private Vector2 hidePosition = new Vector2(0f, 1000f);
        [SerializeField]
        private KeyCode keyCode = KeyCode.None;
        public KeyCode KeyCode {
            get => keyCode;
        }

        private bool hasInitialized = false;
        public bool HasInitialized { get => hasInitialized; }

        public event EventHandler EndInit;

        public EventAction DescribeAction = new EventAction();
        public EventAction UndescribeAction = new EventAction();
        public EventAction AbandonAction = new EventAction();


        void Awake() {
            TimerSystem.GameWatch.Main.WatchUpdate += DspUpdate;
            slotBases = transform.GetComponentsInChildren<SlotBase>();
            enable = !defaultEnable;
            Switch();
            hasInitialized = true;
            TKLog.Log("InventoryBase Init Success!", this, enableLog);
        }

        private void DspUpdate(object sender, TimerSystem.WatchArgs e) {
            if (Input.GetKeyDown(KeyCode)) {
                Switch();
            }
        }

        public void Switch() {
            if (!enable) {
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(lastPosition.x, lastPosition.y);
            }
            else {
                lastPosition = transform.GetComponent<RectTransform>().anchoredPosition;
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(hidePosition.x, hidePosition.y);
            }
            enable = !enable;
            TKLog.Log("InventoryBase 'enable' is " + enable, this, enableLog);
        }

        public void Enable() {
            if (!enable) {
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(lastPosition.x, lastPosition.y);
                enable = !enable;
            }
            TKLog.Log("InventoryBase 'enable' is " + enable, this, enableLog);
        }

        public void Disable() {
            if (enable) {
                lastPosition = transform.GetComponent<RectTransform>().anchoredPosition;
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(hidePosition.x, hidePosition.y);
                enable = !enable;
            }
            TKLog.Log("InventoryBase 'enable' is " + enable, this, enableLog);
        }

        #region # Describer Callback
        public void AddDescribeAction(SlotBase slot) {            
            slot.HoverEventTriggerEnter += DescribeAction.OnActionTrigger;
            slot.HoverEventTriggerExit += UndescribeAction.OnActionTrigger;
        }
        public void RemoveDescribeAction(SlotBase slot) {
            slot.HoverEventTriggerEnter -= DescribeAction.OnActionTrigger;
            slot.HoverEventTriggerExit -= UndescribeAction.OnActionTrigger;
        }        
        #endregion
        #region # Describer Callback
        public void AddAbandonAction(SlotBase slot) {
            slot.Abandon += AbandonAction.OnActionTrigger;            
        }
        public void RemoveAbandonAction(SlotBase slot) {
            slot.Abandon -= AbandonAction.OnActionTrigger;
        }
        #endregion
    }
}
