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
    public class Describer : MonoBehaviour {

        public bool enableLog = false;

        private InventoryBase Base;

        private bool isDescribing = false;        

        public RectTransform informationPanel;
        //public GameObject InformationPanel;

        public Text description;
        public Text count;
        public Text price;

        [HideInInspector]
        public UnityEvent onDescribe;
        [HideInInspector]
        public UnityEvent onUndescribe;


        void Awake() {
            Timer.CentiSecond += Counterdown;
            Base = GetComponent<InventoryBase>();
            Base.EndInit += Init;            
        }

        private void Init(object sender, EventArgs e) {
            for (int i = 0; i < Base.SlotBases.Length; i++) {
                Base.SlotBases[i].Describe += OnDescribe;
                Base.SlotBases[i].Undescribe += OnUndescribe;
            }
        }

        private void Counterdown(object sender, Watch e) {            
            if (isDescribing) {
                informationPanel.position = Input.mousePosition;
            }
        }

        private void OnDescribe(object sender, Slot e) {            
            onDescribe.Invoke();
            isDescribing = true;
            informationPanel.position = Input.mousePosition;
            description.text = e.Item.Description;
            count.text = e.StackCount.ToString("000");
            price.text = e.Item.Price.ToString("000");
            informationPanel.gameObject.SetActive(isDescribing);
            TKLog.Log("Describe " + e.Item.Index, this, enableLog);
        }

        private void OnUndescribe(object sender, Slot e) {            
            onUndescribe.Invoke();
            isDescribing = false;
            informationPanel.position = Input.mousePosition;
            description.text = "It is expect on undescribe";            
            informationPanel.gameObject.SetActive(isDescribing);
            TKLog.Log("Undescribe " + e.Item.Index, this, enableLog);
        }
    }
}
