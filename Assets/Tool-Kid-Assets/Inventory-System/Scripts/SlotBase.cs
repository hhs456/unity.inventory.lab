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

        private bool isDescribing = false;
        private bool isHovering = false;
        private bool isDragging = false;
        private bool isValidDrop = false;        

        public float StopWatch { get => stopWatch; set => stopWatch = value; }
        

        [SerializeField]
        private float describeHoverTime = 0.5f;

        private Image dragging;

        public event EventHandler<Slot> Describe;
        public event EventHandler<Slot> Undescribe;

        void OnValidate() {
            index = transform.GetSiblingIndex();
        }

        void Awake() {
            ModifyTo(props);
            Timer.CentiSecond += Counterdown;            
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
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if (eventData.dragging) {
                return;
            }            
            isHovering = true;
        }

        private void Counterdown(object sender, Watch e) {
            if (isHovering) {
                // if pointer hovers on slot ...                
                stopWatch += 0.01f;
                if (stopWatch >= describeHoverTime && !isDescribing && !isDragging && props.Item.Name != "") {
                    isDescribing = true;
                    Describe?.Invoke(this, props);
                }
            }

        }

        public void OnPointerExit(PointerEventData eventData) {
            isHovering = false;
            stopWatch = 0;
            if (isDescribing) {
                isDescribing = false;
                Undescribe?.Invoke(this, props);                
            }
        }

        private void Clear() {
            if (props.Item != null) {
                Addressables.LoadAssetAsync<Sprite>(props.Item.SpriteAddress).Completed -= OnAssetObjLoaded;
            }
            props.Clear();
            ModifyTo(null);
            Debug.Log("Clear " + this, this);
        }

        public void OnDrop(PointerEventData eventData) {
            Debug.Log("Drop On " + this, this);
            
            InventoryBase dragFrom = eventData.pointerDrag.GetComponentInParent<InventoryBase>();
            SlotBase dragSlot = eventData.pointerDrag.GetComponent<SlotBase>();
            SlotBase dropSlot = this;

            dragSlot.isValidDrop = true;
            dragSlot.itemImage.transform.localPosition = Vector3.zero;            
            if (dropSlot != dragSlot) {                
                if (dropSlot.props.Item != null) {
                    if (dropSlot.props.Item.Index != dragSlot.props.Item.Index) {
                        Slot temp = new Slot(dragSlot.props, dragSlot.index);
                        dragSlot.ModifyTo(dropSlot.props);
                        dropSlot.ModifyTo(temp);
                        Debug.Log("Exchange Slot Index " + dragSlot.index + " And Index " + index, this);
                    }
                    else {
                        int overStack = dropSlot.props.Add(dragSlot.props.StackCount);
                        if (overStack == dragSlot.props.StackCount) {
                            dragSlot.props.StackCount = dropSlot.props.StackCount;
                            dropSlot.props.StackCount = overStack;
                            dragSlot.ModifyTo(dragSlot.props);
                            dropSlot.ModifyTo(dropSlot.props);
                        }
                        else {
                            dragSlot.props.StackCount = overStack;
                            dropSlot.ModifyTo(dropSlot.props);
                            if (dragSlot.props.StackCount > 0) {
                                dragSlot.ModifyTo(dragSlot.props);
                            }
                            else {
                                dragSlot.Clear();
                            }
                            Debug.Log("Stack " + props.Item.Index, this);
                        }
                    }
                }
                else {
                    dropSlot.ModifyTo(dragSlot.props);                    
                    dragSlot.Clear();
                }
            }
        }

        public void DropOutside() {
            Debug.Log("Drop Outside", this);
        }

        public void OnDrag(PointerEventData eventData) {            
            dragging.transform.position = eventData.position;            
        }

        public void OnBeginDrag(PointerEventData eventData) {
            Debug.Log("Begin Drag", this);
            isValidDrop = false;
            if (isDescribing) {
                isDescribing = false;
                Undescribe?.Invoke(this, props);
                //OnUndescribe();
            }
            isDragging = true;
            dragging = Instantiate(itemImage, transform.parent);
            dragging.raycastTarget = false;
        }

        public void OnEndDrag(PointerEventData eventData) {
            isDragging = false;
            if (!isValidDrop) {
                DropOutside();
            }
            Destroy(dragging.gameObject);            
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
