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

        private Dictionary<int, LinkedList<Slot>> Slots { get; set; }

        public void AddNewItem(Item item) {
            LinkedList<Slot> slots = new LinkedList<Slot>();            
            LinkedListNode<Slot> slot = new LinkedListNode<Slot>(new Slot(item));
            slots.AddLast(slot);
            Slots.Add(item.Index, slots);
        }

        public void AddSlotStack(Item item, int count) {
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

        public void Remove(int itemIndex) {
            Slots.Remove(itemIndex);
        }


    }
    [System.Serializable]
    public class Slot {

        [SerializeField]
        private Item item;
        public Item Item { get => item; }

        [SerializeField] private int stackLimit;
        public int StackLimit { get => stackLimit; }

        [SerializeField] private int stackCount;
        public int StackCount { get => stackCount;}

        [SerializeField] private int slotIndex;
        public int SlotIndex { get => slotIndex; }

        [SerializeField] private int addedIndex;
        public int AddedIndex { get => addedIndex; }

        public Slot() {
            item = null;
        }

        public Slot (Item item) {
            this.item = item;            
        }
        public Slot(Slot slot) {
            item = slot.item;
            stackLimit = slot.stackLimit;
            stackCount = slot.stackCount;
            slotIndex = slot.slotIndex;
            addedIndex = slot.addedIndex;
        }

        public int Add(int count) {
            stackCount += count;
            int overload = stackCount - stackLimit;
            if (overload > 0) {
                stackCount = stackLimit;
            }
            return overload;
        }
        public void Remove(int count) {
            if (count > stackCount) {
                stackCount = 0;
            }
            else {
                stackCount -= count;
            }
        }
        
        public void SetPropsFrom(Slot slot) {
            item = slot.item;
            stackLimit = slot.stackLimit;
            stackCount = slot.stackCount;
            slotIndex = slot.slotIndex;
            addedIndex = slot.addedIndex;
        }

    }
    [System.Serializable]
    public class Item {

        [SerializeField]
        private string name;
        public string Name { get => name; }

        [SerializeField]
        private string description;
        public string Description { get => description; }

        [SerializeField]
        private int index;
        public int Index { get => index; }
        
        [SerializeField]
        private Sprite sprite;
        public Sprite Sprite { get => sprite; }

    }
}