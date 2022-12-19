using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToolKid {
    public class SliceBase : MonoBehaviour {

        public float textDistance = 65f;

        protected CircleGraph pieChart;
        protected Text percenText;

        public SliceData data;
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

        public List<SliceData> sliceDatas;

        public event EventHandler<float> ValueChanged;
        [Label("色調反轉"), Tooltip("If enabled, the color of number will be set to the contrasting color of its graph.")]
        public bool enableContrastingColor = false;

        public void Begin(int index, float count) {
            data = new SliceData(index, count, transform.GetChild(0).GetChild(0).GetComponent<Image>());

            // Random color
            float r = UnityEngine.Random.Range(0, 256) / 255f;
            float g = UnityEngine.Random.Range(0, 256) / 255f;
            float b = UnityEngine.Random.Range(0, 256) / 255f;
            data.Image.color = new Color(r, g, b);

            pieChart = GetComponentInParent<CircleGraph>();
            data.Updated += OnUpdated;
            pieChart.ValueChanged += data.OnTotalChanged;
            pieChart.Total += count;
        }

        public void OnUpdated() {
            float slice_rot = pieChart.GetPosition(data.Index);
            float percent_rot = (1 - data.Rate) * 180f;
            float number_rot = -(slice_rot + percent_rot);
            percenText = transform.GetChild(0).GetChild(1).GetComponent<Text>();

            transform.localEulerAngles = new Vector3(0f, 0f, slice_rot);
            transform.GetChild(0).localEulerAngles = new Vector3(0f, 0f, percent_rot);            
            percenText.transform.localEulerAngles = new Vector3(0f, 0f, number_rot);
            data.Image.transform.localEulerAngles = new Vector3(0f, 0f, -percent_rot);
            data.Image.transform.localPosition = new Vector3(0f, pieChart.DistanceFromCenter, 0f);

            percenText.text = data.Rate.ToString("##0.0%");

            float r = data.Image.GetComponent<RectTransform>().sizeDelta.x / 2f;
            float text_rot = Mathf.Abs(number_rot);
            float comp_pos = Mathf.Sin(text_rot / 360f * (2f * Mathf.PI));
            percenText.transform.localPosition = new Vector3(0f, textDistance + (textDistance - r) * comp_pos, 0f);
            if (enableContrastingColor) {
                percenText.color = new Color(1f - data.Image.color.r, 1f - data.Image.color.g, 1f - data.Image.color.b);
            }
            else {
                percenText.color = data.Image.color;
            }
        }
    }
}
