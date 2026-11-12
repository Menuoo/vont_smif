using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grindstone : MonoBehaviour
{   
    public ParticleSystem sparks;
    public Color sparksColour = new Color(1f,1f,1f);
    // Update is called once per frame
    void FixedUpdate()
    {
        //grind.transform.Rotate(0f,-10f,0f); THE RETARDED WAY
    }

    void Start()
    {
        sparks.Stop();
    }
    public void OnTriggerStay(Collider other)
    {
        sparks.transform.position = other.ClosestPoint(transform.position);

        // DO SHIT HERE
        // sparksColour.a = polishProcess; IDEA FOR SHIT DOING
    }

    public void OnTriggerEnter(Collider other)
    {
        sparks.Play();
    }
    public void OnTriggerExit(Collider other)
    {
        sparks.Stop();
    }

}
