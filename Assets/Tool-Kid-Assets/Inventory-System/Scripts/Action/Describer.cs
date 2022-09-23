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

    public class Describer : SlotBehaviour {
        
        public RectTransform informationPanel;
        public Text description;
        public Text count;
        public Text price;

        void Awake() {
            Base = GetComponent<InventoryBase>();
            Enter.Action = Enable;
            Stay.Action = Follow;
            Exit.Action = Unable;
            Base.DescribeAction.Trigger += OnEnter;
            Base.UndescribeAction.Trigger += OnExit;
        }

        void OnDestroy() {
            Base.DescribeAction.Trigger -= OnEnter;
            Base.UndescribeAction.Trigger -= OnExit;
        }

        private void Follow(SlotProps e) {
            informationPanel.position = Input.mousePosition;
        }

        private void Enable(SlotProps e) {            
            informationPanel.position = Input.mousePosition;
            description.text = e.Item.Description;
            count.text = e.StackCount.ToString("000");
            price.text = e.Item.Price.ToString("000");
            informationPanel.gameObject.SetActive(IsExcuting);            
            TKLog.Log("Describe " + e.Item.Index, this, enableLog);            
        }

        private void Unable(SlotProps e) {            
            informationPanel.position = Input.mousePosition;
            description.text = "It is expect on undescribe";
            informationPanel.gameObject.SetActive(IsExcuting);            
            TKLog.Log("Undescribe " + e.Item.Index, this, enableLog);
        }
    }
}
