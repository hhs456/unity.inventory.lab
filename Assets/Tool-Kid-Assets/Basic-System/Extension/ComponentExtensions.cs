using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ToolKid.ExtensionMethods.UnityEngine {
    public static class ComponentExtensions {

        public static Component[] GetComponentsArrayInRoot(this Component value) {
            return value.GetComponentsArrayInRoot<Component>(t => t);
        }
        public static Component[] GetComponentsArrayInRoot(this Component value, uint depth) {
            return value.GetComponentsArrayInRoot<Component>(depth, t => t);
        }

        public static T[] GetComponentsArrayInRoot<T>(this Component value) {
            return value.GetComponentsArrayInRoot<T>(t => t as Component);
        }
        public static T[] GetComponentsArrayInRoot<T>(this Component value, uint depth) {
            return value.GetComponentsArrayInRoot<T>(depth, t => t as Component);
        }
        public static T[] GetComponentsArrayInRoot<T>(this Component value, Predicate<T> match) {            
            return value.GetComponentsQueueInRoot<T>(t => match(t)).ToArray();
        }
        public static T[] GetComponentsArrayInRoot<T>(this Component value, uint depth, Predicate<T> match) {                     
            return value.GetComponentsQueueInRoot<T>(depth, t => match(t)).ToArray();
        }

        public static Queue<T> GetComponentsQueueInRoot<T>(this Component value, Predicate<T> match) {
            T t = value.GetComponent<T>();
            Queue<T> q1 = new Queue<T>();
            if (match(t)) {
                q1.Enqueue(t);
            }
            int size = value.transform.childCount;
            for (int i = 0; i < size; i++) {                
                Queue<T> q2 = value.transform.GetChild(i).GetComponentsQueueInRoot<T>(ct => match(ct));
                int q2_size = q2.Count;
                while (q2_size > 0) {
                    q1.Enqueue(q2.Dequeue());
                    q2_size--;
                }
            }
            return q1;
        }

        public static Queue<T> GetComponentsQueueInRoot<T>(this Component value, uint depth, Predicate<T> match) {
            T t = value.GetComponent<T>();
            Queue<T> q1 = new Queue<T>();
            if (match(t)) {
                q1.Enqueue(t);
            }
            if (depth > 0) {
                int size = value.transform.childCount;
                uint nextDepth = depth - 1;
                for (int i = 0; i < size; i++) {                    
                    Queue<T> q2 = value.transform.GetChild(i).GetComponentsQueueInRoot<T>(nextDepth, ct => match(ct));
                    int q2_size = q2.Count;
                    while (q2_size > 0) {
                        q1.Enqueue(q2.Dequeue());
                        q2_size--;
                    }
                }
            }
            return q1;
        }
    }
}
