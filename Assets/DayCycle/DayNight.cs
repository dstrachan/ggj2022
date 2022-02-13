using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using Time = UnityEngine.Time;

public class DayNight : MonoBehaviour
{
    Light mainLight;
    public float maxIntensity = 3f;
    public float minIntensity = 0f;
    
    public float maxAmbient = 1f;
    public float minAmbient = 0f;

    public Gradient nightDayColor;
    
    public AnimationCurve fogDensityCurve;
    public float fogScale = 0f;
    
    public float dayTimeSeconds = 180f;
    public float nightTimeSeconds = 180f;

    float _skySpeed = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        mainLight = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        float dotDayTime = Mathf.Clamp01(Vector3.Dot(mainLight.transform.forward, Vector3.down));
        
        float i = ((maxIntensity - minIntensity) * dotDayTime) + minIntensity;

        mainLight.intensity = i;
        
        i = ((maxAmbient - minAmbient) * dotDayTime) + minAmbient;
        RenderSettings.ambientIntensity = i;

        mainLight.color = nightDayColor.Evaluate(dotDayTime);   
        RenderSettings.ambientLight = mainLight.color;
        
        RenderSettings.fogDensity = fogDensityCurve.Evaluate(dotDayTime) * fogScale;

        var currentTime = GameState.Instance.Time;

        var position = transform.position;
        
        transform.localRotation = Quaternion.Euler(new Vector3(position.x, position.y, (currentTime.Hour + currentTime.Minute/60f) * 15 + 180));

        if (Input.GetKeyDown(KeyCode.O)) _skySpeed *= 0.5f;
        if (Input.GetKeyDown(KeyCode.P)) _skySpeed *= 2f;
        
    }
}
