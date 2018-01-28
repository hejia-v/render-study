using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace xgame
{
    enum MouseButtonDown
    {
        MBD_LEFT = 0,
        MBD_RIGHT,
        MBD_MIDDLE,
    };

    public class ObserveCamera : MonoBehaviour
    {
        public bool lockCursor;
        public float mouseSensitivity = 10;
        public Transform target;
        public float dstFromTarget = 2;
        public float zoomSpeed = 1;
        public Vector2 pitchMinMax = new Vector2(-40, 85);

        public float rotationSmoothTime = .12f;
        Vector3 rotationSmoothVelocity;
        Vector3 currentRotation;

        float yaw;
        float pitch;

        public bool showInstWindow = true;
        Vector3 lastPos;
        Vector3 focus = Vector3.zero;

        void Start()
        {
            if (target == null)
            {
                GameObject obj = new GameObject("CameraFocusObject");
                obj.transform.position = this.focus;
                // obj.transform.LookAt(this.transform.position);
                target = obj.transform;
            }

            focus=target.transform.position;
            currentRotation = transform.eulerAngles;
            yaw = currentRotation.y;
            UpdateCamera();
        }

        void Update()
        {
            HandleMouseEvent();
        }

        void OnGUI()
        {
            if (showInstWindow)
            {
                GUI.Box(new Rect(Screen.width - 210, Screen.height - 100, 200, 90), "相机操作");
                GUI.Label(new Rect(Screen.width - 200, Screen.height - 80, 200, 30), "右键 / Alt+左键: 旋转");
                GUI.Label(new Rect(Screen.width - 200, Screen.height - 60, 200, 30), "中键 / Alt+Cmd+左键: 平移");
                GUI.Label(new Rect(Screen.width - 200, Screen.height - 40, 200, 30), "滚轮 / 两手指触摸: 推拉");
            }
        }

        void HandleMouseEvent()
        {
            float delta = Input.GetAxis("Mouse ScrollWheel");
            if (delta != 0.0f)
                HandleMouseWheel(delta);

            if (Input.GetMouseButtonDown((int)MouseButtonDown.MBD_LEFT) ||
                Input.GetMouseButtonDown((int)MouseButtonDown.MBD_MIDDLE) ||
                Input.GetMouseButtonDown((int)MouseButtonDown.MBD_RIGHT))
                this.lastPos = Input.mousePosition;

            HandleMouseDrag(Input.mousePosition);
        }

        // Dolly
        void HandleMouseWheel(float delta)
        {
            float distance = delta * zoomSpeed;
            dstFromTarget += distance;
            transform.position = target.position - transform.forward * dstFromTarget;
        }

        void HandleMouseDrag(Vector3 mousePos)
        {
            Vector3 diff = mousePos - lastPos;

            if (Input.GetMouseButton((int)MouseButtonDown1.MBD_LEFT))
            {
                // mac上的平移操作 : "Left Alt + Left Command + LMB Drag"
                if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftCommand))
                {
                    if (diff.magnitude > Vector3.kEpsilon)
                        CameraTranslate(-diff / 100.0f);
                }
                // mac上的旋转操作 : "Left Alt + LMB Drag"
                else if (Input.GetKey(KeyCode.LeftAlt))
                {
                    if (diff.magnitude > Vector3.kEpsilon)
                        CameraRotate(new Vector3(diff.y, diff.x, 0.0f));
                }
            }
            // 平移
            else if (Input.GetMouseButton((int)MouseButtonDown1.MBD_MIDDLE))
            {
                if (diff.magnitude > Vector3.kEpsilon)
                    CameraTranslate(-diff / 100.0f);
            }
            // 旋转
            else if (Input.GetMouseButton((int)MouseButtonDown1.MBD_RIGHT))
            {
                if (diff.magnitude > Vector3.kEpsilon)
                    CameraRotate(new Vector3(diff.y, diff.x, 0.0f));
            }

            this.lastPos = mousePos;
        }

        void CameraTranslate(Vector3 vec)
        {

        }

        void CameraRotate(Vector3 eulerAngle)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            //Debug.Log("mouseX: " + mouseX + ", mouseY: " + mouseY);

            yaw += mouseX * mouseSensitivity;
            pitch -= mouseY * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
            transform.eulerAngles = currentRotation;

            transform.position = target.position - transform.forward * dstFromTarget;
        }

        public void UpdateCamera()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            //Debug.Log("mouseX: " + mouseX + ", mouseY: " + mouseY);

            yaw += mouseX * mouseSensitivity;
            pitch -= mouseY * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
            transform.eulerAngles = currentRotation;

            transform.position = target.position - transform.forward * dstFromTarget;
        }


    }
}