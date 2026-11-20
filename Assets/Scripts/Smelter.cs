using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Smelter : MonoBehaviour
{
    public ParticleSystem lavaParticles;
    public MeshRenderer movingLava;
    public CastArea castArea;
    public Material[] mat;
    public Renderer prefab;
    MetalType whichMetal;

    public float cookingTime = 3f;
    [Range(0, 100)]
    public int metalCount = 0;

    MaterialPropertyBlock block;
    bool available = true;
    

    void OnTriggerEnter(Collider other)
    {
        Heatable heatable = other.GetComponent<Heatable>();
        if (heatable != null)
            whichMetal = heatable.metal;
        print("Object is \'"+whichMetal+"\'");

        this.metalCount += 50;

        print("Metal: "+this.metalCount);
        if(this.metalCount == 100 && available)
        {
            available = false;
            castArea.currentCast.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            StartCoroutine(Cook());
        }
                
        Destroy(other.gameObject); // make sure player hands donut touch it lol :)


    }

    IEnumerator Cook()
    {
        bool doOnce = true;
        bool doOnce2 = true;
        for (float alpha = 0f; alpha < 1f; alpha += Time.deltaTime / cookingTime)
        {
            float factor = 0f;
            if (alpha < 0.35f)
                factor = alpha / 0.35f;
            else if (alpha < 0.65)
                factor = 1f;
            else
                factor = 1f + (alpha - 0.65f) / 0.35f;

            block.SetFloat("_ShowAmount", factor);
            movingLava.SetPropertyBlock(block);

            if (doOnce)
                castArea.currentCast.Cook(alpha);

            if (doOnce2 && alpha > 0.9f)
            {
                lavaParticles.Play();
            }

            if (doOnce && alpha > 0.99f)
            {
                castArea.currentCast.Cook(0f);
                castArea.currentCast.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                SpawnObject();
                doOnce = false;
                available = true;
            }

            yield return null;
        }
    }

    void SpawnObject()
    {
        Renderer newObj = prefab;
        if (castArea.currentCast != null)
            newObj = castArea.currentCast.GetCastObject();

        Renderer newObject = Instantiate(newObj, transform.position + transform.forward, quaternion.identity);
        newObject.material = mat[(int)whichMetal];
        this.metalCount -= 100;

        Heatable temp = newObject.GetComponent<Heatable>();
        temp?.changeTemperature(1f);
        temp?.changeSmoothness(-1f);
    }


    // Start is called before the first frame update
    void Start()
    {
        block = new MaterialPropertyBlock();
        block.SetFloat("_ShowAmount", 0f);
        movingLava.SetPropertyBlock(block);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
