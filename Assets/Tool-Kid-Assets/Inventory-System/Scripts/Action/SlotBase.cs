using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace ToolKid.InventorySystem {
    public class SlotBase : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler*/ {

        private InventoryBase inventoryBase;

        public bool enableLog = false;
        [SerializeField]
        private SlotProps props;
        public SlotProps Props { get => props; }
        public Text nameText;
        public Image slotImage;
        public Image itemImage;
        public Text stackCount;

        public string emptyIconAddress = "Assets/RPG_inventory_icons/f.PNG";


        [SerializeField]
        private float stopWatch;

        private int index;
        public int Index { get => index;}

        public float StopWatch { get => stopWatch; set => stopWatch = value; }


        void OnValidate() {
            index = transform.GetSiblingIndex();
            props.SlotIndex = index;
        }

        void Awake() {
            ModifyTo(props);
            props.SlotUpdate += SlotUpdate;
            inventoryBase = GetComponentInParent<InventoryBase>();
        }

        private void SlotUpdate(object sender, EventArgs e) {
            ModifyTo(props);            
        }

        /// <summary>
        /// Modify SlotBase's properties to be same as target's properties.
        /// </summary>
        /// <param name="target">The target has the properties which you want to set into this SlotBase.</param>
        public void ModifyTo(SlotProps target) {
            if (target != null) {
                props.RelocateItemFrom(target);
                LoadDataFrom(props.Item.SpriteAddress);
                nameText.text = props.Item.Name;
                if (props.Item.StackLimit > 1) {
                    // is stackable item
                    stackCount.text = props.StackCount.ToString();
                }
                else {
                    stackCount.text = "";
                }
            }
            else {
                LoadDataFrom(null);
                nameText.text = "";
                stackCount.text = "";
            }
            TKLog.Log("Modify " + this, this, enableLog);            
        }

        public void LoadDataFrom(string address) {
            address = address ?? emptyIconAddress;
            if (address == "") {
                props.Clear();
                address = emptyIconAddress;
            }            
            Addressables.LoadAssetAsync<Sprite>(address).Completed += OnAssetObjLoaded;
        }
        public void OnAssetObjLoaded(AsyncOperationHandle<Sprite> asyncOperationHandle) {
            itemImage.sprite = asyncOperationHandle.Result;
        }
    }
}
