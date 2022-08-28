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
            informationPanel.gameObject.SetActive(isDescribing);
            informationPanel.GetComponentInChildren<Text>().text = slotBase.Props.Item.Description;
            Debug.Log("Describe " + slotBase.Props.Item.Index, this);
        }

        private void OnUndescribe(object sender, Slot e) {            
            onUndescribe.Invoke();
            informationPanel.GetComponentInChildren<Text>().text = "It is expect on undescribe";
            isDescribing = false;
            informationPanel.gameObject.SetActive(isDescribing);
            Debug.Log("Undescribe " + slotBase.Props.Item.Index, this);
        }
    }
}
