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
        private bool limitDiagonalSpeed = true;
        private CharacterController _controller;
        private Camera _mainCamera;
        private Vector3 _moveDirection = Vector3.zero;
        private GameManager _gameManager;
        private Transform _controlDirection;

        void Start()
        {
            _controller = GetComponent<CharacterController>();
            _mainCamera = Camera.main;
            _gameManager = GameObject.FindGameObjectWithTag(Tags.GameManager).GetComponent<GameManager>();
            _controlDirection = GameObject.FindGameObjectWithTag(Tags.ControlDirection).GetComponent<Transform>();

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
            if (!_gameManager.TimeIsWarping)
            {
                if (Camera.main != null)
                {
                    var inputMoveX = Input.GetAxis("MoveHorizontal");
                    var inputMoveY = Input.GetAxis("MoveVertical");
                    
  
                    var action = Input.GetButton("Action");
                    
                    // Movement direction Depends on camera angle
                   // var cameraForward = Camera.main.transform.forward;
           
                    // Movement
                    _moveDirection = ((_controlDirection.right * inputMoveX) + (_controlDirection.forward * inputMoveY));

                    _controller.Move(_moveDirection * speed * Time.deltaTime);
                    

                    // Facing
                    // TODO face second joy stick direction if using joy pad.
                    if (_moveDirection != Vector3.zero)
                    {
                        transform.LookAt(transform.position + _moveDirection);
                    }

                }
            }
        }
    }
}