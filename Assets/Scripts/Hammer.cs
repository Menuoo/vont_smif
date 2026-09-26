using System.Collections;
using System.Collections.Generic;
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
            obj.HammerThis(power);

            //ready = false;
            Invoke("Reload", cooldown);
        }
    }

    void Reload()
    {
        ready = true;
    }

}
