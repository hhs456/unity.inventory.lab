using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolKid.InventorySystem {
    [System.Serializable]
    public class InventoryProps {
        [SerializeField] private string name;
        [SerializeField] private int index;
        [SerializeField] private int currentSelected;
        [SerializeField] private int defaultSelected;
        [SerializeField] private int min;
        [SerializeField] private int max;
        private Dictionary<string, LinkedList<SlotBase>> slots = new Dictionary<string, LinkedList<SlotBase>>();
        public string Name { get => name; }
        public int Index { get => index; }
        public int CurrentSelected { get => currentSelected; }
        public int DefaultSelected { get => defaultSelected; }
        public int Min { get => min; }
        public int Max { get => max; }
        public Dictionary<string, LinkedList<SlotBase>> Slots {
            get {
                return slots;
            }
            set {
                slots = value;
            }
        }

        public bool TryAddAtEmptyWith(ItemProps item, int count) {
            if (Slots.ContainsKey("")) {
                SlotBase target = FindMinSiblingIndexFrom(Slots[""]);
                Slots[""].Remove(target);
                if (Slots[""].Count == 0) {
                    Slots.Remove("");
                }
                target.Props.Set(item, count);
                BuildTableWith(target);
                return true;
            }
            return false;
        }

        public SlotBase FindMinSiblingIndexFrom(LinkedList<SlotBase> list) {
            LinkedListNode<SlotBase> ref_node = list.First;
            SlotBase current = ref_node.Value;            
            while (ref_node.Next != null) {
                ref_node = ref_node.Next;
                if (ref_node.Value.transform.GetSiblingIndex() < current.transform.GetSiblingIndex()) {
                    current = ref_node.Value;
                }                
            }
            return current;
        }

        public LinkedListNode<SlotBase> FindNode(SlotBase slot) {            
            return Slots[slot.Props.Item.Index].Find(slot);
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
            if (Slots.TryGetValue(item.Index, out slots)) {
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
            if (overload > 0) {
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
}