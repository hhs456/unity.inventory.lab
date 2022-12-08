using System.Collections;
using System.Collections.Generic;
using ToolKid.TimerSystem;
using UnityEngine;

public class TargetArrow : MonoBehaviour {
    
    public Transform cam;
    public Transform arrow;
    public Transform target;

    public float direction;
    /// <summary>
    /// Cross value, to deside Sign of direction.
    /// </summary>
    public Vector3 normal;

    [SerializeField]
    float boundaryDistance = 10f;
    [SerializeField]
    Vector2 screenPosition;

    Vector2 displayRange;

    Quaternion originRotation;

    public float t1 { get; private set; }

    void Start() {
        GameWatch.Main.WatchUpdate += DspUpdate;
        originRotation = transform.rotation;
        displayRange.x = Screen.width / 2 - boundaryDistance;
        displayRange.y = Screen.height / 2 - boundaryDistance;
    }

    private void DspUpdate(object sender, WatchArgs e) {
        
        Vector3 cam_forward = cam.forward;  // camera (player) forward vector  
        Vector3 tar_normal = (target.position - cam.position).normalized;  // unit vector between camera and target

        Vector3 plane_V3 = Vector3.ProjectOnPlane(tar_normal - cam_forward, cam_forward).normalized; // get a vector which project on self plane
        Vector3 originUp = cam.up;

        direction = Vector3.Dot(originUp, plane_V3);    // dot to be angley
        normal = Vector3.Cross(originUp, plane_V3);     // cross to be sign

        direction = Mathf.Acos(direction) * Mathf.Rad2Deg; // to degree  

        normal = cam.InverseTransformDirection(normal);  // convert cross value to camera position

        // set rotation of arrow  
        // arrow.rotation = originRotation * Quaternion.Euler(new Vector3(0f, 0f, direction * Mathf.Sign(normal.z)));

        screenPosition = Camera.main.WorldToScreenPoint(target.position);

        float mid_w = Screen.width / 2f;
        float mid_h = Screen.height / 2f;

        bool inWidth = Mathf.Abs((screenPosition.x - mid_w)) > (mid_w - boundaryDistance);
        bool inHight = Mathf.Abs((screenPosition.y - mid_h)) > (mid_h - boundaryDistance);
        // out of view
        if (inWidth || inHight || Vector3.Dot(cam_forward, tar_normal) < 0) {
            Vector3 result = Vector3.zero;
            // special angle (avoid tan90)            
            float direction_new = 90 - direction;
            if (direction != 0 && direction != 180) {
                float k = Mathf.Tan(Mathf.Deg2Rad * direction_new);                
                result.x = displayRange.y / k;

                if (Mathf.Abs(result.x) < displayRange.x) {
                    // angle at top & bot
                    result.y = displayRange.y;
                    if (direction > 90) {
                        result.y = -displayRange.y;
                        result.x = result.x * -1;
                    }
                }
                else {
                    // angle at right & left
                    result.y = displayRange.x * k;
                    if (Mathf.Abs(result.y) < displayRange.y) {
                        result.x = result.y / k;
                    }
                }
                result.x *= Mathf.Sign(-normal.z);
            }
            else {
                result.y = Mathf.Sign(direction_new) * displayRange.y;
            }
            arrow.localPosition = result;
        }
        else {
            Vector2 pos = target.transform.position;  // get the game object position
            Vector2 s = Camera.main.WorldToViewportPoint(pos);  //convert game object position to VievportPoint
            screenPosition = new Vector2((s.x - 0.5f) * Screen.height, (s.y - 0.5f) * Screen.width);
            arrow.GetComponent<RectTransform>().anchoredPosition = screenPosition;
            t1 += 1f * Time.deltaTime;
            arrow.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(arrow.GetComponent<RectTransform>().anchoredPosition, screenPosition, t1);
            //arrow.position = screenPosition;
        }

    }
}