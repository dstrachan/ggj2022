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

        void Start()
        {
            _controller = GetComponent<CharacterController>();
            _mainCamera = Camera.main;
        }

        void Update()
        {
            if (!TimeWarp.TimeIsWarping)
            {
                var inputX = Input.GetAxis("Horizontal");
                var inputY = Input.GetAxis("Vertical");
                
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
