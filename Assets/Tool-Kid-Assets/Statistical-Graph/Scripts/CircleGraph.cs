using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToolKid {
    public class CircleGraph : MonoBehaviour {

        [Label("¼Ë¦¡ Style"), Tooltip("The appearance of circle graph.")]
        public GameObject style;        
        protected float total;
        public float Total {
            get => total;
            set {
                if (total != value) {
                    total = value;
                    ValueChanged?.Invoke(this, total);
                }
            }
        }
        [SerializeField, Label("¶ê¤ß¶ZÂ÷ Distance"), Tooltip("The distance between data graph and circle center.")]
        protected float distanceFromCenter = 1f;

        public float DistanceFromCenter {
            get => distanceFromCenter;
            set {
                if (value >= 0f && value <= 3f) {
                    distanceFromCenter = value;
                }
            }
        }

        [SerializeField, Label("­È"), Tooltip("Each value in circle graph.")]
        protected List<float> sliceValues;
        protected List<float> positionCompensations = new List<float>();
        protected List<SliceData> sliceDatas = new List<SliceData>();

        public event EventHandler<float> ValueChanged;

        public bool IsReady { get; private set; }

        private void Awake() {            
            Begin();
        }

        void Begin() {
            total = 0;
            int i_size = sliceValues.Count;
            float compensation = 0;
            for (int i = 0; i < i_size; i++) {
                if (i == i_size - 1) {
                    IsReady = true;
                }
                GameObject newSlice = Instantiate(style, transform, false);
                SliceBase sliceBase = newSlice.GetComponent<SliceBase>();

                positionCompensations.Add(compensation);
                sliceBase.Begin(i, sliceValues[i]);
                //sliceDatas.Add(sliceBase.data);            
                compensation += sliceValues[i];
            }
            //UpdateBeginPosition();
        }

        public void UpdateBeginPosition() {
            int i_size = positionCompensations.Count;
            for (int i = 1; i < i_size; i++) {
                sliceDatas[i].Image.transform.localEulerAngles = new Vector3(0f, 0f, -positionCompensations[i] / total * 360f);
            }
        }

        public float GetPosition(int i) {
            if (i > 0 && IsReady) {
                Debug.Log("Set Pos " + i);
                return -positionCompensations[i] / total * 360f;
            }
            else {
                return 0;
            }
        }

        public void Update() {
            
        }
    }
}