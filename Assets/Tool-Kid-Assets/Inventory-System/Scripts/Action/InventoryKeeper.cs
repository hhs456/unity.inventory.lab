using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ToolKid.InventorySystem {
    public class InventoryKeeper : MonoBehaviour {
        [SerializeField] private List<InventoryBase> inventoryBases;
        [SerializeField] private List<InventoryProps> inventories;
        public List<InventoryProps> Inventories { get => inventories; }        
        public List<InventoryBase> InventoryBases { get => inventoryBases; }

        public Dictionary<string, InventoryBase> InventoryTable = new Dictionary<string, InventoryBase>();

        void OnValidate() {
            Init();
        }

        public void Init() {
            InventoryTable.Clear();
            int size = inventoryBases.Count;
            for (int i = 0; i < size; i++) {                
                if (InventoryTable.ContainsKey(inventories[i].Name)) {
                    Debug.LogErrorFormat("Repeat Name!", this);
                }
                else {
                    inventoryBases[i].Props = inventories[i];
                    InventoryTable.Add(inventories[i].Name, inventoryBases[i]);
                }
            }
        }

        public void OpenInventory(string name) {
            InventoryTable[name].Enable(false);
        }
    }
}
