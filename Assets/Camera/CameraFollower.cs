using DefaultNamespace;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    //public float zoomedInPosition;
    public float zoomedOutPosition;

    private float _smoothFactor;

    private Transform _player;
    public Vector3 cameraOffset; 

    private void Start()
    {
        _smoothFactor = 2;
        _player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Transform>();
    }

    private void Update()
    {
        
        // TODO zoom out?
        // if (Input.GetKey(KeyCode.Joystick1Button4))
        // {
        //     targetPos.z = zoomedOutPosition;
        // }
        // else
        // {
        //     targetPos.z = zoomedInPosition;             
        // }


        var playerPosition = _player.position;
        var cameraPosition = transform.position;
        
        var newPosition = playerPosition + cameraOffset;
        cameraPosition = Vector3.Lerp(cameraPosition, newPosition, Time.deltaTime * _smoothFactor);
        transform.position = cameraPosition;
    }

    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, player.position.z), new Vector3(followBounds, followBounds, 1));
    // }
}
