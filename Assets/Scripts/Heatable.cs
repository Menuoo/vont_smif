using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heatable : MonoBehaviour
{
    [Range (0.0f, 1.0f)]
    public float temperature = 0.0f;
    public Material material;
    public Color colour;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeTemperature(float input)
    {
        temperature += input;
        colour.r = temperature;
        material.SetColor(Shader.PropertyToID("_Emission"),colour);
    }
    
}
