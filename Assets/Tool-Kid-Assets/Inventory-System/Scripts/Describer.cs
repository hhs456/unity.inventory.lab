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

        private SlotBase slotBase;

        private bool isDescribing = false;        

        public RectTransform informationPanel;
        //public GameObject InformationPanel;

        [HideInInspector]
        public UnityEvent onDescribe;
        [HideInInspector]
        public UnityEvent onUndescribe;


        void Awake() {
            Timer.CentiSecond += Counterdown;
            slotBase = GetComponent<SlotBase>();
            slotBase.Describe += OnDescribe;
            slotBase.Undescribe += OnUndescribe;         
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
            informationPanel.GetComponentInChildren<Text>().text = slotBase.Props.Item.Description;            
            informationPanel.gameObject.SetActive(isDescribing);
            TKLog.Log("Describe " + slotBase.Props.Item.Index, this, enableLog);
        }

        private void OnUndescribe(object sender, Slot e) {            
            onUndescribe.Invoke();
            isDescribing = false;
            informationPanel.position = Input.mousePosition;
            informationPanel.GetComponentInChildren<Text>().text = "It is expect on undescribe";            
            informationPanel.gameObject.SetActive(isDescribing);
            TKLog.Log("Undescribe " + slotBase.Props.Item.Index, this, enableLog);
        }
    }
}
