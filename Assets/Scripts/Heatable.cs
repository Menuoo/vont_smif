using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Heatable : MonoBehaviour
{
    [Range (0.0f, 1.0f)]
    public float temperature = 0.0f;

    public Material material;
    public MetalType metal;
    public Color colour;

    Renderer render;
    MaterialPropertyBlock propertyBlock;

    // Start is called before the first frame update
    void Start()
    {
        //propertyBlock = new MaterialPropertyBlock();
        render = GetComponent<MeshRenderer>();
        propertyBlock = new MaterialPropertyBlock();
        material = render.material;
    }

    // Update is called once per frame
    void Update()
    {
        changeTemperature(0f);
    }

    public void changeTemperature(float input)
    {
        temperature += input;
        temperature = Mathf.Min(1f, temperature);
        colour.r = temperature;
        colour.g = temperature / 2f;
        colour.a = 1f;

        propertyBlock.SetColor("_EmissionColor", colour * 3f);
        render.SetPropertyBlock(propertyBlock);
        material.EnableKeyword("_EMISSION");
        //render.material.SetColor(Shader.PropertyToID("_EmissionColor"), colour * 3f);
        //this.GetComponent<Renderer>().material.color = colour;
    }
    
}

public enum MetalType{ iron,bronze }
