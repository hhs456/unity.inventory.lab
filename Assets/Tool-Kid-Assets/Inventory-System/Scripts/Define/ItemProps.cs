using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace ToolKid.InventorySystem {
    /// <summary>
    /// Define the entire data of Item.
    /// </summary>
    [System.Serializable]
    public struct ItemProps {

        [SerializeField]
        private string name;
        public string Name { get => name; }

        [SerializeField]
        private string description;
        public string Description { get => description; }

        [SerializeField]
        private string index;
        public string Index { get => index; }
        
        [SerializeField]
        private int stackLimit;
        public int StackLimit { get => stackLimit; }

        [SerializeField]
        private int price;
        public int Price { get => price; }

        [SerializeField]
        private string[] tag;
        public string[] Tag { get => tag; }

        [SerializeField]
        private string spriteAddress;

        public string SpriteAddress { get => spriteAddress; }

        public ItemProps(ItemProps props) {
            name = props.name;
            description = props.description;
            index = props.index;
            stackLimit = props.stackLimit;
            price = props.price;
            spriteAddress = props.spriteAddress;
            tag = props.tag;
        }
        public ItemProps(SlotProps slot) {
            name = slot.Item.name;
            description = slot.Item.description;
            index = slot.Item.index;
            stackLimit = slot.Item.stackLimit;
            price = slot.Item.price;
            spriteAddress = slot.Item.spriteAddress;
            tag = slot.Item.tag;
        }

        public void Clear() {
            name = "";
            description = "";
            index = "";
            stackLimit = 0;
            price = 0;
            spriteAddress = "Assets/RPG_inventory_icons/f.PNG";
        }

        public void LoadDataFromAddress(ItemArgs itemArgs) {
            Addressables.LoadAssetAsync<TextAsset>(itemArgs.Address).Completed += OnItemDataLoaded;
        }
        public void OnItemDataLoaded(AsyncOperationHandle<TextAsset> asyncOperationHandle) {
            ItemProps itemProps = JsonUtility.FromJson<ItemProps>(asyncOperationHandle.Result.text);
            name = itemProps.name;
            description = itemProps.description;
            index = itemProps.index;
            price = itemProps.price;
            spriteAddress = itemProps.spriteAddress;
        }
    }

    /// <summary>
    /// Define the save data of Item.
    /// </summary>
    [System.Serializable]
    public struct ItemArgs {
        /// <summary>
        /// The identity number of Item.
        /// </summary>
        [SerializeField]
        private string index;
        public string Index { get => index; }
        /// <summary>
        /// The save path of Item data.
        /// </summary>
        [SerializeField]
        private string address;
        public string Address { get => address; }
    }
}