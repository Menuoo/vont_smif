using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooling : MonoBehaviour
{
    public ParticleSystem smoke;
    public Color smokeColour = new Color(1f,1f,1f,1f);

    // Start is called before the first frame update
    void Start()
    {
        smoke.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        smoke.Play();
    }
    void OnTriggerStay(Collider other)
    {
        Heatable obj = other.gameObject.GetComponent<Heatable>();
        if(obj !=null)
        {
            obj.changeTemperature(Time.fixedDeltaTime/5f * -1.0f);
            smokeColour.a = obj.temperature;
            smoke.startColor = smokeColour;
        }
    }
    void OnTriggerExit(Collider other)
    {
        smoke.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
