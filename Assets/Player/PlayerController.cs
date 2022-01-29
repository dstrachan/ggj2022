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
            var inputX = Input.GetAxis("Horizontal");
            var inputY = Input.GetAxis("Vertical");
            
            var inputModifyFactor = (inputX != 0.0f && inputY != 0.0f) ? .7071f : 1.0f;
            
            RaycastHit hit;
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("ground"))
                {
                    var transform1 = transform;
                    transform.LookAt(new Vector3(hit.point.x, transform1.position.y, hit.point.z));
                }
            }
            
            
            _moveDirection = new Vector3(inputX * inputModifyFactor, 0, inputY * inputModifyFactor);
            _moveDirection = transform.TransformDirection(_moveDirection) * speed;

            if (Vector3.Distance(hit.point, transform.position) > 1.5f)
            {
                _controller.Move(_moveDirection * Time.deltaTime);
            }

        }
      
    }
}
