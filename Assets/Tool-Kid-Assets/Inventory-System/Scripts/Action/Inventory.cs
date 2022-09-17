using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

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

        private Dictionary<string, LinkedList<Slot>> Slots { get; set; }

        public void AddNewItem(ItemProps item) {
            LinkedList<Slot> slots = new LinkedList<Slot>();            
            LinkedListNode<Slot> slot = new LinkedListNode<Slot>(new Slot(item));
            slots.AddLast(slot);
            Slots.Add(item.Index, slots);
        }

        public void AddSlotStack(ItemProps item, int count) {
            Slots.TryGetValue(item.Index, out LinkedList<Slot> slots);
            LinkedListNode<Slot> slotNode = slots.First;           
            AddSlotStack(slotNode, count);
        }

        public void AddSlotStack(LinkedListNode<Slot> node, int count) {
            Slot slot = node.Value;
            int overload = slot.Add(count);
            if(overload > 0) {
                AddSlotStack(node.Next, count);
            }
        }

        public void Remove(string itemIndex) {
            Slots.Remove(itemIndex);
        }


    }

    public delegate void SlotAction(object sender, Slot e);

    [System.Serializable]
    public class Slot {

        public event EventHandler SlotUpdate;

        [SerializeField]
        private ItemProps item;
        public ItemProps Item { get => item; }

        [SerializeField] private int stackLimit;
        public int StackLimit { get => stackLimit; }

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
        public int SlotIndex { get => slotIndex; }

        [SerializeField]
        private int addedIndex;
        public int AddedIndex { get => addedIndex; }

        #endregion

        public Slot() {
            item = null;
            SlotUpdate?.Invoke(this, new EventArgs());
        }

        public Slot (ItemProps item) {
            this.item = item;
            SlotUpdate?.Invoke(this, new EventArgs());
        }
        public Slot(Slot slot, int index) {
            item = slot.item;
            stackLimit = slot.stackLimit;
            stackCount = slot.stackCount;
            slotIndex = index;
            addedIndex = slot.addedIndex;
            SlotUpdate?.Invoke(this, new EventArgs());
        }

        public int Add(int count) {
            if(stackCount == stackLimit) {
                return count;
            }
            stackCount += count;
            int overload = stackCount - stackLimit;
            if (overload > 0) {
                stackCount = stackLimit;
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
            stackLimit = slot.stackLimit;
            stackCount = slot.stackCount;            
            addedIndex = slot.addedIndex;
            //SlotUpdate?.Invoke(this, new EventArgs());
        }

        public void Clear() {
            item.Clear();
            stackLimit = 0;
            stackCount = 0;
            addedIndex = 0;
            SlotUpdate?.Invoke(this, new EventArgs());
        }
    }
}