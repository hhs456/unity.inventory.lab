using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolKid.InventorySystem {
    [System.Serializable]
    public class Inventory {
        [SerializeField]
        private string name;
        public string Name { get => name; }
        [SerializeField]
        private int index;
        public int Index { get => index; }
        [SerializeField]
        private int currentSelected;
        public int CurrentSelected { get => currentSelected; }
        [SerializeField]
        private int defaultSelected;
        public int DefaultSelected { get => defaultSelected; }
        [SerializeField]
        private int min;
        public int Min { get => min; }
        [SerializeField]
        private int max;
        public int Max { get => max; }

        private Dictionary<string, LinkedList<SlotBase>> slots = new Dictionary<string, LinkedList<SlotBase>>();
        public Dictionary<string, LinkedList<SlotBase>> Slots {
            get {
                return slots;
            }
            set {
                slots = value;
            }
        }

        public LinkedListNode<SlotBase> FindNode(SlotBase slot) {
            Slots.TryGetValue(slot.Props.Item.Index, out LinkedList<SlotBase> slots);
            return slots.Find(slot);
        }

        public void BuildTableWith(SlotBase slot) {
            if (Slots.TryGetValue(slot.Props.Item.Index, out LinkedList<SlotBase> slots)) {                
                slots.AddLast(slot);                
            }
            else {
                AddNewItem(slot);
            }
        }

        public int TryAdd(ItemProps item, int count, out LinkedList<SlotBase> slots) {
            if(Slots.TryGetValue(item.Index, out slots)) {
                LinkedListNode<SlotBase> slotNode = slots.First;
                return TryStack(slotNode, count);               
            }
            return count;
        }

        public void AddNewItem(SlotBase slot) {
            LinkedList<SlotBase> slots = new LinkedList<SlotBase>();
            slots.AddLast(slot);
            Slots.Add(slot.Props.Item.Index, slots);
        }

        public int TryStack(LinkedListNode<SlotBase> node, int count) {
            SlotBase slot = node.Value;
            int overload = slot.Props.Add(count);
            if(overload > 0) {
                if (node.Next != null) {                    
                    return TryStack(node.Next, count);
                }
            }
            return overload;
        }

        public void Remove(string itemIndex) {
            Slots.Remove(itemIndex);
        }

    }    

    [System.Serializable]
    public class Slot {

        public event EventHandler SlotUpdate;

        [SerializeField]
        private ItemProps item;
        public ItemProps Item { get => item; }
                

        [SerializeField] private int stackCount;
        public int StackCount {
            get {
               return stackCount;
            }
            set {
                stackCount = value;
                SlotUpdate?.Invoke(this, new EventArgs());
                if (stackCount < 1) {
                    Clear();
                }
            }
        }


        #region # Inventory Informations

        [SerializeField]
        private int inventoryIndex;
        public int InventoryIndex { get => inventoryIndex; }

        [SerializeField]
        private int slotIndex;
        public int SlotIndex { get => slotIndex; set => slotIndex = value; }

        [SerializeField]
        private int addedIndex;
        public int AddedIndex { get => addedIndex; }

        #endregion

        public Slot() {
            item = null;
            SlotUpdate?.Invoke(this, new EventArgs());
        }

        public Slot(ItemProps item) {
            this.item = item;
            SlotUpdate?.Invoke(this, new EventArgs());
        }
        public Slot(Slot slot, int index) {
            item = slot.item;            
            stackCount = slot.stackCount;
            slotIndex = index;
            addedIndex = slot.addedIndex;
            SlotUpdate?.Invoke(this, new EventArgs());
        }
        public Slot(Slot slot, ItemProps item, int index) {
            this.item = item;            
            stackCount = slot.stackCount;
            slotIndex = index;
            addedIndex = slot.addedIndex;
            SlotUpdate?.Invoke(this, new EventArgs());
        }

        public void Set(ItemProps item, int count) {
            this.item = item;
            stackCount = count;
            SlotUpdate?.Invoke(this, new EventArgs());
        }

        public int Add(int count) {
            if(stackCount == item.StackLimit) {
                return count;
            }
            stackCount += count;
            int overload = stackCount - item.StackLimit;
            if (overload > 0) {
                stackCount = item.StackLimit;
            }
            SlotUpdate?.Invoke(this, new EventArgs());
            return overload;
        }
        public void Remove(int count) {
            if (count > stackCount) {
                stackCount = 0;
            }
            else {
                stackCount -= count;
            }
            SlotUpdate?.Invoke(this, new EventArgs());
        }
        
        public void RelocateItemFrom(Slot slot) {
            item = slot.item;            
            stackCount = slot.stackCount;            
            addedIndex = slot.addedIndex;
            //SlotUpdate?.Invoke(this, new EventArgs());
        }

        public void Clear() {
            item.Clear();            
            stackCount = 0;
            addedIndex = 0;
            SlotUpdate?.Invoke(this, new EventArgs());
        }
    }
}