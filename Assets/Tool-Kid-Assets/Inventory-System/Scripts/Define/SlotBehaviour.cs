using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ToolKid.InventorySystem {

    public delegate void SlotEventHandler(object sender, SlotProps e);

    public class SlotEvent {

        public event SlotEventHandler Trigger;        

        public void OnTrigger(object sender, SlotProps e) {
            Trigger?.Invoke(sender, e);
        }
    }
    [Serializable]
    public class SlotEventAction {

        public Action<SlotProps> Action;
        public UnityEvent onTrigger;
        public event SlotEventHandler TriggerEnter;

        public SlotEventAction(Action<SlotProps> action) {
            Action = action;
        }

        public void Invoke(object sender, SlotProps e) {
            onTrigger?.Invoke();
            TriggerEnter?.Invoke(sender, e);
            Action?.Invoke(e);
        }
    }
    public abstract class SlotBehaviour : TKBehaviour {
        protected InventoryBase Base;
        private bool isExcuting = false;
        protected bool IsExcuting {
            get {
                return isExcuting;
            }
            set {
                isExcuting = value;
            }
        }

        public SlotEventAction Enter;
        public SlotEventAction Stay;
        public SlotEventAction Exit;

        public virtual void OnEnter(object sender, SlotProps e) {
            isExcuting = true;
            StartCoroutine(Action(e));
        }

        public virtual void OnExit(object sender, SlotProps e) {
            isExcuting = false;
        }

        private IEnumerator Action(SlotProps e) {
            Enter.onTrigger.Invoke();
            Enter.Invoke(this, e);
            while (isExcuting) {
                Stay.Invoke(this, e);
                yield return new WaitForEndOfFrame();
            }
            Exit.Invoke(this, e);
            Exit.onTrigger.Invoke();
        }

    }
}
