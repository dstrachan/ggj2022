using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    private Camera _mapCam;

    // Start is called before the first frame update
    void Start()
    {
        _mapCam = GetComponent<Camera>();
        Camera.main.SetReplacementShader(Shader.Find("URP/unlit"), "RenderType");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
