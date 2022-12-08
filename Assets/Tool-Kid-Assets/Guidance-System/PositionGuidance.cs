using System.Collections;
using System.Collections.Generic;
using ToolKid.TimerSystem;
using UnityEngine;
using UnityEngine.UI;

namespace ToolKid.GuidanceSystem {
    public class PositionGuidance : MonoBehaviour {

        public bool disableComp;

        public Transform Point;

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
        private float rotationAngle;
        [SerializeField]
        private float distance;
        [SerializeField]
        private float height;

        [SerializeField]
        private Vector3 cameraAngle = new Vector3();

        public Vector2 screenPosition;
        public Vector2 originPosition;

        public Camera customCamera;
        public Transform customTarget;
        public Canvas customCanvas;
        public Text distanceText;

        public Vector2 shadowVector;
        public Vector3 shadowPoint;

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
            Vector2 pos = target.transform.position;  // get the game object position
            Vector2 s = Camera.main.WorldToViewportPoint(pos);  //convert game object position to VievportPoint
            originPosition = new Vector2((s.x - 0.5f) * Screen.height, (s.y - 0.5f) * Screen.width);
            screenPosition = originPosition;
            //set MIN and MAX Anchor values(positions) to the same position(ViewportPoint)
            //rectTransform.anchorMin = viewportPoint;
            //rectTransform.anchorMax = viewportPoint;

            distance = GetDistance(target.position, cam.transform.position);
            distanceText.text = distance.ToString("0.00 m");

            //float fieldOfView_comp = Mathf.Cos(cam.fieldOfView / 2f * Mathf.PI / 180f);            
            //if (disableComp) {
            //    fieldOfView_comp = 1;
            //}
            //// Vector from camera to target
            //Vector3 base_on_cam = target.transform.position - cam.transform.position;

            //#region # Rotation compensating
            //Vector3 ref_x = TransformPoint(target.position, cam.transform.position, cam.transform.forward) - cam.transform.position;
            ////rotationAngle = (Mathf.Asin(ref_x.x) - Mathf.Asin(cam.transform.right.x)) * 180f / Mathf.PI;
            //rotationAngle = Vector3.Angle(ref_x, cam.transform.right) * Mathf.Sign(ref_x.x - cam.transform.right.x);
            //#endregion

            //#region # Horizontal View
            //Vector3 h_tarV3 = new Vector3(base_on_cam.x, 0f, base_on_cam.z);
            //Vector3 h_eyeV3 = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z);
            //angle = GetAngle(h_eyeV3, h_tarV3);

            //float h_rad = (angle - 90) * Mathf.PI / 180f;
            //float cos = Mathf.Cos(h_rad);
            //float temp_x = cos * Screen.width * fieldOfView_comp * fieldOfView_comp;
            //screenPosition.x = temp_x;

            //#endregion

            //#region # Vertical View
            //Vector3 tarV3 = new Vector3(base_on_cam.x, base_on_cam.y, base_on_cam.z).normalized;
            ////ensure vector is a magnitude of 1
            //elevationAngle = (Mathf.Asin(tarV3.y) - Mathf.Asin(cam.transform.forward.y)) * 180f / Mathf.PI;

            //float v_rad = (elevationAngle) * Mathf.PI / 180f;
            //float sin = Mathf.Sin(v_rad);
            //float temp_y = sin * Screen.height * fieldOfView_comp * Mathf.Sqrt(fieldOfView_comp);
            //screenPosition.y = temp_y;

            //#endregion

            ////rotationAngle = GetAngle(ref_o, new Vector3(temp_x, temp_y, 0f));
            //float c_rad = rotationAngle * Mathf.PI / 180f;
            //float final_x = Mathf.Sqrt(temp_x * temp_x + temp_y * temp_y) * Mathf.Cos(c_rad);
            //screenPosition.x = final_x;
            //float final_y = Mathf.Sqrt(temp_x * temp_x + temp_y * temp_y) * Mathf.Sin(c_rad);
            //screenPosition.y = final_y;

            //if (Mathf.Abs(temp_x) > Screen.width * 0.4f) {
            //    screenPosition.x = Mathf.Sign(angle) * Screen.width * 0.4f;
            //}

            //if (Mathf.Abs(temp_y) > Screen.height * 0.4f) {
            //    screenPosition.y = Mathf.Sign(elevationAngle) * Screen.height * 0.4f;
            //}

            //Vector3 camera_near_plane = cam.transform.position + canvas.planeDistance * cam.transform.forward;
            //shadowPoint = TransformPoint(target.position, camera_near_plane, cam.transform.forward);
            //shadowVector = (shadowPoint - cam.transform.position) / Vector3.Dot(base_on_cam, cam.transform.forward) / Mathf.Tan(Mathf.PI / 6f) / 2f;
            //screenPosition = new Vector2(shadowVector.x * Screen.height, shadowVector.y * Screen.width);

            //Point.position = shadowPoint;

            //arrow.gameObject.GetComponent<Image>().enabled = !((Mathf.Abs(temp_y) < Screen.height * 0.42f) && (Mathf.Abs(temp_x) < Screen.width * 0.42f));

            if (Mathf.Abs(originPosition.x) > Screen.width * 0.4f) {
                screenPosition.x = Mathf.Sign(Vector3.Dot(cam.transform.forward, target.position - cam.transform.position)) * Screen.width * 0.4f;
            }

            if (Mathf.Abs(originPosition.y) > Screen.height * 0.4f) {
                screenPosition.y = Mathf.Sign(Vector3.Dot(cam.transform.forward, target.position - cam.transform.position)) * Screen.height * 0.4f;
            }

            t1 += 1f * Time.deltaTime;
            arrow.anchoredPosition = Vector2.Lerp(arrow.anchoredPosition, screenPosition, t1);            
        }
        /// <summary>
        /// Get the distance in Vector3 between two point.
        /// </summary>
        /// <param name="a">The Vector3 of the start point.</param>
        /// <param name="b">The Vector3 of the end point.</param>
        /// <returns>Distance in meter.(Only positive value)</returns>
        private float GetDistance(Vector3 a, Vector3 b) {
            Vector3 v3 = a - b;            
            return Mathf.Sqrt(v3.x * v3.x + v3.y * v3.y + v3.z * v3.z);            
        }
        /// <summary>
        /// Get the angle degree in Vector3 between two point.
        /// </summary>
        /// <param name="from">The Vector3 of the start point.</param>
        /// <param name="to">The Vector3 of the end point.</param>
        /// <returns>Angle in radian.</returns>
        private float GetAngle(Vector3 from, Vector3 to) {
            float angle = Vector3.Angle(from, to);
            float normal = Vector3.Cross(from, to).y;
            return angle *= Mathf.Sign(normal);
        }
        /// <summary>
        /// Get the point position at minimum distance between target and plane.
        /// </summary>
        /// <param name="target">The Vector3 of the target.</param>
        /// <param name="planeCenter">The Vector3 of the plane.</param>
        /// <param name="planeNormal">The Vector3 of the plane's normal.</param>
        /// <returns>Point position.</returns>
        public Vector3 TransformPoint(Vector3 target, Vector3 planeCenter, Vector3 planeNormal) {       
            Vector3 vec3 = target - planeCenter;
            float minDist = Vector3.Dot(vec3, planeNormal);
            Vector3 normal = planeNormal * minDist;
            return target - normal;
        }
    }
}
