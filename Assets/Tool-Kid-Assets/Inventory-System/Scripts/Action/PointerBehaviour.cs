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
    public class PointerBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler {

        private InventoryBase inventoryBase;

        public bool enableLog = false;
        public string emptyIconAddress = "Assets/RPG_inventory_icons/f.PNG";


        [SerializeField]
        private float stopWatch;

        private bool isAfterHoverTrigger = false;
        private bool isHovering = false;        
        private bool isValidDrop = false;

        public float StopWatch { get => stopWatch; set => stopWatch = value; }


        [SerializeField]
        private float HoverEventTriggerTime = 0.02f;

        private Image dragging;

        public event EventHandler<SlotProps> HoverEventTriggerEnter;
        public event EventHandler<SlotProps> HoverEventTriggerExit;

        public event EventHandler<SlotProps> Abandon;

        private SlotBase slotBase;

        void Awake() {
            slotBase = GetComponent<SlotBase>();
            TimerSystem.GameWatch.Main.WatchUpdate += Counterdown;
            inventoryBase = GetComponentInParent<InventoryBase>();
            inventoryBase.AddDescribeTrigger(this);
            inventoryBase.AddAbandonTrigger(this);
        }
        void OnDestroy() {
            inventoryBase.RemoveDescribeTrigger(this);
            inventoryBase.RemoveAbandonTrigger(this);
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
                if (stopWatch >= HoverEventTriggerTime && !isAfterHoverTrigger && !dragging && slotBase.Props.Item.Name != "") {
                    isAfterHoverTrigger = true;
                    HoverEventTriggerEnter?.Invoke(this, slotBase.Props);
                }
            }

        }

        public void OnPointerExit(PointerEventData eventData) {
            TKLog.Log("Exit " + slotBase, this, enableLog);
            isHovering = false;
            stopWatch = 0;
            if (isAfterHoverTrigger) {
                isAfterHoverTrigger = false;
                HoverEventTriggerExit?.Invoke(this, slotBase.Props);
            }
        }

        public void OnDrop(PointerEventData eventData) {
            PointerBehaviour dragPointer = eventData.pointerDrag.GetComponent<PointerBehaviour>();
            SlotBase dragSlot = dragPointer.slotBase;

            if (!eventData.dragging) {
                return;
            }

            TKLog.Log("Drop To " + this, this, enableLog);

            isHovering = true;
            SlotBase dropSlot = slotBase;

            dragPointer.isValidDrop = true;
            dragSlot.itemImage.transform.localPosition = Vector3.zero;

            if (dropSlot == dragSlot) {
                return;
            }
            inventoryBase.Relocate(dragSlot, dropSlot);
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
                HoverEventTriggerExit?.Invoke(this, slotBase.Props);
            }
            if (slotBase.Props.Item.Name != "") {
                dragging = Instantiate(slotBase.itemImage, transform.parent);
                dragging.raycastTarget = false;
                TKLog.Log("Valid Drag From " + slotBase.Props.Item.Index, this, enableLog);
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            //isDragging = false;
            if (!isValidDrop) {
                Abandon?.Invoke(this, slotBase.Props);
            }
            if (dragging) {
                Destroy(dragging.gameObject);
            }
        }
    }
}
