using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grindstone : MonoBehaviour
{   
    public ParticleSystem sparks;
    public Color sparksColour = new Color(1f,1f,1f);
    public AudioSource grindingNoise;
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
        Vector3 closePoint = other.ClosestPoint(transform.position);
        sparks.transform.position = closePoint;
        sparks.transform.LookAt(transform.position + other.transform.position - transform.position);

        // DO SHIT HERE
        // sparksColour.a = polishProcess; IDEA FOR SHIT DOING             w idea - A
    }

    public void OnTriggerEnter(Collider other)
    {
        
        sparks.Play();
        grindingNoise.Play();
    }
    public void OnTriggerExit(Collider other)
    {
        sparks.Stop();
        grindingNoise.Pause();
    }

}
