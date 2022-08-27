using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ToolKid.InventorySystem {
    [CreateAssetMenu(fileName = "Item Viewer", menuName = "ScriptableObjects/Item Viewer", order = 1)]
    public class ItemViewer : ScriptableObject {        
        public string dataBaseAddress;
        public TextAsset dataBase;
        [SerializeField]
        public ItemArray Items;


        public void LoadDataFromAddress() {            
            Addressables.LoadAssetAsync<TextAsset>(dataBaseAddress).Completed += OnAssetObjLoaded;            
        }
        public void OnAssetObjLoaded(AsyncOperationHandle<TextAsset> asyncOperationHandle) {
            dataBase = asyncOperationHandle.Result;
        }
    }

    [System.Serializable]
    public class ItemArray {
        [SerializeField]
        public Item[] Data;
    }
}
