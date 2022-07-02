using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : GameBehaviour<FlashLight>
{
    public Light myLight;

    [Header("Range Variables")]
    public bool changeRange = false;
    public float rangeSpeed = 1.0f;
    public float maxRange = 10.0f;

    [Header("Intensity Variables")]
    public float intensitySpeed = 0.1f;
    public float maxIntensity = 10.0f;


    [Header("Color Variables")]
    public bool changeColours = false;
    public float colourSpeed = 1.0f;
    public Color startColor;
    public Color endColor;

    float startTime;

    void Start()
    {
        myLight = GetComponent<Light>();
        myLight.intensity = maxIntensity;
    }

    void Update()
    {
        myLight.intensity = Mathf.Clamp(myLight.intensity, 0, maxIntensity);
        myLight.intensity -= Time.deltaTime * intensitySpeed;
    }

    public void ChangeIntensity(float intensity)
    {
        if (intensity >= 10f)
            intensity = 10;
        if (intensity <= 0f)
            intensity = 0f;
        myLight.intensity += intensity;
    }
}
