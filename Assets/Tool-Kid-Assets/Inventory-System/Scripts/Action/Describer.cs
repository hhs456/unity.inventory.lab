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
        public Text description;
        public Text count;
        public Text price;

        public UnityEvent onDescribe;
        public UnityEvent onUndescribe;


        void Awake() {
            Base = GetComponent<InventoryBase>();

            Base.DescribeAction.ActionTrigger += OnDescribe;
            Base.UndescribeAction.ActionTrigger += OnUndescribe;
        }

        void OnDestroy() {
            Base.DescribeAction.ActionTrigger -= OnDescribe;
            Base.UndescribeAction.ActionTrigger -= OnUndescribe;
        }

        public void OnDescribe(object sender, Slot e) {            
            isDescribing = true;           
            StartCoroutine(DescribeAction(e));            
        }

        public void OnUndescribe(object sender, Slot e) {            
            isDescribing = false;                    
        }

        /// <summary>
        /// If the time lap which pointer move from old to new less than a frame, keep describer showing.
        /// </summary>
        /// <param name="e"></param>
        /// <returns>WaitForEndOfFrame()</returns>
        private IEnumerator DescribeAction(Slot e) {
            OnDescribe(e);
            while (isDescribing) {
                informationPanel.position = Input.mousePosition;
                yield return new WaitForEndOfFrame();
            }
            OnUndescribe(e);
        }

        private void OnDescribe(Slot e) {            
            informationPanel.position = Input.mousePosition;
            description.text = e.Item.Description;
            count.text = e.StackCount.ToString("000");
            price.text = e.Item.Price.ToString("000");
            informationPanel.gameObject.SetActive(isDescribing);
            onDescribe.Invoke();
            TKLog.Log("Describe " + e.Item.Index, this, enableLog);            
        }

        private void OnUndescribe(Slot e) {            
            informationPanel.position = Input.mousePosition;
            description.text = "It is expect on undescribe";
            informationPanel.gameObject.SetActive(isDescribing);
            onUndescribe.Invoke();
            TKLog.Log("Undescribe " + e.Item.Index, this, enableLog);
        }
    }
}
