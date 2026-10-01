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
        temperature = Mathf.Min(1f, temperature);
        colour.r = temperature;
        colour.g = temperature / 2f;
        material.SetColor(Shader.PropertyToID("_EmissionColor"), colour * 3f);
        //this.GetComponent<Renderer>().material.color = colour;
    }
    
}
