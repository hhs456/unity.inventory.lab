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
    public class SlotBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler {

        private InventoryBase inventoryBase;

        public bool enableLog = false;
        [SerializeField]
        private Slot props;
        public Slot Props { get => props; }
        public Text nameText;
        public Image slotImage;
        public Image itemImage;
        public Text stackCount;

        public string emptyIconAddress = "Assets/RPG_inventory_icons/f.PNG";


        [SerializeField]
        private float stopWatch;

        private int index;
        public int Index { get => index;}

        private bool isAfterHoverTrigger = false;
        private bool isHovering = false;
        //private bool isDragging = false;
        private bool isValidDrop = false;        

        public float StopWatch { get => stopWatch; set => stopWatch = value; }
        

        [SerializeField]
        private float HoverEventTriggerTime = 0.02f;

        private Image dragging;

        public event EventHandler<Slot> HoverEventTriggerEnter;
        public event EventHandler<Slot> HoverEventTriggerExit;

        public event EventHandler<Slot> Abandon;

        void OnValidate() {
            index = transform.GetSiblingIndex();
        }

        void Awake() {
            ModifyTo(props);
            props.SlotUpdate += SlotUpdate;
            TimerSystem.GameWatch.Main.WatchUpdate += Counterdown;
            inventoryBase = GetComponentInParent<InventoryBase>();
            inventoryBase.AddDescribeAction(this);
            inventoryBase.AddAbandonAction(this);
        }

        void OnDestroy() {
            inventoryBase.RemoveDescribeAction(this);
            inventoryBase.RemoveAbandonAction(this);
        }

        private void SlotUpdate(object sender, EventArgs e) {
            ModifyTo(props);            
        }

        /// <summary>
        /// Modify SlotBase's properties to be same as target's properties.
        /// </summary>
        /// <param name="target">The target has the properties which you want to set into this SlotBase.</param>
        public void ModifyTo(Slot target) {
            if (target != null) {
                props.RelocateItemFrom(target);
                LoadDataFrom(props.Item.SpriteAddress);
                nameText.text = props.Item.Name;
                if (props.StackLimit > 1) {
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

        public void OnPointerEnter(PointerEventData eventData) {
            if (eventData.dragging) {
                return;
            }            
            isHovering = true;
        }

        private void Counterdown(object sender, TimerSystem.WatchArgs e) {
            if (isHovering) {
                // if pointer hovers on slot ...                
                stopWatch += 0.01f;
                if (stopWatch >= HoverEventTriggerTime && !isAfterHoverTrigger && !dragging && props.Item.Name != "") {
                    isAfterHoverTrigger = true;                    
                    HoverEventTriggerEnter?.Invoke(this, props);
                }
            }

        }

        public void OnPointerExit(PointerEventData eventData) {
            isHovering = false;
            stopWatch = 0;
            if (isAfterHoverTrigger) {
                isAfterHoverTrigger = false;
                HoverEventTriggerExit?.Invoke(this, props);                
            }
        }

        private void Clear() {
            TKLog.Log("Clear " + this, this, enableLog);            
            if (props.Item != null) {
                Addressables.LoadAssetAsync<Sprite>(props.Item.SpriteAddress).Completed -= OnAssetObjLoaded;
            }
            props.Clear();                       
        }

        public void OnDrop(PointerEventData eventData) {

            InventoryBase dragFrom = eventData.pointerDrag.GetComponentInParent<InventoryBase>();
            SlotBase dragSlot = eventData.pointerDrag.GetComponent<SlotBase>();

            if (!dragSlot.dragging) {
                return;
            }

            TKLog.Log("Drop To " + this, this, enableLog);
            SlotBase dropSlot = this;

            dragSlot.isValidDrop = true;
            dragSlot.itemImage.transform.localPosition = Vector3.zero;

            if (dropSlot == dragSlot) {
                return;
            }

            if (dropSlot.props.Item.Index != dragSlot.props.Item.Index) {
                Slot temp = new Slot(dragSlot.props, dragSlot.index);
                dragSlot.ModifyTo(dropSlot.props);
                dropSlot.ModifyTo(temp);
                TKLog.Log("Finish Exchanging", this, enableLog);
            }
            else {
                TKLog.Log("Stack To " + this, this, enableLog);
                int overStack = dropSlot.props.Add(dragSlot.props.StackCount);
                
                if (overStack == dragSlot.props.StackCount) {
                    // exchange slot stack count
                    dragSlot.props.StackCount = dropSlot.props.StackCount;
                    dropSlot.props.StackCount = overStack;
                }                
                else {
                    // calculate drag slot count
                    dragSlot.props.StackCount = overStack;                    
                }
            }
        }
        public void InvalidDrop() {
            TKLog.Log("Abandon " + props.Item.Index + " From " + this, this, enableLog);
            Abandon?.Invoke(this, props);
            props.Clear();
        }

        public void OnDrag(PointerEventData eventData) {
            if (dragging) {
                dragging.transform.position = eventData.position;
            }
        }

        public void OnBeginDrag(PointerEventData eventData) {            
            isValidDrop = false;
            if (isAfterHoverTrigger) {
                isAfterHoverTrigger = false;
                HoverEventTriggerExit?.Invoke(this, props);                
            }
            if (props.Item.Name != "") {                
                dragging = Instantiate(itemImage, transform.parent);
                dragging.raycastTarget = false;                
                TKLog.Log("Valid Drag From " + props.Item.Index, this, enableLog);
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            //isDragging = false;
            if (!isValidDrop) {
                InvalidDrop();
            }
            if (dragging) {
                Destroy(dragging.gameObject);
            }
        }

        public void LoadDataFrom(string address) {
            address = address ?? emptyIconAddress;
            Addressables.LoadAssetAsync<Sprite>(address).Completed += OnAssetObjLoaded;
        }
        public void OnAssetObjLoaded(AsyncOperationHandle<Sprite> asyncOperationHandle) {
            itemImage.sprite = asyncOperationHandle.Result;
        }
    }
}
