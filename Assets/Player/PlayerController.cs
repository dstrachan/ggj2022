using System.Linq;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speed;

        private CharacterController _controller;
        private Camera _mainCamera;
        private Vector3 _moveDirection = Vector3.zero;
        private bool _usingJoyPad = true;

        void Start()
        {
            _controller = GetComponent<CharacterController>();
            _mainCamera = Camera.main;
        }

        void Update()
        {
            if (!TimeWarp.TimeIsWarping)
            {
                var mouseInputX = Input.GetAxis("Horizontal");
                var mouseinputY = Input.GetAxis("Vertical");
                var mouseAction = Input.GetButton("Jump");
                bool usingMouse = mouseInputX != 0 || mouseinputY != 0 || mouseAction;
                var joyLInputX = Input.GetAxis("LJoyHorizontal");
                var joyLInputY = Input.GetAxis("LJoyVertical");
                // var joyRInputX = Input.GetAxis("RJoyHorizontal");
                // var joyRInputY = Input.GetAxis("RJoyVertical");
                var joyinputAction = Input.GetButtonDown("JoyJump");
                bool usingJoy = joyLInputX != 0
                                || joyLInputY != 0
                                // || joyRInputY != 0
                                // || joyRInputY != 0
                                || joyinputAction;
                _usingJoyPad = usingJoy || (!usingMouse && _usingJoyPad);

                if (_usingJoyPad)
                {
                    if (Camera.main != null)
                    {
                        // Joy Pad Controls

                        // Depends on camera angle
                        var cameraForward = Camera.main.transform.forward;
                        var dirForward = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
                        var dirRight = Vector3.Cross(dirForward, Vector3.down);

                        // Movement
                        _moveDirection = ((dirRight * joyLInputX) + (dirForward * joyLInputY)) * speed;
                        _controller.Move(_moveDirection * Time.deltaTime);

                        // Facing
                        if (_moveDirection != Vector3.zero)
                            transform.LookAt(transform.position + _moveDirection);
                    }
                }
                else
                {
                    // Keyboard and Mouse Controls
                    var inputX = mouseInputX;
                    var inputY = mouseinputY;

                    var inputModifyFactor = (inputX != 0.0f && inputY != 0.0f) ? .7071f : 1.0f;

                    var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                    var hits = Physics.RaycastAll(ray);
                    Vector3 groundHit = transform.position;
                    if (hits.Any())
                    {
                        foreach (var hit in hits)
                        {
                            if (hit.collider.CompareTag(Tags.Ground))
                            {
                                groundHit = hit.point;
                                var transform1 = transform;
                                transform.LookAt(new Vector3(hit.point.x, transform1.position.y, hit.point.z));
                                break;
                            }
                        }
                    }

                    _moveDirection = new Vector3(inputX * inputModifyFactor, -1, inputY * inputModifyFactor);
                    _moveDirection = transform.TransformDirection(_moveDirection) * speed;

                    // Dont move too close to target and go crazy
                    if (Vector3.Distance(groundHit, transform.position) > 1.2f)
                    {
                        _controller.Move(_moveDirection * Time.deltaTime);
                    }
                }
            }
        }
    }
}