using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ToolKid.InventorySystem {
    public class InventoryBase : MonoBehaviour {

        public bool enableLog = false;
        [SerializeField] private InventoryProps props = new InventoryProps();
        [SerializeField] private SlotBase[] slotBases;
        [SerializeField] private Vector2 lastPosition;
        [SerializeField] private Vector2 hidePosition = new Vector2(0f, 1000f);
        [SerializeField] private KeyCode keyCode = KeyCode.None;
        private bool hasInitialized = false;
        public InventoryProps Props { get => props; set => props = value; }
        public SlotBase[] SlotBases { get => slotBases; }
        public bool defaultEnable = false;
        private bool enable = false;
        
        public KeyCode KeyCode { get => keyCode; }        
        public bool HasInitialized { get => hasInitialized; }
        
        public SlotEvent DescribeAction = new SlotEvent();
        public SlotEvent UndescribeAction = new SlotEvent();
        public SlotEvent AbandonAction = new SlotEvent();

        public SlotBase hoveringSlot;

        void Awake() {
            TimerSystem.GameWatch.Main.WatchUpdate += DspUpdate;
            slotBases = transform.GetComponentsInChildren<SlotBase>();
            enable = !defaultEnable;
            Enable(!enabled);
            hasInitialized = true;
            TKLog.Log("InventoryBase Init Success!", this, enableLog);

            for (int i = 0; i < slotBases.Length; i++) {
                TKLog.Log("Build " + slotBases[i].Props.StackCount + " " + slotBases[i].Props.Item.Index + " into " + slotBases[i].name, this, enableLog);
                props.BuildTableWith(slotBases[i]);
            }
        }

        private void DspUpdate(object sender, TimerSystem.WatchArgs e) {
            if (Input.GetKeyDown(KeyCode)) {
                Enable(!enable);
            }
        }

        public void Enable(bool next) {
            if (!enable) {
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(lastPosition.x, lastPosition.y);
            }
            else {
                lastPosition = transform.GetComponent<RectTransform>().anchoredPosition;
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(hidePosition.x, hidePosition.y);
            }
            enable = next;
            TKLog.Log("InventoryBase 'enable' is " + enable, this, enableLog);
        }

        /// <summary>
        /// Add a item with given item and count arguments into this inventory.
        /// </summary>
        /// <param name="item">Item properties of target</param>
        /// <param name="count">Item count of target</param>
        public void Add(ItemProps item, int count) {
            if (props.TryAdd(item, count, out LinkedList<SlotBase> slots) > 0) {
                //int i = FirstEmptySlotIndex();
                if (Props.TryAddAtEmptyWith(item, count)) {                    
                    TKLog.Log("Build new node!", this, enableLog);
                }
                else {
                    TKLog.Log("Inventory is full!", this, enableLog);
                }
            }
        }

        public void Relocate(SlotBase from, SlotBase to) {
            if (from.Props.Item.Index != to.Props.Item.Index) {
                Exchange(from, to);
            }
            else {
                TKLog.Log("Stack To " + this, this, enableLog);
                int overStack = to.Props.Add(from.Props.StackCount);

                if (overStack == from.Props.StackCount) {
                    // exchange slot stack count
                    from.Props.StackCount = to.Props.StackCount;
                    to.Props.StackCount = overStack;
                }
                else {
                    // calculate drag slot count
                    from.Props.StackCount = overStack;
                    if (from.Props.StackCount == 0) {
                        Props.BuildTableWith(from);
                    }
                }
            }
        }

        public void Exchange(SlotBase a, SlotBase b) {
            SlotProps temp = new SlotProps(b.Props, b.Index);
            Props.FindNode(a).Value = b;
            Props.FindNode(b).Value = a;
            b.ModifyTo(a.Props);
            a.ModifyTo(temp);
            TKLog.Log("Exchanged " + a + " & " + b, this, enableLog);
        }

        #region # Describe Action Callback
        public void AddDescribeTrigger(PointerBehaviour slot) {            
            slot.HoverEventTriggerEnter += DescribeAction.OnTrigger;
            slot.HoverEventTriggerExit += UndescribeAction.OnTrigger;
        }
        public void RemoveDescribeTrigger(PointerBehaviour slot) {
            slot.HoverEventTriggerEnter -= DescribeAction.OnTrigger;
            slot.HoverEventTriggerExit -= UndescribeAction.OnTrigger;
        }
        #endregion

        #region # Abandon Action Callback
        public void AddAbandonTrigger(PointerBehaviour slot) {
            slot.Abandon += AbandonAction.OnTrigger;            
        }
        public void RemoveAbandonTrigger(PointerBehaviour slot) {
            slot.Abandon -= AbandonAction.OnTrigger;
        }
        #endregion
    }
}
