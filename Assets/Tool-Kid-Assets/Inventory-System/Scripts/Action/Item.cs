using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolKid.InventorySystem {
    public class Item : MonoBehaviour {

        public bool enableLog = false;
        public InventoryBase InventoryBase;

        [SerializeField]
        private ItemProps props;
        public ItemProps Props {
            get {
                return props;
            }
            set {
                props = value;
            }
        }

        [SerializeField] private int stackCount;
        public int StackCount {
            get {
                return stackCount;
            }
            set {
                stackCount = value;
            }
        }

        public void Collect() {
            InventoryBase.Add(props, stackCount);
        }
    }
}
