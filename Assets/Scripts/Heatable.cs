using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Heatable : MonoBehaviour
{
    [Range (0.0f, 1.0f)]
    public float temperature = 0.0f;
    [Range(0.0f, 1.0f)]
    public float smoothness = 1f;

    public Material material;
    public MetalType metal;
    public Color colour;

    Renderer render;
    MaterialPropertyBlock propertyBlock;

    // Start is called before the first frame update
    void Awake()
    {
        //propertyBlock = new MaterialPropertyBlock();
        render = GetComponent<MeshRenderer>();
        if (propertyBlock == null)
            propertyBlock = new MaterialPropertyBlock();
        material = render.material;

        changeSmoothness(0f);
        changeColour();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (temperature > 0f)
            changeTemperature(-Time.fixedDeltaTime/20f);
    }

    public void changeTemperature(float input)
    {
        temperature += input;
        //temperature = Mathf.Min(1f, temperature);
        temperature = Mathf.Clamp01(temperature);
        changeColour();
    }

    public void changeColour()
    {
        colour.r = temperature;
        colour.g = temperature / 2f;
        colour.a = 1f;

        propertyBlock.SetColor("_EmissionColor", colour * 1.5f);
        render.SetPropertyBlock(propertyBlock);
        material.EnableKeyword("_EMISSION");
        //render.material.SetColor(Shader.PropertyToID("_EmissionColor"), colour * 3f);
        //this.GetComponent<Renderer>().material.color = colour;
    }
    public void changeSmoothness(float input)
    {
        smoothness += input;
        smoothness = Mathf.Clamp01(smoothness);

        propertyBlock.SetFloat("_Smooth", smoothness);
        render.SetPropertyBlock(propertyBlock);

        /*propertyBlock.SetFloat("_Smooth", smoothness);
        render.SetPropertyBlock(propertyBlock);*/
    }
}

public enum MetalType{ iron,bronze }
