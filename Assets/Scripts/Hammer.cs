using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public Rigidbody rb;
    public float velocityRequirement = 5f;
    

    public float cooldown = 0.1f;
    bool ready = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(rb.velocity.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("fuck");

        Hammerable obj = collision.gameObject.GetComponent<Hammerable>();

        if (obj != null && ready && rb.velocity.magnitude > velocityRequirement)
        {
            Debug.Log("fuck 2");
            float power = rb.velocity.magnitude;
            obj.HammerThis(power, collision.GetContact(0).point, rb.velocity);

            ready = false;
            Invoke("Reload", cooldown);
        }
    }

    public void Reload()
    {
        Debug.Log("trigger reload");
        ready = true;
    }

}
