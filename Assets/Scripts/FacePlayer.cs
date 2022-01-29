using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private GameObject _player;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Tags.Player);
    }

    // Update is called once per frame
    void Update()
    {
        var position = _player.transform.position;
        transform.LookAt(new Vector3(position.x, transform.position.y, position.z));
    }
}
