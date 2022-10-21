using System.Collections;
using System.Collections.Generic;
using ToolKid.TimerSystem;
using UnityEngine;
using UnityEngine.UI;

namespace ToolKid.GuidanceSystem {
    public class PositionGuidance : MonoBehaviour {
        [SerializeField]
        private static Camera cam;
        [SerializeField]
        private static Transform target;
        [SerializeField]
        private static Canvas canvas;
        [SerializeField]
        private RectTransform arrow;
        [SerializeField]
        private float angle;
        [SerializeField]
        private float elevationAngle;
        [SerializeField]
        private float distance;
        [SerializeField]
        private float height;

        public Vector2 screenPosition;

        public Camera customCamera;
        public Transform customTarget;
        public Canvas customCanvas;
        public Text distanceText;

        public static Camera Camera { get => cam; set => cam = value; }
        public static Transform Target { get => target; set => target = value; }
        public RectTransform Arrow { get => arrow; set => arrow = value; }
        public static Canvas Canvas { get => canvas; set => canvas = value; }

        private float t1;

        // Start is called before the first frame update
        void Awake() {
            TimerSystem.GameWatch.Main.WatchUpdate += DspUpdate;
            cam = customCamera;
            target = customTarget;
            canvas = customCanvas;
        }

        private void DspUpdate(object sender, WatchArgs e) {
            // Vector from camera to target
            float direct_x = target.transform.position.x - cam.transform.position.x;
            float direct_y = target.transform.position.y - cam.transform.position.y;
            float direct_z = target.transform.position.z - cam.transform.position.z;
            Vector3 h_tarV3 = new Vector3(direct_x, 0f, direct_z);
            Vector3 h_eyeV3 = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z);                                
            angle = GetAngle(h_eyeV3, h_tarV3);
            Vector3 tarV3 = new Vector3(direct_x, direct_y, direct_z).normalized;            
            elevationAngle = (Mathf.Asin(tarV3.y) - Mathf.Asin(cam.transform.forward.y)) * 180f / Mathf.PI;
            //screenPosition = WorldToScreenSpace(target.position, cam, canvas);

            arrow.gameObject.GetComponent<Image>().enabled = !(Mathf.Abs(angle) <= cam.fieldOfView / 2f && (Mathf.Abs(elevationAngle) <= cam.fieldOfView / 2f));
            
            float h_rad = (angle - 90) * Mathf.PI / 180f;
            float cos = Mathf.Cos(h_rad);
            screenPosition.x = cos * Screen.width;
            if (Mathf.Abs(screenPosition.x) > Screen.width * 0.4f) {
                screenPosition.x = Mathf.Sign(angle) * Screen.width * 0.4f;
            }
            float v_rad = elevationAngle * Mathf.PI / 180f;
            float sin = Mathf.Sin(v_rad);
            screenPosition.y = sin * Screen.height;
            if (Mathf.Abs(screenPosition.y) > Screen.height * 0.4f) {
                screenPosition.y = Mathf.Sign(elevationAngle) * Screen.height * 0.4f;
            }

            t1 += 1f * Time.deltaTime;
            arrow.anchoredPosition = Vector2.Lerp(arrow.anchoredPosition, screenPosition, t1);
            distance = GetDistance(target.position, cam.transform.position);
            distanceText.text = distance.ToString("0.00 m");
        }

        private float GetDistance(Vector3 a, Vector3 b) {
            float dist_x = a.x - b.x;
            float dist_y = a.y - b.y;
            float dist_z = a.z - b.z;
            return Mathf.Sqrt(dist_x * dist_x + dist_y * dist_y + dist_z * dist_z);            
        }

        private float GetAngle(Vector3 from, Vector3 to) {
            float angle = Vector3.Angle(from, to);
            float normal = Vector3.Cross(from, to).y;
            return angle *= Mathf.Sign(normal);
        }

        public static Vector2 WorldToScreenSpace(Vector3 worldPos, Camera cam, Canvas area) {
            Vector3 screenPos = cam.WorldToScreenPoint(target.position);
            float h = Screen.height;
            float w = Screen.width;
            float x = screenPos.x - (w / 2);
            float y = screenPos.y - (h / 2);
            float s = area.scaleFactor;
            return new Vector2(x, y);
        }

    }
}
