using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolKid.InventorySystem {
    public class InventoryBase : MonoBehaviour {

        public bool enableLog = false;
        [SerializeField]
        private Inventory props = new Inventory();
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
        
        public SlotEvent DescribeAction = new SlotEvent();
        public SlotEvent UndescribeAction = new SlotEvent();
        public SlotEvent AbandonAction = new SlotEvent();


        void Awake() {
            TimerSystem.GameWatch.Main.WatchUpdate += DspUpdate;
            slotBases = transform.GetComponentsInChildren<SlotBase>();
            enable = !defaultEnable;
            Switch();
            hasInitialized = true;
            TKLog.Log("InventoryBase Init Success!", this, enableLog);

            for (int i = 0; i < slotBases.Length; i++) {
                TKLog.Log("Build " + slotBases[i].Props.StackCount + " " + slotBases[i].Props.Item.Index + " into " + slotBases[i].name, this, enableLog);
                props.BuildTableWith(slotBases[i].Props);
            }
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
        /// <summary>
        /// Add a item with given item and count arguments into this inventory.
        /// </summary>
        /// <param name="item">Item properties of target</param>
        /// <param name="count">Item count of target</param>
        public void Add(ItemProps item, int count) {
            if (props.TryAdd(item, count, out LinkedList<Slot> slots) > 0) {
                int i = FirstEmptySlotIndex();
                if (i != -1) {
                    SlotBases[i].Props.Set(item, count);
                    slots.AddLast(SlotBases[i].Props);
                    TKLog.Log("Build " + SlotBases[i].Props.StackCount + " " + SlotBases[i].Props.Item.Index + " into " + SlotBases[i].name, this, enableLog);
                }
                else {
                    TKLog.Log("Inventory is full!", this, enableLog);
                }
            }
        }

        public int FirstEmptySlotIndex() {
            for (int i = 0; i < SlotBases.Length; i++) {
                if (SlotBases[i].Props.Item.Index == "") {                    
                    return i;
                }
            }
            return -1;
        }

        #region # Describe Action Callback
        public void AddDescribeTrigger(SlotBase slot) {            
            slot.HoverEventTriggerEnter += DescribeAction.OnTrigger;
            slot.HoverEventTriggerExit += UndescribeAction.OnTrigger;
        }
        public void RemoveDescribeTrigger(SlotBase slot) {
            slot.HoverEventTriggerEnter -= DescribeAction.OnTrigger;
            slot.HoverEventTriggerExit -= UndescribeAction.OnTrigger;
        }
        #endregion

        #region # Abandon Action Callback
        public void AddAbandonTrigger(SlotBase slot) {
            slot.Abandon += AbandonAction.OnTrigger;            
        }
        public void RemoveAbandonTrigger(SlotBase slot) {
            slot.Abandon -= AbandonAction.OnTrigger;
        }
        #endregion
    }
}
