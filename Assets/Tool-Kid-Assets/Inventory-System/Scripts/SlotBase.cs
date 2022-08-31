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
        //private bool isDragging = false;
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
            props.SlotUpdate += SlotUpdate;
            Timer.CentiSecond += Counterdown;            
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
            Debug.Log("Modify " + this, this);
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
                if (stopWatch >= describeHoverTime && !isDescribing && !dragging && props.Item.Name != "") {
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
            Debug.Log("Clear " + this, this);
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
            Debug.Log("Drop To " + this, this);
            SlotBase dropSlot = this;

            dragSlot.isValidDrop = true;
            dragSlot.itemImage.transform.localPosition = Vector3.zero;            
            if (dropSlot != dragSlot) {                
                if (dropSlot.props.Item != null) {
                    if (dropSlot.props.Item.Index != dragSlot.props.Item.Index) {
                        Slot temp = new Slot(dragSlot.props, dragSlot.index);
                        dragSlot.ModifyTo(dropSlot.props);
                        dropSlot.ModifyTo(temp);
                        Debug.Log("Finish Exchanging", this);
                    }
                    else {
                        Debug.Log("Stack To " + this, this);
                        int overStack = dropSlot.props.Add(dragSlot.props.StackCount);
                        if (overStack == dragSlot.props.StackCount) {
                            // exchange slot stack count
                            dragSlot.props.StackCount = dropSlot.props.StackCount;
                            dropSlot.props.StackCount = overStack;                            
                        }
                        else {
                            dragSlot.props.StackCount = overStack;                            
                            if (dragSlot.props.StackCount < 1) {
                                // slot get into empty
                                dragSlot.Clear();
                            }                            
                        }
                    }
                }
                else {
                    dropSlot.ModifyTo(dragSlot.props);                    
                    dragSlot.Clear();                    
                }
            }
        }

        public void InvalidDrop() {
            Debug.Log("Invalid Drop From " + this, this);
        }

        public void OnDrag(PointerEventData eventData) {
            if (dragging) {
                dragging.transform.position = eventData.position;
            }
        }

        public void OnBeginDrag(PointerEventData eventData) {            
            isValidDrop = false;
            if (isDescribing) {
                isDescribing = false;
                Undescribe?.Invoke(this, props);
                //OnUndescribe();
            }
            if (props.Item.Name != "") {
                //isDragging = true;
                dragging = Instantiate(itemImage, transform.parent);
                dragging.raycastTarget = false;
                Debug.Log("Valid Drag From " + props.Item.Index, this);
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
