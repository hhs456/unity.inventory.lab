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
    public class ItemProps {

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
        private string spriteAddress;

        public string SpriteAddress { get => spriteAddress; }

        public void Clear() {
            name = "";
            description = "";
            index = "";
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