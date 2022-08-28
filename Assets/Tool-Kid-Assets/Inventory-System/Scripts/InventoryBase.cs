using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ToolKid.InventorySystem {
    public class InventoryBase : MonoBehaviour {
        [SerializeField]
        private Inventory props;
        public Inventory Props {
            get {
                return props;
            }
            set {
                props = value;
            }
        }

        private SlotBase[] slotBases;
        public bool defaultEnable = false;
        private bool enable = false;
        [SerializeField]
        private Vector2 lastPosition;
        [SerializeField]
        private Vector2 hidePosition = new Vector2(0f, 1000f);
        [SerializeField]
        private KeyCode keyCode = KeyCode.None;
        public KeyCode KeyCode {
            get => keyCode;
        }

        void Awake() {
            Timer.CentiSecond += DspUpdate;
            slotBases = transform.GetComponentsInChildren<SlotBase>();
            enable = !defaultEnable;
            Switch();
        }

        private void DspUpdate(object sender, Watch e) {
            if (Input.GetKeyDown(KeyCode)) {
                Switch();
            }
        }

        public void Switch() {
            if (!enable) {
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(lastPosition.x, lastPosition.y);
            }
            else {
                lastPosition = transform.GetComponent<RectTransform>().anchoredPosition;
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(hidePosition.x, hidePosition.y);
            }
            enable = !enable;
        }

        public void Enable() {
            if (!enable) {
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(lastPosition.x, lastPosition.y);
                enable = !enable;
            }
        }

        public void Disable() {
            if (enable) {
                lastPosition = transform.GetComponent<RectTransform>().anchoredPosition;
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(hidePosition.x, hidePosition.y);
                enable = !enable;
            }
        }
    }
}
