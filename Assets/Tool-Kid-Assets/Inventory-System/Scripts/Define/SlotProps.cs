using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolKid.InventorySystem {
    [System.Serializable]
    public class SlotProps {

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

        public SlotProps() {
            item = new ItemProps();
            SlotUpdate?.Invoke(this, new EventArgs());
        }

        public SlotProps(ItemProps item) {
            this.item = item;
            SlotUpdate?.Invoke(this, new EventArgs());
        }
        public SlotProps(SlotProps slot, int index) {
            item = slot.item;
            stackCount = slot.stackCount;
            slotIndex = index;
            addedIndex = slot.addedIndex;
            SlotUpdate?.Invoke(this, new EventArgs());
        }
        public SlotProps(SlotProps slot, ItemProps item, int index) {
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
            if (stackCount == item.StackLimit) {
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

        public void RelocateItemFrom(SlotProps slot) {
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