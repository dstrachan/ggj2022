using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speed;
        
        private List<Shader> oldShaders = new List<Shader>();
        private bool shaderApplied = false;
        
        private CharacterController _controller;
        private Camera _mainCamera;
        private Vector3 _moveDirection = Vector3.zero;
        private bool _usingJoyPad = true;

        void Start()
        {
            _controller = GetComponent<CharacterController>();
            _mainCamera = Camera.main;
            
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                oldShaders.Add(item.material.shader);
            }
        }

        // private void FixedUpdate()
        // {
        //     var camPosition = _mainCamera.transform.position;
        //     var rayPlayer = new Ray(camPosition, transform.position - camPosition);
        //     if (Physics.Raycast(rayPlayer, out var firstHit))
        //     {
        //         if (!firstHit.collider.CompareTag(Tags.Player))
        //         {
        //             if (!shaderApplied)
        //             {
        //                 shaderApplied = true;
        //                 var items = GetComponentsInChildren<Renderer>();
        //                 for (var i = 0; i < items.Length; ++i)
        //                 {
        //                     if(items[i].CompareTag(Tags.NoShader)) continue;
        //                     
        //                     items[i].material.shader = Shader.Find("HighlightEffect");
        //                 }
        //             }
        //         }
        //         else
        //         {
        //             if (shaderApplied)
        //             {
        //                 shaderApplied = false;
        //                 var items = GetComponentsInChildren<Renderer>();
        //                 
        //                 for (var i = 0; i < items.Length; ++i)
        //                 {
        //                     if(items[i].CompareTag(Tags.NoShader)) continue;
        //                     
        //                     items[i].material.shader = oldShaders[i];
        //                 }
        //             }
        //         }
        //     }
        // }

        void Update()
        {
            if (!TimeWarp.TimeIsWarping)
            {
                if (Camera.main != null)
                {
                    var inputMoveX = Input.GetAxis("MoveHorizontal");
                    var inputMoveY = Input.GetAxis("MoveVertical");
                    var action = Input.GetButton("Action");
                    
                    // Movement direction Depends on camera angle
                    var cameraForward = Camera.main.transform.forward;
                    var dirForward = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
                    var dirRight = Vector3.Cross(dirForward, Vector3.down);

                    // Movement
                    _moveDirection = ((dirRight * inputMoveX) + (dirForward * inputMoveY)) * speed;
                    _controller.Move(_moveDirection * Time.deltaTime);

                    // Facing
                    // TODO face second joy stick direction if using joy pad.
                    if (_moveDirection != Vector3.zero)
                        transform.LookAt(transform.position + _moveDirection);
                    
                    // What are we mousing over?
                    var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                    var hits = Physics.RaycastAll(ray);
                    Vector3 groundHit = transform.position;
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
            }
        }
    }
}